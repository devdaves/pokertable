var pokerHubProxy = $.connection.pokerHub;

pokerHubProxy.client.playerJoined = function(playerName) {
    console.log(playerName + " joined the table, refreshing the table.");
    var scope = angular.element($("#pokerViewContainer")).scope();
    scope.playerAdded(playerName);
    scope.refresh();
};

pokerHubProxy.client.refresh = function () {
    var scope = angular.element($("#pokerViewContainer")).scope();
    scope.refresh();
};

