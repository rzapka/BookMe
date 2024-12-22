// Initialize the opening time picker
var openingTimePicker = flatpickr("#openingTime", {
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i", // 24-hour time format
    time_24hr: true,
    onChange: function (selectedDates, dateStr, instance) {
        var openingTime = new Date(selectedDates[0]);

        var closingMinTime = new Date(openingTime.getTime() + 5 * 60000);

        closingTimePicker.set('minTime', closingMinTime.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' }));
    }
});

// Initialize the closing time picker
var closingTimePicker = flatpickr("#closingTime", {
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i", // 24-hour time format
    time_24hr: true
});
