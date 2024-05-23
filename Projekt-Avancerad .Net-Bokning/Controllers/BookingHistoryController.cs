using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt_Avancerad_.Net_Bokning.DTO;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin,Customer,Company")]
[ApiController]
[Route("api/[controller]")]
public class BookingHistoryController : ControllerBase
{
    private readonly IBookingHistory _bookingHistoryRepo;

    public BookingHistoryController(IBookingHistory bookingHistoryRepo)
    {
        _bookingHistoryRepo = bookingHistoryRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingHistoryDTO>>> GetAllBookingHistories()
    {
        var bookingHistories = await _bookingHistoryRepo.GetAllBookingHistoriesAsync();
        var bookingHistoryDtos = bookingHistories.Select(bh => new BookingHistoryDTO
        {
            Id = bh.Id,
            AppointmentId = bh.AppointmentId,
            ChangedAt = bh.ChangedAt,
            ChangeType = bh.ChangeType,
            ChangedBy = bh.ChangedBy
        }).ToList();

        return Ok(bookingHistoryDtos);
    }

    [HttpGet("appointment/{appointmentId}")]
    public async Task<ActionResult<IEnumerable<BookingHistoryDTO>>> GetBookingHistoriesByAppointment(int appointmentId)
    {
        var bookingHistories = await _bookingHistoryRepo.GetBookingHistoriesByAppointmentIdAsync(appointmentId);
        var bookingHistoryDtos = bookingHistories.Select(bh => new BookingHistoryDTO
        {
            Id = bh.Id,
            AppointmentId = bh.AppointmentId,
            ChangedAt = bh.ChangedAt,
            ChangeType = bh.ChangeType,
            ChangedBy = bh.ChangedBy
        }).ToList();

        return Ok(bookingHistoryDtos);
    }
}