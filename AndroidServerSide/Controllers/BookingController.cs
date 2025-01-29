using AndroidServerSide.Interfaces;
using AndroidServerSide.Migrations;
using AndroidServerSide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace AndroidServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMemoryCache _memoryCache;

        public BookingController(IBookingRepository bookingRepository, IMemoryCache memoryCache)
        {
            _bookingRepository = bookingRepository;
            _memoryCache = memoryCache;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetAllBookings()
        {
            string cacheKey = "AllBookings";
            if (!_memoryCache.TryGetValue(cacheKey, out var bookingsFromCache))
            {
                var bookings = await _bookingRepository.GetAllBookingsAsync();
                var bookingDTOs = new List<BookingDTO?>();
                foreach (var booking in bookings)
                {
                    BookingDTO? bookingDTO = null;
                    ConvertBookingToBookingDTO(booking, ref bookingDTO);
                    bookingDTOs.Add(bookingDTO);
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(cacheKey, bookingDTOs, cacheEntryOptions);
                return Ok(bookingDTOs);
            }
            return Ok(bookingsFromCache);
        }

        // GET: api/Booking/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBookingById(int id)
        {
            string cacheKey = "BookingId" + id;
            if (!_memoryCache.TryGetValue(cacheKey, out var bookingFromCache))
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(id);
                if (booking == null)
                    return NotFound();

                BookingDTO? bookingDTO = null;
                ConvertBookingToBookingDTO(booking, ref bookingDTO);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(cacheKey, bookingDTO, cacheEntryOptions);
                return Ok(bookingDTO);
            }
            return Ok(bookingFromCache);    
        }

        // GET: api/Booking/User/{userEmail}
        [HttpGet("User/{userEmail}")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookingsByUserEmail(string userEmail)
        {
            string cacheKey = "BookingUser" + userEmail;
            if (!_memoryCache.TryGetValue(cacheKey, out var bookingsFromCache))
            {
                var bookings = await _bookingRepository.GetBookingsByUserEmailAsync(userEmail);
                var bookingDTOs = new List<BookingDTO?>();
                foreach (var booking in bookings)
                {
                    BookingDTO? bookingDTO = null;
                    ConvertBookingToBookingDTO(booking, ref bookingDTO);
                    bookingDTOs.Add(bookingDTO);
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(cacheKey, bookingDTOs, cacheEntryOptions);
                return Ok(bookingDTOs);
            }
            return Ok(bookingsFromCache);
        }

        // GET: api/Booking/ServiceProvider/{serviceProviderId}
        [HttpGet("ServiceProvider/{serviceProviderId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByServiceProviderId(int serviceProviderId)
        {
            string cacheKey = "BookingProvider" + serviceProviderId;
            if (!_memoryCache.TryGetValue(cacheKey, out var bookingsFromCache))
            {
                var bookings = await _bookingRepository.GetBookingsByServiceProviderIdAsync(serviceProviderId);
                var bookingDTOs = new List<BookingDTO?>();
                foreach (var booking in bookings)
                {
                    BookingDTO? bookingDTO = null;
                    ConvertBookingToBookingDTO(booking, ref bookingDTO);
                    bookingDTOs.Add(bookingDTO);
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(cacheKey, bookingDTOs, cacheEntryOptions);
                return Ok(bookingDTOs);
            }
            return Ok(bookingsFromCache);
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<ActionResult> CreateBooking(BookingDTO bookingDTO)
        {
            Booking? booking = null;
            ConvertBookingDTOToBooking(bookingDTO, ref booking);
            int? bookingId = await _bookingRepository.AddBookingAsync(booking);

            _memoryCache.Remove("AllBookings");
            _memoryCache.Remove("BookingUser" + booking.CustomerEmail);
            _memoryCache.Remove("BookingProvider" + booking.ServiceProviderId);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        // DELETE: api/Booking/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            var booking = await _bookingRepository.DeleteBookingAsync(id);

            if (booking != null)
            {
                _memoryCache.Remove("AllBookings");
                _memoryCache.Remove("BookingId" + booking.Id);
                _memoryCache.Remove("BookingUser" + booking.CustomerEmail);
                _memoryCache.Remove("BookingProvider" + booking.ServiceProviderId);
            }
            return NoContent();
        }

        private void ConvertBookingDTOToBooking(BookingDTO bookingDTO, ref Booking? booking)
        {
            booking = new Booking
            {
                Id = bookingDTO.Id,
                ServiceProviderId = bookingDTO.ServiceProviderId,
                CustomerEmail = bookingDTO.CustomerEmail,
                ServiceName = bookingDTO.ServiceName,
                Price = bookingDTO.Price ?? 0.0,
                noOfHours = bookingDTO.noOfHours,
                CreatedAt = DateTime.Parse(bookingDTO.CreatedAt),
                DeliveryAt = DateTime.Parse(bookingDTO.DeliveryAt),
                Location = bookingDTO.Location,
                PaymentOption = bookingDTO.PaymentOption ?? "COD",
                Status = bookingDTO.Status
            };
        }

        private void ConvertBookingToBookingDTO(Booking booking, ref BookingDTO? bookingDTO)
        {
            bookingDTO = new BookingDTO
            {
                Id = booking.Id,
                ServiceProviderId = booking.ServiceProviderId,
                CustomerEmail = booking.CustomerEmail,
                ServiceName = booking.ServiceName,
                Price = booking.Price,
                noOfHours = booking.noOfHours,
                CreatedAt = booking.CreatedAt.ToString("dd MMM yyyy, dddd hh:mm tt"),
                DeliveryAt = booking.DeliveryAt.ToString("dd MMM yyyy, dddd hh:mm tt"),
                Location = booking.Location,
                PaymentOption = booking.PaymentOption,
                Status = booking.Status
            };
        }

        // PUT: api/Booking/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBooking(int id, BookingDTO bookingDTO)
        {
            if (id != bookingDTO.Id)
                return BadRequest();

            Booking? booking = null;
            ConvertBookingDTOToBooking(bookingDTO, ref booking);
            await _bookingRepository.UpdateBookingAsync(booking);

            _memoryCache.Remove("AllBookings");
            _memoryCache.Remove("BookingId" + booking.Id);
            _memoryCache.Remove("BookingUser" + booking.CustomerEmail);
            _memoryCache.Remove("BookingProvider" + booking.ServiceProviderId);
            return NoContent();
        }
    }
}
