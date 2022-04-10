using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task<List<Booking>> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Create(Booking booking)
        {
            booking.Property.AddBooking(booking);
            _dataContext.SaveChanges();
        }

        public async Task<Booking> BookProperty(BookingRequest bookingRequest)
        {
            /* Find the property */
            var property = await _propertiesService.Get(bookingRequest.PropertyId);

            if (property is null)
            {
                throw new PropertiesException("Property not found");
            }

            /* Check if we can book it*/
            if (property.IsBooked(bookingRequest.StartDate, bookingRequest.EndDate))
            {
                throw new PropertiesException($"Property is booked in: {bookingRequest.StartDate}");
            }

            /* If this point is reached, it means we can create a booking */
            var booking = new Booking()
            {
                //TODO get the user from request
                User = new User(),
                Property = property,
                StartDate = bookingRequest.StartDate,
                EndDate = bookingRequest.EndDate,
                Reasoning = bookingRequest.Reasoning,
                SpecialRequests = bookingRequest.SpecialRequests
            };

            /* Create object in DB */
            Create(booking);

            return booking;
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
    }
}