var pokerHubProxy = $.connection.pokerHub;

pokerHubProxy.client.playerJoined = function(playerName) {
    console.log(playerName + " joined the table, refreshing the table.");
    var scope = angular.element($("#tableContainer")).scope();
    scope.refreshTable();
};

