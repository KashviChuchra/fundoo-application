
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Request.UserRequest;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext _fundooContext;

        public UserRL(FundooContext fundooContext)
        {
            _fundooContext = fundooContext;
        }

        public async Task<ResponseModel<string>> RegisterUserRL(
            RegistrationModel registrationModel)
        {
            var email = registrationModel.Email.Trim();

            var existingUser = await _fundooContext.Users
                .FirstOrDefaultAsync(user => user.Email == email);

            if (existingUser != null)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "An account with this email already exists."
                };
            }

            var user = new UserEntity
            {
                FirstName = registrationModel.FirstName,
                LastName = registrationModel.LastName,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(registrationModel.Password),
                PhoneNumber = registrationModel.ContactNo
            };

            await _fundooContext.Users.AddAsync(user);
            await _fundooContext.SaveChangesAsync();

            return new ResponseModel<string>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = "Registration successful."
            };
        }

        public async Task<ResponseModel<LoginResponseModel>> LoginUserRL(
            LoginModel loginModel)
        {
            var email = loginModel.Email.Trim();

            var user = await _fundooContext.Users
                .FirstOrDefaultAsync(user => user.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            {
                return new ResponseModel<LoginResponseModel>
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            return new ResponseModel<LoginResponseModel>
            {
                Success = true,
                Message = "Login successful.",
                Data = new LoginResponseModel
                {
                    UserId = user.UserId,
                    Email = user.Email
                }
            };
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            email = email.Trim();

            return await _fundooContext.Users
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<bool> UpdatePasswordAsync(
            int userId, string passwordHash)
        {
            var user = await _fundooContext.Users
                .FirstOrDefaultAsync(user => user.UserId == userId);

            if (user == null)
                return false;

            user.Password = passwordHash;

            await _fundooContext.SaveChangesAsync();

            return true;
        }
    }
}
