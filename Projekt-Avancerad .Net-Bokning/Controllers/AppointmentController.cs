﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projekt_Avancerad_.Net_Bokning.DTO;
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
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAllAppointments()
        {
            var appointments = await _appointmentRepo.GetAllAsync();
            var appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                Id = a.id,
                AppointDiscription = a.AppointDiscription,
                PlacedApp = a.PlacedApp,
                CustomerId = a.CustomerId,
                CompanyId = a.CompanyId
            }).ToList();
            return Ok(appointmentDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDTO>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepo.GetAppointmentAsync(id);
            if (appointment == null)
            {
                return NotFound("Id Was Not Found");
            }

            var appointmentDto = new AppointmentDTO
            {
                Id = appointment.id,
                AppointDiscription = appointment.AppointDiscription,
                PlacedApp = appointment.PlacedApp,
                CustomerId = appointment.CustomerId,
                CompanyId = appointment.CompanyId
            };
            return Ok(appointmentDto);
        }

        [HttpGet("day/{date}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentByDay(DateTime date)
        {
            var appointments = await _appointmentRepo.GetAppointmentDayAsync(date);
            var appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                Id = a.id,
                AppointDiscription = a.AppointDiscription,
                PlacedApp = a.PlacedApp,
                CustomerId = a.CustomerId,
                CompanyId = a.CompanyId
            }).ToList();
            return Ok(appointmentDtos);
        }

        [HttpGet("month/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentByMonth(int year, int month)
        {
            var appointments = await _appointmentRepo.GetAppointmentMonthAsync(year, month);
            var appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                Id = a.id,
                AppointDiscription = a.AppointDiscription,
                PlacedApp = a.PlacedApp,
                CustomerId = a.CustomerId,
                CompanyId = a.CompanyId
            }).ToList();
            return Ok(appointmentDtos);
        }

        [HttpGet("week/{year}/{week}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentByWeek(int year, int week)
        {
            var appointments = await _appointmentRepo.GetAppointmentWeekAsync(year, week);
            var appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                Id = a.id,
                AppointDiscription = a.AppointDiscription,
                PlacedApp = a.PlacedApp,
                CustomerId = a.CustomerId,
                CompanyId = a.CompanyId
            }).ToList();
            return Ok(appointmentDtos);
        }

        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<BookingHistory>>> GetBookingHistory(int id)
        {
            var history = await _appointmentRepo.GetBookingHistoryAsync(id);
            return Ok(history);
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDTO>> AddAppointment(AppointmentDTO appointmentDto)
        {
            var appointment = new Appointment
            {
                AppointDiscription = appointmentDto.AppointDiscription,
                PlacedApp = appointmentDto.PlacedApp,
                CustomerId = appointmentDto.CustomerId,
                CompanyId = appointmentDto.CompanyId
            };

            var createdAppointment = await _appointmentRepo.AddAppointmentAsync(appointment);

            appointmentDto.Id = createdAppointment.id;

            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentDto.Id }, appointmentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentDTO appointmentDto)
        {
            if (id != appointmentDto.Id)
            {
                return BadRequest();
            }

            var appointment = new Appointment
            {
                id = appointmentDto.Id,
                AppointDiscription = appointmentDto.AppointDiscription,
                PlacedApp = appointmentDto.PlacedApp,
                CustomerId = appointmentDto.CustomerId,
                CompanyId = appointmentDto.CompanyId
            };

            await _appointmentRepo.UpdateAppointmentAsync(appointment);

            return Ok(appointmentDto);
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
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentsSortedAndFiltered(string sortField, string sortOrder, string filterField, string filterValue)
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

            var appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                Id = a.id,
                AppointDiscription = a.AppointDiscription,
                PlacedApp = a.PlacedApp,
                CustomerId = a.CustomerId,
                CompanyId = a.CompanyId
            }).ToList();

            return Ok(appointmentDtos);
        }
    }
}