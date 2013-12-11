var pokerHubProxy = $.connection.pokerHub;

pokerHubProxy.client.hello = function () {
    alert("Hello method called from server");
};
