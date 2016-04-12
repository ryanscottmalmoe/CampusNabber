/*The purpose of this javascript is to allow users to click on 
column headers of a table to sort the data. - Ahenry*/

//Global values that determine the initial sort order
var order_title = -1;
var order_flags = -1;
var order_date = -1;

/*Method that handles when the "Title" header is clicked. It will sort the table and
display a glyphicon that illustrates the current sort order.*/
$("#title_header").click(function () {
    order_title *= -1;
    //Determine the index of the column header
    var column_index = $(this).prevAll().length;
    //Sort the table based on the title column
    sortTable(order_title, column_index);
    var icon = "";
    /*Based on whether the table is sorted in ascending/descending order, set the 
    glyphicon accordingly.*/
    if (order_title == -1) {
        icon = "glyphicon-chevron-down";
    }
    else {
        icon = "glyphicon-chevron-up";
    }
    $("#title_order_icon").removeClass().addClass("glyphicon").addClass(icon);
    //Reset the other column sort orders and glyphicons
    $("#date_order_icon").removeClass();
    $("#flag_order_icon").removeClass();
    order_flags = -1;
    order_date = -1;
});

//Method that handles when the "Date" header is clicked
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

//Method that handles when the "Flag" header is clicked
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

//Method that sorts the table according to the specified column index number
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
    /*This function pulls the value of the row at the specified column_index. If the
    value is a number, it will be parsed accordingly.*/
    function getValue(element) {
        var value = $(element).children('td').eq(column_index);
        value = value[0].innerText;
        //Try parsing the value to see if it's a date.
        var date = Date.parse(value);
        //Test to see if the value is a number
        if ($.isNumeric(value)) {
            value = parseInt(value);
        }
        //If it's not a number, determine whether the value is a valid date
        else if (!isNaN(date)) {
            value = date;
        }
        //Otherwise, it's a string
        else {
            value = value.toUpperCase();
        }
        return value;
    }

    //Add the sorted rows back to the table.
    $.each(rows, function (index, row) {
        $("table").children('tbody').append(row);
    });
}