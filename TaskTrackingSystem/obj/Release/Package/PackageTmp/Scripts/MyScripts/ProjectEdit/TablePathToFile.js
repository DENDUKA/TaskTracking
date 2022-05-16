async function ButtonRemove(button) {

    var id = $(button).attr("id");
    var response = await $.post(getURLForRequest('Project', 'DeleteFilePath'), { 'filePathId': id });

    console.log("Removed " + response);

    if (response === "True") {
        $($(button)).closest("div.row").remove();
    }
}

async function ButtonAdd(button) {

    var projectId = $("#projectId").attr("value");

    var response = await $.post(getURLForRequest('Project', 'AddFilePath'), { 'projectId': projectId });

    console.log("Added " + response);

    if (response !== "0") {
        var template = $("#RowTemplate").clone().removeAttr("style").removeAttr("id");
        template.find("[deleteButton]").attr("id", response);

        var AddRow = $("#AddRow");
        AddRow.before(template);
    };

    return false;
}

async function Update(input)
{
    var row = $(input).closest(".row");
    var filePathId = row.find("[deleteButton]").attr("id");
    var description = row.find("[description]").val();
    var path = row.find("[path]").val();

    row.find("[openpath]").attr("href", "myproto://" + path);

    var response = await $.post(getURLForRequest('Project', 'UpdateFilePath'), { 'filePathId': filePathId, 'description': description, 'path': path });

    console.log("Updated " + response);
}

//$($("#0").parent().parent()[0]).find("[description]").val()
//$($("#0").parent().parent()[0]).find("[path]").val()
