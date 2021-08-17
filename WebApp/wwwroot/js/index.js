////function day() {
////    document.querySelector('p.day').innerHTML = Date();
////};

//set hover css
let card = document.querySelectorAll('.laptop .card');
for (let i = 0; i < card.length; i++) {
    card[i].addEventListener('mouseover', function () {
        this.style.color = 'red';
    });
    card[i].addEventListener('mouseout', function () {
        this.style.color = '';
    });
};

/*------------------------------------------------------*/
$('body').on('click', '.btn-cart', function (e) {
    e.preventDefault();
    const culture = $('#getCulture').val();
    const id = $(this).data('id');

    $.ajax({
        type: "POST",
        url: "/" + culture + '/Cart/AddCart',
        data: {
            id: id,
            languageId: culture
        },
        success: function (res) {
            $('#countCart').text(res.length);
        },
        error: function (err) {
            console.log(err);
        }
    });
})

function loadData() {
    const culture = $('#getCulture').val();
    $.ajax({
        type: "GET",
        url: "/" + culture + '/Cart/GetList',
        success: function (res) {
            $('#countCart').text(res.length != 0 ? res.length : "");
        }
    });
}

loadData();

// js
//var addClick = document.querySelectorAll('.btn-cart');
//for (var i = 0; i < addClick.length; i++) {
//    var valueClick = addClick[i];
//    valueClick.addEventListener('click', callAjax);
//}

//function callAjax() {
//    const culture = document.getElementById('getCulture').value;
//    const id = this.getAttribute('data-id');

//    var r = new XMLHttpRequest();
//    var u = '/' + culture + '/Cart/AddCart';

//    r.open('POST', u, true);
//    r.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');
//    r.onreadystatechange = function () {
//        if (r.readyState == 4 && r.status == 200) {
//            alert(r.responseText);
//        }
//    };
//    var d = JSON.stringify({ id: id, languageId: culture });
//    r.send(d);
//};
