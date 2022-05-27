mergeInto(LibraryManager.library, {

    FBGameStart:function() {
        analytics.logEvent("game_start", {});
    },

    FBLevelSelect:function(level) {
        analytics.logEvent("level_select", {
            level: Pointer_stringify(level),
        });
    },

    FBLeaveLevel:function(level) {
        analytics.logEvent("leave_level", {
            level: Pointer_stringify(level),
        });
    },

    FBOncomerSpawned:function(level, oncomerName) {
        analytics.logEvent("oncomer_spawned", {
            level: Pointer_stringify(level),
            oncomer_name: Pointer_stringify(oncomerName),
        });
    },

    FBTowerPlaced:function(level, position, towerName) {
        analytics.logEvent("tower_placed", {
            level: Pointer_stringify(level),
            position: Pointer_stringify(position),
            tower_name: Pointer_stringify(towerName),
        });
    },

    FBHitMoat:function(level, oncomerName, oncomerPH, oncomerVolume, moatPH, moatVolume) {
        analytics.logEvent("hit_moat", {
            level: Pointer_stringify(level),
            oncomer_name: Pointer_stringify(oncomerName),
            oncomer_ph: Pointer_stringify(oncomerPH),
            oncomer_vol: Pointer_stringify(oncomerVolume),
            moat_ph: Pointer_stringify(moatPH),
            moat_vol: Pointer_stringify(moatVolume),
        });
    },

    FBLevelEnd:function(level, ph, volume) {
        analytics.logEvent("level_end", {
            level: Pointer_stringify(level),
            moat_ph: Pointer_stringify(ph),
            moat_vol: Pointer_stringify(volume),
        });
    },

});
