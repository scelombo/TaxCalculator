var dataTable;

$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');
    loadDataTable(id);
});

function loadDataTable(id) {

    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/RegionTaxes/" + id +"/list",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "rate", "width": "20%" },
            { "data": "rateType", "width": "20%" },
            { "data": "fromAmount", "width": "25%" },
            { "data": "toAmount", "width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                           <a onclick=Delete('/api/RegionTaxes/'+${data}) style='cursor:pointer; width:70px;'>
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

$(function () {
    
    $('#submit').on('click', function (evt) {
        alert('hit');
        evt.preventDefault();
        $.post('/api/RegionTaxes', $('form').serialize(), function () {
            alert('Posted using jQuery');
        });
    });
});


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