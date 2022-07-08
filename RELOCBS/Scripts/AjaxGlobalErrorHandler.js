$(function () {

    //setup ajax error handling
    $.ajaxSetup({
        error: function (jqXHR, textStatus, errorThrown) {
            //Use Base handleError method  
            debugger;

            try {
                
                var msg = "";
                if (jqXHR.status === 0) {
                    msg = 'Not connect. Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                }
                else if (jqXHR.status == 401) {
                    //location.href = "/";
                    location.reload();
                    return;
                }
                else if (jqXHR.status == 440) {
                    //location.href = "/";
                    location.reload();
                    alert('Someone has already login using the same credentials');
                    return;
                }
                else if (textStatus === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (textStatus === 'timeout') {
                    msg = 'Time out error.';
                } else if (textStatus === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    try {
                        msg = JSON.parse(jqXHR.responseText);
                    }
                    catch (ers) {
                        msg = jqXHR.responseText;
                    }
                }

                if (msg && msg!="") alert(msg);
            }
            catch (err) {

                alert(err);
            }

        }
    });

});

