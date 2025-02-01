
chrome.runtime.onMessage.addListener((message) => {
    
    if (message.type == "SELECT_USER") {
        chrome.action.openPopup();
        chrome.runtime.sendMessage({ type: 'SHOW_SELECTION_WINDOW', users: message.users });
    }

    else if (message.type == "TOKEN_EXPIRED") {
        chrome.action.openPopup();
    }
})





