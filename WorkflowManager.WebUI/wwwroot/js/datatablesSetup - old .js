$(document).ready(function () {
    var i;
    var table;
    var numOfDatatables = document.querySelectorAll("table[id^=dataTable").length;
    for (i = 1; i <= numOfDatatables; i++) {

        table = document.getElementById("dataTable" + i);
        if (table != null) {
            const defaultSortClassRegex = /DefaultSort-\d-(desc|asc)/;
            const nonResponsiveClassRegex = /NonResponsiveColumn-\d*/;

            if (table.className.match(defaultSortClassRegex) != null && table.className.match(nonResponsiveClassRegex) != null) {
                // default sort
                let targetClass = table.className.match(defaultSortClassRegex)[0];
                targetClass = targetClass.split('-');
                let sortDir = targetClass[2];
                let sortCol = targetClass[1];
                // responsivness
                targetClass = table.className.match(nonResponsiveClassRegex)[0];
                targetClass = targetClass.split('-');
                let nonResCol = targetClass[1];

                SetUpDataTableWithDefaultOrderAndNonResponsiveColumn("#dataTable" + i, sortCol, sortDir, nonResCol);
                continue;
            }
            else if (table.className.match(defaultSortClassRegex) != null) {
                let targetClass = table.className.match(defaultSortClassRegex)[0];
                targetClass = targetClass.split('-');
                let sortDir = targetClass[2];
                let sortCol = targetClass[1];
                SetUpDataTableWithDefaultOrder("#dataTable" + i, sortCol, sortDir);
                continue;
            }
            else if (table.className.match(nonResponsiveClassRegex) != null) {
                let targetClass = table.className.match(nonResponsiveClassRegex)[0];
                targetClass = targetClass.split('-');
                let nonResCol = targetClass[1];
                SetUpDataTableWithNonResponsiveColumn("#dataTable" + i, nonResCol);
                continue;
            }
            else {
                SetUpDataTable("#dataTable" + i);
                continue;
            }
        }
    }
});



// normal datatable
function SetUpDataTable(TableId) {
    // Setup - add a text input to each footer cell
    $(TableId + ' tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" style="width: 100%;" class = "form-control" placeholder=' + title + ' />');
    });
    var table;
    table = $(TableId).DataTable({
        dom: "lrtip",
        processing: true,
        responsive: true,
        stateSave: true,
        stateDuration: 60 * 60,
        language: {
            processing: "Przetwarzanie...",
            search: "Szukaj:",
            lengthMenu: "Pokaż _MENU_ pozycji",
            info: "Pozycje od _START_ do _END_ z _TOTAL_",
            infoEmpty: "Pozycji 0 z 0 dostępnych",
            infoFiltered: "(filtrowanie spośród _MAX_ dostępnych pozycji)",
            infoPostFix: "",
            loadingRecords: "Wczytywanie...",
            zeroRecords: "Nie znaleziono pasujących pozycji",
            emptyTable: "Brak rekordów",
            paginate: {
                first: "Pierwsza",
                previous: "Poprzednia",
                next: "Następna",
                last: "Ostatnia"
            },
            aria: {
                sortAscending: ": aktywuj, by posortować kolumnę rosnąco",
                sortDescending: ": aktywuj, by posortować kolumnę malejąco"
            }

        }
    });

    // Apply the search
    table.columns().eq(0).each(function (colIdx) {
        $('input', table.column(colIdx).footer()).on('keyup change clear', function () {
            table
                .column(colIdx)
                .search(this.value)
                .draw();
        });
    });
}

// datatable with specific column sorted on render
function SetUpDataTableWithDefaultOrder(TableId, sortCol, sortDir) {
    sortCol = Number(sortCol);
    sortDir = String(sortDir);
    
    // Setup - add a text input to each footer cell
    $(TableId + ' tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" style="width: 100%;" class = "form-control" placeholder=' + title + ' />');
    });
    var table;
    table = $(TableId).DataTable({
        order: [[sortCol, sortDir]],
        dom: "lrtip",
        processing: true,
        responsive: true,
        stateSave: false,
        language: {
            processing: "Przetwarzanie...",
            search: "Szukaj:",
            lengthMenu: "Pokaż _MENU_ pozycji",
            info: "Pozycje od _START_ do _END_ z _TOTAL_",
            infoEmpty: "Pozycji 0 z 0 dostępnych",
            infoFiltered: "(filtrowanie spośród _MAX_ dostępnych pozycji)",
            infoPostFix: "",
            loadingRecords: "Wczytywanie...",
            zeroRecords: "Nie znaleziono pasujących pozycji",
            emptyTable: "Brak rekordów",
            paginate: {
                first: "Pierwsza",
                previous: "Poprzednia",
                next: "Następna",
                last: "Ostatnia"
            },
            aria: {
                sortAscending: ": aktywuj, by posortować kolumnę rosnąco",
                sortDescending: ": aktywuj, by posortować kolumnę malejąco"
            }
        }
    });

    // Apply the search
    table.columns().eq(0).each(function (colIdx) {
        $('input', table.column(colIdx).footer()).on('keyup change clear', function () {
            table
                .column(colIdx)
                .search(this.value)
                .draw();
        });
    });
}

