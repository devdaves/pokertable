pokerApp.controller('StartCtrl', function ($scope, $location, PokerService) {
    $scope.newTableName = "";
    $scope.tableCode = "";
    $scope.playerName = "";

    $scope.createTable = function () {
        PokerService.createTable($scope.newTableName).then(function(data) {
            console.log(data);

            if (data.Status == 1) {
                alert(data.FailureMessage);
            } else {
                $location.path('/tableview/' + data.TableId);
            }
        });
    };

    $scope.joinTable = function() {
        PokerService.joinTable($scope.tableCode, $scope.playerName).then(function(data) {
            console.log(data);

            if (data.Status == 1) {
                alert(data.FailureMessage);
            } else {
                $location.path('/playerview/' + data.TableId + '/' + data.PlayerId);
            }
        });
    };
});

pokerApp.controller('TableCtrl', function($scope, $routeParams, PokerService) {
    $scope.tableId = $routeParams.tableId;
    $scope.signalRConnected = false;
    $scope.groupConnection = false;
    $scope.table = null;


    $scope.refreshTable = function () {
        console.log("Loading Table");
        pokerHubProxy.server.getTable($scope.tableId).done(function (t) {
            console.log("Table Loaded");
            console.log(t);
            $scope.$apply(function() {
                $scope.table = t;
            });
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
        .done(function() {
            console.log('Now connected, connection ID=' + $.connection.hub.id);
            $scope.signalRConnected = true;
            $scope.joinGroup();
            $scope.refreshTable();
        })
        .fail(function () { console.log('Could not Connect!'); });
});

pokerApp.controller('PlayerCtrl', function ($scope, $routeParams, PokerService) {

});