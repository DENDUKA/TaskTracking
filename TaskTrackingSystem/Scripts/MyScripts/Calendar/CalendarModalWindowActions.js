$(function () {
    var actions;
    
    actions = $("#dialog-form-actions").dialog({
        autoOpen: false,

        height: 135,
        width: 350,

        modal: true,
        resizable: false,

        close: function () {
        }
    });

    $("#dialog-form-actions-delete").button().on("click", async function () {
        var response = await $.post(getURLForRequest('Calendar', 'DeleteEvent'), { 'id': globalSelectableEvent.id});
        if (response === "True") {
            $('#calendar').fullCalendar('removeEvents', globalSelectableEvent.id);
            actions.dialog("close");
        };
    });
});
