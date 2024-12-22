using AutoMapper;
using BookMe.Application.Employee.Commands.UpdateEmployee;
using BookMe.Application.Employee.Dto;
using BookMe.Application.Mappings;
using FluentAssertions;
using Xunit;

namespace BookMe.Application.Mappings.Tests
{
    public class EmployeeMappingProfileTests
    {
        private readonly IMapper _mapper;

        public EmployeeMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<EmployeeMappingProfile>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void MappingProfile_ShouldMapEmployeeToEmployeeDto()
        {
            // arrange
            var employee = new Domain.Entities.Employee
            {
                ServiceId = 1,
                Service = new Domain.Entities.Service
                {
                    Name = "Test Service",
                    Id = 1
                },
                User = new Domain.Entities.ApplicationUser
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    AvatarUrl = "https://example.com/avatar.jpg"
                }
            };

            // act
            var result = _mapper.Map<EmployeeDto>(employee);

            // assert
            result.Should().NotBeNull();
            result.ServiceId.Should().Be(employee.ServiceId);
            result.ServiceName.Should().Be(employee.Service.Name);
            result.FirstName.Should().Be(employee.User.FirstName);
            result.LastName.Should().Be(employee.User.LastName);
            result.Email.Should().Be(employee.User.Email);
            result.AvatarUrl.Should().Be(employee.User.AvatarUrl);
            result.FullName.Should().Be("John Doe");
        }

        [Fact]
        public void MappingProfile_ShouldMapEmployeeToUpdateEmployeeCommand()
        {
            // arrange
            var employee = new Domain.Entities.Employee
            {
                ServiceId = 2,
                Service = new Domain.Entities.Service
                {
                    Name = "Another Service",
                    Id = 2
                },
                User = new Domain.Entities.ApplicationUser
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    AvatarUrl = "https://example.com/jane.jpg"
                }
            };

            // act
            var result = _mapper.Map<UpdateEmployeeCommand>(employee);

            // assert
            result.Should().NotBeNull();
            result.ServiceId.Should().Be(employee.ServiceId);
            result.ServiceName.Should().Be(employee.Service.Name);
            result.FirstName.Should().Be(employee.User.FirstName);
            result.LastName.Should().Be(employee.User.LastName);
            result.Email.Should().Be(employee.User.Email);
            result.AvatarUrl.Should().Be(employee.User.AvatarUrl);
            result.FullName.Should().Be("Jane Smith");
        }

        [Fact]
        public void MappingProfile_ShouldMapEmployeeDtoToApplicationUser()
        {
            // arrange
            var employeeDto = new EmployeeDto
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                Gender = Domain.Enums.Gender.Female,
                AvatarUrl = "https://example.com/alice.jpg"
            };

            // act
            var result = _mapper.Map<Domain.Entities.ApplicationUser>(employeeDto);

            // assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be(employeeDto.FirstName);
            result.LastName.Should().Be(employeeDto.LastName);
            result.Email.Should().Be(employeeDto.Email);
            result.UserName.Should().Be(employeeDto.Email);
            result.Gender.Should().Be(employeeDto.Gender);
            result.AvatarUrl.Should().Be(employeeDto.AvatarUrl);
        }

        [Fact]
        public void MappingProfile_ShouldMapEmployeeDtoToEmployee()
        {
            // arrange
            var employeeDto = new EmployeeDto
            {
                ServiceId = 3
            };

            // act
            var result = _mapper.Map<Domain.Entities.Employee>(employeeDto);

            // assert
            result.Should().NotBeNull();
            result.ServiceId.Should().Be(employeeDto.ServiceId);
        }
    }
}
