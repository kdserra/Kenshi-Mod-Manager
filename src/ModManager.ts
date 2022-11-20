import { Mod } from "./Mod";
import { ModTableManager } from "./ModTableManager";

export class ModManager {
    private static s_ModList: Mod[] = [];

    public static IsModInModList(mod: Mod): boolean {
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (ModManager.s_ModList[i].FilePath != mod.FilePath) { continue; }
            return true;
        }
        return false;
    }

    public static AddMod(mod: Mod) {
        if (ModManager.IsModInModList(mod)) { return; }
        ModManager.s_ModList.push(mod);
        ModTableManager.RefreshModTable();
    }

    public static RemoveMod(mod: Mod) {
        ModManager.s_ModList = ModManager.s_ModList.filter(e => e !== mod);
        ModTableManager.RefreshModTable();
    }

    public static GetModWithFilePath(filePath: string): Mod | null {
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (ModManager.s_ModList[i].FilePath != filePath) { continue; }
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

    public static GetFirstModWithName(name: string): Mod | null {
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (ModManager.s_ModList[i].DisplayName != name) { continue; }
            return ModManager.s_ModList[i];
        }
        return null;
    }

    public static GetFirstModWithFileName(fileName: string): Mod | null {
        for (let i = 0; i < ModManager.s_ModList.length; i++) {
            if (ModManager.s_ModList[i].FileName != fileName) { continue; }
            return ModManager.s_ModList[i];
        }
        return null;
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
