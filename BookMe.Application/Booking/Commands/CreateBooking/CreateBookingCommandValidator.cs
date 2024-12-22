using BookMe.Application.Booking.Commands.CreateBooking;
using BookMe.Domain.Interfaces;
using FluentValidation;
using System.Globalization;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IServiceRepository _serviceRepository;

    public CreateBookingCommandValidator(IBookingRepository bookingRepository, IServiceRepository serviceRepository)
    {
        _bookingRepository = bookingRepository;
        _serviceRepository = serviceRepository;

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Data rozpoczęcia jest wymagana.")
            .Must(BeAValidDate).WithMessage("Nieprawidłowy format daty.")
            .Must(NotBeInThePast).WithMessage("Data rozpoczęcia nie może być w przeszłości.")
            .Must(BeAtLeastOneHourLater).WithMessage("Data rozpoczęcia musi być co najmniej godzinę później niż czas obecny.");

        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("ID pracownika jest wymagane.");

        RuleFor(x => x).CustomAsync(async (command, context, cancellationToken) =>
        {
            var userBookings = await _bookingRepository.GetBookingsByUserId(command.UserId);
            if (userBookings.Any(b => b.StartTime < command.EndTime && b.EndTime > command.StartTime))
            {
                context.AddFailure("StartTime", "Masz już umówioną wizytę w wybranym przedziale czasowym.");
            }

            if (command.EmployeeId > 0)
            {
                var employeeBookings = await _bookingRepository.GetBookingsByEmployeeId(command.EmployeeId);
                if (employeeBookings.Any(b => b.StartTime < command.EndTime && b.EndTime > command.StartTime))
                {
                    context.AddFailure("StartTime", "Wybrany pracownik jest zajęty w wybranym przedziale czasowym.");
                }
            }

            if (command.Offer != null)
            {
                var service = await _serviceRepository.GetServiceByIdAsync(command.Offer.ServiceId);
                if (service != null)
                {
                    var selectedDayOfWeek = command.StartTime.ToString("dddd", new CultureInfo("pl-PL"));
                    var openingHours = service.OpeningHours
                        .FirstOrDefault(oh => oh.DayOfWeek.Equals(selectedDayOfWeek, StringComparison.InvariantCultureIgnoreCase));

                    if (openingHours == null || openingHours.Closed)
                    {
                        context.AddFailure("StartTime", "Serwis jest zamknięty w wybranym dniu.");
                    }
                    else
                    {
                        if (command.StartTime.TimeOfDay < openingHours.OpeningTime)
                        {
                            context.AddFailure("StartTime", "Wybrana godzina rozpoczęcia jest przed godziną otwarcia serwisu.");
                        }

                        if (command.EndTime.TimeOfDay > openingHours.ClosingTime)
                        {
                            context.AddFailure("EndTime", "Wybrana godzina zakończenia przekracza godziny otwarcia serwisu.");
                        }
                    }
                }
                else
                {
                    context.AddFailure("Offer", "Nie znaleziono serwisu dla podanej oferty.");
                }
            }
            else
            {
                context.AddFailure("Offer", "Oferta nie może być pusta.");
            }

        });
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }

    private bool NotBeInThePast(DateTime date)
    {
        return date > DateTime.UtcNow;
    }

    private bool BeAtLeastOneHourLater(DateTime date)
    {
        return date >= DateTime.UtcNow.AddHours(1);
    }
}
