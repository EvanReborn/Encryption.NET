<?php
$varPassword = "randompasswordhere";

// Connect to our DB
$con = mysqli_connect("enter", "your", "own", "credentials");
if (mysqli_connect_error()) 
{
    die("Error conencting to our database!");
}

// Encryption
function Encrypt($string)
{
    $plaintext = $string;
    $password = base64_decode($_POST['session_id']);
    $method = 'aes-256-cbc';
    $password = substr(hash('sha256', $password, true), 0, 32);
    $iv = base64_decode($_POST['request_id']);
    $encrypted = base64_encode(openssl_encrypt($plaintext, $method, $password, OPENSSL_RAW_DATA, $iv));
    return $encrypted;
}

function Decrypt($string)
{
    $plaintext = $string;
    $password = base64_decode($_POST['session_id']);
    $method = 'aes-256-cbc';
    $password = substr(hash('sha256', $password, true), 0, 32);
    $iv = base64_decode($_POST['request_id']);
    $decrypted = openssl_decrypt(base64_decode($plaintext), $method, $password, OPENSSL_RAW_DATA, $iv);
    return $decrypted;
}

// Check if the request is coming from the client library (Change the useragent)
if(strlen(strstr($_SERVER['HTTP_USER_AGENT'], "CUSTOM_AGENT")) <= 0 ){
    die("Incorrect User-Agent!");
}

// Check if encryption key and iv is present
if (!isset($_POST['session_id']) || !isset($_POST['request_id'])) {
    die("Encryption information couldn't be retrieved!");
}

// Grab the variable
$name       = $con->real_escape_string(Decrypt($_POST['varname']));
$password   = $con->real_escape_string(Decrypt($_POST['varpass']));

if ($password !== $varPassword)
{
    die(Encrypt("[Incorrect Variable Password]"));
}

$result = mysqli_query($con,"SELECT * FROM variables WHERE name = '$name'");
if (mysqli_num_rows($result) > 0) 
{
    while ($row = mysqli_fetch_array($result)) 
    {
        die(Encrypt($row['value']));
    }
} 
else 
{
    die(Encrypt("[Error Finding Variable]"));
}

?>
