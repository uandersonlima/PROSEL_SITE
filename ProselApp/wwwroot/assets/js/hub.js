var connection = new signalR.HubConnectionBuilder().withUrl("/Msg").build();
connection.start();