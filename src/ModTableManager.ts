import { BrowserWindow, ipcMain } from "electron";
import { Mod } from "./Mod";
import { ModManager } from "./ModManager";
import { Utilities } from "./Utilities";

/**
 * Manages the Mod Table the user interacts with for managing their mods.
 */
export class ModTableManager {
  protected static s_RegisteredEventHandler: boolean = false;

  static {
    ModTableManager.RegisterEventHandler();
  }

  /**
   * Refreshes the Mod Table, this method should only be called when
   * a change has been made to the the mod list.
   */
  public static RefreshModTable() {
    ModTableManager.DisplayMods(ModManager.GetAllMods());
  }

  protected static ClearModTable() {
    BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
      `document.getElementById('modTableRoot')?.replaceChildren()`
    );
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
    ModTableManager.RegisterModTableEntryEventListeners();
  }

  /**
   * Registers the events to all Mod Table Entries to provide functionality.
   */
  protected static RegisterEventHandler() {
    if (ModTableManager.s_RegisteredEventHandler) { return; }
    ipcMain.on("modTableEntryInputClick", (event, guid: string) => {
      const mod = ModManager.GetModWithGuid(guid);
      if (mod === null) { return; }
      mod.Active = !mod.Active;
    });
    ModTableManager.s_RegisteredEventHandler = true;
  }

  /**
   * Registers the events to all Mod Table Entries to provide functionality.
   */
  protected static RegisterModTableEntryEventListeners() {
    const mods: Mod[] = ModManager.GetAllMods();
    for (let i = 0; i < mods.length; i++) {
      if (!ModManager.IsModInModList(mods[i])) { continue; }
      const mod: Mod = mods[i];
      BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
        `document?.getElementById("${ModTableManager.GetModTableEntryInputIDString(mod)}")?.addEventListener("click", () =>
        {
          const { ipcRenderer } = require("electron");
          ipcRenderer?.send("modTableEntryInputClick", "${mod.Guid}");
        });`
      ).catch();
    }
  }

  /**
   * Returns an HTML string representing an entry in the Mod Table.
   */
  protected static GetModTableEntryString(mod: Mod): string {
    const iconSize: number = 40;
    const index: number = ModManager.GetAllMods().indexOf(mod);
    if (index == -1) { return ""; }
    const modTableEntryString: string = `<tr id="${ModTableManager.GetModTableEntryRootIDString(mod)}"><th scope="row">${index + 1}</th><td><img src="${Utilities.EncodeURL(mod.ImageFilePath)}" width="${iconSize}px" height="${iconSize}px" /><text>${Utilities.EncodeHTMLEntity(mod.Title)}</text></td><td><div class="form-check form-switch"><input id="${ModTableManager.GetModTableEntryInputIDString(mod)}" class="form-check-input" type="checkbox" role="switch"><label class="form-check-label" for="${ModTableManager.GetModTableEntryInputIDString(mod)}"></label></div></td></tr>`;
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
    const modTableEntryRootID: string = `modTableEntryRoot${mod.Guid}`;
    return modTableEntryRootID;
  }

  protected static GetModTableEntryInputIDString(mod: Mod): string {
    const modTableEntryInputID: string = `modTableEntryInput${mod.Guid}`;
    return modTableEntryInputID;
  }
}
