
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Request.UserRequest;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Middleware.Services.Hashing;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext _fundooContext;
        private readonly CustomHashing _customHasher;


        public UserRL(FundooContext fundooContext, CustomHashing customHasher)
        {
            _fundooContext = fundooContext;
            _customHasher = customHasher;
        }

        public async Task<UserEntity?> RegisterUserRL(RegistrationModel registrationModel)
        {
            var email = registrationModel.Email.Trim();
            var existingUser = await _fundooContext.Users.FirstOrDefaultAsync(user => user.Email == email);

            if (existingUser != null)
            {
                return null;
            }

            var user = new UserEntity
            {
                FirstName = registrationModel.FirstName,
                LastName = registrationModel.LastName,
                Email = email,
                //Password = BCrypt.Net.BCrypt.HashPassword(registrationModel.Password),
                Password = registrationModel.Password,
                PhoneNumber = registrationModel.ContactNo
            };

            await _fundooContext.Users.AddAsync(user);
            await _fundooContext.SaveChangesAsync();

            return user;
        }

        public async Task<UserEntity?> LoginUserRL(LoginModel loginModel)
        {
            var email = loginModel.Email.Trim();

            var user = await _fundooContext.Users.FirstOrDefaultAsync(user => user.Email == email);

            if (user == null || //!BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password)) 
               !_customHasher.VerifyPassword(loginModel.Password, user.Password))
            {
                return null;
            }

            return user;
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            email = email.Trim();

            return await _fundooContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string passwordHash)
        {
            var user = await _fundooContext.Users.FirstOrDefaultAsync(user => user.UserId == userId);

            if (user == null)   return false;

            user.Password = passwordHash;

            await _fundooContext.SaveChangesAsync();

            return true;
        }
    }
}
