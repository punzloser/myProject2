function day(){
    document.querySelector('p.day').innerHTML = Date();
};

//set hover css
let card = document.querySelectorAll('.laptop .card');
for(let i = 0; i < card.length; i++){
    card[i].addEventListener('mouseover', function(){
        this.style.color = 'red';
    });
    card[i].addEventListener('mouseout', function(){
        this.style.color = '';
    });
};
/*------------------------------------------------------*/ 

/*shopping cart*/ 

//step:
//1
let carts = document.querySelectorAll('.btn-cart');

for(let i = 0; i < carts.length; i++){
    carts[i].addEventListener('click', ()=>{
        cartNumbers(products[i]);
        totalCost(products[i]);
    })
}

//2
function cartNumbers(product){
    //localStorage : in chrome
    let productNumbers = localStorage.getItem('item');

    //default value is string
    productNumbers = parseInt(productNumbers);
    
    //check exist product
    if(productNumbers){
        localStorage.setItem('item', productNumbers + 1);
        //because secure should not use innerHTM
        document.querySelector('.nav-item span').textContent = productNumbers + 1;
    }
    else{
        localStorage.setItem('item', 1);
        document.querySelector('.nav-item span').textContent = 1;
    }

    setItems(product); //6
}

//4 load again when press f5
function loadCartNumbers(){
    let productNumbers = localStorage.getItem('item');

    if (productNumbers) {
        document.querySelector('.nav-item span').textContent = productNumbers;
    }
}

loadCartNumbers();

//5 list of products - test 3 laptop items - it called string object
let products = [
    {
        name: 'Lenovo IdeaPad 3',
        tag: '81WE0132VN',
        price: 11790000,
        inCart: 0
    },
    {
        name: 'Lenovo ThinkBook',
        tag: '20SM00D9VN',
        price: 11490000,
        inCart: 0
    },
    {
        name: 'Lenovo ThinkBook 14s',
        tag: '20VA000NVN',
        price: 19990000,
        inCart: 0
    }
];

//6 set item function
function setItems(product){
    let cartItems = localStorage.getItem('productsInCart');
    //overwrite and convert json
    cartItems = JSON.parse(cartItems);

    //check exists
    if (cartItems != null) {
        
        if (cartItems[product.tag] == undefined) {
            cartItems = {
                ...cartItems, //rest operator in js
                [product.tag]: product
            }
        }
        cartItems[product.tag].inCart += 1;
    } else {
        //update product incart 1
        product.inCart = 1;
        //create variable cart items
        cartItems = {
            [product.tag]: product
        }
    }

    localStorage.setItem('productsInCart', JSON.stringify(cartItems));
}

//7 total cost function
function totalCost(product){
    let cartCost = localStorage.getItem('totalCost');
    
    if (cartCost != null) {
        cartCost = parseInt(cartCost);
        localStorage.setItem('totalCost', cartCost + product.price);
    } else {
        localStorage.setItem('totalCost', product.price);
    }
    // test
    console.log('total is: ', cartCost);
    console.log(typeof cartCost);
}

//8 display cart
function displayCart(){
    let cartItems = localStorage.getItem('productsInCart');
    cartItems = JSON.parse(cartItems);
    let cartCost = localStorage.getItem('totalCost');
    //test
    // console.log(cartItems);

    let cartSection = document.querySelector('.cart');
    //check if exists cart item and this section
    if (cartItems && cartSection) {
        // console.log('ok');
        
        //check values inside current loop
        Object.values(cartItems).map(item => {
            cartSection.innerHTML += 
            //best way to inject html using js with ``
            `
            <div class="col-12 col-md-3">
                <button style="border: none; background: white;">
                <svg class="text-muted" xmlns="http://www.w3.org/2000/svg" width="25" height="25" fill="currentColor" class="bi bi-x-circle-fill" viewBox="0 0 16 16">
                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293 5.354 4.646z"/>
                </svg>
                </button>
                <img class="card-img w-75 py-2" src="img/${item.tag}.jpg">
            </div>
            
            <div class="col-12 col-md-6 text-center">
                <span>${item.name}</span>
                <span class="d-block">
                    <button style="border: none; background: white;">
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-dash-square" viewBox="0 0 16 16">
                    <path d="M14 1a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h12zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z"/>
                    <path d="M4 8a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7A.5.5 0 0 1 4 8z"/>
                    </svg>
                    </button>

                    <span class="text-primary">${item.inCart}</span>

                    <button style="border: none; background: white;">
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-plus-square" viewBox="0 0 16 16">
                    <path d="M14 1a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h12zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z"/>
                    <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
                    </svg>
                    </button>
                </span>
            </div>
            <div class="col-12 col-md-3 text-end text-danger">
                <span>${item.price} VND</span>
            </div>
            `;
        });
        cartSection.innerHTML += 
        `
        <hr style="width: 98%;" class="mx-auto">
        <div class="row row-cols-1 row-cols-md-2 m-0">
            <div class="col">Tổng tiền</div>

            <div class="col text-end p-0 text-danger">
                <span>${cartCost} VND</span>
            </div>
        </div>
        <a href="payment.html" class="btn btn-primary w-25 mx-auto">Thanh toán</a>
        `
    }
}
displayCart();

