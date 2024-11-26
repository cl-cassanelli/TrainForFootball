// wwwroot/js/login.js
function saveLoginData(token, username) {
    localStorage.setItem('token', token);
    localStorage.setItem('username', username);
}

async function login(username, password) {
    const response = await fetch('/ControllerName/Login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password }),
    });
    const data = await response.json();
    if (data.success) {
        //saveLoginData(data.token, data.username);
        console.log(data)
        alert('Login riuscito!');
    } else {
        alert(data.message);
    }
}
