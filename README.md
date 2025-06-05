# Password Manager Software
## Platforms
The application is designed for Windows 10 x64 operating system\
The extension is designed for Chromium-based browsers
## Features
The application provides a convenient interface for storing sensitive data\
At the moment it allows you to store such types of data as:
* bank card data
* application data
* website data

The application provides a convenient password generator with the ability to choose the password length and the characters to be used for password generation\
that will be used during generation\
In addition, the application has an extension that allows you to automatically fill out forms on websites\
To do this, you need to go to the desired site and press the “insert” button, that will appear on the password form\
After that, the extension will offer either to insert the account data or to choose one of the accounts, if there are several of them.
## Protection
A single master password is used to access the application and the extension, which is not stored anywhere\
**Master password cannot be recovered!!**\
Scrypt is used as the hashing algorithm\
AES-256 is used to encrypt SQLite database, the key is also not stored anywhere and is obtained from the password\
JWT token authorization is used for api security\
The token key is generated at web API startup and is rotated every 7 days
## Installation
Download and unpack the archive
### App
* run PasswordManager/PasswordManager.exe
### API
For a one-time launch, run PasswordManager.WebAPI.exe from the PasswordManagerAPI folder of the application\
For long time usage:
* to install API in Windows services run PasswordManagerAPI/install_service.bat as administrator
* to remove API from Windows services run PasswordManagerAPI/uninstall_service.bat as administrator

### Extension
* Go to your browser and type chrome://extensions/ in the address bar
* Enable developer mode
* Click “download packaged extension” and specify the path to the Extension folder in the root directory of the application

