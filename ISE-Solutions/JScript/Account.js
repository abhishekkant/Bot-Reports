$(document).ready(function () {
    $("#savebtn").val("Add");
    productgrid = $("#grid").grid({
        dataKey: "RoleID",
        uiLibrary: "bootstrap",
        columns: [
           
            { field: "RoleName", title: "Role Name", sortable: true },
            { field: "Description", title: "Description", sortable: true },
                { field: "IsActive", title: "Active Status", sortable: true },
              
            { title: "Action", field: "Edit", type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditRow } }
        ]
       
       
        //pager: { enable: true, limit: 10, sizes: [10, 20, 30, 40, 50] }
    });
    //$("#btnAddPlayer").on("click", Add);
    //$("#btnSave").on("click", Save);
   
});




function EditRow(rowdata)
{   
    $.get("/Account/EditRow", { RoleID: rowdata.data.id }, function (result) {
        $("#savebtn").val("Update");
        $("#RoleID").val(result.RoleID);
        $("#RoleName").val(result.RoleName);
        $("#Description").val(result.Description);
        $("#initialRoleName").val(result.RoleName);
        result.IsActive == true ? $("#active").prop('checked', true) : $("#active").prop('checked', false)        
    })   
}




