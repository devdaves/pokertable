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

pokerApp.controller('TableCtrl', function($scope, $routeParams, PokerService, toaster) {
    $scope.tableId = $routeParams.tableId;
    $scope.groupConnection = false;
    $scope.table = null;
    $scope.currentSeat = 0;

    $scope.playerAdded = function(playerName) {
        toaster.pop('success', null, playerName + " joined");
    };

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

    $scope.seatsTop = function() {
        if ($scope.table != null && $scope.table.Seats != null && $scope.table.Seats.length > 0) {
            return $scope.table.Seats.slice(0, 5);
        }

        return [];
    };
    
    $scope.seatsBottom = function() {
        if ($scope.table != null && $scope.table.Seats != null && $scope.table.Seats.length > 0) {
            return $scope.table.Seats.slice(5, 10).reverse();
        }

        return [];
    };

    $scope.seatName = function(seat) {
        if (seat.PlayerId == null) {
            return "Open";
        } else {
            var p = $scope.getPlayer(seat.PlayerId);
            if (seat.IsDealer) {
                return p.Name + "(Dealer)";
            } else {
                return p.Name;
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

    $scope.dealPlayers = function () {
        PokerService.dealPlayers($scope.tableId).then(function(result) {
            if (result.Status == 0) {
                $scope.refresh();
            }
        });
    };

    $scope.dealFlop = function() {
        PokerService.dealFlop($scope.tableId).then(function(result) {
            if (result.Status == 0) {
                $scope.refresh();
            }
        });
    };

    $scope.dealTurn = function() {
        PokerService.dealTurn($scope.tableId).then(function(result) {
            if (result.Status == 0) {
                $scope.refresh();
            }
        });
    };

    $scope.dealRiver = function() {
        PokerService.dealRiver($scope.tableId).then(function(result) {
            if (result.Status == 0) {
                $scope.refresh();
            }
        });
    };

    $scope.resetTable = function() {
        PokerService.resetTable($scope.tableId).then(function(result) {
            if (result.Status == 0) {
                $scope.refresh();
            }
        });
    };

    $scope.showSeatOptions = function(seatId) {
        $scope.currentSeat = seatId;
        $('#seatModal').modal('show');
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