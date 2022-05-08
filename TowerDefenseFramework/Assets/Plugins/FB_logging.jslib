mergeInto(LibraryManager.library, {

    FBGameStart:function() {
        analytics.logEvent("game_start", {});
    },

    FBLevelSelect:function(level) {
        analytics.logEvent("level_select", {
            level: Pointer_stringify(level),
        });
    },

});
