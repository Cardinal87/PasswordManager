document.addEventListener("DOMContentLoaded", async () => {
    var token = await chrome.storage.local.get(["token"]).then(result => result.token);
    
    if (token == null || token === "") {
        document.getElementById("form").style.visibility = "visible";
    }
    else {
        document.getElementById("text").style.visibility = "visible";
    }


    document.getElementById("submit").addEventListener("click", async function () {
        var pass = document.getElementById('password').innerText;
        if (pass === "") {
            var token = await fetch("http://localhost:5167/api/login/user", {
                method: "GET",
                headers: {
                    Authorization: pass
                }
            }).then(data => data.text);
            chrome.storage.local.set({ "token": token });
            window.close();
        }
    });

    document.getElementById("cancel").addEventListener("click", function () {
        window.close();
    });
});


