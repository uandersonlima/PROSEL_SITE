var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("newMsg", () => Location.reload());