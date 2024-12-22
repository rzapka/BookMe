using AutoMapper;
using BookMe.Application.Mappings;
using BookMe.Application.Service.Dto;
using BookMe.Domain.Entities;
using FluentAssertions;
using Xunit;
using System.Collections.Generic;

namespace BookMe.Application.Tests.Mappings
{
    public class ServiceMappingProfileTests
    {
        private readonly IMapper _mapper;

        public ServiceMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<OfferMappingProfile>();
                cfg.AddProfile<OpeningHourMappingProfile>();
                cfg.AddProfile<EmployeeMappingProfile>(); // Dodanie zależnego profilu
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ServiceToServiceDto_ShouldMapContactDetailsAndCollections()
        {
            // Arrange
            var service = new Domain.Entities.Service
            {
                Id = 1,
                Name = "Test Service",
                ServiceCategory = new Domain.Entities.ServiceCategory { Name = "Test Category" },
                ContactDetails = new Domain.Entities.ServiceContactDetails
                {
                    City = "Test City",
                    Street = "Test Street",
                    PostalCode = "12345",
                    PhoneNumber = "123-456-789"
                },
                Offers = new List<Domain.Entities.Offer>
                {
                    new Domain.Entities.Offer { Id = 1, Name = "Offer 1" },
                    new Domain.Entities.Offer { Id = 2, Name = "Offer 2" }
                },
                Employees = new List<Domain.Entities.Employee>
                {
                    new Domain.Entities.Employee
                    {
                        Id = 1,
                        User = new Domain.Entities.ApplicationUser
                        {
                            FirstName = "John",
                            LastName = "Doe",
                            Email = "john.doe@example.com"
                        }
                    },
                    new Domain.Entities.Employee
                    {
                        Id = 2,
                        User = new Domain.Entities.ApplicationUser
                        {
                            FirstName = "Jane",
                            LastName = "Smith",
                            Email = "jane.smith@example.com"
                        }
                    }
                },
                OpeningHours = new List<Domain.Entities.OpeningHour>
                {
                    new Domain.Entities.OpeningHour { Id = 1, DayOfWeek = "Poniedziałek" },
                    new Domain.Entities.OpeningHour { Id = 2, DayOfWeek = "Wtorek" }
                }
            };

            // Act
            var result = _mapper.Map<ServiceDto>(service);

            // Assert
            result.Should().NotBeNull();
            result.ServiceCategoryName.Should().Be(service.ServiceCategory.Name);
            result.City.Should().Be(service.ContactDetails.City);
            result.Street.Should().Be(service.ContactDetails.Street);
            result.PostalCode.Should().Be(service.ContactDetails.PostalCode);
            result.PhoneNumber.Should().Be(service.ContactDetails.PhoneNumber);

            result.Offers.Should().HaveCount(2);
            result.Employees.Should().HaveCount(2);
            result.OpeningHours.Should().HaveCount(2);

            result.Employees[0].FirstName.Should().Be("John");
            result.Employees[1].FirstName.Should().Be("Jane");
        }

        [Fact]
        public void ServiceToServiceDto_ShouldHandleNullServiceCategory()
        {
            // Arrange
            var service = new Domain.Entities.Service
            {
                Id = 1,
                Name = "Test Service",
                ServiceCategory = null,
                ContactDetails = new Domain.Entities.ServiceContactDetails
                {
                    City = "Test City",
                    Street = "Test Street",
                    PostalCode = "12345",
                    PhoneNumber = "123-456-789"
                }
            };

            // Act
            var result = _mapper.Map<ServiceDto>(service);

            // Assert
            result.Should().NotBeNull();
            result.ServiceCategoryName.Should().BeNull();
        }

        [Fact]
        public void ServiceDtoToService_ShouldMapContactDetailsAndIgnoreProperties()
        {
            // Arrange
            var serviceDto = new ServiceDto
            {
                Id = 1,
                Name = "Test Service",
                City = "Test City",
                Street = "Test Street",
                PostalCode = "12345",
                PhoneNumber = "123-456-789"
            };

            // Act
            var result = _mapper.Map<Domain.Entities.Service>(serviceDto);

            // Assert
            result.Should().NotBeNull();
            result.ContactDetails.Should().NotBeNull();
            result.ContactDetails.City.Should().Be(serviceDto.City);
            result.ContactDetails.Street.Should().Be(serviceDto.Street);
            result.ContactDetails.PostalCode.Should().Be(serviceDto.PostalCode);
            result.ContactDetails.PhoneNumber.Should().Be(serviceDto.PhoneNumber);

            result.ServiceCategory.Should().BeNull();
            result.EncodedName.Should().BeEmpty();
        }
    }
}
