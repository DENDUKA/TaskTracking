function addCalendarEvent(id, start, end, title) {
    var eventObject = {
        id: id,
        start: start,
        end: end,
        title: title,
        StatusColor: "blue",
    };
    $('#calendar').fullCalendar('renderEvent', eventObject, true);
}

function deletCalendarEvent(id) {
    $('#calendar').fullCalendar('removeEvents', id);
}
