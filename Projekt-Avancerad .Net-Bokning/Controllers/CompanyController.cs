using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projekt_Avancerad_.Net_Bokning.DTO;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Controllers
{
    [Authorize(Roles = "Admin,Company")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompany _companyRepo;
        private readonly IAppointment _appointmentRepo;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompany companyRepo, IAppointment appointmentRepo, ILogger<CompanyController> logger)
        {
            _companyRepo = companyRepo;
            _appointmentRepo = appointmentRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetAllCompanies()
        {
            var companies = await _companyRepo.GetAllCompaniesAsync();
            var companyDtos = companies.Select(c => new CompanyDTO
            {
                CompanyId = c.CompanyId,
                CompanyName = c.CompanyName
            }).ToList();
            return Ok(companyDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDTO>> GetCompanyById(int id)
        {
            var company = await _companyRepo.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound("Company With That ID Not Found");
            }

            var companyDto = new CompanyDTO
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName
            };

            return Ok(companyDto);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDTO>> AddCompany(CompanyDTO companyDto)
        {
            var company = new Company
            {
                CompanyName = companyDto.CompanyName
            };

            var createdCompany = await _companyRepo.AddCompanyAsync(company);
            var createdCompanyDto = new CompanyDTO
            {
                CompanyId = createdCompany.CompanyId,
                CompanyName = createdCompany.CompanyName
            };

            return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompanyDto.CompanyId }, createdCompanyDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyDTO companyDto)
        {
            if (id != companyDto.CompanyId)
            {
                return BadRequest("Company with that ID not found");
            }

            var company = new Company
            {
                CompanyId = companyDto.CompanyId,
                CompanyName = companyDto.CompanyName
            };

            await _companyRepo.UpdateCompanyAsync(company);
            return Ok("Update Success");
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
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentsByMonth(int id, int year, int month)
        {
            var appointments = await _appointmentRepo.GetAppointmentMonthAsync(year, month);

            if (appointments != null)
            {
                return NotFound("No Appointment Found That Week");
            }

            var companyAppointments = appointments.Where(a => a.CompanyId == id)
                                                  .Select(a => new AppointmentDTO
                                                  {
                                                      Id = a.id,
                                                      AppointDiscription = a.AppointDiscription,
                                                      PlacedApp = a.PlacedApp,
                                                      CustomerId = a.CustomerId,
                                                      CompanyId = a.CompanyId
                                                  }).ToList();

            return Ok(companyAppointments);
        }

        [HttpGet("{id}/appointments/week/{year}/{week}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentsByWeek(int id, int year, int week)
        {
            var appointments = await _appointmentRepo.GetAppointmentWeekAsync(year, week);
            var companyAppointments = appointments.Where(a => a.CompanyId == id)
                                                  .Select(a => new AppointmentDTO
                                                  {
                                                      Id = a.id,
                                                      AppointDiscription = a.AppointDiscription,
                                                      PlacedApp = a.PlacedApp,
                                                      CustomerId = a.CustomerId,
                                                      CompanyId = a.CompanyId
                                                  }).ToList();

            return Ok(companyAppointments);
        }
    }
}