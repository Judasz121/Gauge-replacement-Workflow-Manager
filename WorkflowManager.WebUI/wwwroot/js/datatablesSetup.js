$(document).ready(function () {
    var i;
    var dataTables = document.querySelectorAll("table[id^=dataTable");
    console.log(dataTables);
    for (let table of dataTables) {
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

            SetUpDataTableWithDefaultOrderAndNonResponsiveColumn(table, sortCol, sortDir, nonResCol);
            continue;
        }
        else if (table.className.match(defaultSortClassRegex) != null) {
            let targetClass = table.className.match(defaultSortClassRegex)[0];
            targetClass = targetClass.split('-');
            let sortDir = targetClass[2];
            let sortCol = targetClass[1];
            SetUpDataTableWithDefaultOrder(table, sortCol, sortDir);
            continue;
        }
        else if (table.className.match(nonResponsiveClassRegex) != null) {
            let targetClass = table.className.match(nonResponsiveClassRegex)[0];
            targetClass = targetClass.split('-');
            let nonResCol = targetClass[1];
            SetUpDataTableWithNonResponsiveColumn(table, nonResCol);
            continue;
        }
        else {
            SetUpDataTable(table + i);
            continue;
        }
        
    }
});



// normal datatable
function SetUpDataTable(Table) {
    // add a text input to each footer cell
    $('#' + Table.id + ' tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" style="width: 100%;" class = "form-control" placeholder=' + title + ' />');
    });

    var dataTable = $(Table).DataTable({
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
    dataTable.columns().eq(0).each(function (colIdx) {
        $('input', dataTable.column(colIdx).footer()).on('keyup change clear', function () {
            dataTable
                .column(colIdx)
                .search(this.value)
                .draw();
        });
    });
}

// datatable with specific column sorted on render
function SetUpDataTableWithDefaultOrder(Table, sortCol, sortDir) {
    sortCol = Number(sortCol);
    sortDir = String(sortDir);
    
    // add a text input to each footer cell
    $('#' + Table.id + ' tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" style="width: 100%;" class = "form-control" placeholder=' + title + ' />');
    });

    var dataTable = $(Table).DataTable({
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
    dataTable.columns().eq(0).each(function (colIdx) {
        $('input', dataTable.column(colIdx).footer()).on('keyup change clear', function () {
            dataTable
                .column(colIdx)
                .search(this.value)
                .draw();
        });
    });
}

// datatable with column that cannot be hidden on resize
function SetUpDataTableWithNonResponsiveColumn(Table, nonResCol) {
    nonResCol = Number(nonResCol);
    // add a text input to each footer cell
    $('#' + Table.id + ' tfoot th').each(function () {
        var title = $(this).text();
        if (title.trim() === 'Opcje')
            $(this).html('');
        else
            $(this).html('<input type="text" style="width: 100%;" class = "form-control" placeholder=' + title + ' />');
    });

    var dataTable = $(Table).DataTable({
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
    dataTable.columns().eq(0).each(function (colIdx) {
        $('input', dataTable.column(colIdx).footer()).on('keyup change clear', function () {
            dataTable
                .column(colIdx)
                .search(this.value)
                .draw();
        });
    });
}


function SetUpDataTableWithDefaultOrderAndNonResponsiveColumn(Table, sortCol, sortDir, nonResCol) {
    sortCol = Number(sortCol);
    sortDir = String(sortDir);

    // Setup - add a text input to each footer cell
    $('#' + Table.id + ' tfoot th').each(function () {
        var title = $(this).text();
        if (title.trim() === 'Opcje')
            $(this).html('');
        else
            $(this).html('<input type="text" style="width: 100%;" class = "form-control" placeholder=' + title + ' />');
    });

    var dataTable = $(Table).DataTable({
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
    dataTable.columns().eq(0).each(function (colIdx) {
        $('input', dataTable.column(colIdx).footer()).on('keyup change clear', function () {
            dataTable
                .column(colIdx)
                .search(this.value)
                .draw();
        });
    });
}
