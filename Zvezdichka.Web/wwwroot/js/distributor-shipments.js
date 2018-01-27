function addProductRow(rowNum) {

    var html =
        '<tr>' +
            '<td>' +
            (rowNum + 1) +
            '</td>' +
            '<td>' +
            '<input name="Name" />' +
            '</td>' +
            '<td>' +
            '<input name="Quantity" type="number" />' +
            '</td>' +
            '<td>' +
            '<input name="Price" type="number" />' +
            '</td>' +
            '<td>' +
            '<input name="DiscountPercentage" type="number"/>' +
            '</td>' +
            '<td>' +
            '<a class="btn btn-success" onclick="addProductRow(' +
            (rowNum + 1) +
            ')">Add</a>' +
            '</td>' +
            '</tr>';

    var row = $(html);
    $('#delivery-products').append(row);
}