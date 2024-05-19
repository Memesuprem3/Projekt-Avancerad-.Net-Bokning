using Microsoft.AspNetCore.Identity;
using Projekt_Models;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Services.Interface
{
    public interface IAuthentication
    {
        Task<IdentityResult> RegisterAsync(User userModel);
        Task<bool> LoginAsync(User loginModel);
    }
}