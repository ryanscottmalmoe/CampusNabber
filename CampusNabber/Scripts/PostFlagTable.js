/*The purpose of this javascript is to allow users to click on 
column headers of a table to sort the data.*/

var order_title = -1;
var order_flags = -1;
var order_date = -1;
$("#title_header").click(function () {
    order_title *= -1;
    var column_index = $(this).prevAll().length;
    sortTable(order_title, column_index);
    var icon = "";
    if (order_title == -1) {
        icon = "glyphicon-chevron-down";
    }
    else {
        icon = "glyphicon-chevron-up";
    }
    $("#title_order_icon").removeClass().addClass("glyphicon").addClass(icon);
    //Reset the other column orders
    $("#date_order_icon").removeClass();
    $("#flag_order_icon").removeClass();
    order_flags = -1;
    order_date = -1;
});

$("#date_header").click(function () {
    order_date *= -1;
    var column_index = $(this).prevAll().length;
    sortTable(order_date, column_index);
    var icon = "";
    if (order_date == -1) {
        icon = "glyphicon-chevron-down";
    }
    else {
        icon = "glyphicon-chevron-up";
    }
    $("#date_order_icon").removeClass().addClass("glyphicon").addClass(icon);
    $("#title_order_icon").removeClass();
    $("#flag_order_icon").removeClass();
    order_title = -1;
    order_flags = -1;
});

$("#flag_header").click(function () {
    order_flags *= -1;
    var column_index = $(this).prevAll().length;
    sortTable(order_flags, column_index);
    var icon = "";
    if (order_flags == -1) {
        icon = "glyphicon-chevron-down";
    }
    else {
        icon = "glyphicon-chevron-up";
    }
    $("#flag_order_icon").removeClass().addClass("glyphicon").addClass(icon);
    $("#date_order_icon").removeClass();
    $("#title_order_icon").removeClass();
    order_title = -1;
    order_date = -1;
});


function sortTable(sort_order, column_index) {
    var rows = $("tbody tr").get();
    rows.sort(function (a, b) {
        var value1 = getValue(a);
        var value2 = getValue(b);
        if (value1 < value2) {
            return -1 * sort_order;
        }
        if (value1 > value2) {
            return 1 * sort_order;
        }
        return 0;
    });

    function getValue(element) {
        var value = $(element).children('td').eq(column_index).text().toUpperCase();
        return value;
    }

    $.each(rows, function (index, row) {
        $("table").children('tbody').append(row);
    });
}