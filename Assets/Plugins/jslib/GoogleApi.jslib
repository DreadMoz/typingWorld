mergeInto(LibraryManager.library, {

    IsGoogleSignedIn: function () {
        return isSignedIn();
    },

    GoogleAuth: function () {
    },

    GoogleSignOut: function () {
        signOut();
    },
    setFirebase: function () {
        setFirebaseData();
    },

    getFirebase: function () {
        getFirebaseData();
    },

    Firebase: function () {
        auth();
    }
});
