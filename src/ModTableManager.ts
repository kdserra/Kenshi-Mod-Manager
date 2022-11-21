import { BrowserWindow } from "electron";
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
   * Removes a Mod from the Mod Table, this is significantly slower
   * than RefreshModTable so should only be used if absolutely necessary.
   */
  protected static RemoveModFromModTable(mod: Mod) {
    const displayModTable: Mod[] = ModManager.GetAllMods();
    displayModTable.push(mod);
    BrowserWindow.getFocusedWindow()?.webContents.executeJavaScript(
      `document.getElementById('modTableRoot').innerHTML = '${ModTableManager.GetModTableString(displayModTable)}'`
    );
  }

  /**
   * Returns an HTML string representing an entry in the Mod Table.
   */
  protected static GetModTableEntryString(mod: Mod): string {
    const iconSize: string = "40px";
    const index: number = ModManager.GetAllMods().indexOf(mod);
    if (index == -1) { return ""; }
    const modTableEntryString: string = `<tr><th scope="row">${index + 1}</th><td><img src="file://${Utilities.EncodeHTML(mod.ImageFilePath)}" width="${Utilities.EncodeHTML(iconSize)}" /><text>${Utilities.EncodeHTML(mod.DisplayName)}</text></td><td><div class="form-check form-switch"><input class="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckDefault"><label class="form-check-label" for="flexSwitchCheckDefault"></label></div></td></tr>`;
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
}
