using AutoMapper;
using BookMe.Application.Mappings;
using BookMe.Application.Opinion.Commands.UpdateOpinion;
using BookMe.Application.Opinion.Dto;
using FluentAssertions;
using Xunit;

namespace BookMe.Tests.Mappings
{
    public class OpinionMappingProfileTests
    {
        private readonly IMapper _mapper;

        public OpinionMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OpinionMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void OpinionToOpinionDto_ShouldMapUserAndOfferProperties()
        {
            // Arrange
            var opinion = new Domain.Entities.Opinion
            {
                Id = 1,
                Rating = 5,
                Content = "Great service!",
                User = new Domain.Entities.ApplicationUser
                {
                    FirstName = "John",
                    LastName = "Doe",
                    AvatarUrl = "http://example.com/avatar.jpg"
                },
                Offer = new Domain.Entities.Offer
                {
                    Name = "Premium Package"
                },
                Employee = new Domain.Entities.Employee
                {
                    User = new Domain.Entities.ApplicationUser
                    {
                        FirstName = "Jane",
                        LastName = "Smith"
                    }
                }
            };

            // Act
            var result = _mapper.Map<OpinionDto>(opinion);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be(opinion.User.FirstName);
            result.LastName.Should().Be(opinion.User.LastName);
            result.AvatarUrl.Should().Be(opinion.User.AvatarUrl);
            result.OfferName.Should().Be(opinion.Offer.Name);
            result.EmployeeFullName.Should().Be($"{opinion.Employee.User.FirstName} {opinion.Employee.User.LastName}");
        }

        [Fact]
        public void OpinionToUpdateOpinionCommand_ShouldMapDetailedProperties()
        {
            // Arrange
            var service = new Domain.Entities.Service
            {
                Name = "Spa Service"
            };
            service.EncodeName(); // Wywołanie metody EncodeName() ustawia wartość EncodedName

            var opinion = new Domain.Entities.Opinion
            {
                Id = 1,
                Rating = 5,
                Content = "Great service!",
                User = new Domain.Entities.ApplicationUser
                {
                    FirstName = "John",
                    LastName = "Doe",
                    AvatarUrl = "http://example.com/avatar.jpg"
                },
                Service = service,
                Offer = new Domain.Entities.Offer
                {
                    Name = "Premium Package",
                    Price = 150m,
                    Duration = 60
                },
                Booking = new Domain.Entities.Booking
                {
                    StartTime = new DateTime(2024, 12, 25, 10, 0, 0)
                },
                Employee = new Domain.Entities.Employee
                {
                    User = new Domain.Entities.ApplicationUser
                    {
                        FirstName = "Jane",
                        LastName = "Smith"
                    }
                }
            };

            // Act
            var result = _mapper.Map<UpdateOpinionCommand>(opinion);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be(opinion.User.FirstName);
            result.LastName.Should().Be(opinion.User.LastName);
            result.AvatarUrl.Should().Be(opinion.User.AvatarUrl);
            result.ServiceName.Should().Be(opinion.Service.Name);
            result.ServiceEncodedName.Should().Be(opinion.Service.EncodedName);
            result.OfferName.Should().Be(opinion.Offer.Name);
            result.OfferPrice.Should().Be(opinion.Offer.Price);
            result.OfferDuration.Should().Be(opinion.Offer.Duration);
            result.BookingStartTime.Should().Be(opinion.Booking.StartTime);
            result.EmployeeFullName.Should().Be($"{opinion.Employee.User.FirstName} {opinion.Employee.User.LastName}");
        }
    }
}
