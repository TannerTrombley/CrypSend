const version = "0.0.1";

const backendHost = () => {
    if (window.location.hostname === "localhost" || window.location.hostname === "127.0.0.1") {
        return "http://localhost:7071/api/";
        //return "https://crypsendfunctions.azurewebsites.net/api/";
    }
    else {
        return "https://crypsendfunctions.azurewebsites.net/api/";
    }
}

export { version, backendHost }