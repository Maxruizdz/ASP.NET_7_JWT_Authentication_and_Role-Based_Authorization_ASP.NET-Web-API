using AspNet7JWT_Autentication.Core.Dtos;
using AspNet7JWT_Autentication.Core.Entities;
using AspNet7JWT_Autentication.Core.Interfaces;
using AspNet7JWT_Autentication.Core.OtherObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspNet7JWT_Autentication.Core.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AuthServicesResponse> LoginAsync(LoginDtos loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user is null)
                return new AuthServicesResponse { IsSucceed = false, Message = "Invalid Credentials" };

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
                return new AuthServicesResponse { IsSucceed = false, Message = "Invalid Credentials" };

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return new AuthServicesResponse() { IsSucceed=true,Message=token };
        }

        public async  Task<AuthServicesResponse> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
      
            
            var user_Future_Admin = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user_Future_Admin is null)
            {

                return new AuthServicesResponse()
                {
                    IsSucceed = false,
                    Message = "Invalid username"
                };

            }


            await _userManager.AddToRoleAsync(user_Future_Admin, StaticUserRoles.ADMIN);
    
                return new AuthServicesResponse()
                {
                    IsSucceed = true,
                    Message = "User is now an Admin",
   
                }; 
        }

        public  async Task<AuthServicesResponse> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user_Future_Owner = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user_Future_Owner is null)
            {

                return new AuthServicesResponse()
                {
                    IsSucceed = false,
                    Message = "Invalid username"
                };

            }


            await _userManager.AddToRoleAsync(user_Future_Owner, StaticUserRoles.OWNER);
    
                return new AuthServicesResponse()
                {
                    IsSucceed = true,
                    Message = "User is now an Owner",
   
                }; 
        }

        public async Task<AuthServicesResponse> RegisterAsync(RegisterDtos registerDto)
        {

            var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (isExistsUser != null)
                return new AuthServicesResponse() {

                    IsSucceed = false,
                    Message = "UserName Already Exists", };

            ApplicationUser newUser = new ApplicationUser()
            {
                FirtsName = registerDto.UserName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Beacause: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new AuthServicesResponse { IsSucceed = false, Message = errorString };
            }

            // Add a Default USER Role to all users
            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

            return new AuthServicesResponse() {IsSucceed =true , Message="User Created Successfully" };
        }

        public async  Task<AuthServicesResponse> SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
                return new AuthServicesResponse {IsSucceed=false ,Message="Roles Seeding is Already Done"};

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));

            return new AuthServicesResponse { IsSucceed= true ,Message="Role Seeding Done Successfully" };
        }
        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
    }
}
