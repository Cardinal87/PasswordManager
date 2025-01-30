function getToken(key) {
    return new Promise((resolve) => {
        chrome.storage.local.get(['token'], (result) => {
            resolve(result[key]);
        });
    });
}

async function checkToken(token) {
    
    var payload = token.split('.')[1];
    const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
    if (!decoded) {
        console.error("incorrect token format");
    }
    const time = Math.floor(Date.now() / 1000);
    if (decoded.exp && decoded.payload.exp < currentTime) {

        return true;
    }
    else {
        return false;
    }
    
    
}

async function getData() {
    const currentUrl = window.location.href;
    var token = await getToken("token");
    if (token != undefined) {
        var b = checkToken(token);
        if (b) {
            var responce = await fetch('http://localhost:5167/api/login/user?url=${encodeURIComponent(currentUrl)}', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': token
                }
            });
            var data = await responce.json();
            chrome.runtime.sendMessage({ message: 'SELECT_USER', users: data});
        }
        else {
            chrome.storage.local.remove(['token']);
            chrome.runtime.sendMessage({ message: 'TOKEN_EXPIRED' });
        }
    }

}

async function main() {
    var pass = document.querySelector('[type="password"]');
    
    if (pass != undefined) {
        
        pass.addEventListener('focus', async () => {
            await getData();
        });
    }

    
}

chrome.runtime.onMessage.addListener(async (message) => {
    if (message.type === 'NEW_TOKEN_RECEIVED') {
        await getData();
    }
});

chrome.runtime.onMessage.addListener((message) => {
    if (message.type === 'USER_SELECTED') {
        var pass = document.querySelector('[type="password"]');
        pass.value = message.password;
    }
});


main();
