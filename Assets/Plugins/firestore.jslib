mergeInto(LibraryManager.library, {
  // ä÷êîåƒÇ—èoÇµ
  Hello: function () {
    window.alert("Hello, world!");
  },
  Firestore: function() {
    const firebaseConfig = {
        apiKey: "AIzaSyBAeNlEDb8NSlW2M_grZl4XQCJpXyq2Y94",
        authDomain: "authexam-f7b35.firebaseapp.com",
        databaseURL: "https://authexam-f7b35-default-rtdb.firebaseio.com",
        projectId: "authexam-f7b35",
        storageBucket: "authexam-f7b35.appspot.com",
        messagingSenderId: "860886528068",
        appId: "1:860886528068:web:9949ff9eaf762bd1e00c11",
        measurementId: "G-HKMB18YC9C"
    };
      firebase.initializeApp(firebaseConfig);
      var db = firebase.firestore();
      db.collection("unity").doc("dq5NSHDLdP0gbFkOVjAI")
      .onSnapshot(function(doc) {
        console.log("Current data: ", doc.data());
        SendMessage('Text', 'UpdateText', doc.data().text);
      });
  }
});