// datatable with column that cannot be hidden on resize
function SetUpDataTableWithNonResponsiveColumn(TableId, nonResCol) {
    nonResCol = Number(nonResCol);

    // Setup - add a text input to each footer cell
    $(TableId + ' tfoot th').each(function () {
        var title = $(this).text();
        if (title.trim() === 'Opcje')
            $(this).html('');
        else
            $(this).html('<input type="text" style="width: 100%;" class = "form-control" placeholder=' + title + ' />');
    });
    var table;
    table = $(TableId).DataTable({
        columnDefs: [{ responsivePriority: 1, targets: nonResCol }],
        dom: "lrtip",
        processing: true,
        responsive: true,
        stateSave: true,
        stateDuration: 60 * 60,
        language: {
            processing: "Przetwarzanie...",
            search: "Szukaj:",
            lengthMenu: "Pokaż _MENU_ pozycji",
            info: "Pozycje od _START_ do _END_ z _TOTAL_",
            infoEmpty: "Pozycji 0 z 0 dostępnych",
            infoFiltered: "(filtrowanie spośród _MAX_ dostępnych pozycji)",
            infoPostFix: "",
            loadingRecords: "Wczytywanie...",
            zeroRecords: "Nie znaleziono pasujących pozycji",
            emptyTable: "Brak rekordów",
            paginate: {
                first: "Pierwsza",
                previous: "Poprzednia",
                next: "Następna",
                last: "Ostatnia"
            },
            aria: {
                sortAscending: ": aktywuj, by posortować kolumnę rosnąco",
                sortDescending: ": aktywuj, by posortować kolumnę malejąco"
            }

        }
    });

    // Apply the search
    table.columns().eq(0).each(function (colIdx) {
        $('input', table.column(colIdx).footer()).on('keyup change clear', function () {
            table
                .column(colIdx)
                .search(this.value)
                .draw();
        });
    });
}


function SetUpDataTableWithDefaultOrderAndNonResponsiveColumn(TableId, sortCol, sortDir, nonResCol) {
    sortCol = Number(sortCol);
    sortDir = String(sortDir);

    // Setup - add a text input to each footer cell
    $(TableId + ' tfoot th').each(function () {
        var title = $(this).text();
        if (title.trim() === 'Opcje')
            $(this).html('');
        else
            $(this).html('<input type="text" style="width: 100%;" class = "form-control" placeholder=' + title + ' />');
    });
    var table;
    table = $(TableId).DataTable({
        order: [[sortCol, sortDir]],
        columnDefs: [{ responsivePriority: 1, targets: nonResCol }],
        dom: "lrtip",
        processing: true,
        responsive: true,
        stateSave: false,
        language: {
            processing: "Przetwarzanie...",
            search: "Szukaj:",
            lengthMenu: "Pokaż _MENU_ pozycji",
            info: "Pozycje od _START_ do _END_ z _TOTAL_",
            infoEmpty: "Pozycji 0 z 0 dostępnych",
            infoFiltered: "(filtrowanie spośród _MAX_ dostępnych pozycji)",
            infoPostFix: "",
            loadingRecords: "Wczytywanie...",
            zeroRecords: "Nie znaleziono pasujących pozycji",
            emptyTable: "Brak rekordów",
            paginate: {
                first: "Pierwsza",
                previous: "Poprzednia",
                next: "Następna",
                last: "Ostatnia"
            },
            aria: {
                sortAscending: ": aktywuj, by posortować kolumnę rosnąco",
                sortDescending: ": aktywuj, by posortować kolumnę malejąco"
            }
        }
    });

    // Apply the search
    table.columns().eq(0).each(function (colIdx) {
        $('input', table.column(colIdx).footer()).on('keyup change clear', function () {
            table
                .column(colIdx)
                .search(this.value)
                .draw();
        });
    });
}
