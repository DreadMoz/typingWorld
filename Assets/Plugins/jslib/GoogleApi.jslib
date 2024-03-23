mergeInto(LibraryManager.library, {
    RequestDataFromExtension: function(stageNo) {
        // 直接数値を使用
        var message = { action: "requestDataFromExtension", stageNo: stageNo };
        window.postMessage(message, "*");
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
        var value = Pointer_stringify(valuePtr);
        saveFirebaseEquipment(value);
    },

    SaveFbInventory: function (valuePtr) {
        var value = Pointer_stringify(valuePtr);
        saveFirebaseInventory(value);
    },

    SaveFbMedals: function (value) {
        saveFirebaseMedals(value);
    },

    SaveFbKpm: function (value) {
        saveFirebaseKpm(value);
    }

});
