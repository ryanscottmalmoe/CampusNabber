var oTable;
$(function () {
    oTable = $('#flaggedPostTable').dataTable({
        sPaginationType: "full_numbers",
        bPaginate: true,
        iDisplayLength: 10,
        scrollCollapse: true,
        scrollY: '50vh',
        scrollX: false,
        columns: [
            { type: "string" },
            { type: "date" },
            {type: "num"}
        ]
    });
});

