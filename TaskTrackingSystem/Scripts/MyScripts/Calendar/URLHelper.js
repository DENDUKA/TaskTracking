function getURLForRequest(controller, action) {

    var pathArray = DeleteAllEmptyItemsInArray(window.location.pathname.split('/'));
    var newURL = window.location.origin;
    for (i = 0; i < pathArray.length; i++) {
        newURL += "/";
        newURL += pathArray[i];

        if (pathArray[i] == controller)
            break;
    }
    return newURL + '/' + action;
}

function DeleteAllEmptyItemsInArray(array) {
    return array.filter(function (el) {
        return (el != null && el != '');
    });
}
