using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PopUp_Now_API.Database;
using PopUp_Now_API.Exceptions;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;
using SendGrid.Helpers.Errors.Model;

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

        /**
         * Create a booking
         */
        private async Task Create(Booking booking)
        {
            await _dataContext.Bookings.AddAsync(booking);
            await _dataContext.SaveChangesAsync();
        }

        /**
         * Book a property
         */
        public async Task<Booking> BookProperty(User user, BookingRequest bookingRequest)
        {
            /* Find the property */
            var property = await _propertiesService.Get(bookingRequest.PropertyId);

            if (property is null)
            {
                throw new PopUpNowException("Property not found");
            }

            /* Check if we can book it*/
            if (await IsBooked(property.Id, bookingRequest.StartDate, bookingRequest.EndDate))
            {
                throw new PopUpNowException($"Property is booked in: {bookingRequest.StartDate.ToLocalTime()}");
            }

            /* If this point is reached, it means we can create a booking */
            var booking = new Booking
            {
                User = user,
                Property = property,
                StartDate = bookingRequest.StartDate,
                EndDate = bookingRequest.EndDate,
                Reasoning = bookingRequest.Reasoning,
                SpecialRequests = bookingRequest.SpecialRequests,
                BookingStatus = BookingStatus.Pending
            };

            /* Create object in DB */
            await Create(booking);

            return booking;
        }

        /**
         * Check if a property is booked on given dates
         */
        private async Task<bool> IsBooked(int propertyId, DateTime bookingRequestStartDate,
            DateTime bookingRequestEndDate)
        {
            var bookings = await _dataContext.Bookings
                .Where(booking => booking.Property.Id.Equals(propertyId))
                .Where(booking => booking.BookingStatus != BookingStatus.Declined)
                .Where(booking =>
                    ((bookingRequestStartDate >= booking.StartDate) && (bookingRequestStartDate <= booking.EndDate))
                    || ((bookingRequestEndDate >= booking.StartDate) && (bookingRequestEndDate <= booking.EndDate)))
                .ToListAsync();
            return bookings.Any();
        }

        /**
         * Get a booking by id
         */
        private async Task<Booking> Get(int bookingId)
        {
            var result = await _dataContext.Bookings.FindAsync(bookingId);
            if (result is null)
            {
                throw new PopUpNowException($"No booking found with ID = {bookingId}");
            }

            return result;
        }

        /**
         * Confirm a booking
         */
        public async Task ConfirmBooking(int bookingId, BookingStatus status)
        {
            var booking = await Get(bookingId);
            booking.BookingStatus = status;
            await _dataContext.SaveChangesAsync();
        }

        /**
         * Get all the bookings for a give user
         */
        public async Task<List<Booking>> GetAll(User user)
        {
            var bookings = await _dataContext.Bookings
                .Where(booking => booking.User.Id.Equals(user.Id))
                .Include(booking => booking.Property)
                .ToListAsync();
            //Check if we have any booking in the result list
            if (bookings.Any())
            {
                return bookings;
            }

            //Booking list is empty, so throw an error
            throw new NotFoundException("No booking was found");
        }

        /**
         * Get all the bookings for a landlord
         */
        public async Task<List<Booking>> GetBookingRequests(User user)
        {
            var result = await _dataContext.Bookings
                .Include(booking => booking.Property.User)
                .Include(booking => booking.User)
                .Where(booking => booking.Property.User.Id.Equals(user.Id))
                .ToListAsync();
            if (result.Any())
            {
                return result;
            }

            throw new NotFoundException("No booking requests found for user " + user.Name);
        }
    }
}