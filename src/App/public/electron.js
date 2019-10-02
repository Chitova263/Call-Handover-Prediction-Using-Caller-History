const electron = require('electron');
const app = electron.app;
const BrowserWindow = electron.BrowserWindow;

const path = require('path');
const isDev = require('electron-is-dev');
const { ConnectionBuilder } = require('electron-cgi');
const { ipcMain } = require('electron');


let mainWindow;
let connection;

function createWindow() {
  mainWindow = new BrowserWindow({width: 900, height: 680});
  mainWindow.loadURL(isDev ? 'http://localhost:3000' : `file://${path.join(__dirname, '../build/index.html')}`);
  if (isDev) {
    // Open the DevTools.
    //BrowserWindow.addDevToolsExtension('<location to your react chrome extension>');
    mainWindow.webContents.openDevTools();
  }

  mainWindow.on('closed', () => mainWindow = null);
}

app.on('ready', createWindow);

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  if (mainWindow === null) {
    createWindow();
  }
});

connection = new ConnectionBuilder()
    .connectTo("./publish/VerticalHandoverPrediction")
    .build();

connection.onDisconnect = () => {
  console.log("lost");
};

ipcMain.on('results', (event, response) => {
  connection.send("greeting", "Mom from C#", response => {
    console.log(`${response} herer`)
    //event.sender.send("res", response)
    mainWindow.webContents.send("res", response);
  });
})
