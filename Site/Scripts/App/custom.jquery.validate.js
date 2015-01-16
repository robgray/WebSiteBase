function getDateFormat(formatString) {

    var separator = formatString.match(/[.\/\-\s].*?/),
        parts = formatString.split(/\W+/);
    if (!separator || !parts || parts.length === 0) {
        throw new Error("Invalid date format.");
    }
    return { separator: separator, parts: parts };
}
function MyParseDate(value, format) {
    var parts = value.split(format.separator),
        date = new Date(0),
        year = -1,
        month = -1,
        day = -1;
                   
    if (parts.length === format.parts.length) {     
        for (var i = 0, cnt = format.parts.length; i < cnt; i++) {
            val = parseInt(parts[i], 10) || 1;

            switch (format.parts[i]) {
                case 'dd':
                case 'd':
                    day = val;
                    break;
                case 'mm':
                case 'm':
                    month = val - 1;
                    break;                
                case 'yyyy':
                    year = val;
                    break;
            }
        }
    }
    
    // Must be in this OrderInPhase.
    date.setFullYear(year);
    date.setMonth(month);
    date.setDate(day);
    
    return date.getDate() === day && date.getMonth() === month && date.getFullYear() === year;
}

$(document).ready(function () {

    jQuery.validator.addMethod('date',
        function (value, element, params) {                    
            if (this.optional(element)) {
                return true;
            }            
            try {                        
                var format = getDateFormat('dd/mm/yyyy');
                var result = MyParseDate(value, format);
                return result;
            } catch(err) {                
                return false;
            }            
        });
});