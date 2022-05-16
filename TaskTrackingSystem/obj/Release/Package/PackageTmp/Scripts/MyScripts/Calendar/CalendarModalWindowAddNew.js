$(function () {
    var dialog,
        name = $("#Name"),
        dateStart = $("#DateStart"),
        dateEnd = $("#DateEnd");

    function clearmodalWindow() {
        name.val('');
        dateStart.val('');
        dateEnd.val('');
    }

    async function addUser() {
        var valid = true;

        var datestart = new Date(dateStart.val());
        var dateend = new Date(dateEnd.val());
        dateend.setDate(dateend.getDate() + 1);

        valid = valid && name.val().length != 0;
        valid = valid && !isNaN(datestart);
        valid = valid && !isNaN(dateend);
        valid = valid && datestart < dateend;

        if (valid) {
            var response = await $.post(getURLForRequest('Calendar', 'CreateEvent'), { 'name': name.val(), 'dateStart': dateStart.val(), 'dateEnd': dateEnd.val() });

            if (response != "0")
            {
                addCalendarEvent(response, datestart.format("yyyy-mm-dd"), dateend.format("yyyy-mm-dd"), name.val());
                clearmodalWindow();
                dialog.dialog("close");
            }
        }
        return valid;
    }

    dialog = $("#dialog-form").dialog({
        autoOpen: false,

        height: 350,
        width: 300,
        resizable: false,
        modal: true,
        buttons: {
            "Добавить": addUser,
            "Назад": function () {
                dialog.dialog("close");
            }
        },
        close: function () {
            clearmodalWindow();
        }
    });

    form = dialog.find("form").on("submit", function (event) {
        event.preventDefault();
        addUser();
    });

    $("#create-event").button().on("click", function () {
        dialog.dialog("open");
    });
});
