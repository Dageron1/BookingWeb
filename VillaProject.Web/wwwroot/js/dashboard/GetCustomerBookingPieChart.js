$(document).ready(function () {
    loadTotalBookingPieChart();
});

function loadTotalBookingPieChart() {
    $(".chat-spinner").show();

    $.ajax({
        url: "/Dashboard/GetBookingPieChartData",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            
            loadPieChart("customerBookingsPieChart", data);

            $(".chart-spinner").hide();
        }
    })
}

function loadPieChart(id, data) {
    var chartColors = getChartColorsArray(id);

    options = {
        series: data.series,
        labels: data.labels,
        colors: chartColors,
        dataLabels: {
            style: {
                colors: ['#FFFFFF']
            }
        },
        chart: {
            type: 'pie',
            width: 380,
        },
        stroke: {
            show:false
        },
        legend: {
            position: 'bottom',
            horizontalAlign: 'center',
            labels: {
                colors: "#fff",
                useSeriesColors: false
            },      
        },

    }

    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}