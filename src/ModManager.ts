import { Mod } from "./Mod";
import { ModTableManager } from "./ModTableManager";

/**
 * Manages keeping track of the users Mods and providing a way to interact
 * with them.
 */
export class ModManager {
    private static s_ModList: Mod[] = [];

    public static IsModInModList(mod: Mod): boolean {
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (ModManager.s_ModList[i].ModFilePath != mod.ModFilePath) { continue; }
            return true;
        }
        return false;
    }

    public static AddMod(mod: Mod) {
        if (ModManager.IsModInModList(mod)) { return; }
        ModManager.s_ModList.push(mod);
        ModTableManager.RefreshModTable();
    }

    public static AddMods(mods: Mod[]) {
        for (let i = 0; i < mods.length; i++) {
            if (ModManager.IsModInModList(mods[i])) { continue; }
            ModManager.s_ModList.push(mods[i]);
            mods[i].Index = i;
        }
        ModTableManager.RefreshModTable();
    }

    public static RemoveMod(mod: Mod) {
        ModManager.s_ModList = ModManager.s_ModList.filter(e => e !== mod);
        ModTableManager.RefreshModTable();
    }

    public static GetModWithFilePath(filePath: string): Mod | null {
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (ModManager.s_ModList[i].ModFilePath != filePath) { continue; }
            return ModManager.s_ModList[i];
        }
        return null;
    }

    public static GetModsWithDisplayName(displayName: string): Mod[] {
        let output: Mod[] = [];
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (ModManager.s_ModList[i].DisplayName != displayName) { continue; }
            output.push(ModManager.s_ModList[i]);
        }
        return output;
    }

    public static GetModsWithFileName(fileName: string): Mod[] {
        let output: Mod[] = [];
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (ModManager.s_ModList[i].FileName != fileName) { continue; }
            output.push(ModManager.s_ModList[i]);
        }
        return output;
    }

    public static GetModsContainingDisplayName(displayName: string, ignoreCase: boolean = false): Mod[] {
        let output: Mod[] = [];
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            let modDisplayName:string = ModManager.s_ModList[i].DisplayName;
            if (ignoreCase) { modDisplayName = modDisplayName.toLowerCase(); }
            if (!modDisplayName.includes(displayName)) { continue; }
            output.push(ModManager.s_ModList[i]);
        }
        return output;
    }

    public static GetModsContainingFileName(fileName: string, ignoreCase: boolean = false): Mod[] {
        let output: Mod[] = [];
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            let modFileName:string = ModManager.s_ModList[i].FileName;
            if (ignoreCase) { modFileName = modFileName.toLowerCase(); }
            if (!modFileName.includes(fileName)) { continue; }
            output.push(ModManager.s_ModList[i]);
        }
        return output;
    }

    public static GetAllMods(): Mod[] {
        return ModManager.s_ModList;
    }

    public static GetActiveMods(): Mod[] {
        let activeMods: Mod[] = [];
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (!ModManager.s_ModList[i].Active) { continue; }
            activeMods.push(ModManager.s_ModList[i]);
        }
        return activeMods;
    }

    public static GetInactiveMods(): Mod[] {
        let inactiveMods: Mod[] = [];
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (ModManager.s_ModList[i].Active) { continue; }
            inactiveMods.push(ModManager.s_ModList[i]);
        }
        return inactiveMods;
    }
}
