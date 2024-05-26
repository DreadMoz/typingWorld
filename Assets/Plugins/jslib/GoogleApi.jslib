mergeInto(LibraryManager.library, {
    GetOAuth: function() {
        getOAuth();
    },

    OAuthLogout: function() {
        oAuthLogout();
    },

    LoadDataFromExtension: function() {
        var message = { action: "loadDataFromExtension" };
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

    LoadFromGss: function(dataInfo) {
        loadFromGss(dataInfo);
    },

    SaveToGss: function(dataPointer, dataInfo) {
        console.log("Received pointer:", dataPointer); // ポインタ受け取り時のデバッグ
        var data = UTF8ToString(dataPointer);
        console.log("Converted data:", data); // 文字列変換後のデバッグ
        saveToGss(data, dataInfo);
    }
});
