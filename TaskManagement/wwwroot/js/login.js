document.getElementById('loginForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {
        const response = await axios.post('https://localhost:7079/api/auth/login', {
            username: username,
            password: password
        });

        if (response.status === 200) {
            const token = response.data.token;
            localStorage.setItem('token', token);
            window.location.href = '/Home/Index';
        }
    } catch (error) {
        alert('Login failed');
    }
});