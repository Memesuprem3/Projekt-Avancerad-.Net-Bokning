using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Controllers
{
    [Authorize(Roles = "Admin,Company,Customer")]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointment _appointmentRepo;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(IAppointment appointmentRepo, ILogger<AppointmentController> logger)
        {
            _appointmentRepo = appointmentRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointments()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims.ToList();
                foreach (var claim in claims)
                {
                    _logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }
            }

            var appointments = await _appointmentRepo.GetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
        {
            
                var appointment = await _appointmentRepo.GetAppointmentAsync(id);
            if (appointment == null)
            {
                return NotFound("Id Was Not Found");
            }
            return Ok(appointment);
        }

        [HttpGet("day/{date}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentByDay(DateTime date)
        {
            var appointments = await _appointmentRepo.GetAppointmentDayAsync(date);
            return Ok(appointments);
        }

        [HttpGet("month/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentByMonth(int year, int month)
        {
            var appointments = await _appointmentRepo.GetAppointmentMonthAsync(year, month);
            return Ok(appointments);
        }

        [HttpGet("week/{year}/{week}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentByWeek(int year, int week)
        {
            var appointments = await _appointmentRepo.GetAppointmentWeekAsync(year, week);
            return Ok(appointments);
        }

        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<BookingHistory>>> GetBookingHistory(int id)
        {
            var history = await _appointmentRepo.GetBookingHistoryAsync(id);
            return Ok(history);
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> AddAppointment(Appointment appointment)
        {
            var createdAppointment = await _appointmentRepo.AddAppointmentAsync(appointment);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.id }, createdAppointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
        {
            if (id != appointment.id)
            {
                return BadRequest();
            }
            await _appointmentRepo.UpdateAppointmentAsync(appointment);
            return Ok(appointment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentRepo.GetAppointmentAsync(id);
            if (appointment == null)
            {
                return NotFound("Appointment with that ID not found");
            }
            await _appointmentRepo.DeleteAppointmentAsync(appointment);
            return Ok("Appointment Deleted");
        }

        [HttpGet("sorted-filtered")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsSortedAndFiltered(
            string sortField, string sortOrder, string filterField, string filterValue)
        {
            var appointments = await _appointmentRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                switch (filterField.ToLower())
                {
                    case "appointdiscription":
                        appointments = appointments.Where(a => a.AppointDiscription.Contains(filterValue, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "placedapp":
                        if (DateTime.TryParse(filterValue, out DateTime placedApp))
                        {
                            appointments = appointments.Where(a => a.PlacedApp == placedApp);
                        }
                        break;
                    case "customerid":
                        if (int.TryParse(filterValue, out int customerId))
                        {
                            appointments = appointments.Where(a => a.CustomerId == customerId);
                        }
                        break;
                    case "companyid":
                        if (int.TryParse(filterValue, out int companyId))
                        {
                            appointments = appointments.Where(a => a.CompanyId == companyId);
                        }
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField.ToLower())
                {
                    case "appointdiscription":
                        appointments = sortOrder.ToLower() == "desc" ?
                            appointments.OrderByDescending(a => a.AppointDiscription) :
                            appointments.OrderBy(a => a.AppointDiscription);
                        break;
                    case "placedapp":
                        appointments = sortOrder.ToLower() == "desc" ?
                            appointments.OrderByDescending(a => a.PlacedApp) :
                            appointments.OrderBy(a => a.PlacedApp);
                        break;
                    case "customerid":
                        appointments = sortOrder.ToLower() == "desc" ?
                            appointments.OrderByDescending(a => a.CustomerId) :
                            appointments.OrderBy(a => a.CustomerId);
                        break;
                    case "companyid":
                        appointments = sortOrder.ToLower() == "desc" ?
                            appointments.OrderByDescending(a => a.CompanyId) :
                            appointments.OrderBy(a => a.CompanyId);
                        break;
                    default:
                        break;
                }
            }

            return Ok(appointments);
        }
    }
}