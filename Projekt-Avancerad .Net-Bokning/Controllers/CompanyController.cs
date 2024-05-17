using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_Avancerad_.Net_Bokning.Services;
using Projekt_Models;
using System.Security.Claims;

namespace Projekt_Avancerad_.Net_Bokning.Controllers
{
    [Authorize(Policy = "Company")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompany _companyRepo;
        private readonly IAppointment _appointmentRepo;

        public CompanyController(ICompany companyRepo, IAppointment appointmentRepo)
        {
            _companyRepo = companyRepo;
            _appointmentRepo = appointmentRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        {
            // Log user claims
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims.ToList();
                foreach (var claim in claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }
            }
            var companies = await _companyRepo.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            var company = await _companyRepo.GetCompanyByIdAsync(id);
            return Ok(company);
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            var createdCompany = await _companyRepo.AddCompanyAsync(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.CompanyId }, createdCompany);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest("Company with that ID not found");
            }
            await _companyRepo.UpdateCompanyAsync(company);
            return Ok("Success");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
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