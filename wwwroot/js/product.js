async function updateProduct(e) {
    const btnBuy = e.target;
    console.log(btnBuy);
}

async function getProducts(token) {
    const auth_token = token;
    const url = `https://localhost:7260/api/product`;
    fetch(url, {
        method: 'GET',
        headers: {
            "Authorization": `Bearer ${auth_token}`,
            "Content-Type": "application/json"
        }
    })
        .then(res => {
            if (!res.ok) {
                throw new Error(`Error: ${res.status}`)
            }
            return res.json();
        })
        .then(products => {
            const parent_div = document.getElementById("product_list")
            parent_div.innerHTML = "";
            products.forEach(product => {
                const div = document.createElement("div")
                div.className = "product_item"
                div.innerHTML = `
                <img src="${product.image || './img/image-404.jpg'}" width="200px" alt="${product.name}">
                <h3>${product.name}</h3>
                <p>${product.description}</p>
                <strong>${product.price}</strong>
                <button>Buy</button>
            `
                parent_div.appendChild(div);
            });
        })
}

async function makeOrder() {
    await fetch(
        "https://localhost:7260/api/order",
        {
            method: 'POST',
            headers: {
                "Authorization": `Bearer ${auth_token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "email": email,
                "password": password
            })
        }).then(response => {
            if (!response.ok) {
                throw new Error(`Error auth: ${response.status}`)
            }
            return response.json()
        }).then(data => {

        }).catch(err => console.log(err))
}