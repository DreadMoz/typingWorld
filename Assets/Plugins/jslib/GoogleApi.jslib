mergeInto(LibraryManager.library, {
  RequestDataFromExtension: function() {
    var message = { action: "requestDataFromExtension" };
    window.postMessage(message, "*");

        // タイムアウトを監視する
        var timeoutDuration = 3000; // タイムアウトまでのミリ秒（ここでは3000ms = 3秒）
        var timeout = setTimeout(function() {
            // タイムアウト後の処理
            console.log("タイムアウトになりました。拡張機能からのレスポンスがありません。");
            SendMessage('TitleScene', 'OnRequestTimeout');
        }, timeoutDuration);

        // タイムアウトをキャンセルする関数をグローバルに保存する（後で呼び出せるように）
        window.cancelRequestTimeout = function() {
            clearTimeout(timeout);
        };
    },

    SaveStatusToExtension: function(dataPointer) {
        // ポインタから実際の文字列を取得
        var data = UTF8ToString(dataPointer);
    
        // 拡張機能にメッセージを送信する新しい関数
        window.postMessage({action: "sendStatusToExtension", data: data}, "*");

        // タイムアウト監視の処理をここに追加することも可能
    },

    SendToNecoBase: function(dataPointer) {
        sendToNecoBase(dataPointer);
    },

    FbAuth: function () {
        googleAuth();
    },

    LoadFbData: function () {
        loadFirebaseData();
    },

    SaveFbStatus: function (value) {
        saveFirebaseStatus(value);
    },

    SaveFbEquipment: function (valuePtr) {
        var value = UTF8ToString(valuePtr);
        saveFirebaseEquipment(value);
    },

    SaveFbInventory: function (valuePtr) {
        var value = UTF8ToString(valuePtr);
        saveFirebaseInventory(value);
    },

    SaveFbMedals: function (value) {
        saveFirebaseMedals(value);
    },

    SaveFbKpm: function (value) {
        saveFirebaseKpm(value);
    }

});
