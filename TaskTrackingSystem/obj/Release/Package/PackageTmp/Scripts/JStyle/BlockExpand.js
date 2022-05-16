$(document).ready(function () {
    var modules = ['#peoplemodule', "#datesmodule", "#details-module", "#activitymodule", "#descriptionmodule", "#timetrackmodule", '#resume', '#paylistkmodule'];

    modules.forEach(function (item, index, modules) {
        $(item).find("h3").on('click', changeCollapsedExplanded);
    });
});


function changeCollapsedExplanded() {

    item = $(this).parents(".module");

    if (item.hasClass("collapsed")) {
        item.removeClass("collapsed");
        item.addClass("expanded");

        item.find(".mod-content").removeClass("hidden");
    }
    else {
        item.removeClass("expanded");
        item.addClass("collapsed");

        item.find(".mod-content").addClass("hidden");
    }
}
