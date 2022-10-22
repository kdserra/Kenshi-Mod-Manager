import { app, BrowserWindow } from "electron";
import electronReload from "electron-reload";
electronReload(__dirname, {});

const createWindow = () => {
  const win = new BrowserWindow({
    autoHideMenuBar: true,
    width: 800,
    height: 600,
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

console.log("Hello World");
