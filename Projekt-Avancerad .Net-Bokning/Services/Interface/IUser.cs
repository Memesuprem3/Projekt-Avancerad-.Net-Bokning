using Projekt_Models;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Services.Interface
{
    public interface IUser
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
    }
}