import * as fs from 'fs';
import path from 'path';
import { Mod } from "./Mod";
import { Utilities } from "./Utilities";

/**
 * Manages finding mods on disk, saving Kenshi mod configurations,
 * and handles the saving and loading of Kenshi Mod Profiles.
 */
export class ModIOManager {
    public static readonly DEFAULT_MISSING_ICON_REPLACEMENT_NAME: string = "black-box.png";
    public static readonly DEFAULT_STEAM_MODS_ABSOLUTE_DIRECTORY: string = "C:/Program Files (x86)/Steam/steamapps/workshop/content/233860";
    private static readonly PROFILE_RELATIVE_DIRECTORY: string = "./profiles/";

    /**
     * Returns all the mods that were found on disk searching the provided directories.
     */
    public static GetAllModsFromDisk(searchDirectories: string[]): Mod[] {
        let mods: Mod[] = [];
        for (let i = 0; i < searchDirectories.length; i++) {
            const subDirectories: string[] = Utilities.GetSubdirectories(searchDirectories[i]);
            for (let j = 0; j < subDirectories.length; j++) {
                const subDirectoryPath: string = searchDirectories[i] + "/" + subDirectories[j];
                const files: string[] = fs.readdirSync(subDirectoryPath);

                const modFileName: string = files.filter(elm => elm.endsWith(".mod"))[0];
                const infoFileName: string = files.filter(elm => elm.startsWith("_") && elm.endsWith(".info"))[0];
                let imgFileName: string = files.filter(elm => elm.startsWith("_") && elm.endsWith(".img"))[0];

                const modFilePath: string = subDirectoryPath + "/" + modFileName;
                const infoFilePath: string = subDirectoryPath + "/" + infoFileName;
                let imgFilePath: string = subDirectoryPath + "/" + imgFileName;

                if (modFileName === null || modFileName === undefined) { continue; }
                if (infoFileName === null || infoFileName === undefined) { continue; }
                if (imgFileName === null || imgFileName === undefined) {
                    imgFileName = ModIOManager.DEFAULT_MISSING_ICON_REPLACEMENT_NAME;
                    imgFilePath = path.join(__dirname, "..", "assets", ModIOManager.DEFAULT_MISSING_ICON_REPLACEMENT_NAME).replace(/\\/g, '/');;
                }

                const mod: Mod = new Mod(
                    modFileName,
                    modFileName,
                    modFilePath,
                    infoFilePath,
                    imgFilePath);

                mods.push(mod);
            }
        }
        return mods;
    }

    /**
     * Saves the mods provided to Kenshi's config in the order of the array provided.
     */
    public static SaveModsToKenshi(mods: Mod[]) {
    }

    /**
     * Returns the absolute path of Kenshi Mod Manager profiles.
     */
    public static GetProfilePath(): string {
        return "";
    }

    /**
     * Saves the mods provided to a Kenshi Mod Manager profile.
     */
    public static SaveModsToProfile(mods: Mod[], profileName: string) {
    }

    /**
     * Returns all the mods in a given profile, or null if the profile was not found.
     */
    public static GetModsFromProfile(profileName: string): Mod[] | null {
        return [];
    }
}
