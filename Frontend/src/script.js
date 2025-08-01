const API_BASE = '/api';
let jwtToken = '';

// Общая функция для запросов
async function makeRequest(url, method, body = null) {
    const headers = {
        'Content-Type': 'application/json'
    };
    
    if (jwtToken) {
        headers['Authorization'] = `Bearer ${jwtToken}`;
    }

    try {
        const response = await fetch(`${API_BASE}${url}`, {
            method,
            headers,
            body: body ? JSON.stringify(body) : null,
            credentials: 'include'
        });

        const contentType = response.headers.get('content-type');
        let data;
        
        if (contentType && contentType.includes('application/json')) {
            data = await response.json();
        } else {
            data = await response.text();
        }

        if (!response.ok) {
            throw new Error(data.message || data || `HTTP error! status: ${response.status}`);
        }

        return data;
    } catch (error) {
        console.error('Request failed:', error);
        throw error;
    }
}

// Обработчики действий
async function login() {
    try {
        const response = await makeRequest('/Login', 'POST', {
            name: document.getElementById('login-name').value,
            password: document.getElementById('login-password').value
        });
        
        jwtToken = response;
        showResponse(`Login successful! Token: ${jwtToken}`, 'success');
    } catch (error) {
        showResponse(`Login failed: ${error.message}`, 'error');
    }
}

async function register() {
    try {
        await makeRequest('/Register', 'POST', {
            name: document.getElementById('register-name').value,
            password: document.getElementById('register-password').value,
            roleName: document.getElementById('register-role').value
        });
        
        showResponse('Registration successful!', 'success');
    } catch (error) {
        showResponse(`Registration failed: ${error.message}`, 'error');
    }
}

async function ping() {
    try {
        const response = await makeRequest('/ping', 'GET');
        showResponse(`Ping response: ${response}`, 'success');
    } catch (error) {
        showResponse(`Ping failed: ${error.message}`, 'error');
    }
}

async function testAdmin() {
    try {
        const response = await makeRequest('/TestAdmin', 'GET');
        showResponse(`Admin test passed: ${response}`, 'success');
    } catch (error) {
        showResponse(`Admin test failed: ${error.message}`, 'error');
    }
}

// Вспомогательные функции
function showResponse(message, type) {
    const responseBox = document.getElementById('response');
    responseBox.textContent = message;
    responseBox.className = `response-box ${type}`;
}

// Проверка соединения при загрузке
window.onload = async () => {
    try {
        await ping();
    } catch (error) {
        showResponse('Backend connection failed', 'error');
    }
};