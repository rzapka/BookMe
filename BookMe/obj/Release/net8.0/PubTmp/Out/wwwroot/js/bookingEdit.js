var datetimepicker = new ej.calendars.DateTimePicker({
    timeFormat: 'HH:mm',
    format: 'dd/MM/yyyy HH:mm',
    cssClass: 'e-custom-datetimepicker',
    value: new Date(startTimeValue)
});
datetimepicker.appendTo('#datetimepicker');

datetimepicker.addEventListener('change', () => {
    var formattedDateTime = datetimepicker.value.toLocaleString('pl-PL', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
});