
chrome.runtime.onMessage.addListener((message) => {
    if (message.action == "SELECT_USER") {
        chrome.action.openPopup();
        chrome.runtime.sendMessage({ action: 'SHOW_SELECTION_WINDOW', users: message.users });
    }
})





