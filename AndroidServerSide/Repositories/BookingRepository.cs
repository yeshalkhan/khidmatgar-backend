using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndroidServerSide.DbContexts;
using AndroidServerSide.Interfaces;
using AndroidServerSide.Models;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace AndroidServerSide.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserEmailAsync(string userEmail)
        {
            return await _context.Bookings
                .Where(b => b.CustomerEmail == userEmail)
                .OrderByDescending(b => b.Id) 
                .ToListAsync();
        }


        public async Task<IEnumerable<Booking>> GetBookingsByServiceProviderIdAsync(int serviceProviderId)
        {
            return await _context.Bookings
                .Where(b => b.ServiceProviderId == serviceProviderId)
                .ToListAsync();
        }

        public async Task<int?> AddBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            // After saving, the Id will be set in the booking object
            return booking.Id;
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            var existingBooking = await _context.Bookings.FindAsync(booking.Id);
            if (existingBooking != null)
            {
                //_context.Entry(existingBooking).State = EntityState.Detached;
                //_context.Bookings.Attach(booking);
                //_context.Entry(booking).State = EntityState.Modified;
                _context.Entry(existingBooking).CurrentValues.SetValues(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Booking?> DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return booking;
        }
    }
}
