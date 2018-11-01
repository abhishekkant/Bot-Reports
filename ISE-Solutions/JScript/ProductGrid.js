var productgrid;
//function Add() {
//    $("#playerId").val("");
//    $("#name").val("");
//    $("#placeOfBirth").val("");
//    $("#dateOfBirth").val("");
//    $("#playerModal").modal("show");
//}
//function Edit(e) {
//    $("#playerId").val(e.data.id);
//    $("#name").val(e.data.record.Name);
//    $("#placeOfBirth").val(e.data.record.PlaceOfBirth);
//    $("#dateOfBirth").val(e.data.record.DateOfBirth);
//    $("#playerModal").modal("show");
//}
//function Save() {
//    var player = {
//        ID: $("#playerId").val(),
//        Name: $("#name").val(),
//        PlaceOfBirth: $("#placeOfBirth").val(),
//        DateOfBirth: $("#dateOfBirth").val()
//    };
//    $.ajax({ url: "Home/Save", type: "POST", data: { player: player } })
//        .done(function () {
//            grid.reload();
//            $("#playerModal").modal("hide");
//        })
//        .fail(function () {
//            alert("Unable to save.");
//            $("#playerModal").modal("hide");
//        });
//}
function RedirectLocation(e) {
    //debugger;
   // var url = '/Product/EncryptData';
    //$.post(url, { data: e.data.id }, function (res) {
        location.href = "ManageProduct?pid=" + e.data.id + "";
    //})

   
}
function Search() {
    productgrid.reload({
        //ProductID: $("#ProductID").val(),
        Skuid: $("#Skuid").val(),
        ProductName: $("#ProductName").val(),
        Category: $("#Category").val(),
        Department: $("#Department").val(),
        VendorId: $("#VendorId").val(),
       // status: $("#status").val(),
        Approved: $("#Approved").val()
    });
}
function SelectAll(e) {
    //location.href = "ManageProduct/" + e.data.id + "";
}

$(document).ready(function () {
    productgrid = $("#grid").grid({
        dataKey: "ProductIDEnc",
        uiLibrary: "bootstrap",
        columns: [
            //{ field: "Checkbox", title: "", width: 34, type: "checkbox" },
            //{ title: "", field: "Picture", width: 90, type: "image" },
             { field: "SKU", title: "SKU", sortable: true },
          //  { field: "ProductID", title: "Product Id", sortable: true },
            { field: "ProductName", title: "Product Name", sortable: true },
            { field: "Department", title: "Department", sortable: true },
            { field: "Category", title: "Category", sortable: true },
            { field: "Quantity", title: "Stock Quantity" },
            { field: "ApprovedMsg", title: "Approved" },
           // { field: "PublishedMsg", title: "Published" },
            //{ field: "CreatedBy", title: "Created By" },
            //{ field: "Rating", title: "Rating", sortable: true },
            //{ field: "CreatedDate", title: "Created Date", sortable: true },
             { field: "Reason", title: "Reason" },
            { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": RedirectLocation } }
        ],
        pager: { enable: true, limit: 25, sizes: [10, 20, 30, 40, 50] }
    });
    //$("#btnAddPlayer").on("click", Add);
    //$("#btnSave").on("click", Save);
    $("#btnSearch").on("click", Search);
});


var imagerenderer = function (row, field, value) {
    return '<img style="margin-left: 5px;" height="60" width="50" src="https://n1.sdlcdn.com/' + value + '"/>';
}

function productApproval(mode, xml,rType, reason) {
    var url = '/Product/ProductApproval';
    $.ajax({
        type: "POST",
        url: url,
        data: { Mode: mode, XML: xml, ApprovalReason: rType, ApprovalRemark: reason },
        cache: false,
        success: function (data) {
            var msgnotification = MsgNotification(data.Status.toString().toLowerCase(), data.Massage)
            $('#msgnotificationdiv').html(msgnotification);
            if (data.Status.toLowerCase() == 'success') {
                //debugger;
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
        case 'Approved':
            ProductApproved();
            break;
        case 'Disapproved':
            ProductDisapproved();
            break;
        case 'ApprovedPublish':
            ApprovedNPublish();
            break;
    }
});


function ProductApproved() {
    if ($("input[type=checkbox]:checked").length == 0) {
        $("#alertHeader").text("Alert");
        $("#dv_alertBody").text("Please select atleast one checkbox");
        $("#btnAlertYes").val("Ok");
        $("#btnAlertNo").addClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnApproved").prop('checked', false);
            return false;
        });
    }
    else {
        $("#alertHeader").text("Confirm");
        $("#dv_alertBody").text("Are you sure you want to approve selected product?");
        $("#btnAlertYes").val("Yes");
        $("#btnAlertNo").val("No");
        $("#btnAlertNo").removeClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#loader").show();
            window.setTimeout(function () {
                var mode = 'Approved';
                var xml = '<NewDataSet>'
                $("input[type=checkbox]:checked").each(function () {
                    xml = xml + '<ItemRow><ProductId>' + $(this).val() + '</ProductId></ItemRow>';
                });
                xml = xml + '</NewDataSet>';
                productApproval(mode, xml, '','');
                //$("#btnApproved").prop('checked', false);
                $(".loader").fadeOut("slow");

            }, 600);
        });
        $("#alertModelPopup").modal().one('click', '#btnAlertNo', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnApproved").prop('checked', false);
        });
    }
}

