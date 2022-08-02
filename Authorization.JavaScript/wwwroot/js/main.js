document.getElementById("login").addEventListener("click", login)
document.getElementById("callapi").addEventListener("click", callApi)
document.getElementById("refresh").addEventListener("click", refresh)
document.getElementById("logout").addEventListener("click", logout)

const settings = {
    authority: "https://localhost:5003",
    client_id: "client_id_js",
    response_type: "code",
    scope: "openid profile OrdersAPI",
    redirect_uri: "https://localhost:5006/callback.html",
    silent_redirect_uri: "https://localhost:5006/refresh.html",
    post_logout_redirect_uri: "https://localhost:5006/index.html",
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
}

const manager = new Oidc.UserManager(settings)

manager.events.addUserSignedOut(function() {
    print("User sing out. Good bye.");
});

manager.getUser()
    .then(function (user) {
        if (user) {
            print("User logged in", user);
        } else {
            print("user not logged in");
        }
    })

function logout() {
    manager.signoutRedirect();
}

function callApi() {
    manager.getUser().then(function (user) {
        if (user === null) {
            print("Unauthorized")
        } else {
            const xhr = new XMLHttpRequest();
            xhr.open("GET", "https://localhost:5001/site/secret");
            xhr.onload = function () {
                if (xhr.status === 200) {
                    print(xhr.responseText, xhr.response);
                } else {
                    print("Something went wrong");
                }
            }

            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            xhr.send();
        }
    }).catch(print)
}

function refresh() {
    manager.signinSilent()
        .then(function (user) {
            print("Token refresh", user)
        })
        .catch(function(err) {
            print("Something went wrong")
        });
    
}

function login() {
    manager.signinRedirect();
}

function print(message, data) {
    if (message) {
        document.getElementById("message").innerText = message;
    } else {
        document.getElementById("message").innerText = "";
    }

    if (data && typeof data === "object") {
        document.getElementById("data").innerText = JSON.stringify(data, null, 2);
    } else {
        document.getElementById("data").innerText = "";
    }
}