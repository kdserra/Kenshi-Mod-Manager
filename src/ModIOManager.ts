import * as fs from 'fs';
import path from 'path';
import { Mod } from "./Mod";
import { ModManager } from './ModManager';
import { Tag } from './Tag';
import { TagHelper } from './TagHelper';
import { Utilities } from "./Utilities";
import lockfile from "proper-lockfile";

/**
 * Manages finding mods on disk, saving Kenshi mod configurations,
 * and handles the saving and loading of Kenshi Mod Profiles.
 */
export class ModIOManager {
    public static readonly DEFAULT_MISSING_ICON_REPLACEMENT_FILE_NAME: string = "black-box.png";
    public static readonly DEFAULT_STEAM_MODS_PATH: string = "C:/Program Files (x86)/Steam/steamapps/workshop/content/233860";
    public static readonly DEFAULT_STEAM_KENSHI_PATH: string = "C:/Program Files (x86)/Steam/steamapps/common/Kenshi";
    private static readonly PROFILE_RELATIVE_DIRECTORY: string = "./profiles/";

    /**
     * Returns all the mods that were found on disk searching the provided directories.
     */
    public static GetAllModsFromDisk(): Mod[] {
        let mods: Mod[] = [];
        const kenshiModsPath: string = ModIOManager.GetKenshiModsPath();
        const subDirectories: string[] = Utilities.GetSubdirectories(kenshiModsPath);
        for (let i = 0; i < subDirectories.length; i++) {
            const subDirectoryPath: string = kenshiModsPath + "/" + subDirectories[i];
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
                imgFileName = ModIOManager.DEFAULT_MISSING_ICON_REPLACEMENT_FILE_NAME;
                imgFilePath = path.join(__dirname, "..", "assets", ModIOManager.DEFAULT_MISSING_ICON_REPLACEMENT_FILE_NAME).replace(/\\/g, '/');;
            }

            const infoFileContent: string = fs.readFileSync(infoFilePath, 'utf-8');
            let modName: string = Utilities.MatchFirmFirst(infoFileContent, /(?<=\<mod\>)(.*?)(?=\<\/mod\>)/);
            let modTitle: string = Utilities.MatchFirmFirst(infoFileContent, /(?<=\<title\>)(.*?)(?=\<\/title\>)/);
            let modTags: Tag[] = TagHelper.StringsToTags(Utilities.MatchFirm(infoFileContent, /(?<=\<string\>)(.*?)(?=\<\/string\>)/g));

            const mod: Mod = new Mod(
                modName,
                modTitle,
                modFilePath,
                infoFilePath,
                imgFilePath,
                modTags
            );

            mods.push(mod);
        }
        return mods;

    }

    /**
     * Saves the mods provided to Kenshi's config in the order of the array provided.
     * The relative directory for the file is ./Kenshi/data/mods.cfg
     */
    public static SaveModsToKenshiModConfig(mods: Mod[]) {
        let dataArray: string[] = [];
        for (let i = 0; i < mods.length; i++) {
            const modFileName: string = `${mods[i].GetModFileName()}\n`;
            if (dataArray.includes(modFileName)) { continue; }
            dataArray.push(modFileName);
        }
        const data: string = dataArray.join("");
        const stream = fs.createWriteStream(ModIOManager.GetKenshiModConfigFilePath());
        stream.write(dataArray.join(""));
        stream.close();

        const streamBackupFile = fs.createWriteStream(ModIOManager.GetKenshiModConfigBackupFilePath());
        streamBackupFile.write(dataArray.join(""));
        streamBackupFile.close();
    }

    /**
     * Saves the mods provided to Kenshi's mod list in the order of the array provided.
     */
    public static SaveModsToKenshiModList(mods: Mod[]) {
        let data: string[] = [];
        for (let i = 0; i < mods.length; i++) {
            const modFileName: string = `${mods[i].GetModFileName()}\n`;
            if (data.includes(modFileName)) { continue; }
            data.push(modFileName);
        }
        const stream = fs.createWriteStream(ModIOManager.GetKenshiModListFilePath());
        stream.write(data.join(""));
        stream.close();
    }

    public static LockFiles() {
        lockfile.lock(ModIOManager.GetKenshiModConfigFilePath());
        lockfile.lock(ModIOManager.GetKenshiModConfigBackupFilePath());
        lockfile.lock(ModIOManager.GetKenshiModListFilePath());
    }

    public static UnlockFiles() {
        lockfile.unlock(ModIOManager.GetKenshiModConfigFilePath());
        lockfile.unlock(ModIOManager.GetKenshiModConfigBackupFilePath());
        lockfile.unlock(ModIOManager.GetKenshiModListFilePath());
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


    /**
     * Returns the absolute file path to the Kenshi Mod Config file.
     */
    public static GetKenshiModConfigFilePath(): string {
        return path.join(this.GetKenshiPath(), "data", "mods.cfg");
    }

    /**
     * Returns the absolute file path to the backup Kenshi Mod Config file.
     */
    public static GetKenshiModConfigBackupFilePath(): string {
        return path.join(this.GetKenshiPath(), "data", "mods.cfg.backup");
    }

    /**
     * Returns the absolute file path to the Kenshi Mod List file.
     */
    public static GetKenshiModListFilePath(): string {
        return path.join(this.GetKenshiPath(), "data", "__mods.list");
    }

    /**
     * Returns the absolute path to the Kenshi mods folder.
     */
    public static GetKenshiModsPath(): string {
        return this.DEFAULT_STEAM_MODS_PATH;
    }

    /**
     * Returns the absolute path to the Kenshi game folder.
     */
    public static GetKenshiPath(): string {
        return this.DEFAULT_STEAM_KENSHI_PATH;
    }

}
