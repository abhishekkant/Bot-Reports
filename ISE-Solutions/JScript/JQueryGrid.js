var grid;
function Add() {
    $("#playerId").val("");
    $("#name").val("");
    $("#placeOfBirth").val("");
    $("#dateOfBirth").val("");
    $("#playerModal").modal("show");
}
function Edit(e) {
    $("#playerId").val(e.data.id);
    $("#name").val(e.data.record.Name);
    $("#placeOfBirth").val(e.data.record.PlaceOfBirth);
    $("#dateOfBirth").val(e.data.record.DateOfBirth);
    $("#playerModal").modal("show");
}
function Save() {
    var player = {
        ID: $("#playerId").val(),
        Name: $("#name").val(),
        PlaceOfBirth: $("#placeOfBirth").val(),
        DateOfBirth: $("#dateOfBirth").val()
    };
    $.ajax({ url: "Home/Save", type: "POST", data: { player: player } })
        .done(function () {
            grid.reload();
            $("#playerModal").modal("hide");
        })
        .fail(function () {
            alert("Unable to save.");
            $("#playerModal").modal("hide");
        });
}
function RedirectLocation(e) {
    location.href = "VendorRegistration/" + e.data.id + "";
}
function Search() {
    grid.reload({ vendorId: $("#vendorId").val(), firstName: $("#firstName").val(), status: $("#status").val() });
}

$(document).ready(function () {
    grid = $("#grid").grid({
        dataKey: "Vendorid",
        uiLibrary: "bootstrap",
        columns: [
            { field: "Vendorid", title: "Vendor id", sortable: true },
            { field: "FristName", title: "Frist Name", sortable: true },
            { field: "LastName", title: "Last Name", sortable: true },
            { field: "MobileNo", title: "Mobile No" },
            { field: "AlternateMobileNo", title: "Alternate No" },
            { field: "Address", title: "Address" },
            { field: "CreatedBy", title: "Created By" },
            { field: "Rating", title: "Rating", sortable: true },
            { field: "CreatedDate", title: "Created Date", sortable: true },
            { field: "IsActive", title: "Active Status" },
            { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": RedirectLocation } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 20, 30, 40, 50] }
    });
    $("#btnAddPlayer").on("click", Add);
    $("#btnSave").on("click", Save);
    $("#btnSearch").on("click", Search);
});


