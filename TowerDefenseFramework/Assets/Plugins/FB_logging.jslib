mergeInto(LibraryManager.library, {

    FBGameStart:function() {
        console.log("LOGGING EVENT: game_start");
        analytics.logEvent("game_start", {});
    },

    FBLevelSelect:function(level) {
        console.log("LOGGING EVENT: level_select");
        analytics.logEvent("level_select", {
            level: Pointer_stringify(level),
        });
    },

});
