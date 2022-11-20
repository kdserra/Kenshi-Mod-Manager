import { BrowserWindow } from "electron";
import { Mod } from "./Mod";
import { ModManager } from "./ModManager";

/**
 * Manages the ModTable the user interacts with for managing their mods.
 */
export class ModTableManager {
  public static ClearModTable() {
    BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
      `document.getElementById('modTableRoot')?.replaceChildren()`
    );
  }

  /**
   * Refreshes the ModTable, this method should only be called when
   * something changes.
   */
  public static RefreshModTable() {
    BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
      "document.getElementById('modTableRoot').innerHTML = '" + ModTableManager.GetModTableString(ModManager.GetAllMods()) + "'"
    );
  }

  /**
   * Forces the Mod Table to display only certain mods.
   * Be aware: changes to the mod list will force a refresh overriding
   * changes made.
   */
  public static ForceDisplayMods(mods: Mod[]) {
    BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
      "document.getElementById('modTableRoot').innerHTML = '" + ModTableManager.GetModTableString(ModManager.GetAllMods()) + "'"
    );
  }

  /**
   * Adds a Mod to the end of the ModTable, this is significantly slower
   * than RefreshModTable so should only be used if absolutely necessary.
   */
  protected static AddModToModTable(mod: Mod) {
    const displayModTable: Mod[] = ModManager.GetAllMods();
    displayModTable.push(mod);
    BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
      `document.getElementById('modTableRoot').innerHTML = '${ModTableManager.GetModTableString(displayModTable)}'`
    );
  }

  /**
   * Removes a Mod from the ModTable, this is significantly slower
   * than RefreshModTable so should only be used if absolutely necessary.
   */
  protected static RemoveModFromModTable(mod: Mod) {
    const displayModTable: Mod[] = ModManager.GetAllMods();
    displayModTable.push(mod);
    BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
      `document.getElementById('modTableRoot').innerHTML = '${ModTableManager.GetModTableString(displayModTable)}'`
    );
  }

  protected static GetModTableEntryString(mod: Mod): string {
    const iconSize: string = "40px";
    const imgSrc: string = "../assets/icon.png";
    const index: number = ModManager.GetAllMods().indexOf(mod);
    if (index == -1) { return ""; }
    const elementString: string = `<tr><th scope="row">${index + 1}</th><td><img src="${imgSrc}" width="${iconSize}" /><text>${mod.DisplayName}</text></td><td><div class="form-check form-switch"><input class="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckDefault"><label class="form-check-label" for="flexSwitchCheckDefault"></label></div></td></tr>`;
    return elementString;
  }

  protected static GetModTableString(mods: Mod[]): string {
    let modTableString: string = "";
    if (mods == null) { return ""; }
    for (let i = 0; i < mods.length; i++) {
      modTableString += ModTableManager.GetModTableEntryString(mods[i]);
    }
    return modTableString;
  }
}
