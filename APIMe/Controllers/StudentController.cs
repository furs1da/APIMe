﻿using System.Threading.Tasks;
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
using APIMe.Entities.DataTransferObjects.Admin.Student;

namespace APIMe.Controllers
{
    [ApiController]
    [Route("studentApi")]
    public class StudentController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly JwtHandler _jwtHandler;
        private APIMeContext _aPIMeContext;
        private readonly IMapper _mapper;

        public StudentController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, JwtHandler jwtHandler, IMapper mapper)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _aPIMeContext = aPIMeContext;
            _mapper = mapper;
        }

        // GET: api/Students
        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            return await _aPIMeContext.Students
                 .Include(s => s.StudentSections)
                 .ThenInclude(ss => ss.Section)
                 .Select(s => new StudentDto
                 {
                     Id = s.Id,
                     Email = s.Email,
                     FirstName = s.FirstName,
                     LastName = s.LastName,
                     StudentId = s.StudentId,
                     ApiKey = s.ApiKey,
                     Sections = s.StudentSections.Select(ss => new SectionDTO
                     {
                         Id = ss.Section.Id,
                         SectionName = ss.Section.SectionName,
                         ProfessorName = ss.Section.Professor.FirstName + " " + ss.Section.Professor.LastName,
                         AccessCode = ss.Section.AccessCode
                     }).ToList()
                 })
                 .ToListAsync();
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("sections")]
        public async Task<ActionResult<List<SectionDTO>>> GetSections()
        {
            var sections = await _aPIMeContext.Sections
                .Select(s => new SectionDTO
                {
                    Id = s.Id,
                    SectionName = s.SectionName,
                    ProfessorName = s.Professor.FirstName + " " + s.Professor.LastName,
                    AccessCode = s.AccessCode,
                    NumberOfStudents = s.StudentSections.Where(ss => ss.SectionId == s.Id).Count()
                })
                .ToListAsync();

            return sections;
        }

        // GET: api/Students/5
        [HttpGet("student/{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _aPIMeContext.Students
               .Include(s => s.StudentSections)
               .ThenInclude(ss => ss.Section)
               .Select(s => new StudentDto
               {
                   Id = s.Id,
                   Email = s.Email,
                   FirstName = s.FirstName,
                   LastName = s.LastName,
                   StudentId = s.StudentId,
                   ApiKey = s.ApiKey,
                   Sections = s.StudentSections.Select(ss => new SectionDTO
                   {
                       Id = ss.Section.Id,
                       SectionName = ss.Section.SectionName,
                       ProfessorName = ss.Section.Professor.FirstName + " " + ss.Section.Professor.LastName,
                       AccessCode = ss.Section.AccessCode
                   }).ToList()
               })
               .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Students/5
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentDto studentDTO)
        {
            var student = _mapper.Map<Student>(studentDTO);
            if (id != student.Id)
            {
                return BadRequest();
            }

            _aPIMeContext.Entry(student).State = EntityState.Modified;

            try
            {
                await _aPIMeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        [HttpPost("add")]
        public async Task<ActionResult<StudentDto>> CreateStudent(StudentDto studentDTO)
        {
            var student = _mapper.Map<Student>(studentDTO);
            _aPIMeContext.Students.Add(student);
            await _aPIMeContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, _mapper.Map<StudentDto>(student));
        }

        // DELETE: api/Students/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _aPIMeContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _aPIMeContext.Students.Remove(student);
            await _aPIMeContext.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _aPIMeContext.Students.Any(e => e.Id == id);
        }


    }

}