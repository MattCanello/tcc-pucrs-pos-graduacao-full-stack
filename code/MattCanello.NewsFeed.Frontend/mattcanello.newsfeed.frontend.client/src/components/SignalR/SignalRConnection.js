import * as signalR from "@microsoft/signalr";
export default function getSignalRConnection() {
    function createConnection() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl('/hubs/article') 
            .configureLogging(signalR.LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        async function start() {
            try {
                await connection.start();
                console.log("SignalR connected");
            } catch (err) {
                console.log("SignalR connection error", err);
            }
        };

        connection.onclose(async () => {
            await start();
        });

        start();

        return connection;
    }

    if (window.signalRConnection === undefined) {
        window.signalRConnection = createConnection();
    }

    return (onNewArticleFound) => {
        window.signalRConnection.on("NewArticleFound", (article) => {
            onNewArticleFound(article);
        });
    };
}