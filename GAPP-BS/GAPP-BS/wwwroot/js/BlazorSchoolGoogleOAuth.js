let blazorSchoolAuthenticationStateProviderInstance = null;
function sayhello(mesg) {
    alert(mesg)
}
function blazorSchoolGoogleInitialize(clientId, blazorSchoolAuthenticationStateProvider) {
    // disable Exponential cool-down
    /*document.cookie = `g_state=;path=/;expires=Thu, 01 Jan 1970 00:00:01 GMT`;*/
    blazorSchoolAuthenticationStateProviderInstance = blazorSchoolAuthenticationStateProvider;
    google.accounts.id.initialize({ client_id: clientId, callback: blazorSchoolCallback });
}

function blazorSchoolGooglePrompt() {
    google.accounts.id.prompt((notification) => {
        if (notification.isNotDisplayed() || notification.isSkippedMoment()) {
            console.info(notification.getNotDisplayedReason());
            console.info(notification.getSkippedReason());
        }
    });
}

function blazorSchoolCallback(googleResponse) {
    blazorSchoolAuthenticationStateProviderInstance.invokeMethodAsync("GoogleLogin", { ClientId: googleResponse.clientId, SelectedBy: googleResponse.select_by, Credential: googleResponse.credential });
}

function handleCredentialResponse(response) {
    console.log("response ", decodeJWT(response.credential));
    // console.log("Encoded JWT ID token: " + response.credential);
}
function decodeJWT(token) {
    const parts = token.split('.');
    if (parts.length !== 3) {
        alert('JWT must have three parts');
        return "no token";
    }
    
    const decodedHeader = base64urlDecode(parts[0]);
    const decodedPayload = base64urlDecode(parts[1]);

    return {
        header: decodedHeader,
        payload: decodedPayload
    };
}
function base64urlDecode(str) {
    // Convert base64url to base64 by replacing '-' with '+' and '_' with '/'
    // and removing trailing '='
    const base64 = str.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
}
window.onload = function () {
    google.accounts.id.initialize({
        client_id: "25082372106-gaaa8ulgs4s5hi5qpsaagek5c50emjtl.apps.googleusercontent.com",
        callback: handleCredentialResponse
    });
    google.accounts.id.renderButton(
        document.getElementById("buttonDiv"),
        { theme: "outline", size: "large" }  // customization attributes
    );
    google.accounts.id.prompt(); // also display the One Tap dialog
}
function signOut() {
    alert("signOut");
    // console.log('User signed out.', auth2);
    // var auth2 = gapi.auth2.getAuthInstance();
    // auth2.signOut().then(function () {
    //     console.log('User signed out.');

    //     // Invoke the Blazor method
    //     DotNet.invokeMethodAsync('GAPP_BS', 'Logout')
    //         .then(data => {
    //             console.log('Blazor logout invoked');
    //         })
    //         .catch(error => {
    //             console.error(error);
    //         });
    // });
}