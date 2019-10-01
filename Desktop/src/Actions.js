import { ConnectionBuilder } from 'electron-cgi';



export const getResults = () => {
    let connection = new ConnectionBuilder()
        .connectTo("dotnet", "run", "--project", "./Core")
        .build();

    connection.onDisconnect = () => {
        console.log("lost");
        connection = new ConnectionBuilder()
            .connectTo("dotnet", "run", "--project", "./Core")
            .build();
    };
    return new Promise((resolve, reject) => {
        connection.send("greeting", "Mom from C#", (response) => {
            console.log("sending");
            console.log(response);
            resolve(response);
        });
    })
}