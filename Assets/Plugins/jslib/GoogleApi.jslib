mergeInto(LibraryManager.library, {
    IsGoogleSignedIn: function () {
        return isSignedIn();
    },
    GoogleAuth: function () {
        auth();
    },
    GoogleSignOut: function () {
        signOut();
    }
});
