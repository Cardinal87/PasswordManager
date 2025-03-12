var attemps = 0;
var pass_f;
var login_f;
var submit_b;

function findSubmitButton() {
    const candidates = Array.from(document.querySelectorAll(
        'button, input[type="submit"], input[type="button"], [role="button"]'
    ));

    const scoreButton = (element) => {
        let score = 0;
        const attributes = ['id', 'name', 'value', 'aria-label', 'title'];
        const textContent = (element.textContent || element.value || '').toLowerCase();
        
        const keywords = [
            'sign in', 'log in', 'continue', 'submit',
            'confirm', 'next', 'login', 'authorize'
        ];
        
        keywords.forEach(keyword => {
            if (textContent.includes(keyword)) score += 30;
            attributes.forEach(attr => {
                const attrValue = (element.getAttribute(attr) || '').toLowerCase();
                if (attrValue.includes(keyword)) score += 20;
            });
        });


        if (element.tagName === 'BUTTON' && element.type === 'submit') score += 50;
        if (element.type === 'submit') score += 70;
        if (element.getAttribute('role') === 'button') score += 20;

        const form = element.closest('form');
        if (form) {
            // Наличие полей логина/пароля в форме
            const hasLoginFields = form.querySelector(
                'input[type="email"], input[type="text"], input[type="password"]'
            );
            if (hasLoginFields) score += 40;
            
            // Наличие формы с действием авторизации
            const formAction = (form.getAttribute('action') || '').toLowerCase();
            if (formAction.includes('login') || formAction.includes('auth')) {
                score += 30;
            }
        }

        const hasSubmitHandler = Array.from(element.attributes).some(attr => 
            attr.name.startsWith('on') && attr.name.toLowerCase() !== 'onclick'
        );
        if (hasSubmitHandler) score += 25;

        return score;
    };

    let maxScore = 0;
    let result = null;
    
    candidates.forEach(button => {
        const currentScore = scoreButton(button);
        if (currentScore > maxScore) {
            maxScore = currentScore;
            result = button;
        }
    });

    return result;
};

function findLoginField (){
    
    const candidates = Array.from(document.querySelectorAll('input'))
        .filter(input => {
            
            return input.type !== 'password' && 
                   input.type !== 'hidden';
        });

    const scoreInput = (input) => {
        let score = 0;
        const attributes = ['name', 'id', 'placeholder', 'autocomplete'];
        
        attributes.forEach(attr => {
            const value = (input.getAttribute(attr) || '').toLowerCase();
            
            if (value.includes('user') || 
                value.includes('login') || 
                value.includes('email') || 
                value.includes('identifier')) {
                score += 25;
            }
        });


        if (input.type === 'email') score += 50;
        if (input.type === 'text') score += 30;

        if (input.autocomplete === 'username' || 
            input.autocomplete === 'email') {
            score += 100;
        }

        const form = input.closest('form');
        if (form && form.querySelector('input[type="password"]')) {
            score += 70;
        }

        if (form) {
            const formAttrs = ['action', 'id', 'name'];
            formAttrs.forEach(attr => {
                const value = (form.getAttribute(attr) || '').toLowerCase();
                if (value.includes('login') || 
                    value.includes('signin') || 
                    value.includes('auth')) {
                    score += 40;
                }
            });
        }

        return score;
    };

    let maxScore = 0;
    let result = null;
    
    candidates.forEach(input => {
        const currentScore = scoreInput(input);
        if (currentScore > maxScore) {
            maxScore = currentScore;
            result = input;
        }
    });

    return result;
};

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
    
    
    var pass = document.querySelector('input[type="password"]');
    var login = findLoginField()
    var submit = findSubmitButton()
    if (attemps === 5) return;
    if (pass != undefined || login != undefined || submit != undefined) {

        submit_b = submit
        pass_f = pass;
        login_f = login
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
        pass_f.value = message.user.password;
        login_f.value = message.user.login;
    }
});


main();
