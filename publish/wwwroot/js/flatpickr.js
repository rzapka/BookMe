
var startTimeValue = typeof startTimeValue !== 'undefined' ? startTimeValue : null;


var datepicker = flatpickr("#datetimepicker", {
    enableTime: true,
    enableSeconds: false, 
    dateFormat: "d/m/Y H:i",
    locale: "pl",
    minDate: "today",
    time_24hr: true,
    ...(startTimeValue && { defaultDate: startTimeValue })
});


var calendarButton = document.getElementById('calendar-button');
if (calendarButton) {
    calendarButton.addEventListener('click', function () {
        datepicker.open();
    });
}
