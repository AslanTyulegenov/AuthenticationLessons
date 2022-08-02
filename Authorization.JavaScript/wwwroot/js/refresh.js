var manager = new Oidc.UserManager({
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage })
});

manager.signinSilentCallback()
    .then(function (data) {
        console.log(data);
    })
    .catch(function (data) {
        console.log(data)
    });

