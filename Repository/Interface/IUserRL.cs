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
        Task<UserEntity?> RegisterUserRL(RegistrationModel registrationModel);
        Task<UserEntity?> LoginUserRL(LoginModel loginModel);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<bool> UpdatePasswordAsync(int userId, string passwordHash);

    }
}
