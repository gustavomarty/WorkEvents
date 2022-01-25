var toast = {
    success: function (message) {
        $.toast({
            heading: 'Success',
            text: message,
            showHideTransition: 'slide',
            position: 'bottom-right',
            icon: 'success'
        });
    },
    info: function (message) {
        $.toast({
            heading: 'Information',
            text: message,
            position: 'bottom-right',
            icon: 'info',
            loader: true,        // Change it to false to disable loader
            loaderBg: '#9EC600'  // To change the background
        });
    },
    warning: function (message) {
        $.toast({
            heading: 'Warning',
            text: message,
            position: 'bottom-right',
            showHideTransition: 'plain',
            icon: 'warning'
        });
    },
    error: function (message) {
        $.toast({
            heading: 'Error',
            text: message,
            position: 'bottom-right',
            showHideTransition: 'fade',
            icon: 'error'
        });
    }
}