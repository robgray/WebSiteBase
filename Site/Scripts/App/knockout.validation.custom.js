ko.validation.rules['formattedDate'] = {
    validator: function (dateToValidate, dateFormat) {

        /********************************************************

        The dateFormat argument must be in the following format:

            { 
                format: 'dmy', 
                delimiter: '/' 
            }

        The format propery specified the OrderInPhase of the day, month
        and year components of the date.

        The delimiter property specified the delimiter used by
        the date.

        The above example will accept dates in the following formats:

            dd/mm/yy
            dd/mm/yyyy
            d/m/yy
            d/m/yyyy

        *********************************************************/

        var invalidCharsRegEx = new RegExp("[^0-9" + dateFormat.delimiter + "]");

        if (invalidCharsRegEx.test(dateToValidate)) {
            // The given date contains invalid characters
            return false;
        }

        var dayIndex = dateFormat.format.indexOf("d");
        var monthIndex = dateFormat.format.indexOf("m");
        var yearIndex = dateFormat.format.indexOf("y");
        var dateParts = dateToValidate.split(dateFormat.delimiter);

        // Convert the date component parts to numbers
        var dayNumber = parseInt(dateParts[dayIndex], 10);
        var monthNumber = parseInt(dateParts[monthIndex], 10);
        var yearNumber = parseInt(dateParts[yearIndex], 10);

        if (isNaN(dayNumber) || isNaN(monthNumber) || isNaN(yearNumber)) {
            // The day, month or year cannot be determined
            return false;
        }

        if (dayNumber < 1 || dayNumber > 31) {
            // Invalid day
            return false;
        }

        if (monthNumber < 1 || monthNumber > 12) {
            // Invalid month
            return false;
        }

        if (monthNumber == 2) {            
            // The month is Feb; see if it's a leap year
            var leapYear = (yearNumber % 4 == 0 &&
                            (yearNumber % 100 != 0 ||
                             yearNumber % 400 == 0));

            if (leapYear) {
                // It is a leap year, so there's 29 days
                if (dayNumber > 29) {
                    // Invalid day in Feb
                    return false;
                }
            }
            else {
                // Not a leap year
                if (dayNumber > 28) {
                    // It's not a leap year
                    return false;
                }
            }
        }
        else {
            // Not February
            if (dayNumber == 31) {
                // It's the 31st day
                if (monthNumber == 4 ||
                    monthNumber == 6 ||
                    monthNumber == 9 ||
                    monthNumber == 11) {
                    // The 31st is invalid
                    return false;
                }
            }
        }

        // Try to create a new dateobject
        var parsedDate = new Date(
            yearNumber,
            monthNumber - 1,
            dayNumber);

        return parsedDate != "Invalid Date";
    },
    message: 'Invalid date'
};

ko.validation.rules['mustEqual'] = {
    validator: function(val, otherVal) {
        return val === otherVal;
    },
    message: "The field must equal {0}"
};

// overide my datePicker shizzle.
(function () {
    var init = ko.bindingHandlers['datePicker'].init;

    ko.bindingHandlers['datePicker'].init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {

        init(element, valueAccessor, allBindingsAccessor);

        return ko.bindingHandlers['validationCore'].init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
    };
}());