function ProductDisapproved() {
    if ($("input[type=checkbox]:checked").length == 0) {
        $("#alertHeader").text("Alert");
        $("#dv_alertBody").text("Please select atleast one checkbox");
        $("#btnAlertYes").val("Ok");
        $("#btnAlertNo").addClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnDisApproved").prop('checked', false);
            return false;
        });
    }
    else {

        $("#alertHeader").text("Confirm");
        $("#dv_alertBody").text("Are you sure you want to approve selected product?");
        $("#btnAlertYes").val("Yes");
        $("#btnAlertNo").val("No");
        $("#btnAlertNo").removeClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');

            $("#loader").show();
            window.setTimeout(function () {
                $('#dv_approvalReason').modal('show');
                $(".loader").fadeOut("slow");
            }, 400);

        });
        $("#alertModelPopup").modal().one('click', '#btnAlertNo', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnApproved").prop('checked', false);
        });

    }
}
$("#btnDisapprovedReason").click(function () {
    $("#dv_reasionMsg").html('');

    var rType = $("#ddlReason").val();
    var reason = $("#txtReason").val();
    if (rType != null && rType != '' && reason != null && reason != '') {
        window.setTimeout(function () {
            $("#loader").show();
            var mode = 'Rejected';
            var xml = '<NewDataSet>'
            $("input[type=checkbox]:checked").each(function () {
                xml = xml + '<ItemRow><ProductId>' + $(this).val() + '</ProductId></ItemRow>';
            });
            xml = xml + '</NewDataSet>';
            rType = $("#ddlReason option:Selected").text();
            productApproval(mode, xml, rType,reason);
            $("#txtReason").val('');
            $('#dv_approvalReason').modal('hide');
            //$("#btnDisApproved").prop('checked', false);
            $(".loader").fadeOut("slow");

        }, 600);
    }
    else {
        $("#dv_reasionMsg").html("Reason type and Reason are required.");
        return false;
    }
});

function ApprovedNPublish() {
    if ($("input[type=checkbox]:checked").length == 0) {
        $("#alertHeader").text("Alert");
        $("#dv_alertBody").text("Please select atleast one checkbox");
        $("#btnAlertYes").val("Ok");
        $("#btnAlertNo").addClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnApprovPublish").prop('checked', false);
            return false;
        });
    }
    else {
        $("#alertHeader").text("Confirm");
        $("#dv_alertBody").text("Are you sure you want to approve and publish selected product?");
        $("#btnAlertYes").val("Yes");
        $("#btnAlertNo").val("No");
        $("#btnAlertNo").removeClass("hide");
        $("#alertModelPopup").modal('show').one('click', '#btnAlertYes', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#loader").show();
            window.setTimeout(function () {
                var mode = 'ApprovedNPublish';
                var xml = '<NewDataSet>'
                $("input[type=checkbox]:checked").each(function () {
                    xml = xml + '<ItemRow><ProductId>' + $(this).val() + '</ProductId></ItemRow>';
                });
                xml = xml + '</NewDataSet>';
                productApproval(mode, xml, '', '');
                $(".loader").fadeOut("slow");

            }, 600);
        });
        $("#alertModelPopup").modal().one('click', '#btnAlertNo', function (e) {
            $("#alertModelPopup").modal('hide');
            $("#btnApprovPublish").prop('checked', false);
        });
    }
}
