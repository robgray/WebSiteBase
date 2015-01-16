function addRequired(searchClass) {
    $(searchClass).each(function () {     
        if ($(this).parent().hasClass("input-append"))
            $(this).parent().addClass("input-prepend");
        else
            $(this).wrap("<div class='input-prepend'>");
    });
 
    $(searchClass).before("<span class='add-on requiredmarker'></span>");
}

$(document).ready(function () {
    //addRequired('input[type=text][data-val-required]');
    //addRequired('input[type=password][data-val-required]');
    //addRequired('select[data-val-required]');    
});