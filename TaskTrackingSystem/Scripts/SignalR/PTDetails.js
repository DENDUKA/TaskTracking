$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var connect = $.connection.generalHub;

    connect.client.onConnected = function (message) {
        
    };

    $.connection.hub.start().done(function () {

    });

    $(document).ready(function () {
        var checkBoxIsPremiumPaid = $("[name='PremiumPaid']");
        checkBoxIsPremiumPaid.removeAttr('disabled');
        checkBoxIsPremiumPaid.click(function () {
            connect.server.premiumPaid($(this).parent().parent().attr('bdid'), this.checked);
        });
    });
});
