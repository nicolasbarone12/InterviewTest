using ApiInterviewTest.Contracts;
using ApiInterviewTest.Contracts.Requests;
using ApiInterviewTest.Contracts.Responses;
using Infraestructure;
using Infraestructure.Auth;
using Infraestructure.Errors;
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
        private readonly JwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public HomeController(IService userService, IConfiguration configuration, JwtTokenService jwtTokenService)
        {
            _userService = userService;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("RegisterUser")]
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
                    

                    string jwtToken = _jwtTokenService.GenerateJwtToken(newUser.Password, newUser.UserCode);//, oAuthRequest.audience);

                    var userResponse = new UserProfileResponse()
                    {
                        LastName = newUser.UserLastName,
                        UserCode = request.UserCode,
                        UserName = request.UserName,
                        Auth = new AuthResponse()
                        {
                            access_token = jwtToken,
                            token_type = OAuthConstants.BEARER,
                            expires_in = (long)TimeSpan.FromSeconds(_jwtTokenService.DURATION_SECONDS).TotalSeconds
                        }
                        
                    };
                    return Ok(userResponse);
                    
                }

                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = "An error ocurred trying to register user.",
                    StatusCode = (int)HttpStatusCode.BadRequest
                });
                
            }
            catch(BaseException ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
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


        [HttpPost("LogIn")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IResponse> LogIn([FromBody] LogInRequest request)
        {
            try
            {
                User loginUser = null;
               
                string userCode = request.UserCode;
                string pass = this.EncodePassWord(request.Password);
                loginUser = ((UserService)_userService).Login(userCode, pass);
                if (loginUser is not null)
                {
                    

                    string jwtToken = _jwtTokenService.GenerateJwtToken(pass, userCode);//, oAuthRequest.audience);

                    var userResponse = new UserProfileResponse()
                    {
                        LastName = loginUser.UserLastName,
                        UserCode = loginUser.UserCode,
                        UserName = loginUser.UserName,
                        Auth = new AuthResponse()
                        {
                            access_token = jwtToken,
                            token_type = OAuthConstants.BEARER,
                            expires_in = (long)TimeSpan.FromSeconds(_jwtTokenService.DURATION_SECONDS).TotalSeconds
                        }

                    };
                    return Ok(userResponse);
                }

                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = "An error ocurred trying to login user.",
                    StatusCode = (int)HttpStatusCode.BadRequest
                });

                
            }
            catch (BaseException ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    ErrorMessage = ex.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                });
            }
            catch (Exception ex)
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
            return Encriptation.Encrypt(pass);
        }

        
    }
}
