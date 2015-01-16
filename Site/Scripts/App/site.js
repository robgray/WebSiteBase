//
// General functions and classes for use throughout the WebBase site.
//
function htmlEncode(value) {
    if (value) {
        return $('<div/>').text(value).html();
    } else {
        return '';
    }
}

function ifundef(thevalue, thedefault) {
    return typeof(thevalue) != 'undefined' ? thevalue : thedefault;
}

function AjaxResponseFailure(response) {
    response = response || {};
    
    if (!response.status) {
        response.status = 0;
    }
    
    if (response.status == 500) {
        toastr.error(response.statusText);
    } else {
        var result = ko.utils.parseJson(response.responseText);

        if (result.Message)
            toastr.error(ifundef(result.MessageDetail, result.Message));
        if (result.Reason)
            toastr.error(result.Reason);
    }    
}

function ajaxRequest(type, url, data, noJson) {
    var options = {
        dataType: "json",
        contentType: "application/json",
        cache: false,
        type: type,
        data: (noJson) ? data : ko.toJSON(data)
    };

    return $.ajax(url, options);
}