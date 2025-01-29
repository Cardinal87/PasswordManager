function getToken(key) {
    return new Promise((resolve) => {
        chrome.storage.local.get(['token'], (result) => {
            resolve(result[key]);
        });
    });
}


document.addEventListener("DOMContentLoaded", async () => {
    var token = await getToken("token");
    if (token == undefined) {
        document.getElementById("form").style.visibility = "visible";
    }
    else {
        document.getElementById("text").style.visibility = "visible";
    }


    document.getElementById("submit").addEventListener("click", async () => {
        var pass = document.getElementById("password").value;
        var encodedPass = btoa(pass);
        if (pass !== "") {
            var responce = await fetch("http://localhost:5167/api/login/user", {
                headers: {
                    'Authorization': "Basic " + encodedPass,
                }
            });
            var data = await responce.json();
            chrome.storage.local.set({ "token": data.token });
            
            
            window.close();
        }
    });
    
    document.getElementById("cancel").addEventListener("click", function () {
        window.close();
    });
});

