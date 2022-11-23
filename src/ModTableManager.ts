import { BrowserWindow, ipcRenderer } from "electron";
import { Mod } from "./Mod";
import { ModManager } from "./ModManager";
import { Utilities } from "./Utilities";

/**
 * Manages the Mod Table the user interacts with for managing their mods.
 */
export class ModTableManager {
  public static ClearModTable() {
    BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
      `document.getElementById('modTableRoot')?.replaceChildren()`
    );
  }

  /**
   * Refreshes the Mod Table, this method should only be called when
   * a change has been made to the the mod list.
   */
  public static RefreshModTable() {
    ModTableManager.DisplayMods(ModManager.GetAllMods());
  }

  /**
   * Displays mods in the Mod Table.
   * Be aware: changes to the Mod Manager's mod list will force a refresh overriding
   * any changes made to the Mod Table. Additionally, you can only display mods that
   * are currently in the Mod Manager's Mod List.
   */
  protected static DisplayMods(mods: Mod[]) {
    BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
      `document.getElementById('modTableRoot').innerHTML = '${ModTableManager.GetModTableString(mods)}'`
    );
  }

  /**
   * TODO: Finish this.
   * Registers the events to all Mod Table Entries to provide functionality.
   */
  protected static RegisterEventListeners() {
    /*
    const mods: Mod[] = ModManager.GetAllMods();
    for (let i = 0; i < mods.length; i++) {
      if (!ModManager.IsModInModList(mods[i])) { continue; }
      const mod: Mod = mods[i];
      BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
        `document.getElementById('modTableEntryInputExample').addEventListener("click", () => { console.log("Hello World!"); });'`
        //`document.getElementById('${ModTableManager.GetModTableEntryInputIDString(mod)}').addEventListener("click", () => { console.log("Hello World!"); });'`
      ).catch(() => { });
    }
    */
  }

  /**
   * Returns an HTML string representing an entry in the Mod Table.
   */
  protected static GetModTableEntryString(mod: Mod): string {
    const iconSize: number = 40;
    const index: number = ModManager.GetAllMods().indexOf(mod);
    if (index == -1) { return ""; }
    const modTableEntryString: string = `<tr id="${ModTableManager.GetModTableEntryRootIDString(mod)}"><th scope="row">${index + 1}</th><td><img src="${Utilities.EncodeURL(mod.ImageFilePath)}" width="${iconSize}px" height="${iconSize}px" /><text>${Utilities.EncodeHTMLEntity(mod.DisplayName)}</text></td><td><div class="form-check form-switch"><input id="${ModTableManager.GetModTableEntryInputIDString(mod)}" class="form-check-input" type="checkbox" role="switch"><label class="form-check-label" for="${ModTableManager.GetModTableEntryInputIDString(mod)}"></label></div></td></tr>`;
    return modTableEntryString;
  }

  /**
   * Returns an HTML string of the provided mods as entries in the Mod Table.
   */
  protected static GetModTableString(mods: Mod[]): string {
    let modTableString: string = "";
    if (mods == null) { return ""; }
    for (let i = 0; i < mods.length; i++) {
      modTableString += ModTableManager.GetModTableEntryString(mods[i]);
    }
    return modTableString;
  }

  protected static GetModTableEntryRootIDString(mod: Mod): string {
    const modTableEntryRootID: string = `modTableEntryRoot${mod.Guid.ToString()}`;
    return modTableEntryRootID;
  }

  protected static GetModTableEntryInputIDString(mod: Mod): string {
    const modTableEntryInputID: string = `modTableEntryInput${mod.Guid.ToString()}`;
    return modTableEntryInputID;
  }
}
