using AutoMapper;
using BookMe.Application.Mappings;
using BookMe.Application.Offer.Commands.CreateOffer;
using BookMe.Application.Offer.Commands.UpdateOffer;
using BookMe.Application.Offer.Dto;
using FluentAssertions;
using Xunit;

namespace BookMe.Tests.Mappings
{
    public class OfferMappingProfileTests
    {
        private readonly IMapper _mapper;

        public OfferMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OfferMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void OfferToOfferDto_ShouldMapServiceProperties()
        {
            // Arrange
            var offer = new Domain.Entities.Offer
            {
                Id = 1,
                Name = "Test Offer",
                Service = new Domain.Entities.Service
                {
                    Name = "Test Service",
                    Id = 2
                }
            };

            // Act
            var result = _mapper.Map<OfferDto>(offer);

            // Assert
            result.Should().NotBeNull();
            result.ServiceName.Should().Be(offer.Service.Name);
            result.ServiceEncodedName.Should().Be(offer.Service.EncodedName);
        }

        [Fact]
        public void OfferToCreateOfferCommand_ShouldMapServiceEncodedName()
        {
            // Arrange
            var offer = new Domain.Entities.Offer
            {
                Id = 1,
                Name = "Test Offer",
                Service = new Domain.Entities.Service
                {
                    Name = "Test Service",
                    Id = 2
                }
            };

            // Act
            var result = _mapper.Map<CreateOfferCommand>(offer);

            // Assert
            result.Should().NotBeNull();
            result.ServiceEncodedName.Should().Be(offer.Service.EncodedName);
        }

        [Fact]
        public void OfferToUpdateOfferCommand_ShouldMapServiceEncodedName()
        {
            // Arrange
            var offer = new Domain.Entities.Offer
            {
                Id = 1,
                Name = "Test Offer",
                Service = new Domain.Entities.Service
                {
                    Name = "Test Service",
                    Id = 2
                }
            };

            // Act
            var result = _mapper.Map<UpdateOfferCommand>(offer);

            // Assert
            result.Should().NotBeNull();
            result.ServiceEncodedName.Should().Be(offer.Service.EncodedName);
        }
    }
}
