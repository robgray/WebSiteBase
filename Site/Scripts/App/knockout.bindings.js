ko.bindingHandlers.tooltip = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var options = ko.utils.unwrapObservable(valueAccessor());
        var defaultOptions = { };

        console.log(options);
        if (options.constructor !== Object) {
            options = { title: options };
        }

        options = $.extend(true, {}, defaultOptions, options);

        if (options.text)
            options.title = options.text;
        
        $(element).tooltip(options);               
    }
};

ko.bindingHandlers.popover = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {                
        var options = ko.utils.unwrapObservable(valueAccessor());
        var defaultOptions = { exclusive: true };

        options = $.extend(true, {}, defaultOptions, options);

        var htmlContent = '';
        var containerId; 
        if (options.contentHtmlId) {
            
            containerId = 'popoverHtml-' + options.contentHtmlId;
            htmlContent = "<div id='" + containerId + "'>" + $("#" + options.contentHtmlId).html() + "</div>";
            options.content = htmlContent;
        }

        if (options.exclusive) {
            $(element).attr("exclusive", "");                       
        }

        $(element).popover(options);
                
        ko.utils.registerEventHandler(element, "click", function () {
                        
            $('*').filter(function() {
                if ($(this).data('popover') !== undefined) {                         
                    return !$(this).is($(element));                    
                }
                return false;
            }).popover('hide');
            
            
            if (options.contentHtmlId) {
                var thePopover = document.getElementById(containerId);

                if (thePopover) {
                    ko.applyBindings(viewModel, thePopover);                    
                }
            }

            
            $('button[data-popoverclose]').click(function() {
                $(element).popover('hide');
            });
        });
    }
};

ko.bindingHandlers.datePicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {        
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datepickerOptions || { format: 'dd/mm/yyyy', autoclose: true };
        $(element).datepicker(options);
       
        //when a user changes the date, update the view model
        ko.utils.registerEventHandler(element, "changeDate", function (event) {
            var value = valueAccessor();
            if (ko.isObservable(value)) {
                value(event.date);
            }
        });
        
        ko.utils.registerEventHandler(element, "change", function () {
            var widget = $(element).data("datepicker");
            
            var value = valueAccessor();            
            if (ko.isObservable(value)) {
                if (element.value) {
                    var date = widget.getUTCDate();                                        
                    value(date);
                } else {                    
                    value(null);
                }

            }
        });
    },
    update: function (element, valueAccessor) {
            
        var widget = $(element).data("datepicker");

        //when the view model is updated, update the widget
        if (widget) {
            widget.date = ko.utils.unwrapObservable(valueAccessor());

            if (!widget.date) {
                return;
            }

            if (_.isString(widget.date)) {
                widget.setDate(moment(widget.date).toDate());
                return;
            }

            widget.setValue();
        }        
    }
};

ko.bindingHandlers.readOnly = {
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value) {
            element.setAttribute("readOnly", true);
        } else {
            element.removeAttribute("readOnly");
        }
    }
}

ko.bindingHandlers.fadeVisible = {
    init: function(element, valueAccessor) {
        var shouldDisplay = valueAccessor();        
        $(element).toggle(shouldDisplay);
    },
    
    update: function (element, valueAccessor) {
        // On update, fade in/out
        var shouldDisplay = valueAccessor();
        shouldDisplay ? $(element).fadeIn() : $(element).fadeOut();
    }
};

ko.bindingHandlers.escape = {
    update: function(element, valueAccessor, allBindingsAccessor, viewModel) {
        var command = valueAccessor();
        $(element).keyup(function(event) {
            if (event.keyCode === 27) { // ESC  
                command.call(viewModel, viewModel, event);
            }
        });
    }
};

ko.bindingHandlers.executeOnEnter = {
    init: function(element, valueAccessor, allBindingsAccessor, viewModel) {
        var command = valueAccessor();
        $(element).keypress(function(event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13) { // ENTER
                command.call(viewModel);
                return false;
            }
            return true;
        });
    }
};

ko.bindingHandlers.dateString = {
    update: function(element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        if (valueUnwrapped) {
            $(element).text(moment(valueUnwrapped).calendar());
        }
    }
}