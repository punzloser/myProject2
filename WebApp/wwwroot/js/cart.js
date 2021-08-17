

function loadData() {
    const culture = $('#getCulture').val();

    $.ajax({
        type: "GET",
        url: "/" + culture + '/Cart/GetList',
        error: function (err) {
            console.log(err);
        },
        success: function (res) {
            if (res.length === 0) {
                $('#cartContainer').remove();
            }

            var html = '';
            var total = 0;

            $.each(res, function (i, item) {
                var amount = item.price * item.quantity;
                html +=
                    `
                    <div class="col-12 col-md-3">
                        <button class="btn-del" data-id="${item.productId}" style="border: none; background: white;">
                            <svg class="text-muted" xmlns="http://www.w3.org/2000/svg" width="25" height="25" fill="currentColor" class="bi bi-x-circle-fill" viewBox="0 0 16 16">
                                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293 5.354 4.646z" />
                            </svg>
                        </button>
                        <img class="card-img w-75 py-2" src="${$('#getBaseAddress').val() + item.img}">
                    </div>

                    <div class="col-12 col-md-6 text-center">
                        <span>${item.name}</span>
                        <span class="d-block">
                            <button class="btn btn-minus" data-id="${item.productId}" style="border: none; background: white;">
                                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-dash-square" viewBox="0 0 16 16">
                                    <path d="M14 1a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h12zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z" />
                                    <path d="M4 8a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7A.5.5 0 0 1 4 8z" />
                                </svg>
                            </button>

                            <span class="text-primary" id="quantity-${item.productId}">${item.quantity}</span>

                            <button class="btn btn-plus" data-id="${item.productId}" style="border: none; background: white;">
                                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-plus-square" viewBox="0 0 16 16">
                                    <path d="M14 1a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h12zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z" />
                                    <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z" />
                                </svg>
                            </button>
                        </span>
                    </div>
                    <div class="col-12 col-md-3 text-end text-danger">
                        <span>${numberWithCommas(item.price)} VND</span>
                    </div>
                    `

                total += amount;
            });

            $('#cart').html(html);
            $('#total').html(numberWithCommas(total) + ' VND');
        }
    });
}


function cartStaging() {
    $('body').on('click', '.btn-plus', function (e) {
        e.preventDefault();

        const id = $(this).data('id');
        var quantity = parseInt($('#quantity-' + id).text()) + 1;
        //c2 by me
        //span class="text-primary quantity" data-id="${item.productId}" > ${ item.quantity }</span >
        //var quantity = parseInt($(`.quantity[data-id="${id}"]`).text()) + 1;
        editOrRemove(id, quantity);
    });

    $('body').on('click', '.btn-minus', function (e) {
        e.preventDefault();

        const id = $(this).data('id');
        var quantity = parseInt($('#quantity-' + id).text()) - 1;

        editOrRemove(id, quantity);
    });

    $('body').on('click', '.btn-del', function (e) {
        e.preventDefault();

        const id = $(this).data('id');
        editOrRemove(id, 0);
    });
}

function editOrRemove(id, quantity) {
    const culture = $('getCulture').val();

    $.ajax({
        type: 'POST',
        url: '/' + culture + '/Cart/Edit',
        data: {
            id: id, quantity: quantity
        },
        error: function (err) {
            console.log(err);
        },
        success: function (res) {
            $('#countCart').text(res.length);
            if (quantity >= 10) {
                alert('Bạn không được spam trên 10 sp');
                return;
            }
            loadData();
        }
    });
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

loadData();
cartStaging();