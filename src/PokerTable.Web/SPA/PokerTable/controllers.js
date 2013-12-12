pokerApp.controller('StartCtrl', function ($scope, $location, PokerService) {
    $scope.newTableName = "";
    $scope.tableCode = "";
    $scope.playerName = "";

    $.connection.hub.start()
        .done(function () {
            console.log('Now connected, connection ID=' + $.connection.hub.id);
        })
        .fail(function () {
            console.log('Could not Connect!');
        });

    $scope.createTable = function () {
        PokerService.createTable($scope.newTableName).then(function(t) {
            $location.path('/tableview/' + t);
        });
    };

    $scope.joinTable = function () {
        PokerService.joinTable($scope.tableCode, $scope.playerName).then(function(t, p) {
            $location.path('/playerview/' + t + '/' + p);
        });
    };
});

pokerApp.controller('TableCtrl', function($scope, $routeParams, PokerService) {
    $scope.tableId = $routeParams.tableId;
    $scope.groupConnection = false;
    $scope.table = null;

    $scope.refreshTable = function () {
        PokerService.getTable($scope.tableId).then(function(t) {
            $scope.table = t;
        });
    };

    $scope.joinGroup = function () {
        console.log('Joining group: ' + $scope.tableId);
        pokerHubProxy.server.joinGroup($scope.tableId).done(function() {
            console.log('Joined group: ' + $scope.tableId);
            $scope.groupConnection = true;
        });
    };

    $scope.playerCount = function() {
        if ($scope.table == null) {
            return 0;
        }
        return $scope.table.Players.length;
    };
    
    $.connection.hub.start()
        .done(function () {
            console.log('Now connected, connection ID=' + $.connection.hub.id);
            $scope.joinGroup();
            $scope.refreshTable();
        })
        .fail(function () {
            console.log('Could not Connect!');
        });
});

pokerApp.controller('PlayerCtrl', function ($scope, $routeParams, PokerService) {
    $scope.tableId = $routeParams.tableId;
    $scope.playerId = $routeParams.playerId;

    $scope.joinGroup = function () {
        console.log('Joining group: ' + $scope.tableId);
        pokerHubProxy.server.joinGroup($scope.tableId).done(function () {
            console.log('Joined group: ' + $scope.tableId);
            $scope.groupConnection = true;
        });
    };

    $.connection.hub.start()
        .done(function () {
            console.log('Now connected, connection ID=' + $.connection.hub.id);
            $scope.joinGroup();
        })
        .fail(function () {
            console.log('Could not Connect!');
        });
});