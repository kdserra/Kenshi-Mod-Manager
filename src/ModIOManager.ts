import { readdirSync } from "original-fs";
import { Mod } from "./Mod";
import { Utilities } from "./Utilities";

/**
 * Manages finding mods on disk, saving Kenshi mod configurations,
 * and handles the saving and loading of Kenshi Mod Profiles.
 */
export class ModIOManager {
    public static readonly DEFAULT_MISSING_ICON_REPLACEMENT: string = "assets\\black-box.png";
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
                const files: string[] = readdirSync(subDirectoryPath);

                const modFile: string = files.filter(elm => elm.endsWith(".mod"))[0];
                const infoFile: string = files.filter(elm => elm.startsWith("_") && elm.endsWith(".info"))[0];
                let imgFile: string = files.filter(elm => elm.startsWith("_") && elm.endsWith(".img"))[0];
                if (imgFile === null || imgFile === undefined) {
                    // Switching to ts-node
                    //                                                  v---- dist is the problem
                    //C:\Users\User\Home\Development\Kenshi Mod Manager\dist\assets\black-box.png
                    imgFile = `${__dirname}\\${ModIOManager.DEFAULT_MISSING_ICON_REPLACEMENT}`;
                    console.log(imgFile);
                }


                const modFilePath: string = subDirectoryPath + "/" + modFile;
                const infoFilePath: string = subDirectoryPath + "/" + infoFile;
                const imgFilePath: string = subDirectoryPath + "/" + imgFile;

                const mod: Mod = new Mod(
                    modFile,
                    modFile,
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
