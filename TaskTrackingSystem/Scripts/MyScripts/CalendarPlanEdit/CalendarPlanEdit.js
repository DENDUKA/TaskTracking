async function ButtonRemove(button) {

    var id = $(button).attr("id");
    var response = await $.post(getURLForRequest('Project', 'DeleteCalendarPlanItem'), { 'cpId': id });

    console.log("Removed " + response);

    if (response === "True") {
        $($(button)).closest("div.row").remove();
    }
}

async function ButtonAdd(button) {

    var projectId = $("#projectId").attr("value");

    var response = await $.post(getURLForRequest('Project', 'AddCalendarPlanItem'), { 'projectId': projectId });

    console.log("Added " + response);

    if (response !== "0") {
        var template = $("#RowTemplate").clone().removeAttr("style").removeAttr("id");
        template.find("[deleteButton]").attr("id", response);

        var AddRow = $("#AddRow");
        AddRow.before(template);
    };
}

async function Update(input)
{
    var row = $(input).parent().parent();
    var cpId = row.find("[deleteButton]").attr("id");
    var stageNum = row.find("[stageNum]").val();
    var deadlines = row.find("[deadlines]").val();
    var workType = row.find("[workType]").val();
    
    var response = await $.post(getURLForRequest('Project', 'UpdateCalendarPlanItem'), { 'cpId': cpId, 'stageNum': stageNum, 'workType': workType, 'deadlines': deadlines });

    console.log("Updated " + response);
}
