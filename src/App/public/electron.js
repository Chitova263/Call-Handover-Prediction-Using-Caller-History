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
  mainWindow = new BrowserWindow({width: 1000, height: 680});
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
    .connectTo("dotnet", "run", "--project", "./Main")
    .build();

connection.onDisconnect = () => {
  connection = new ConnectionBuilder()
    .connectTo("dotnet", "run", "--project", "./Main")
    .build();
};

ipcMain.on('results', (event, request) => {
  connection.send("greeting", request, response => {
    //event.sender.send("res", response)
    mainWindow.webContents.send("res", response);
  });
})

//Load users from datastore
connection.send('getusers', null, response => {
  console.log(response)
  mainWindow.webContents.send('getusers', response)
})

