var productgrid;

function Search() {
    productgrid.reload({
        ProductID: $("#ProductID").val(),
        Skuid: $("#Skuid").val(),
        ProductName: $("#ProductName").val(),
        Category: $("#Category").val(),
        Department: $("#Department").val(),
        VendorId: $("#VendorId").val(),
        status: $("#status").val(),
        Published: $("#Published").val(),
    });
}

$(document).ready(function () {
    productgrid = $("#grid").grid({
        dataKey: "ProductID",
        uiLibrary: "bootstrap",
        columns: [
            //{ field: "Checkbox", title: "", width: 34, type: "checkbox" },
            //{ title: "", field: "Picture", width: 90, type: "image" },
            //{ field: "ProductID", title: "Product Id", sortable: true },
            { field: "SKU", title: "SKU", sortable: true },
            { field: "Department", title: "Department", sortable: true },
            { field: "Category", title: "Category", sortable: true },
            { field: "ProductName", title: "Product Name", sortable: true },
            
            { field: "Quantity", title: "Stock Quantity" },
           // { field: "PublishedMsg", title: "Published" }
        ],
        pager: { enable: true, limit: 25, sizes: [10, 20, 30, 40, 50] }
    });
    $("#btnSearch").on("click", Search);

    var rowCount = $('#grid tbody tr').length;
    if (rowCount <= 1) {
        $('input[type=radio][name=rbtnApproval]').prop('disabled', true);
        $("#dv_chkAll").addClass("hide");
    }
    else {
        $("#dv_chkAll").removeClass("hide");
    }
});



function productApproval(mode, xml, reason) {
    var url = '/Product/ProductApproval';
    $.ajax({
        type: "POST",
        url: url,
        data: { Mode: mode, XML: xml, ApprovalReason: reason, ApprovalRemark: '' },
        cache: false,
        success: function (data) {
            var msgnotification = MsgNotification(data.Status.toString().toLowerCase(), data.Massage)
            $('#msgnotificationdiv').html(msgnotification);
            if (data.Status.toLowerCase() == 'success') {
                productgrid.reload();
            }
        }
    });
};
$('#chkAll').click(function () {
    $('input:checkbox').prop('checked', this.checked);
});
$('input[type=radio][name=rbtnApproval]').on('change', function () {
    switch ($(this).val()) {
        case 'Publish':
            ProductPublish();
            break;
        case 'Unpublish':
            ProductUnpublish();
            break;
    }
});
function ProductPublish() {
    if ($("input[type=checkbox]:checked").length == 0) {
        $("#alertHeader").text("Alert");
        $("#dv_alertBody").text("Please select atleast one checkbox");
        $("#btnAlertYes").val("Ok");
        $("#btnAlertNo").addClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnPublish").prop('checked', false);
            return false;
        });
    }
    else {
        $("#alertHeader").text("Confirm");
        $("#dv_alertBody").text("Are you sure you want to publish selected product?");
        $("#btnAlertYes").val("Yes");
        $("#btnAlertNo").val("No");
        $("#btnAlertNo").removeClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#loader").show();
            window.setTimeout(function () {
                var mode = 'Published';
                var xml = '<NewDataSet>'
                $("input[type=checkbox]:checked").each(function () {
                    xml = xml + '<ItemRow><ProductId>' + $(this).val() + '</ProductId></ItemRow>';
                });
                xml = xml + '</NewDataSet>';
                productApproval(mode, xml, '');
                $(".loader").fadeOut("slow");

            }, 600);
        });
        $("#alertModelPopup").modal().one('click', '#btnAlertNo', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnApproved").prop('checked', false);
        });
    }
}
function ProductUnpublish() {
    if ($("input[type=checkbox]:checked").length == 0) {
        $("#alertHeader").text("Alert");
        $("#dv_alertBody").text("Please select atleast one checkbox");
        $("#btnAlertYes").val("Ok");
        $("#btnAlertNo").addClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnUnpublish").prop('checked', false);
            return false;
        });
    }
    else {

        $("#alertHeader").text("Confirm");
        $("#dv_alertBody").text("Are you sure you want to unpublish selected product?");
        $("#btnAlertYes").val("Yes");
        $("#btnAlertNo").val("No");
        $("#btnAlertNo").removeClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');

            $("#loader").show();
            window.setTimeout(function () {
                $('#dv_unpublishReason').modal('show');
                $(".loader").fadeOut("slow");
            }, 400);

        });
        $("#alertModelPopup").modal().one('click', '#btnAlertNo', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnUnpublish").prop('checked', false);
        });
    }
}
$("#btnUnpublishReason").click(function () {
    $("#dv_unpublishReasionMsg").html('');
    var reason = $("#txtUnpublishReason").val();
    if (reason != null && reason != '') {
        window.setTimeout(function () {
            $("#loader").show();
            var mode = 'Unpublished';
            var xml = '<NewDataSet>'
            $("input[type=checkbox]:checked").each(function () {
                xml = xml + '<ItemRow><ProductId>' + $(this).val() + '</ProductId></ItemRow>';
            });
            xml = xml + '</NewDataSet>';
            productApproval(mode, xml, reason);
            $("#txtUnpublishReason").val('');
            $('#dv_unpublishReason').modal('hide');
            //$("#btnUnpublish").prop('checked', false);
            $(".loader").fadeOut("slow");

        }, 600);
    }
    else {

        $("#dv_unpublishReasionMsg").html("Please give unpublish reason and save it.");
        return false;
    }
    
});