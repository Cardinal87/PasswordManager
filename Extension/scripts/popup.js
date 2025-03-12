let isDOMReady = false;
const messageQueue = [];
var password;
var login;
var currentUrl;


//gttint jwt token
function getToken(key) {
    return new Promise((resolve) => {
        chrome.storage.local.get(['token'], (result) => {
            resolve(result[key]);
        });
    });
}


//main listener
document.addEventListener("DOMContentLoaded", async () => {

    document.getElementById("passwordFrom").style.display = "inline-block";
    
    var token = await getToken("token");
    if (token == undefined) {
        document.getElementById("form").style.visibility = "visible";
        configurePasswordForm();
    }
    else {
        document.getElementById("text").style.visibility = "visible";
    }

    var list = document.getElementsByName("cancel");
    for (let item of list) {
        item.addEventListener("click", function () {
            window.close();
        });
    }

    //saving new password
    document.getElementById('saveEntry').addEventListener('click', async () => {
        let name = document.getElementById('name').value;
        if (name === ""){
            document.getElementById('name-warn').style.visibility = "visible";
        }
        else {
            var params = new URLSearchParams();
            params.append("url", currentUrl);
            params.append("password", password);
            params.append("login", login);
            params.append("name", name)
            var responce = await fetch("http://localhost:5167/api/authorization/post?" + params, {
                method: 'POST',
                headers: {
                    'Authorization': "Bearer " + token,
                }
            });
            if (responce.ok){
                document.getElementById("add-form").style.visibility = "hidden";
                document.getElementById("successful").style.visibility = "visible";
                setTimeout(() => {
                    document.getElementById('successful').style.display = 'hidden';
                    window.close();
                }, 1000);
            }
            else{
                document.getElementById("add-form").style.visibility = "hidden";
                document.getElementById("bad").style.visibility = "visible";
                document.getElementById("error-mes").innerHTML = responce.statusText;
            }
        }
    });

    isDOMReady = true;
    messageQueue.forEach(handleMessage);
});


//message listener
chrome.runtime.onMessage.addListener((message) => {
    
    if (!isDOMReady) {
        messageQueue.push(message); 
    }
    else {
        handleMessage(message); 
    }
})


//hanfling messages
function handleMessage(message) {
    if (message.type == "SHOW_SELECTION_WINDOW") {
        showUserList(message.users)
    }
    if (message.type == "SAVE_PASSWORD"){
        showAddingWindow(message);
    }
}


//showing window for saving new password
function showAddingWindow(message){
    document.getElementById("passwordFrom").style.display = "none";
    document.getElementById("confirmationForm").style.display = "inline-block";
    document.getElementById("add-form").style.visibility = "visible";
    password = message.password;
    currentUrl = message.url;
    login = message.login;
}



//forming list of users
function showUserList(users) {
    document.getElementById("passwordFrom").style.display = "none";
    document.getElementById("confirmationForm").style.display = "inline-block";
    document.getElementById("users").style.visibility = "visible";
    const list = document.getElementById('userList');
    users.forEach(user => {
        const li = document.createElement('li');
        li.innerHTML = user.login;
        li.addEventListener('click', () => {
            chrome.tabs.query({ active: true, currentWindow: true }, (tabs) => {
                var curTab = tabs[0];
                chrome.tabs.sendMessage(curTab.id, { type: "USER_SELECTED", user: user });
            });
            window.close();
        });
        list.appendChild(li);
    });

    
}




//check if the password is correct
function configurePasswordForm() {
    document.getElementById("submitPassword").addEventListener("click", async () => {
        var pass = document.getElementById("password").value;
        var encodedPass = btoa(pass);
        if (pass !== "") {
            
            var responce = await fetch("http://localhost:5167/api/login/get", {
                headers: {
                    'Authorization': "Basic " + encodedPass,
                }
            });
            if (responce.ok){
                document.getElementById('password-warn').style.visibility = "hidden";
                var data = await responce.json();
                chrome.storage.local.set({ token: data.token });
                window.close();
            }
            else{
                document.getElementById('password-warn').style.visibility = "visible";
            }
        }
        else{
            document.getElementById('password-warn').style.visibility = "visible";
        }
    });
}
