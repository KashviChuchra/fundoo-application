using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private FundooContext fundooContext;
        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public RegistrationModel RegisterUserRL(RegistrationModel registrationModel)
        {
            UserEntity userEntity = new UserEntity();
            //UserEntity userEntity1 = new UserEntity();

            userEntity.FirstName = registrationModel.FirstName;
            userEntity.LastName = registrationModel.LastName;
            userEntity.Email = registrationModel.Email;
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(registrationModel.Password);
            userEntity.PhoneNumber = registrationModel.ContactNo;


            fundooContext.Users.Add(userEntity);
            //fundooContext.Users.Add(userEntity1);

            var result =fundooContext.SaveChanges();
            // returns no of rows affeted in result
            Console.WriteLine("Result: " + result);
            return registrationModel;
        }

        public LoginResponseModel LoginUserRL(LoginModel loginModel)
        {
            var user= fundooContext.Users.FirstOrDefault(c => c.Email == loginModel.Email);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }
            bool isPasswordValid =BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password);
            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password");

            }
            return new LoginResponseModel
            {
                UserId = user.UserId,
                Email = user.Email
            };
        }

        public async Task<bool> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var user = await fundooContext.Users.FirstOrDefaultAsync(x => x.Email == forgotPasswordModel.Email);
            if (user == null)
            {
                return false;
            }
            // Gennerate reset token
            var resetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            user.ResetToken = resetToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);
            user.IsResetTokenUsed = false;
            await fundooContext.SaveChangesAsync();

            return true;
        }



        // we can save multiple etities
        // why need? --> eg=? with, sqlpulp
        // multiple transactions? ado.net not preffered
    }
}
