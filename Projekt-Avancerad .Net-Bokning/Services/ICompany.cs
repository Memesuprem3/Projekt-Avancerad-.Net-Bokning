using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public interface ICompany
    {
        
            Task<Company> GetCompanyByIdAsync(int id);
            Task<Company> AddCompanyAsync(Company company);
            Task<Company> UpdateCompanyAsync(Company company);
            Task<Company> DeleteCompanyAsync(int id);

            Task<IEnumerable<Company>> GetAllCompaniesAsync();
        
    }
}
