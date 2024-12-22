using AutoMapper;
using BookMe.Application.Mappings;
using BookMe.Application.OpeningHours.Commands.CreateOpeningHour;
using BookMe.Application.OpeningHours.Commands.UpdateOpeningHour;
using BookMe.Application.OpeningHours.Dto;
using FluentAssertions;
using Xunit;

namespace BookMe.Tests.Mappings
{
    public class OpeningHourMappingProfileTests
    {
        private readonly IMapper _mapper;

        public OpeningHourMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OpeningHourMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void OpeningHourToOpeningHourDto_ShouldMapServiceProperties()
        {
            // Arrange
            var openingHour = new Domain.Entities.OpeningHour
            {
                Id = 1,
                DayOfWeek = "Monday",
                OpeningTime = new TimeSpan(9, 0, 0),
                ClosingTime = new TimeSpan(17, 0, 0),
                Service = new Domain.Entities.Service
                {
                    Name = "Test Service",
                    Id = 2
                }
            };

            // Act
            var result = _mapper.Map<OpeningHourDto>(openingHour);

            // Assert
            result.Should().NotBeNull();
            result.ServiceName.Should().Be(openingHour.Service.Name);
            result.ServiceEncodedName.Should().Be(openingHour.Service.EncodedName);
            result.OpeningTime.Should().Be(openingHour.OpeningTime);
            result.ClosingTime.Should().Be(openingHour.ClosingTime);
        }

        [Fact]
        public void OpeningHourToUpdateOpeningHourCommand_ShouldMapServiceProperties()
        {
            // Arrange
            var openingHour = new Domain.Entities.OpeningHour
            {
                Id = 1,
                DayOfWeek = "Monday",
                OpeningTime = new TimeSpan(9, 0, 0),
                ClosingTime = new TimeSpan(17, 0, 0),
                Service = new Domain.Entities.Service
                {
                    Name = "Test Service",
                    Id = 2
                }
            };

            // Act
            var result = _mapper.Map<UpdateOpeningHourCommand>(openingHour);

            // Assert
            result.Should().NotBeNull();
            result.ServiceName.Should().Be(openingHour.Service.Name);
            result.ServiceEncodedName.Should().Be(openingHour.Service.EncodedName);
            result.OpeningTime.Should().Be(openingHour.OpeningTime);
            result.ClosingTime.Should().Be(openingHour.ClosingTime);
        }

        [Fact]
        public void OpeningHourDtoToCreateOpeningHourCommand_ShouldMapAllProperties()
        {
            // Arrange
            var openingHourDto = new OpeningHourDto
            {
                Id = 1,
                DayOfWeek = "Monday",
                OpeningTime = new TimeSpan(9, 0, 0),
                ClosingTime = new TimeSpan(17, 0, 0),
                ServiceId = 2,
                ServiceName = "Test Service",
                ServiceEncodedName = "test-service"
            };

            // Act
            var result = _mapper.Map<CreateOpeningHourCommand>(openingHourDto);

            // Assert
            result.Should().NotBeNull();
            result.DayOfWeek.Should().Be(openingHourDto.DayOfWeek);
            result.OpeningTime.Should().Be(openingHourDto.OpeningTime);
            result.ClosingTime.Should().Be(openingHourDto.ClosingTime);
            result.ServiceId.Should().Be(openingHourDto.ServiceId);
        }
    }
}
