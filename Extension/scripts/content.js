function getToken(key) {
    return new Promise((resolve) => {
        chrome.storage.local.get(['token'], (result) => {
            resolve(result[key]);
        });
    }).catch((error) => console.log(error));
}

function checkToken(token) {
    
    var payload = token.split('.')[1];
    const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
    const json = JSON.parse(decoded);
    if (!decoded) {
        console.error("incorrect token format");
    }
    const time = Math.floor(Date.now() / 1000);
    var exp = json.exp;
    if (exp && exp > time) {

        return true;
    }
    else {
        return false;
    }
    
    
}

async function getData() {
    const currentUrl = window.location.href;
    var token = await getToken("token");
    debugger;
    if (token != undefined) {
        var b = checkToken(token);
        if (b) {
            var params = new URLSearchParams();
            params.append("url", currentUrl);
            var conStr = "https://localhost:5167/api/authorization/get?url=" + params;
            var responce = await fetch(conStr, {
                method: 'GET',
                headers: {
                    'Authorization': "Bearer " + token,
                }
            }).catch((error) => console.log(error));
            debugger;
            var data = await responce.json();
            chrome.runtime.sendMessage({ type: 'SELECT_USER', users: data });
        }
        else {
            chrome.storage.local.remove(['token']);
            chrome.runtime.sendMessage({ type: 'TOKEN_EXPIRED' });
        }
    }
    else {
        chrome.runtime.sendMessage({ type: 'TOKEN_EXPIRED' });
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


chrome.runtime.onMessage.addListener((message) => {
    if (message.type === 'USER_SELECTED') {
        var pass = document.querySelector('[type="password"]');
        pass.value = message.user.password;
    }
});


main();
