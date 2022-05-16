bkLib.onDomLoaded(function () {
    nicEditors.allTextAreas();

    var descriptionElement = $("#DescriptionId").parent().find(".nicEdit-main");
    descriptionElement.html(descriptionElement.text());


    var pp = descriptionElement.parent();

    descriptionElement.css('width', '99%');
    pp.css('width', '90%');
    pp.parent().find('.nicEdit-panelContain').css('width', '90%');
    $(pp.parent().find('div')[0]).css('width', '100%');

});
