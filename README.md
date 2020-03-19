# Vigilante

Dynamic encryption Key and IV for every request, HTTP Debugger protection and a more secure WebClient all in one library.

*WARNING: This is more of a PoC, not everything has been fully tested and I will update in the future. Feel free to make suggestions!*

**Evan#5948** is the sole creator and owner of this project.

## Usage

- Create an SQL table called **variables** with rows **name** and **value**
- Fully customize the PHP with your SQL credentials and custom variables
- Open the client-side example and enter your websites information and other custom details
- Test it out by creating a variable name and value manually in phpmyadmin and then grabbing it in the client

## How It Works

The client-side generates random encryption Keys and IVs and encrypts requests and sends them to the server-side.
```cs
// Initialize a new session with new encryption info
Encryption.StartSession(); 

// Send the values along with the current Key and IV to the server side for decryption and review
SecureWebClient.SendEncrypteValues("", new NameValueCollection 
{
  ["example"]     = Encryption.Encrypt("data"),
  ["example2"]    = Encryption.Encrypt("data2")
}, "");

// End the session and purge the encryption info
Encryption.EndSession();
```

## TODOs

- Add a 5-10 second limit on all request methods to prevent conenction tampering.
- Add debugger protection as well as using SecureString class for all recieved strings.
- Fix any security bugs I may have overlooked as I hastily made this project.
