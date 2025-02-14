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
    var token = await getToken("token")
    if (token != undefined) {
        var b = checkToken(token);
        if (b) {
            var params = new URLSearchParams();
            params.append("url", currentUrl);
            var conStr = "http://localhost:5167/api/authorization/get?" + params;
            var responce = await fetch(conStr, {
                method: 'GET',
                headers: {
                    'Authorization': "Bearer " + token,
                }
            }).catch((error) => console.error(error));
            if (responce.status == 401) {
                chrome.storage.local.remove(['token']);
                chrome.runtime.sendMessage({ type: 'TOKEN_EXPIRED' });
            }
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
    
    var attemps = 0;
    var pass = document.querySelector('input[type="password"]');
    if (attemps === 5) return;
    if (pass != undefined) {

        const wrapper = document.createElement('div');
        wrapper.className = 'btnContainer';
        var sibling = pass.nextElementSibling;
        var button = document.createElement('button');
        button.innerHTML = 'insert';
        button.className = 'insertbtn';
        if (sibling != undefined && sibling.matches('button[type=button]')) {
            var s_right = parseFloat(getComputedStyle(sibling).right);
            var s_width = parseFloat(getComputedStyle(sibling).width);
            var res = 5 + s_right + s_width;
            button.style.right = res + 'px';
        }
        else {
            button.style.right = '5px';
        }

        pass.parentNode.insertBefore(wrapper, pass);
        wrapper.appendChild(button);
        wrapper.appendChild(pass);

        button.addEventListener('click', async () => {
            await getData();
        });
        return;
    }
    attemps += 1;
    setTimeout(main, 500);
}


chrome.runtime.onMessage.addListener((message) => {
    if (message.type === 'USER_SELECTED') {
        var pass = document.querySelector('input[type="password"]');
        pass.value = message.user.password;
    }
});


main();
