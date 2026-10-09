using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Request;
using ModelLayer.Response;


namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private FundooContext _fundooContext;
        public UserRL(FundooContext fundooContext)
        {
            _fundooContext = fundooContext;
        }

        public async Task<ResponseModel<string>> RegisterUserRL(RegistrationModel registrationModel)
        {

            var exisitingUser = await _fundooContext.Users.FirstOrDefaultAsync(user => user.Email == registrationModel.Email);
            if (exisitingUser != null)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "An account with this email already exists",
                    Data = null
                };
            }
            
            UserEntity userEntity = new UserEntity();
            //UserEntity userEntity1 = new UserEntity();

            userEntity.FirstName = registrationModel.FirstName;
            userEntity.LastName = registrationModel.LastName;
            userEntity.Email = registrationModel.Email;
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(registrationModel.Password);
            userEntity.PhoneNumber = registrationModel.ContactNo;


            await _fundooContext.Users.AddAsync(userEntity);
            //fundooContext.Users.Add(userEntity1);

            var result =await _fundooContext.SaveChangesAsync();
            // returns no of rows affeted in result

            Console.WriteLine("Result: " + result);

            return new ResponseModel<string>
            {
                Success = true,
                Message = "Registration successful",
                Data = null
            };
        }

        public async Task<ResponseModel<LoginResponseModel>> LoginUserRL(LoginModel loginModel)
        {
            var user= await _fundooContext.Users.FirstOrDefaultAsync(c => c.Email == loginModel.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            {
                return new ResponseModel<LoginResponseModel>
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Data = null
                };
            }
            
            return new ResponseModel<LoginResponseModel>
            {
                Success = true,
                Message = "Login successful",
                Data = new LoginResponseModel
                {
                    UserId = user.UserId,
                    Email = user.Email,
                }
            };
        }
       
        // we can save multiple etities
        // why need? --> eg=? with, sqlpulp
        // multiple transactions? ado.net not preffered
    }
}
