/* Common Alert Massage Notification   */
function MsgNotification(status, alertMsg) {
    var msg;
    switch (status) {
        case "success":
            alterclass = "alert-success";
            status = "Success!";
            break;
        case "update":
            alterclass = "alert-success";
            status = "Update!";
            break;
        case "exists":
            alterclass = "alert-danger";
            status = "Exists!";
            break;
        case "error":
            alterclass = "alert-danger";
            status = "Warning";
            break;
        default:
            alterclass = "alert-warning";
            alertMsg = "Oops! Something is going Worng!!";
            //status = "Oops!";
            break;
    }
    if (status != "") {
        msg = '<div class="alert ' + alterclass + ' alert-dismissable" style="text-transform:capitalize"><a href="#" class="close" data-dismiss="alert" title="Close"  aria-label="close">&times;</a><strong>' + status + '</strong>' + alertMsg + '</div>'
    }
    return msg;

}