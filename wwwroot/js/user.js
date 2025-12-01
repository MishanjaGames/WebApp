var user = { t: "", n: "", em: "" }

async function register(mail, pass) {
    console.log(mail, '|', pass)
    const email = mail || document.getElementById("regEmail").value;
    const password = pass || document.getElementById("regPass").value;
    if (!email || !password) {
        throw new Error(`email or password are ampty ...`);
    }
    await fetch(
        "https://localhost:7260/api/user/register",
        {
            method: "POST",
            headers: {
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
            //localStorage.setItem("token", data.token.result)
            user.t = data.token.result
            localStorage.setItem("u", JSON.stringify(user))
            //console.log(data.token.result)
            confirm('Registration complete')
            login(email, password)
            //return data.token.result;
        }).catch(err => console.log(err))
}

async function login(mail, pass) {
    console.log(mail, '|', pass)
    const email = mail || document.getElementById("logEmail").value;
    const password = pass || document.getElementById("logPass").value;
    if (!email || !password) {
        throw new Error(`email or password are ampty ...`);
    }
    await fetch(
        "https://localhost:7260/api/user/login",
        {
            method: "POST",
            headers: {
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
            //localStorage.setItem("token", data.token.result)
            user.t = data.token.result
            console.log(parseJwt(user.t));
            user.n = parseJwt(user.t).unique_name
            user.em = parseJwt(user.t).email
            localStorage.setItem("u", JSON.stringify(user))
            console.log(data.token.result)
            //confirm('You have logged in :)')

            //return data.token.result;
        }).catch(err => console.log(err))
}

function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(
            atob(base64).split('').map(c =>
                '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
            ).join('')
        );
        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}

async function user_profile() {
    const user_profile_div = document.getElementById("user_profile");

    // Отримуємо токен (наприклад, з localStorage)
    const token = user.t// || localStorage.getItem('u').t;
    if (!token) {
        user_profile_div.innerHTML = `<p style="color:red;">JWT токен не знайдено</p>`;
        return;
    }

    const userData = parseJwt(token);
    if (!userData) {
        user_profile_div.innerHTML = `<p style="color:red;">Помилка при розборі токена</p>`;
        return;
    }

    // Формуємо HTML картки профілю
    let fields = '';
    for (const [key, value] of Object.entries(userData)) {
        fields += `
      <div style="display: flex; justify-content: space-between; border-bottom: 1px solid #ccc; padding: 4px 0;">
        <strong>${key}</strong>
        <span>${value}</span>
      </div>
    `;
    }

    user_profile_div.innerHTML = `
    <div style="
      max-width: 400px;
      margin: 20px auto;
      padding: 16px;
      border-radius: 12px;
      background: #f9f9f9;
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
      font-family: Arial, sans-serif;
    ">
      <h2 style="text-align:center; color:#333;">Профіль користувача</h2>
      ${fields}
    </div>
  `;
}

async function session_user() {
    const token = user.t// || localStorage.getItem('u').t;
    const payload = parseJwt(token);

    console.log('Це користувач:', payload);
    if (token) {
        var user_profile_div = document.getElementById("user_profile")
        user_profile_div.style.display = "block";
        await user_profile()
        var box_register = document.querySelector(".box_register")
        box_register.style.display = "none";
        var box_login = document.querySelector(".box_login")
        box_login.style.display = "none";
    }

}

async function logout() {
    localStorage.removeItem("jwt_token");
    window.location.reload()
}

async function showPage(i) {
    if (i === 0) {
        document.querySelector('header').style.display = 'none';
        document.querySelector('.box_shop').style.display = 'none';
        document.querySelector('.box_data').style.display = 'none';
    }
    else if (i === 1) {
        document.querySelector('header').style.display = 'block';
        document.querySelector('.box_shop').style.display = 'block';
        document.querySelector('.box_auth').style.display = 'none';
    }
    else if (i === 2) {
        document.querySelector('.box_shop').style.display = 'none';
        document.querySelector('.box_data').style.display = 'block';
    }
}

async function init() {
    if (localStorage == null) {
        await login()
        user = localStorage.getItem('u')
        console.log(user)
    }
    else {
        user.t = localStorage.getItem('t')
        user.n = localStorage.getItem('n')
        user.em = localStorage.getItem('em')
        document.getElementById("user_name").value = user.n
        document.getElementById("user_email").value = user.em
    }
}

document.addEventListener('DOMContentLoaded', () => {
    var user_profile = document.querySelector(".user_profile")
    user_profile.style.display = 'none';
    session_user()
})

async function main() {
    showPage(0)
    init()
    const token = await login("admin123", "admin123")
    session_user()
    getProducts(user.t)
}

main()