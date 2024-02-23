var dataTable;

$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const status = urlParams.get('status');
    loadDataTable(status);

});


function loadDataTable(status) {
    //will retrieve that ID
    dataTable = $('#tblBookings').DataTable({
        "ajax": {
            url:'/booking/getall?status='+status
        },
        "columns": [
            //проверить на наличие ошибок можно через JSON formatter получив всю дадту просто через json в гугле вызвав метод getall в ручную.
            //дата должна совпадать с той которую возвращает json. Иначе оно не поймет какую дату подставлять в html, если мы укажем не правильно.
            //например в data вернулся стобец checkInDate, а мы пытаемся вывести столбец checkindate, будет ошибка.
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'phone', "width": "10%" },
            { data: 'email', "width": "15%" },
            { data: 'status', "width": "10%" },
            { data: 'checkInDate', "width": "10%" },
            { data: 'nights', "width": "10%" },
            { data: 'totalCost', render: $.fn.dataTable.render.number(',', '.', 2) ,"width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                        <a href="/booking/bookingDetails?bookingId=${data}" class="btn btn-outline-warning mx-2">
                            <i class="bi bi-pencil-square"></i>Details
                        </a>
                    </div>`
                }
            }

        ]
    })
}