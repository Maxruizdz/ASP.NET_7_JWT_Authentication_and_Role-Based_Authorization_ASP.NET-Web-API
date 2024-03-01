using AspNet7JWT_Autentication.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AspNet7JWT_Autentication.Core.Interfaces
{
    public interface IAuthServices
    {
        Task<AuthServicesResponse> SeedRolesAsync();
         Task<AuthServicesResponse> RegisterAsync(RegisterDtos registerDto);
        Task<AuthServicesResponse> LoginAsync(LoginDtos loginDto);
        Task<AuthServicesResponse> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);

        Task<AuthServicesResponse> MakeOwnerAsync( UpdatePermissionDto updatePermissionDto);

    }
}
