using ApiInterviewTest.Contracts;
using ApiInterviewTest.Contracts.Requests;
using ApiInterviewTest.Contracts.Responses;
using Infraestructure.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using Services.Models;
using System.Net;
using System.Text;

namespace ApiInterviewTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IService _patientService;

        private readonly IConfiguration _configuration;

        public PatientController(IService patientService, IConfiguration configuration)
        {
            _patientService = patientService;
            _configuration = configuration;

        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IResponse> Patients()
        {
            try
            {
                IEnumerable<Patient> patients = ((PatientService)_patientService).GetAllPatients();
                PatientResponse response = new PatientResponse();
                response.Patients = patients.ToList<Patient>();
                return Ok(response);

            }
            catch (BaseException ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("NewPatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IResponse>> NewPatientAsync()
        {
            try
            {
                string jsonRequest = string.Empty;

                using (var reader = new StreamReader(Request.Body,
                     encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    jsonRequest = await reader.ReadToEndAsync();
                }

                var patientRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<PatientRequestBase>(jsonRequest, new RequestConverter());


                Patient newPatient = new Patient()
                {
                    DateOfBirth = patientRequest.DateOfBirth,
                    LastName = patientRequest.LastName,
                    Name = patientRequest.Name,
                    Sickness = patientRequest.Sickness,


                };

                var service = (PatientService)this._patientService;
                if (service.InsertNewPatient(newPatient))
                {

                    return Ok("Patient created.");
                }
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = "The patient could not be created.",
                    StatusCode = (int)HttpStatusCode.BadRequest

                };

                return BadRequest(error);

            }
            catch (BaseException ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("UpdatePatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IResponse>> UpdatePatient()
        {
            try
            {
                string jsonRequest = string.Empty;

                using (var reader = new StreamReader(Request.Body,
                     encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    jsonRequest = await reader.ReadToEndAsync();
                }

                UpdatePatientRequest patientRequest = (UpdatePatientRequest)Newtonsoft.Json.JsonConvert.DeserializeObject<PatientRequestBase>(jsonRequest, new RequestConverter());


                Patient newPatient = new Patient()
                {
                    DateOfBirth = patientRequest.DateOfBirth,
                    LastName = patientRequest.LastName,
                    Name = patientRequest.Name,
                    Sickness = patientRequest.Sickness,
                    PatientId = patientRequest.PatientId

                };

                var service = (PatientService)this._patientService;
                if (service.UpdatePAtient(newPatient))
                {

                    return Ok("Patient updated.");
                }
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = "The patient could not be updated.",
                    StatusCode = (int)HttpStatusCode.BadRequest

                };

                return BadRequest(error);

            }
            catch (BaseException ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("DeletePatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IResponse>> DeletePatient([FromBody] DeleteEntityRequest requestDelete)
        {
            try
            {
                if (requestDelete.Id <= 0)
                    throw new BaseException("The patient id can not be null");

                var service = (PatientService)this._patientService;
                var patient = service.GetById(requestDelete.Id);

                if (patient is null)
                    throw new BaseException("Patient does not exist.");

                if (service.DeletePatient(patient))
                {

                    return Ok("Patient deleted.");
                }
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = "The patient could not be deleted.",
                    StatusCode = (int)HttpStatusCode.BadRequest

                };

                return BadRequest(error);

            }
            catch (BaseException ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost("GetPatients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IResponse>> GetPatients([FromBody] GetPatientsRequest getPatients)
        {
            try
            {
                string filters = this.CreateSearchFilters(getPatients);
                var service = (PatientService)this._patientService;

                var patients = service.GetByFilters(filters);
                PatientResponse response = new PatientResponse()
                {
                    Patients = patients.ToList()
                };
                return Ok(patients);
            }
            catch (BaseException ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private string CreateSearchFilters(GetPatientsRequest getPatients)
        {
            string filters = string.Empty;
            string format = "yyyy-MM-dd";

            if (getPatients is not null)
            {
                
                if (!string.IsNullOrEmpty(getPatients.Name))
                    filters += $" Where PatientName Like '%{getPatients.Name}%' ";

                if (!string.IsNullOrEmpty(getPatients.LastName))
                {
                    if (filters.Contains("Where"))
                        filters += $"And PatientLastName Like '%{getPatients.LastName}%' ";
                    else
                        filters += $"Where PatientLastName Like '%{getPatients.LastName}%' ";
                }

                if (!string.IsNullOrEmpty(getPatients.Sickness))
                {
                    if (filters.Contains("Where"))
                        filters += $"And Sickness Like '%{getPatients.Sickness}%' ";
                    else
                        filters += $"Where Sickness Like '%{getPatients.Sickness}%' ";
                }

                if (getPatients.DateOfBirth is not null)
                {
                    if (filters.Contains("Where"))
                        filters += $"And DateOfBirth >= '{getPatients.DateOfBirth.Value.ToString(format)}' ";
                    else
                        filters += $"Where DateOfBirth >= '{getPatients.DateOfBirth.Value.ToString(format)}' ";
                }
            }

            return filters;

        }
    }
}
