using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Authorization;

using APIMe.Interfaces;
using AutoMapper;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using static Duende.IdentityServer.Models.IdentityResources;
using APIMe.JwtFeatures;
using System.IdentityModel.Tokens.Jwt;
using APIMe.Entities.Models;
using APIMe.Entities.DataTransferObjects;
using APIMe.Entities.DataTransferObjects.Authorization;
using APIMe.Entities.DataTransferObjects.Admin.Section;
using APIMe.Entities.DataTransferObjects.Admin.Route;
using APIMe.Services.Routes;
using APIMe.Entities.Constants;
using System.Text.Json;
using System.Security;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using APIMe.Utilities.Validators;
using APIMe.Utilities.JsonHelper;

namespace APIMe.Controllers
{
    [ApiController]
    [Route("routeApi")]
    public class RouteController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IMapper _mapper;
        private readonly RouteService _routeService;
        private APIMeContext _aPIMeContext;
        private readonly RouteLogService _routeLogService;

        private static readonly Regex PHONE_REGEX = new Regex(@"^(\+1\s?)?(\(\d{3}\)|\d{3})[-\s]?\d{3}[-\s]?\d{4}$");
        private static readonly Regex EMAIL_REGEX = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        private static readonly Regex PRICE_REGEX = new Regex(@"^[1-9]\d*(\.\d+)?|0*\.[1-9]\d*$");

        public RouteController(UserManager<IdentityUser> userManager, APIMeContext aPIMeContext, RouteService routeService, IMapper mapper, RouteLogService routeLogService)
        {
            _userManager = userManager;
            _aPIMeContext = aPIMeContext;
            _routeService = routeService;
            _mapper = mapper;
            _routeLogService = routeLogService;
        }

        [HttpGet("routes")]
        public async Task<ActionResult<IEnumerable<RouteDto>>> GetRoutes()
        {      
                var routes = await _routeService.GetRoutesAsync();
                var routeDtos = _mapper.Map<IEnumerable<RouteDto>>(routes);
                return Ok(routeDtos);
        }


        [HttpGet("dataSources")]
        public async Task<ActionResult<IEnumerable<RouteDto>>> GetDataSources()
        {
            var sources = await _routeService.GetDataSourcesAsync();
         
            return Ok(sources);
        }


