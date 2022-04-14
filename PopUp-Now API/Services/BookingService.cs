using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PopUp_Now_API.Database;
using PopUp_Now_API.Exceptions;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Services
{
    public class BookingService : IBookingService
    {
        private readonly DataContext _dataContext;
        private readonly IPropertiesService _propertiesService;

        public BookingService(DataContext dataContext, IPropertiesService propertiesService)
        {
            _dataContext = dataContext;
            _propertiesService = propertiesService;
        }

        public async Task Create(Booking booking)
        {
            await _dataContext.Bookings.AddAsync(booking);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Booking> BookProperty(IdentityUser user, BookingRequest bookingRequest)
        {
            /* Find the property */
            var property = await _propertiesService.Get(bookingRequest.PropertyId);

            if (property is null)
            {
                throw new PropertiesException("Property not found");
            }

            /* Check if we can book it*/
            if (await IsBooked(property.Id, bookingRequest.StartDate, bookingRequest.EndDate))
            {
                throw new PropertiesException($"Property is booked in: {bookingRequest.StartDate}");
            }

            /* If this point is reached, it means we can create a booking */
            var booking = new Booking()
            {
                User = user,
                Property = property,
                StartDate = bookingRequest.StartDate,
                EndDate = bookingRequest.EndDate,
                Reasoning = bookingRequest.Reasoning,
                SpecialRequests = bookingRequest.SpecialRequests
            };

            /* Create object in DB */
            await Create(booking);

            return booking;
        }

        private async Task<bool> IsBooked(int propertyId, DateTime bookingRequestStartDate,
            DateTime bookingRequestEndDate)
        {
            var bookings = await _dataContext.Bookings
                .Where(booking => booking.Property.Id.Equals(propertyId))
                .Where(booking =>
                    bookingRequestStartDate >= booking.StartDate && bookingRequestEndDate <= booking.EndDate).ToListAsync();
            return bookings.Any();
        }

        public async Task<Booking> Get(int bookingId)
        {
            var result = await _dataContext.Bookings.FindAsync(bookingId);
            if (result is null)
            {
                throw new Exception($"No booking found with ID = {bookingId}");
            }

            return result;
        }

        public async Task ConfirmBooking(int bookingId)
        {
            var booking = await Get(bookingId);
            booking.Confirmed = true;
        }

        public Task<List<Booking>> GetAll(IdentityUser user)
        {
            return _dataContext.Bookings
                .Where(booking => booking.User.Id.Equals(user.Id))
                .Include(booking => booking.Property)
                .ToListAsync();
        }

        public Task<List<Booking>> GetBookingRequests(IdentityUser user)
        {
            return _dataContext.Bookings
                .Where(booking => booking.Property.User.Id.Equals(user.Id))
                .Include(booking => booking.Property)
                .ToListAsync();
        }
    }
}