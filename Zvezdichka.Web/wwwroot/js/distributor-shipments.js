function addCreateProductRow(rowNum) {
    var html =
        '<tr>' +
            '<td>' +
            (rowNum + 1) +
            '</td>' +
            '<td>' +
            `<input name="Products[${rowNum}].Name"/>` +
            '</td>' +
            '<td>' +
            `<input name="Products[${rowNum}].Quantity" type="number"/>` +
            '</td>' +
            '<td>' +
            `<input name="Products[${rowNum}].Price" type="number"/>` +
            '</td>' +
            '<td>' +
            `<input name="Products[${rowNum}].DiscountPercentage" type="number"/>` +
            '</td>' +
            '<td>' +
            '<a class="btn btn-success" onclick="addCreateProductRow(' +
            (rowNum + 1) +
            ')">Add</a>' +
            '</td>' +
            '</tr>';


    var row = $(html);
    $('#delivery-products').append(row);
}

function addEditProductRow(rowNum) {
    var html =
        '<tr>' +
            '<td>' +
            (rowNum + 1) +
            '</td>' +
            '<td>' +
            `<input name="Products[${rowNum}].Name"/>` +
            '</td>' +
            '<td>' +
            `<input name="Products[${rowNum}].Quantity" type="number"/>` +
            '</td>' +
            '<td>' +
            `<input name="Products[${rowNum}].Price" type="number"/>` +
            '</td>' +
            '<td>' +
            `<input name="Products[${rowNum}].DiscountPercentage" type="number"/>` +
            '</td>' +
            '<td>' +
            '<a class="btn btn-success" onclick="addCreateProductRow(' +
            (rowNum + 1) +
            ')">Add</a>' +
            '</td>' +
            '</tr>';

    var row = $(html);
    $('#delivery-products').append(row);
}