        [HttpPost("testRoute/{routeId}")]
        public async Task<ActionResult<TestRouteResponse>> TestRoute(int routeId, [FromBody] object values)
        {
            try
            {
                var response = await _routeService.TestRouteAsync(routeId, values);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("properties/{routeId}")]
        public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesByRouteId(int routeId)
        {
            var properties = await _routeService.GetPropertiesByRouteIdAsync(routeId);
            return Ok(properties);
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<RouteDto>> GetRoute(int id)
        {
            var route = await _aPIMeContext.Routes.Include(r => r.RouteType).SingleOrDefaultAsync(r => r.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            var routeDto = _mapper.Map<RouteDto>(route);
            routeDto.RouteTypeName = route.RouteType.Name;
            routeDto.RouteTypeResponseCode = route.RouteType.ResponseCode;
            routeDto.RouteTypeCrudActionName = CrudActions.Actions.FirstOrDefault(r => r.Id == route.RouteType.CrudId) == null ? "" : CrudActions.Actions.FirstOrDefault(r => r.Id == route.RouteType.CrudId).Action;
            routeDto.RouteTypeCrudActionId = route.RouteType.CrudId;

            routeDto.Records = await _routeService.GetRecordsFromDataTableAsync(route.DataTableName, 10);

            return routeDto;
        }

        [HttpPost("add")]
        public async Task<ActionResult<RouteDto>> CreateRoute(RouteDto routeDto)
        {
            var route = _mapper.Map<Entities.Models.Route>(routeDto);
            await _routeService.CreateRouteAsync(route);
            var createdRouteDto = _mapper.Map<RouteDto>(route);
            return CreatedAtAction(nameof(GetRoute), new { id = createdRouteDto.Id }, createdRouteDto);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRoute(int id, RouteDto routeDto)
        {
            if (id != routeDto.Id)
            {
                return BadRequest();
            }

            var route = _mapper.Map<Entities.Models.Route>(routeDto);

            try
            {
                await _routeService.UpdateRouteAsync(route);
            }
            catch (DbUpdateConcurrencyException) when (!_routeService.RouteExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var result = await _routeService.DeleteRouteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("records/{tableName}")]
        public async Task<ActionResult<IEnumerable<object>>> GetRecordsFromTable(string tableName, [FromQuery] int numberOfRecords = 10)
        {
            tableName = tableName.ToLower();
            tableName = char.ToUpper(tableName[0]) + tableName.Substring(1);
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    return BadRequest("Table name is required.");
                }

                if (numberOfRecords <= 0)
                {
                    return BadRequest("Number of records must be greater than 0.");
                }

                if (DataSourceTables.DataSources.FirstOrDefault(item => item.Name == tableName) == null)
                {
                    throw new SecurityException();
                }

                var records = await _routeService.GetRecordsFromDataTableAsync(tableName, numberOfRecords);

                if (records == null)
                {
                    return NotFound("No records found for the specified table.");
                }

              
                await _routeLogService.LogRequestAsync(HttpContext, tableName);
                


                return Ok(records);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized access.");
            }
            catch (SecurityException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Access to the requested resource is forbidden.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
        [HttpGet("records/{tableName}/{id}")]
        public async Task<ActionResult<object>> GetRecordByIdFromTable(string tableName, int id)
        {
            tableName = tableName.ToLower();
            tableName = char.ToUpper(tableName[0]) + tableName.Substring(1);

            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    return BadRequest("Table name is required.");
                }

                if (DataSourceTables.DataSources.FirstOrDefault(item => item.Name == tableName) == null)
                {
                    throw new SecurityException();
                }

                var record = await _routeService.GetRecordByIdFromDataTableAsync(tableName, id);

                if (record == null)
                {
                    return NotFound("No record found for the specified ID in the given table.");
                }

                await _routeLogService.LogRequestAsync(HttpContext, tableName);

                return Ok(record);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized access.");
            }
            catch (SecurityException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Access to the requested resource is forbidden.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [HttpPost("records/{tableName}")]
        public async Task<ActionResult<object>> AddRecordToTable(string tableName, [FromBody] JsonElement recordJson)
        {
            tableName = tableName.ToLower();
            tableName = char.ToUpper(tableName[0]) + tableName.Substring(1);

            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    return BadRequest("Table name is required.");
                }

                if (DataSourceTables.DataSources.FirstOrDefault(item => item.Name == tableName) == null)
                {
                    throw new SecurityException();
                }

                var dbContextType = _aPIMeContext.GetType();
                var tableProperty = dbContextType.GetProperty(tableName);
                if (tableProperty == null)
                {
                    throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
                }

                var table = (IQueryable)tableProperty.GetValue(_aPIMeContext);
                var entityType = table.ElementType;

                // Deserialize the record and perform validation
                var recordString = recordJson.GetRawText();
                var record = JsonConvert.DeserializeObject(recordString, entityType);
                var validationResult = ValidateRecord(record);

                if (validationResult != null)
                {
                    return BadRequest(new { Message = validationResult.ErrorMessage, Errors = validationResult.ValidationResults });
                }

                // Continue with adding the record
                var savedRecord = await _routeService.AddRecordToDataTableAsync(tableName, recordJson);

                if (savedRecord == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
                }

                await _routeLogService.LogRequestAsync(HttpContext, tableName);

                return CreatedAtAction(nameof(AddRecordToTable), new { tableName = tableName, id = savedRecord.GetType().GetProperty("Id").GetValue(savedRecord) }, savedRecord);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized access.");
            }
            catch (SecurityException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Access to the requested resource is forbidden.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }



        [HttpPut("records/{tableName}")]
        public async Task<ActionResult<object>> UpdateRecordInTable(string tableName, [FromBody] JsonElement recordJson)
        {
            tableName = tableName.ToLower();
            tableName = char.ToUpper(tableName[0]) + tableName.Substring(1);

            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    return BadRequest("Table name is required.");
                }

                if (DataSourceTables.DataSources.FirstOrDefault(item => item.Name == tableName) == null)
                {
                    throw new SecurityException();
                }

                var dbContextType = _aPIMeContext.GetType();
                var tableProperty = dbContextType.GetProperty(tableName);
                if (tableProperty == null)
                {
                    throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
                }

                var table = (IQueryable)tableProperty.GetValue(_aPIMeContext);
                var entityType = table.ElementType;

                // Deserialize the record and perform validation
                var recordString = recordJson.GetRawText();
                var record = JsonConvert.DeserializeObject(recordString, entityType);
                var validationResult = ValidateRecord(record);

                if (validationResult != null)
                {
                    return BadRequest(new { Message = validationResult.ErrorMessage, Errors = validationResult.ValidationResults });
                }

                // Continue with updating the record
                var updatedRecord = await _routeService.UpdateRecordInDataTableAsync(tableName, recordJson);

                if (updatedRecord == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
                }

                await _routeLogService.LogRequestAsync(HttpContext, tableName);

                return Ok(updatedRecord);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized access.");
            }
            catch (SecurityException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Access to the requested resource is forbidden.");
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status404NotFound, "The record with the specified key values could not be found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [HttpPatch("records/{tableName}/{id}")]
        public async Task<ActionResult<object>> PatchRecordInTable(string tableName, int id, [FromBody] JsonElement patchDocument)
        {
            tableName = tableName.ToLower();
            tableName = char.ToUpper(tableName[0]) + tableName.Substring(1);

            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    return BadRequest("Table name is required.");
                }

                if (DataSourceTables.DataSources.FirstOrDefault(item => item.Name == tableName) == null)
                {
                    throw new SecurityException();
                }

                // Get the existing record
                var existingRecord = await _routeService.GetRecordByIdFromDataTableAsync(tableName, id);
                if (existingRecord == null)
                {
                    return NotFound("The record with the specified key values could not be found.");
                }

                // Apply the patch
                var patcher = new JsonPatcher();
                var patchedRecordJson = patcher.ApplyPatch(existingRecord, patchDocument);

                // Deserialize the patched record and perform validation
                var dbContextType = _aPIMeContext.GetType();
                var tableProperty = dbContextType.GetProperty(tableName);
                var table = (IQueryable)tableProperty.GetValue(_aPIMeContext);
                var entityType = table.ElementType;

                var patchedRecordString = patchedRecordJson.GetRawText();
                var patchedRecord = JsonConvert.DeserializeObject(patchedRecordString, entityType);
                var validationResult = ValidateRecord(patchedRecord);

                if (validationResult != null)
                {
                    return BadRequest(new { Message = validationResult.ErrorMessage, Errors = validationResult.ValidationResults });
                }

                // Update the existing record with the patched record
                var updatedRecord = await _routeService.PatchRecordInDataTableAsync(tableName, id, patchedRecordJson);

                if (updatedRecord == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
                }

                await _routeLogService.LogRequestAsync(HttpContext, tableName);

                return Ok(updatedRecord);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized access.");
            }
            catch (SecurityException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Access to the requested resource is forbidden.");
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status404NotFound, "The record with the specified key values could not be found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }


        [HttpDelete("records/{tableName}/{id}")]
        public async Task<ActionResult> DeleteRecordFromTable(string tableName, int id)
        {
            tableName = tableName.ToLower();
            tableName = char.ToUpper(tableName[0]) + tableName.Substring(1);

            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    return BadRequest("Table name is required.");
                }

                if (DataSourceTables.DataSources.FirstOrDefault(item => item.Name == tableName) == null)
                {
                    throw new SecurityException();
                }

                await _routeService.DeleteRecordFromDataTableAsync(tableName, id);

                await _routeLogService.LogRequestAsync(HttpContext, tableName);

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized access.");
            }
            catch (SecurityException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Access to the requested resource is forbidden.");
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status404NotFound, "The record with the specified key values could not be found.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [HttpGet("properties/byTableName/{tableName}")]
        public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesByTableName(string tableName)
        {
            try
            {
                tableName = tableName.ToLower();
                tableName = char.ToUpper(tableName[0]) + tableName.Substring(1);

                if (string.IsNullOrEmpty(tableName))
                {
                    return BadRequest("Table name is required.");
                }

                if (DataSourceTables.DataSources.FirstOrDefault(item => item.Name == tableName) == null)
                {
                    throw new SecurityException();
                }

                var properties = await _routeService.GetPropertiesByTableNameAsync(tableName);
                return Ok(properties);
            }
            catch (SecurityException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Ensure that the table name is among the list of accessible resources.");
            }
          
        }





        private CustomValidationResult ValidateRecord(object record)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(record);
            Validator.TryValidateObject(record, validationContext, validationResults, true);

            foreach (var property in record.GetType().GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);

                if (propertyName.Equals("Phone", StringComparison.OrdinalIgnoreCase) && propertyValue != null)
                {
                    if (!PHONE_REGEX.IsMatch(propertyValue.ToString()))
                    {
                        validationResults.Add(new ValidationResult($"Invalid phone number format.", new[] { propertyName }));
                    }
                }
                else if (propertyName.Equals("Email", StringComparison.OrdinalIgnoreCase) && propertyValue != null)
                {
                    if (!EMAIL_REGEX.IsMatch(propertyValue.ToString()))
                    {
                        validationResults.Add(new ValidationResult($"Invalid email address format.", new[] { propertyName }));
                    }
                }                
                else if (propertyName.Equals("Price", StringComparison.OrdinalIgnoreCase) && propertyValue != null)
                {
                    if (!PRICE_REGEX.IsMatch(propertyValue.ToString()))
                    {
                        validationResults.Add(new ValidationResult($"Price should be more than 0.", new[] { propertyName }));
                    }
                }
                else if (propertyName.Equals("TotalAmount", StringComparison.OrdinalIgnoreCase) && propertyValue != null)
                {
                    if (!PRICE_REGEX.IsMatch(propertyValue.ToString()))
                    {
                        validationResults.Add(new ValidationResult($"Total Amount should be more than 0.", new[] { propertyName }));
                    }
                }
                else if (propertyName.Equals("Quantity", StringComparison.OrdinalIgnoreCase) && propertyValue != null)
                {
                    if (!PRICE_REGEX.IsMatch(propertyValue.ToString()))
                    {
                        validationResults.Add(new ValidationResult($"Quantity should be more than 0.", new[] { propertyName }));
                    }
                }
                else if (propertyName.Contains("Date", StringComparison.OrdinalIgnoreCase) && propertyValue != null)
                {
                    if (!DateTime.TryParse(propertyValue.ToString(), out _))
                    {
                        validationResults.Add(new ValidationResult($"Invalid date format.", new[] { propertyName }));
                    }
                }
            }

            return validationResults.Count > 0 ? new CustomValidationResult("Validation failed.", validationResults) : null;
        }


    }
}
