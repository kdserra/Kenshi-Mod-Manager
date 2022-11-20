import { app, BrowserWindow } from "electron";
import electronReload from "electron-reload";
import { Mod } from "./Mod";
import { ModManager } from "./ModManager";
import { ModTableManager } from "./ModTableManager";
electronReload(__dirname, {});

function GenerateRandomString(length: number): string {
  var result = '';
  var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
  var charactersLength = characters.length;
  for (var i = 0; i < length; i++) {
    result += characters.charAt(Math.floor(Math.random() * charactersLength));
  }
  return result;
}


const createWindow = () => {
  const win = new BrowserWindow({
    autoHideMenuBar: false,
    width: 1015,
    height: 900,
    minWidth: 900,
    minHeight: 900,
    title: "Kenshi Mod Manager",
    resizable: true,
  });

  win.loadFile("./public/main.html");
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

console.clear();