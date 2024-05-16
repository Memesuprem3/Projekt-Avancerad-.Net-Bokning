using Microsoft.AspNetCore.Mvc;
using Projekt_Avancerad_.Net_Bokning.Services;
using Projekt_Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointment _appointmentRepo;

        public AppointmentController(IAppointment appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepo.GetAppointmentAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpGet("day/{date}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByDay(DateTime date)
        {
            var appointments = await _appointmentRepo.GetAppointmentDayAsync(date);
            return Ok(appointments);
        }

        [HttpGet("month/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByMonth(int year, int month)
        {
            var appointments = await _appointmentRepo.GetAppointmentMonthAsync(year, month);
            return Ok(appointments);
        }

        [HttpGet("week/{year}/{week}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByWeek(int year, int week)
        {
            var appointments = await _appointmentRepo.GetAppointmentWeekAsync(year, week);
            return Ok(appointments);
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
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentRepo.GetAppointmentAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            await _appointmentRepo.DeleteAppointmentAsync(appointment);
            return NoContent();
        }
    }
}