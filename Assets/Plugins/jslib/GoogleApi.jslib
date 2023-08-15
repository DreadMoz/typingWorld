mergeInto(LibraryManager.library, {
    IsGoogleSignedIn: function () {
        return isSignedIn();
    },
    GoogleAuth: function () {
        auth();
    },
    GoogleSignOut: function () {
        signOut();
    },
    Firebase: function () {

        const firebaseConfig = {
            // Firebaseの設定情報
            apiKey: "AIzaSyBAeNlEDb8NSlW2M_grZl4XQCJpXyq2Y94",
            authDomain: "authexam-f7b35.firebaseapp.com",
            projectId: "authexam-f7b35",
            storageBucket: "authexam-f7b35.appspot.com",
            messagingSenderId: "860886528068",
            appId: "1:860886528068:web:9949ff9eaf762bd1e00c11",
            measurementId: "G-HKMB18YC9C"
        };

        const app = firebase.initializeApp(firebaseConfig); // Firebaseアプリを初期化
        const authInstance = app.auth(); // Firebase Auth インスタンスを取得
        const provider = new firebase.auth.GoogleAuthProvider(); // Google 認証プロバイダー

        authInstance.signInWithPopup(provider)
            .then((result) => {
                // ログイン成功時の処理
                const user = result.user;

                SendMessage('TextTest', 'UpdateText', 'test');
                user.providerData.forEach((profile) => {
                    // Login情報取得
//                    SendMessage('Text', 'UpdateText', 'bbb');
//                    SendMessage('Image', 'UpdateImage', profile.photoURL);
                });

                // location.href = "http://google.com";
            })
            .catch((error) => {
                // ログインエラー時の処理
                // Handle Errors here.
                const errorCode = error.code;
                const errorMessage = error.message;
                // the email of the user's account used.
                const email = error.email;
                // The AuthCredential type that was used.
                // const credential = firebase.auth.GoogleAuthProvider.credentialFromError(error);
            });
    }
});
