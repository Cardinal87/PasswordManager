let isDOMReady = false;
const messageQueue = [];


function getToken(key) {
    return new Promise((resolve) => {
        chrome.storage.local.get(['token'], (result) => {
            resolve(result[key]);
        });
    });
}


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

    isDOMReady = true;
    messageQueue.forEach(handleMessage);
});

chrome.runtime.onMessage.addListener((message) => {
    
    if (!isDOMReady) {
        messageQueue.push(message); 
    }
    else {
        handleMessage(message); 
    }
})

function handleMessage(message) {
    if (message.type == "SHOW_SELECTION_WINDOW") {
        if (message.users.length === 1) {
            showSingleUser(message.users);
        }
        else {
            showManyUsers(message.users);
        }
    }
}


function showSingleUser(users) {
    document.getElementById("passwordFrom").style.display = "none";
    document.getElementById("confirmationForm").style.display = "inline-block";
    document.getElementById("manyUsers").style.visibility = "hidden";
    document.getElementById("singleUser").style.visibility = "visible";
    
    document.getElementById("submitData").addEventListener("click", () => {
        chrome.tabs.query({ active: true, currentWindow: true }, (tabs) => {
            var curTab = tabs[0];
            chrome.tabs.sendMessage(curTab, { type: "USER_SELECTED", user: users[0] });
            window.close();
        });
    });
    
}

function showManyUsers(users) {
    document.getElementById("passwordFrom").style.display = "none";
    document.getElementById("confirmationForm").style.display = "inline-block";
    document.getElementById("manyUsers").style.visibility = "visible";
    document.getElementById("singleUser").style.visibility = "hidden";
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
            var data = await responce.json();
            chrome.storage.local.set({ token: data.token });
            window.close();
        }
    });
}
