using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;

using APIMe.Interfaces;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using static Duende.IdentityServer.Models.IdentityResources;
using APIMe.JwtFeatures;
using System.IdentityModel.Tokens.Jwt;
using APIMe.Entities.Models;
using APIMe.Entities.DataTransferObjects;
using APIMe.Entities.DataTransferObjects.Authorization;
using APIMe.Entities.DataTransferObjects.Admin.Section;
using Duende.IdentityServer.Models;


namespace APIMe.Services.Routes
{
    public class RouteLogService : IRouteLogService
    {
        private readonly APIMeContext _context;
        private readonly IMapper _mapper;

        public RouteLogService(APIMeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddRouteLogAsync(RouteLog routeLog)
        {
            if (routeLog == null)
            {
                throw new ArgumentNullException(nameof(routeLog));
            }

            _context.RouteLogs.Add(routeLog);
            await _context.SaveChangesAsync();
        }

        public async Task LogRequestAsync(HttpContext context, string tableName, int? recordId = null)
        {
            var routeLog = new RouteLog
            {
                Ipaddress = context.Connection.RemoteIpAddress == null ? "" : context.Connection.RemoteIpAddress?.ToString(),
                RequestTimestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")),
                HttpMethod = context.Request.Method,
                TableName = tableName,
                RecordId = recordId,
                RoutePath = context.Request.Path
            };

            var student = await _context.Students.SingleOrDefaultAsync(s => s.Email == context.User.Identity.Name);


            if (student != null)
            {
                routeLog.UserId = student.StudentId;
                routeLog.FullName = student.FirstName + " " + student.LastName;
            }
            else
            {
                routeLog.FullName = "Anonymous";
            }

            // Assuming you have a RouteLogService with a method called AddRouteLogAsync
            await AddRouteLogAsync(routeLog);
        }


        public async Task<IEnumerable<RouteLog>> GetAllRouteLogsAsync()
        {
            return await _context.RouteLogs.Include(r => r.User).ToListAsync();
        }

        public async Task<IEnumerable<RouteLog>> GetRouteLogsByUserIdAsync(int userId)
        {
            return await _context.RouteLogs.Include(r => r.User).Where(r => r.UserId == userId).ToListAsync();        
        }

    }
}
