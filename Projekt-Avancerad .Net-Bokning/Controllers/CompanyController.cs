using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_Avancerad_.Net_Bokning.Services;
using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ICompany _companyRepo;
        private IAppointment _appointmentRepo;

        public CompanyController(ICompany companyRepo, IAppointment appointmentRepo)
        {
            _companyRepo = companyRepo;
            _appointmentRepo = appointmentRepo;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompaines()
        {
            var compaines = await _companyRepo.GetAllCompaniesAsync();
            return Ok(compaines);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            var compaines = await _companyRepo.GetCompanyByIdAsync(id);
            return Ok(compaines);
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            var createdCompany = await _companyRepo.AddCompanyAsync(company);
            return CreatedAtAction(nameof(GetCompanyById),new {id = createdCompany.CompanyId}, createdCompany);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest("Company with that ID not found");
            }
            await _companyRepo.UpdateCompanyAsync(company);
            return Ok("sesccus");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComanpy(int id)
        {
            var company = await _companyRepo.DeleteCompanyAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok("Company Deleted");
        }

        [HttpGet("{id}/appointments/month/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByMonth(int id, int year, int month)
        {
            var appointments = await _appointmentRepo.GetAppointmentMonthAsync(year, month);
            var companyAppointments = appointments.Where(a => a.CompanyId == id);
            return Ok(companyAppointments);
        }

        [HttpGet("{id}/appointments/week/{year}/{week}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByWeek(int id, int year, int week)
        {
            var appointments = await _appointmentRepo.GetAppointmentWeekAsync(year, week);
            var companyAppointments = appointments.Where(a => a.CompanyId == id);
            return Ok(companyAppointments);
        }
    }
}
