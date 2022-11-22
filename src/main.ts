import { app, BrowserWindow } from "electron";
import { ModManager } from "./ModManager";
import { Mod } from "./Mod";
import { ModIOManager } from "./ModIOManager";
import electronReload from "electron-reload";
import path from "path";

electronReload(__dirname, {});

const createWindow = () => {
  const win = new BrowserWindow({
    autoHideMenuBar: false,
    width: 1015,
    height: 1015,
    minWidth: 1015,
    minHeight: 1015,
    title: "Kenshi Mod Manager",
    resizable: true,
  });

  win.loadFile(path.join(__dirname, "public", "main.html"));
  const mods: Mod[] = ModIOManager.GetAllModsFromDisk([ModIOManager.DEFAULT_STEAM_MODS_ABSOLUTE_DIRECTORY]);
  ModManager.AddMods(mods);
};

app.whenReady().then(() => {
  createWindow();

  app.on("activate", () => {
    if (BrowserWindow.getAllWindows().length === 0) createWindow();
  });
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") app.quit();
});
