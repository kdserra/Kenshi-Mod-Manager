import { app, BrowserWindow, ipcMain } from "electron";
import { ModManager } from "./ModManager";
import { Mod } from "./Mod";
import { ModIOManager } from "./ModIOManager";
import electronReload from "electron-reload";
import path from "path";

electronReload(__dirname, {});

const createWindow = () => {
  const win = new BrowserWindow({
    webPreferences: {
      nodeIntegration: true,
      contextIsolation: false,
    },
    autoHideMenuBar: false,
    width: 1015,
    height: 1015,
    minWidth: 1015,
    minHeight: 1015,
    title: "Kenshi Mod Manager",
    resizable: true,
  });

  win.loadFile(path.join(__dirname, "public", "pages", "MainPage", "MainPage.html"));
  const mods: Mod[] = ModIOManager.GetAllModsFromDisk();
  ModManager.SetMods(mods);
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

ipcMain.on("settingsBtnClick", () => {
  console.log("settingsBtnClick");
  const createSettingsWindow = () => {
    const win = new BrowserWindow({
      webPreferences: {
        nodeIntegration: true,
        contextIsolation: false,
      },
      autoHideMenuBar: true,
      width: 300,
      height: 300,
      minWidth: 300,
      minHeight: 300,
      title: "Kenshi Mod Manager [Settings]",
      resizable: true,
    });
    
    win.loadFile(path.join(__dirname, "public", "pages", "SettingsPage", "SettingsPage.html"));
  };

  createSettingsWindow();
});

ipcMain.on("newProfileBtnClick", () => {
  console.log("newProfileBtnClick");
});

ipcMain.on("saveProfileBtnClick", () => {
  console.log("saveProfileBtnClick");
});

ipcMain.on("openProfileBtnClick", () => {
  console.log("openProfileBtnClick");
});

ipcMain.on("saveToKenshiBtnClick", () => {
  console.log("saveToKenshiBtnClick");
  ModIOManager.SaveModsToKenshiModConfig(ModManager.GetActiveMods());
});

ipcMain.on("orderModsBtnClick", () => {
  console.log("orderModsBtnClick");
  const mods: Mod[] = ModManager.OrderMods(ModManager.GetAllMods());
  ModManager.SetMods(mods);
});

ipcMain.on("refreshBtnClick", () => {
  console.log("refreshBtnClick");
  const mods: Mod[] = ModIOManager.GetAllModsFromDisk();
  ModManager.SetMods(mods);
});
