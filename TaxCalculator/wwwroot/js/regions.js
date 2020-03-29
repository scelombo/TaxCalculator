var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/regions",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "35%" },
            { "data": "postalCode", "width": "20%" },
            { "data": "calculationType.name", "width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Tax/TaxRegions?id=${data}" class='fas fa-wave-square' title='Tax Table'>
                        </a>
                        &nbsp;
                        <a href="/regions/Edit?id=${data}" class='far fa-edit text-success' title='Edit Region'>
                        </a>
                        &nbsp;
                        <a onclick=Delete('/api/regions/'+${data}) style='cursor:pointer; width:70px;'>
                            <i class='fas fa-trash-alt text-danger' title='Delete Region'></i>
                        </a>
                        </div>`;
                }, "width": "10%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Delete!",
        text: "Are you sure, you want to delete?",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}