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
            return await responce.json();
        }
        else {
            chrome.storage.local.remove(['token']);
            chrome.runtime.sendMessage({ type: 'TOKEN_EXPIRED' });
            return undefined;
        }
    }

}

async function main() {
    var login = document.getElementById('signup-modal-email');
    
    if (login != undefined) {
        debugger;
        login.addEventListener('focus', async () => {
            var data = await getData();
            if (data != undefined) {
                login.value = data.login;
            }
        });
    }

    
}

chrome.runtime.onMessage.addListener((message) => {
    if (message.type === 'NEW_TOKEN_RECEIVED') {
        main(); 
    }
});


main();
