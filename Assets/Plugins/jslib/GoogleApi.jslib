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

    getFirebase: function () {
        getFirebaseData();
    },

    setFirebase: function () {
        setFirebaseData();
    },

    setFirebaseStatus: function () {
        setFirebaseStatus();
    },

    setFirebaseInventory: function () {
        setFirebaseInventory();
    },

    setFirebaseMedals: function () {
        setFirebaseMedals();
    },

    setFirebaseKpm: function () {
        setFirebaseKpm();
    }

});
