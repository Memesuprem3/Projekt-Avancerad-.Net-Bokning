using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;

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
    public async Task<ActionResult<IEnumerable<BookingHistory>>> GetAllBookingHistories()
    {
        var bookingHistories = await _bookingHistoryRepo.GetAllBookingHistoriesAsync();
        return Ok(bookingHistories);
    }

    [HttpGet("appointment/{appointmentId}")]
    public async Task<ActionResult<IEnumerable<BookingHistory>>> GetBookingHistoriesByAppointment(int appointmentId)
    {
        var bookingHistories = await _bookingHistoryRepo.GetBookingHistoriesByAppointmentIdAsync(appointmentId);
        return Ok(bookingHistories);
    }
}