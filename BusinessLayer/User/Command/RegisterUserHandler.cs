using BusinessLayer.User.Command.HandlerInterface;
using Middleware.Services.Hashing;
using ModelLayer.Request.UserRequest;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer.User.Command
{
    public class RegisterUserHandler: IRegisterUserHandler
    {
        private readonly IUserRL _userRL;
        private readonly CustomHashing _customHasher;

        public RegisterUserHandler(IUserRL userRL, CustomHashing customHasher)
        {
            _userRL = userRL;
            _customHasher = customHasher;
        }
        public async Task<ResponseModel<string>> HandleAsync(RegisterUserCommand command)
        {
            var registrationModel = command.User;
            registrationModel.Password = _customHasher.HashPassword(registrationModel.Password);
            var user = await _userRL.RegisterUserRL(registrationModel);
            if (user == null)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "An account with this email already exists.",
                    Data = null
                };
            }

            return new ResponseModel<string>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = "Registration successful."
            };
        }
    }
}
