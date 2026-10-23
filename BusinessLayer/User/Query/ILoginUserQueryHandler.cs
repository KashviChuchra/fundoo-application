using ModelLayer.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.User.Query
{
    public interface ILoginUserQueryHandler
    {
        Task<ResponseModel<LoginResponseModel>> HandleAsync(LoginUserQuery query);
    }
}
