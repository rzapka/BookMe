using AutoMapper;
using BookMe.Application.Booking.Commands.UpdateBooking;
using BookMe.Application.Booking.Dto;
using FluentAssertions;
using Xunit;

namespace BookMe.Application.Mappings.Tests
{
    public class BookingMappingProfileTests
    {
        private readonly IMapper _mapper;

        public BookingMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<BookingMappingProfile>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void MappingProfile_ShouldMapBookingToBookingDto()
        {
            // arrange
            var offer = new Domain.Entities.Offer
            {
                Name = "Test Offer",
                Price = 150.5m,
                Duration = 60,
                Service = new Domain.Entities.Service
                {
                    Name = "Test Service",
                    Id = 1
                }
            };
            offer.Service.EncodeName();

            var booking = new Domain.Entities.Booking
            {
                Offer = offer,
                Employee = new Domain.Entities.Employee
                {
                    User = new Domain.Entities.ApplicationUser
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    }
                },
                Opinion = new Domain.Entities.Opinion
                {
                    Id = 10,
                    Rating = 5,
                    Content = "Great service!"
                }
            };

            // act
            var result = _mapper.Map<BookingDto>(booking);

            // assert
            result.Should().NotBeNull();
            result.OfferName.Should().Be(booking.Offer.Name);
            result.ServiceName.Should().Be(booking.Offer.Service.Name);
            result.ServiceEncodedName.Should().Be(booking.Offer.Service.EncodedName);
            result.ServiceId.Should().Be(booking.Offer.Service.Id);
            result.OfferPrice.Should().Be(booking.Offer.Price);
            result.OfferDuration.Should().Be(booking.Offer.Duration);
            result.EmployeeFullName.Should().Be("John Doe");
            result.OpinionId.Should().Be(booking.Opinion.Id);
            result.OpinionRating.Should().Be(booking.Opinion.Rating);
            result.OpinionContent.Should().Be(booking.Opinion.Content);
        }

        [Fact]
        public void MappingProfile_ShouldMapBookingToUpdateBookingCommand()
        {
            // arrange
            var offer = new Domain.Entities.Offer
            {
                Name = "Test Offer",
                Price = 200.0m,
                Duration = 90,
                Service = new Domain.Entities.Service
                {
                    Name = "Premium Service",
                    Id = 2
                }
            };
            offer.Service.EncodeName();

            var booking = new Domain.Entities.Booking
            {
                Offer = offer,
                Employee = new Domain.Entities.Employee
                {
                    User = new Domain.Entities.ApplicationUser
                    {
                        FirstName = "Jane",
                        LastName = "Smith"
                    }
                },
                Opinion = new Domain.Entities.Opinion
                {
                    Id = 5,
                    Rating = 4,
                    Content = "Good overall"
                }
            };

            // act
            var result = _mapper.Map<UpdateBookingCommand>(booking);

            // assert
            result.Should().NotBeNull();
            result.OfferName.Should().Be(booking.Offer.Name);
            result.OfferEncodedName.Should().Be(booking.Offer.EncodedName);
            result.ServiceName.Should().Be(booking.Offer.Service.Name);
            result.ServiceId.Should().Be(booking.Offer.Service.Id);
            result.OfferPrice.Should().Be(booking.Offer.Price);
            result.OfferDuration.Should().Be(booking.Offer.Duration);
            result.EmployeeFullName.Should().Be("Jane Smith");
            result.OpinionId.Should().Be(booking.Opinion.Id);
            result.OpinionRating.Should().Be(booking.Opinion.Rating);
            result.OpinionContent.Should().Be(booking.Opinion.Content);
        }

        [Fact]
        public void MappingProfile_ShouldMapOfferToBookingDto()
        {
            // arrange
            var service = new Domain.Entities.Service
            {
                Name = "Basic Service",
                Id = 4
            };
            service.EncodeName();

            var offer = new Domain.Entities.Offer
            {
                Id = 3,
                Name = "Basic Offer",
                Price = 100.0m,
                Duration = 30,
                Service = service
            };

            // act
            var result = _mapper.Map<BookingDto>(offer);

            // assert
            result.Should().NotBeNull();
            result.OfferId.Should().Be(offer.Id);
            result.OfferName.Should().Be(offer.Name);
            result.ServiceName.Should().Be(offer.Service.Name);
            result.ServiceId.Should().Be(offer.Service.Id);
            result.OfferPrice.Should().Be(offer.Price);
            result.OfferDuration.Should().Be(offer.Duration);
        }
    }
}
