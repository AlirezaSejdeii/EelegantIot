using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EelegantIot.Shared.Services
{
    public interface IUserService
    {
        Task<bool> IsLoggedIn();
        Task<string?> LoginOrSignUpUser(string username, string password);
    }
}
