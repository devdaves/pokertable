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
        PokerService.joinTable($scope.tableCode, $scope.playerName).then(function(result) {
            $location.path('/playerview/' + result.tableId + '/' + result.playerId);
        });
    };
});

pokerApp.controller('TableCtrl', function($scope, $routeParams, PokerService) {
    $scope.tableId = $routeParams.tableId;
    $scope.groupConnection = false;
    $scope.table = null;

    $scope.refresh = function () {
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

    $scope.seatCount = function() {
        if ($scope.table == null) {
            return 0;
        }
        return $scope.table.Seats.length;
    };

    $scope.seatName = function(seat) {
        if (seat.PlayerId == null) {
            return seat.Id + ": Open";
        } else {
            var p = $scope.getPlayer(seat.PlayerId);
            if (seat.IsDealer) {
                return seat.Id + ": " + p.Name + "(Dealer)";
            } else {
                return seat.Id + ": " + p.Name;
            }
        }
    };

    $scope.getPlayer = function (playerId) {
        if ($scope.table != null && $scope.table.Players != null) {
            for (var i = 0; i < $scope.table.Players.length; i++) {
                if ($scope.table.Players[i].Id == playerId) {
                    return $scope.table.Players[i];
                }
            }
        }
        return null;
    };

    $scope.addPlayerToSeat = function(seatId, playerId) {
        PokerService.addPlayerToSeat($scope.tableId, seatId, playerId).then(function (result) {
            console.log(result);
            if (result.Status == 0) {
                $scope.refresh();
            }
        });
    };

    $scope.removePlayerFromSeat = function(seatId) {
        PokerService.removePlayerFromSeat($scope.tableId, seatId).then(function(result) {
            console.log(result);
            if (result.Status == 0) {
                $scope.refresh();
            }
        });
    };

    $scope.setDealer = function(seatId) {
        PokerService.setDealer($scope.tableId, seatId).then(function (result) {
            console.log(result);
            if (result.Status == 0) {
                $scope.refresh();
            }
        });
    };
    
    $.connection.hub.start()
        .done(function () {
            console.log('Now connected, connection ID=' + $.connection.hub.id);
            $scope.joinGroup();
            $scope.refresh();
        })
        .fail(function () {
            console.log('Could not Connect!');
        });
    
    // not needed after UX redesign below here...
    $scope.apts_seatId = "";
    $scope.apts_playerId = "";
    $scope.apts = function() {
        $scope.addPlayerToSeat($scope.apts_seatId, $scope.apts_playerId);
    };

    $scope.rpfs_seatId = "";
    $scope.rpfs = function() {
        $scope.removePlayerFromSeat($scope.rpfs_seatId);
    };

    $scope.sd_seatId = "";
    $scope.sd = function() {
        $scope.setDealer($scope.sd_seatId);
    };
});

pokerApp.controller('PlayerCtrl', function ($scope, $routeParams, PokerService) {
    $scope.tableId = $routeParams.tableId;
    $scope.playerId = $routeParams.playerId;
    $scope.table = null;
    $scope.player = null;

    $scope.refresh = function () {
        PokerService.getTable($scope.tableId).then(function (t) {
            $scope.table = t;
            $scope.player = $scope.getPlayer($scope.playerId);
        });
    };

    $scope.getSeatNumber = function() {
        var seat = $scope.getSeat($scope.playerId);
        if (seat != null) {
            return "Seat " + seat.Id;
        }
        return "Not in Seat.";
    };

    $scope.isDealer = function() {
        var seat = $scope.getSeat($scope.playerId);
        if (seat != null &&  seat.IsDealer) {
            return true;
        }
        return false;
    };

    $scope.getSeat = function (playerId) {
        if ($scope.table != null && $scope.table.Seats != null) {
            for (var i = 0; i < $scope.table.Seats.length; i++) {
                if ($scope.table.Seats[i].PlayerId == playerId) {
                    return $scope.table.Seats[i];
                }
            }
        }
        return null;
    };

    $scope.getPlayer = function (playerId) {
        if ($scope.table != null && $scope.table.Players != null) {
            for (var i = 0; i < $scope.table.Players.length; i++) {
                if ($scope.table.Players[i].Id == playerId) {
                    return $scope.table.Players[i];
                }
            }
        }
        return null;
    };

    $scope.joinGroup = function () {
        console.log('Joining group: ' + $scope.tableId);
        pokerHubProxy.server.joinGroup($scope.tableId).done(function () {
            console.log('Joined group: ' + $scope.tableId);
            $scope.groupConnection = true;
            $scope.refresh();
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