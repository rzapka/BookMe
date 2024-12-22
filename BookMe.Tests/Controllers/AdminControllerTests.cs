using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using BookMe.Tests.Helpers;

namespace BookMe.Tests.Controllers
{
    public class AdminControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AdminControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private WebApplicationFactory<Program> SetupFactoryWithMockUser(string role)
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Rejestracja autoryzacji z niestandardowym handlerem
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });
                });
            });
        }

        [Fact]
        public async Task Index_ReturnsAdminPanelView_ForAdmin()
        {
            // Arrange
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });

                    services.AddAuthorization(options =>
                    {
                        options.AddPolicy("AdminPolicy", policy =>
                        {
                            policy.RequireAuthenticatedUser();
                            policy.RequireRole("ADMIN");
                        });
                    });
                });
            });

            var client = factory.CreateClient();

            // Symulacja roli `ADMIN`
            client.DefaultRequestHeaders.Add("TestRole", "ADMIN");

            // Act
            var response = await client.GetAsync("/Admin/Panel");

            // Assert
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var debugContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Debug: Response content: {debugContent}");
            }

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Panel Administracyjny")
                   .And.Contain("Zarządzaj Serwisami");
        }



    }
}
