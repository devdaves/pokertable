pokerApp.controller('StartCtrl', function ($scope, PokerService) {
    $scope.newTableName = "";

    $scope.createTable = function () {
        PokerService.createTable($scope.newTableName).then(function(data) {
            console.log(data);
        });
    };
});