function openNav() {
    document.getElementById("mySidebar").style.width = "50%";
    // document.getElementById("main").style.marginLeft = "0px";
}

function closeNav() {
    document.getElementById("mySidebar").style.width = "0";
    // document.getElementById("main").style.marginLeft = "0";
}

// Database

var product = {
    products: {
        1: {
            id: 1,
            title: "Breakfast",
            price: 20,
            description: "Fast food",
            category: "food",
            img: "./images/p1.png",
        },

        2: {
            id: 2,
            title: "Burger",
            price: 10,
            description: "Burger with french fries",
            category: "food",
            img: "./images/p2.png",
        },
        3: {
            id: 3,
            title: "Toast",
            price: 20,
            description: "Toast with creamy butter",
            category: "food",
            img: "./images/p3.png",
        },

        4: {
            id: 4,
            title: "Noodles",
            price: 10,
            description: "Soupy Noodles ",
            category: "food",
            img: "./images/p4.png",
        },
    },
};

var cart = {
    carts: [],
};

// Add to card
function addCart(event) {
    let value = event.target.getAttribute("value");
    cart.carts.push(product.products[value]);
    addToCart(product.products[value]);
}

// Display Home cards
const cardMaker = (prod) => {
    let output = `
             <div class="col-md-3 p-2">
                <div class="card" style="width: 16rem;">
                    <h5 class="bg-danger price">${prod.price}$</h5>
                    <img src="${prod.img}" class="card-img-top" alt="..."> 
                    <div class="card-body text-center">
                        <h5 class="card-title">${prod.title}</h5>
                        <p class="card-text">${prod.description}</p>
                        <a onclick="addCart(event)" value="${prod.id}"class="btn btn-primary">Add to cart</a>
                    </div>
                </div>
            </div>
            
    `;
    return output;
};
const cardRender = () => {
    let card = document.querySelector(".cardRender");

    var products = [];

    for (var prodId in product.products) {
        products.push(product.products[prodId]);
    }

    const rowStart = `<div class="row">`;
    const rowEnd = `</div>`;
    let result = rowStart;
    for (let prod of products) {
        result += cardMaker(prod);
    }
    result += rowEnd;

    card.innerHTML = result;
};

cardRender();

const cardRenderCond = (category) => {
    let card = document.querySelector(".cardRender");

    var products = [];

    for (var prodId in product.products) {
        if (category === product.products[prodId].category) {
            products.push(product.products[prodId]);
        }
    }

    const rowStart = `<div class="row">`;
    const rowEnd = `</div>`;
    let result = rowStart;
    for (let prod of products) {
        result += cardMaker(prod);
    }
    result += rowEnd;

    card.innerHTML = result;
};

// Display carts
totalItems = 0;

var getitem = document.querySelector(".totalItems");
getitem.innerText = totalItems;

totalPrice = 0;

var getpr = document.querySelector(".totalPrice");
getpr.innerText = totalPrice;

for (let i = 0; i < cart.carts.length; i++) {
    console.log(cart.carts[i]);
}

function del(itemId, cartId, item_price) {
    totalItems--;
    getitem.innerText = totalItems;
    totalPrice -= item_price;
    getpr.innerText = totalPrice;
    document.getElementById(itemId).remove();

    for (let i = 0; i < cart.carts.length; i++) {
        if (cart.carts[i].id === cartId) {
            cart.carts.splice(i, 1);
            break;
        }
    }
}

function addToCart(item) {
    var itemId = totalItems + 1;
    var selectdItem = document.createElement("div");
    selectdItem.classList.add("card");
    selectdItem.classList.add("m-2");
    selectdItem.classList.add("text-center");
    selectdItem.setAttribute("id", itemId);
    selectdItem.setAttribute("style", "width: 12rem;");
    var img = document.createElement("img");
    img.classList.add("card-img-top");
    img.setAttribute("src", item.img);
    var title = document.createElement("h5");
    title.classList.add("card-title");
    title.innerText = item.title;
    var price = document.createElement("h6");
    price.classList.add("card-title");
    price.innerText = "Price: " + item.price;
    var del = document.createElement("div");
    del.innerText = "Remove";
    del.classList.add("btn");
    del.classList.add("btn-danger");
    del.classList.add("m-2");
    del.setAttribute(
        "onclick",
        "del(" + itemId + " ," + item.id + " ," + item.price + ")"
    );

    var cardItem = document.querySelector(".cartCard");
    selectdItem.append(img);
    selectdItem.append(title);
    selectdItem.append(price);
    selectdItem.append(del);
    cardItem.append(selectdItem);

    totalPrice += item.price;
    totalItems++;
    getitem.innerText = totalItems;
    getpr.innerText = totalPrice;
}