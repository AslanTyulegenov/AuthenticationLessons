const manager = new Oidc.UserManager({
    loadUserInfo: true,
    response_mode: "query",
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage })
});

manager.signinRedirectCallback()
    .then(function (user) {
        console.log(user)
        window.location.href = "https://localhost:5006"
    })
    .catch(function (err) {
        console.log(err);
    });
