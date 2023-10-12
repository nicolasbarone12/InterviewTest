using ApiInterviewTest.Contracts;
using ApiInterviewTest.Contracts.Requests;
using ApiInterviewTest.Contracts.Responses;
using Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using Services.Models;
using System.Net;

namespace ApiInterviewTest.Controllers
{
    [Route("api/Home")]
    [ApiController]
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        private readonly IService _userService;
        private readonly Encriptation _encriptor;
        public HomeController(IService userService, Encriptation encriptor)
        {
            _userService = userService;
            _encriptor = encriptor;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IResponse> RegisterUser([FromBody]RegisterUserRequest request)
        {
            try
            {
                User newUser = new User()
                {
                    UserCode = request.UserCode,
                    Id = 0,
                    UserLastName = request.UserLastName,
                    Password = this.EncodePassWord(request.Password),
                    UserName = request.UserName
                };

                
                if(((UserService)_userService).RegisterUser(newUser))
                {
                    var userResponse = new UserProfileResponse()
                    {
                        LastName = newUser.UserLastName,
                        UserCode = request.UserCode,
                        UserName = request.UserName,
                    };

                    return Created(string.Empty, userResponse);
                }

                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = "An error ocurred trying to register user.",
                    StatusCode = (int)HttpStatusCode.BadRequest
                });
                
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                });
            }
            
        }

        private string EncodePassWord(string pass)
        {
            return _encriptor.Encrypt(pass);
        }

        private string DecodePassWord(string pass)
        {
            return _encriptor.Decrypt(pass);
        }
    }
}
