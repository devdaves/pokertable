pokerApp.controller('StartCtrl', function ($scope, $location, PokerService) {
    $scope.newTableName = "";

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
});

pokerApp.controller('TableCtrl', function($scope, $routeParams, PokerService) {
    $scope.tableId = $routeParams.tableId;
});