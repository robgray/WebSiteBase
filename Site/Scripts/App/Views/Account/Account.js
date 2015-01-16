Account = {

    LostPasswordSuccess: function (response) {
        switch (response.ResponseType) {
            case "SUCCESS":
                toastr.success(response.Data);
                break;
            case "WARNING":
                toastr.warning(response.Data);
                break;
            case "ERROR":
                toastr.error(response.Data);
                break;
            case "VALIDATIONERROR":
                toastr.error(response.Data);
                break;
            case "REDIRECT":                
                window.location.href = response.Data;
                break;
            case "REDIRECTANDMESSAGE":                
                window.location.href = response.Data.RedirectUrl;
                toastr.success(response.Data.Message);
                break;
            default:
                toastr.error("Unexpected Error");
                break;
        }
    },

    LostPasswordFailure: function (response) {        
        toastr.error(response.statusText);
    }
};