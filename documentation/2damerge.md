![Documentation Image](images/documentation_header.png)

# LE1 Bio2DA Merge
Starting with ME3Tweaks Mod Manager 9.0, 2DA tables shipped in DLC mods can be automatically merged into the basegame versions. This works around issues in the Bio2DANumberedRows system, where rows could not be relaibly overwritten due to changes in how 2DA tables work - either in the PS3 port of ME1 or the Legendary Edition upgrade. The issue lies in how the row is accessed; some methods properly will find the overwritten row, other accessors will not. The LE1 2DA merge feature simply bypasses the need to overwrite 2DA rows in autoload by directly overwriting them in the basegame 2DAs.

## WARNING

The LE1 2DA Merge feature is on by default, so that mods that use it will work. For this feature to work, every merge will reset all basegame 2DAs to their original values, then apply the changes. If you try your work directly in basegame, it is liable to be overwritten. Be careful!


## How to use 2DA merge
ME3Tweaks Mod Manager will automatically scan DLC mods for `.m3da` files, and merge them in mount-order, then alphabetically per DLC. `.m3da` files are `.json` files that describe changes to make; you can ship multiple `.m3da` files in your DLC folder using the alternates system to provide customizable 2DA overrides, similar to LE1 Config Merge.

In the below picture, it outlines the two files needed for at least one 2DA merge.
![image](https://github.com/ME3Tweaks/ME3TweaksModManager/assets/2738836/a5111211-f029-46b4-910d-b8b4b178dd44)

The only restriction on the `.m3da` filename(s) are that they must start with your DLC foldername, followed by a dash, and at least one more character.
Some examples with the mod folder name of `DLC_MOD_2DAMERGE`:
 - `DLC_MOD_2DAMERGE-My2DAs.m3da` - OK
 - `DLC_MOD_2DAMERGE-HardModeTables.m3da` - OK
 - `DLC_MOD_2DAMERGE-.m3da` - BAD
 - `DLC_MOD_2DAMERGE.m3da` - BAD
 - `My2DAs.m3da` - BAD


Your 2DA package files are referenced by these `.m3da` files and can have any name. They are not actually used by the game.

For example, this is Pinnacle Station DLC's 1.0.5 2DA merge file:
`DLC_MOD_Vegas-2DAs.m3da`:
```json
[
    {
        "comment": "Merges Pinnacle Station's 2DA tables - Engine",
        "packagefile": "Engine.pcc",
        "mergepackagefile": "Pinnacle2DAs.pcc",
        "mergetables": [
            "BIOG_2DA_Vegas_AreaMap_X.Areamap_AreaMap_part_5",
            "BIOG_2DA_Vegas_Equipment_X.Items_Items_part_10",
            "GalaxyMap_Map_part_5",
            "GalaxyMap_Planet_part_5",
            "GalaxyMap_PlotPlanet_part_5",
            "Rewards_Statistics_part_1",
            "BIOG_2DA_Vegas_Music_X.Music_Music_part_2",
            "Treasure_Tables_TreasureDist_part_5",
            "Treasure_Tables_TreasureEntries_part_5",
            "Treasure_Tables_TreasureTables_part_5",
        ]
    },
    {
        "comment": "Merges Pinnacle Station's 2DA tables - SFXGame",
        "packagefile": "SFXGame.pcc",
        "mergepackagefile": "Pinnacle2DAs.pcc",
        "mergetables": [
            "Images_CodexImages_part_5",
            "Images_GalaxyMapImages_part_5",
            "Images_ShopImages_part_5"
        ]
    }
]

```

`Pinnacle2DAs.pcc`:
![image](https://github.com/ME3Tweaks/ME3TweaksModManager/assets/2738836/fb9f1910-14cd-44bb-9dc4-70f54412ca5b)


## M3DA json format
The json format for `.m3da` files is as follows:

| Key              | Value                                          | Description                                                                                                                                                                                                          | Required |
|------------------|------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|
| comment          | String                                         | Anything you want - this is not used for anything beyond human readability and context                                                                                                                               | No       |
| packagefile      | String(Filename)                               | The name of the basegame package file that contains the table you are merging into. This will typically be Engine.pcc, SFXGame.pcc, or EntryMenu.pcc.                                                                | Yes      |
| mergepackagefile | String(Filename)                               | The name of the package file in your DLC folder that contains the 2DA tables to merge                                                                                                                                | Yes      |
| mergetables      | List&lt;string&gt;(Export instanced full paths) | List of tables to merge in mergepackagefile. They must all have their object name end in `_part` and have the same basename as the target table, just like normal autoload 2DAs did. Your 2DA packages should not be referenced by autoload when using this feature. | Yes      |

The values of `mergetables` are the Instanced Full Path of your source table in your package file. This means you can have duplicates of tables that can be referenced by different m3da files - for example, putting different alternate tables for merging under different package exports with a clear package name of what that table contains. For example:

```
HardMode.LevelUp_Experience_part_0
LudicrousMode.LevelUp_Experience_part_0
EasyMode.LevelUp_Experience_part_0
```

The following files are allowed to be merged into:
 - Engine.pcc
 - SFXGame.pcc
 - EntryMenu.pcc
 - BIOG_2DA_UNC_AreaMap_X.pcc
 - BIOG_2DA_UNC_GalaxyMap_X.pcc
 - BIOG_2DA_UNC_GamerProfile_X.pcc
 - BIOG_2DA_UNC_Movement_X.pcc
 - BIOG_2DA_UNC_Music_X.pcc
 - BIOG_2DA_UNC_Talents_X.pcc
 - BIOG_2DA_UNC_TreasureTables_X.pcc
 - BIOG_2DA_UNC_UI_X.pcc

