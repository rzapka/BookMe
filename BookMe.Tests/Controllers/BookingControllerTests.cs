using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using System.Net;
using Moq;
using MediatR;
using BookMe.Application.Booking.Queries.ListBookings;
using BookMe.Application.Booking.Queries.GetBookingDetails;
using BookMe.Application.Booking.Dto;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using BookMe.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using BookMe.Application.ApplicationUser;
using BookMe.Tests.Helpers;

namespace BookMe.Tests.Controllers
{
    public class BookingControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BookingControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Index_ReturnsViewWithBookings_WhenUserIsAuthenticated()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { Id = 1 },
                new Booking { Id = 2 }
            };

            var detailedBookings = new List<BookingDto>
            {
                new BookingDto { Id = 1, ServiceName = "Service 1", OfferName = "Offer 1", StartTime = DateTime.Now.AddHours(1) },
                new BookingDto { Id = 2, ServiceName = "Service 2", OfferName = "Offer 2", StartTime = DateTime.Now.AddHours(2) }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ListBookingsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);

            mediatorMock
                .Setup(m => m.Send(It.Is<GetBookingDetailsQuery>(q => q.BookingId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(detailedBookings[0]);

            mediatorMock
                .Setup(m => m.Send(It.Is<GetBookingDetailsQuery>(q => q.BookingId == 2), It.IsAny<CancellationToken>()))
                .ReturnsAsync(detailedBookings[1]);

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(x => x.GetCurrentUserAsync())
                .ReturnsAsync(new CurrentUser("user123", "test@example.com", "Test", "User", false));

            userContextMock.Setup(x => x.IsEmployeeAsync())
                .ReturnsAsync(false);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(_ => mediatorMock.Object);
                    services.AddScoped(_ => userContextMock.Object);
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });
                });
            }).CreateClient();

            client.DefaultRequestHeaders.Add("TestRole", "USER");

            // Act
            var response = await client.GetAsync("/Wizyty");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("Moje Wizyty")
                   .And.Contain("Service 1")
                   .And.Contain("Offer 1")
                   .And.Contain("Service 2")
                   .And.Contain("Offer 2");
        }
    }
}
