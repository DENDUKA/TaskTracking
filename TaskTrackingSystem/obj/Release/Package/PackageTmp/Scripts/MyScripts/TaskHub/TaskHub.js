$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var connect = $.connection.taskHub;

    connect.client.onConnected = function (message) {
    };

    $.connection.hub.start().done(function () {
        

    });

    $(document).ready(function () {

    });

    $("#approveButton").click(function () {
        var thisButton = $(this);
        var classChecked = "btn-status-selected";
        var isChecked = thisButton.hasClass(classChecked);
        var id = $("#Id").val();

        console.log(!isChecked);


        if (isChecked) {
            thisButton.removeClass(classChecked);
        }
        else {
            thisButton.addClass(classChecked);
        }

        connect.server.approved(id, !isChecked);
    });


    //function fillTable(projectList) {
    //    var tableBody = $('#issuetable tbody');

    //    var arr = JSON.parse(projectList);

    //    arr.forEach(x => tableBody.append("<tr><td>" + x.projecttype + "</td><td>" + x.name + "</td><td>" + x.acc + "</td><td>" + x.cost + "</td></tr>"));
    //}

    //$('button[projectId]').click(function () {
    //    var btn = $(this);
    //    var id = btn.attr('projectId');
    //    connect.server.markAsPaid(id).done(function (res) {
    //        if (res) {
    //            btn.parent().parent().remove();
    //        }
    //    });
    //});

});
