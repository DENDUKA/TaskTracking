$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var connect = $.connection.premiumHub;

    connect.client.onConnected = function (message) {
    };

    $.connection.hub.start().done(function () {

        console.log('connected');
    });

    $(document).ready(function () {
        
    });


function fillTable(projectList) {
    var tableBody = $('#issuetable tbody');

    var arr = JSON.parse(projectList);

    arr.forEach(x => tableBody.append("<tr><td>" + x.projecttype + "</td><td>" + x.name + "</td><td>" + x.acc + "</td><td>" + x.cost + "</td></tr>"));
}

    $('button[projectId]').click(function () {
        var btn = $(this);
        var id = btn.attr('projectId');
        connect.server.markAsPaid(id).done(function (res) {
            if (res) {
                btn.parent().parent().remove();
            }
    });
});

});

/*button projectId*/

// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
