var signalr = {

    // Adicionar um handler para o recebimento de alguma mensagem em um método específico
    bindConnectionMessage: function (method, handler) {
        if (handler != undefined && handler != undefined)
            this.connection.on(method, handler);

        this.connection.onclose(this.onConnectionError);
    },

    // Ação ao criar conexão
    onConnected: function (connection) {
        toast.success('Conexão com SignalR inciada.');
    },

    // Ação ao ocorrer erro de conexão
    onConnectionError: function (error) {
        toast.error('Falha ao criar conexão com o SignalR: ' + error);
    },

    // Criação de uma nova conexão
    createConnection: function (hub) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(hub)
            .build();
    },

    // Iniciar escopo de uma conexão
    startConnection: function () {
        this.connection.start()
            .then(() => this.onConnected(this.connection))
            .catch(error => console.error(error.message));
    },

    connection: {}

};
