
$(document).ready(function () {
    $(document).on('input', '#QuantityToSell, #Discount', function () {
        calculateSellingPrice();
    });
    $('#sellItemBtn').on('click', function (e) {

        Swal.fire({
            title: "Processing...",
            html: "Please wait while we process your request.",
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
                const timer = Swal.getPopup().querySelector("b");
            },
        });
        submitForm();

    });

});

function submitForm() {
    debugger;
    var model = {
        ProductId: document.getElementById('ProductId').value,
        CustomerName: document.getElementById('CustomerName').value,
        CustomerPhoneNumber: document.getElementById('CustomerPhoneNumber').value,
        CustomerEmail: document.getElementById('CustomerEmail').value,
        Address: document.getElementById('Address').value,
        QuantityToSell: document.getElementById('QuantityToSell').value,
        SellingPrice: document.getElementById('SellingPrice').value
    };
    $.ajax({
        url: '/Selles/SellItem',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (response) {
            Swal.close();
            Swal.fire({
                icon: response.success ? 'success' : 'error',
                title: response.message,
                timer: 9000,
                showConfirmButton: false
            });
        },
        error: function (error) {
            console.error('Error selling item:', error);
        }
    });
};


var suggestionsContainer = $("#suggestionsContainer");
var employeeNameInput = $("#CustomerName");

employeeNameInput.on('input', function () {
    var searchTerm = $(this).val();
    $('#CustomerPhoneNumber').val('');
    $('#CustomerEmail').val('');
    $('#Address').val('');
    if (searchTerm.length >= 2) {
        $.ajax({
            url: "/Selles/GetCustomerSuggestions",
            type: "GET",
            data: { term: searchTerm },
            success: function (data) {
                suggestionsContainer.empty();
                if (data.length > 0) {
                    var suggestionsList = $("<ul>");

                    data.forEach(function (customer) {
                        var listItem = $("<li>").text(customer.label);
                        listItem.click(function () {
                            $('#CustomerName').val(customer.value);
                            $('#CustomerPhoneNumber').val(customer.phoneNumber); // Assuming 'phoneNumber' is returned by the server
                            $('#CustomerEmail').val(customer.email); // Assuming 'email' is returned by the server
                            $('#Address').val(customer.address); // Assuming 'address' is returned by the server
                            suggestionsContainer.empty();
                        });

                        suggestionsList.append(listItem);
                    });

                    suggestionsContainer.append(suggestionsList);
                }
            },
        });
    }
    else {
        suggestionsContainer.empty();
    }
});

$(document).on("click", function (event) {
    if (!$(event.target).closest(employeeNameInput).length && !$(event.target).closest(suggestionsContainer).length) {
        suggestionsContainer.empty();
    }
});

suggestionsContainer.on("click", function (event) {
    event.stopPropagation();
});

function getSellingPrice(productId) {
    $.ajax({
        url: '/Selles/GetSellingPrice',
        type: 'GET',
        data: { productId: productId },
        success: function (result) {
            $('#UnitPrice').val(result);
            calculateSellingPrice();
        },
        error: function (error) {
            console.error('Error fetching selling price:', error);
        }
    });
};

function calculateSellingPrice() {
    var quantity = parseFloat($('#QuantityToSell').val()) || 0;
    var discountAmount = parseFloat($('#Discount').val()) || 0;
    var unitPrice = parseFloat($('#UnitPrice').val()) || 0;

    if (!isNaN(quantity) && !isNaN(discountAmount)) {
        var totalPrice = quantity * unitPrice - discountAmount;
        $('#SellingPrice').val(totalPrice.toFixed(2));
    } else if (!isNaN(quantity)) {
        $('#SellingPrice').val((quantity * unitPrice).toFixed(2));
    } else if (!isNaN(discountAmount)) {
        $('#SellingPrice').val((unitPrice - discountAmount).toFixed(2));
    } else {
        $('#SellingPrice').val('0.00');
    }
};

