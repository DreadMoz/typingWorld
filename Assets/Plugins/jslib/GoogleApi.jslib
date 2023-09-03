mergeInto(LibraryManager.library, {

    IsGoogleSignedIn: function () {
        return isSignedIn();
    },
    GoogleSignOut: function () {
        signOut();
    },

    GoogleAuth: function () {
    },
    Firebase: function () {
        auth();
    },

    setFirebase: function () {
        setFirebaseData();
    },

    getFirebase: function () {
        getFirebaseData();
    }

});
