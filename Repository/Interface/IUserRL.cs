using ModelLayer.Request.UserRequest;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        Task<ResponseModel<string>> RegisterUserRL(RegistrationModel registrationModel);
        Task<ResponseModel<LoginResponseModel>> LoginUserRL(LoginModel loginModel);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<bool> UpdatePasswordAsync(int userId, string passwordHash);

    }
}
