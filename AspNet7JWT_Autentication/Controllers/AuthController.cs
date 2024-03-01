using AspNet7JWT_Autentication.Core.Dtos;
using AspNet7JWT_Autentication.Core.Entities;
using AspNet7JWT_Autentication.Core.Interfaces;
using AspNet7JWT_Autentication.Core.OtherObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace AspNet7JWT_Autentication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        // Route For Seeding my roles to DB
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
          var result= await _authServices.SeedRolesAsync();

            if (result.IsSucceed) { 
            return Ok(result);
            }
            return BadRequest(result);
        }


        // Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDtos registerDto)
        {
            var result = await _authServices.RegisterAsync(registerDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }


        // Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDtos loginDto)
        {
            var result = await _authServices.LoginAsync(loginDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



        [HttpPost]
        [Route("make-admin")]


        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto) {


            var result = await _authServices.MakeAdminAsync( updatePermissionDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        [Route("make-Owner")]


        public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var result = await _authServices.MakeOwnerAsync(updatePermissionDto);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



    }
}
