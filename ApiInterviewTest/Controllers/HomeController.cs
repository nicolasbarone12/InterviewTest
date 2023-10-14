using ApiInterviewTest.Contracts;
using ApiInterviewTest.Contracts.Requests;
using ApiInterviewTest.Contracts.Responses;
using Infraestructure;
using Infraestructure.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Interfaces;
using Services.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ApiInterviewTest.Controllers
{
    [Route("api/Home")]
    [ApiController]
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        private readonly IService _userService;
        
        private readonly IConfiguration _configuration;

        public HomeController(IService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
            
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


                    var token = GenerateToken(newUser);

                    var userResponse = new UserProfileResponse()
                    {
                        LastName = newUser.UserLastName,
                        UserCode = newUser.UserCode,
                        UserName = newUser.UserName,
                        Token = token

                    };
                    return Ok(userResponse);
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


                    var token = GenerateToken(loginUser);

                    var userResponse = new UserProfileResponse()
                    {
                        LastName = loginUser.UserLastName,
                        UserCode = loginUser.UserCode,
                        UserName = loginUser.UserName,
                        Token = token

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

        private string GenerateToken(User loginUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,loginUser.UserCode)
            };
            var token = new JwtSecurityToken(null,
                null,
                claims,
                null,
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string EncodePassWord(string pass)
        {
            return Encriptation.Encrypt(pass);
        }

        


    }
}
