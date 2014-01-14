pokerApp.factory('PokerService', function ($q, cfpLoadingBar) {
    return {
        createTable: function (tableName) {
            var deferred = $q.defer();

            try {
                cfpLoadingBar.start();
                pokerHubProxy.server.createTable(tableName).done(function (data) {
                    console.log(data);
                    cfpLoadingBar.complete();
                    if (data.Status == 1) {
                        alert(data.FailureMessage);
                    } else {
                        console.log("Table Created");
                        deferred.resolve(data.TableId);
                    }
                });

            } catch(e) {
                deferred.reject(e);
            }
            
            return deferred.promise;
        },
        joinTable: function (tableCode, playerName) {
            var deferred = $q.defer();

            try {
                cfpLoadingBar.start();
                pokerHubProxy.server.joinTable(tableCode, playerName).then(function (data) {
                    console.log(data);
                    cfpLoadingBar.complete();
                    if (data.Status == 1) {
                        alert(data.FailureMessage);
                    } else {
                        console.log("Table Joined");
                        var result = { tableId: data.TableId, playerId: data.PlayerId };
                        deferred.resolve(result);
                    }
                });
            } catch(e) {
                deferred.reject(e);
            } 

            return deferred.promise;
        },
        getTable: function (tableId) {
            var deferred = $q.defer();

            try {
                console.log("Getting Table");
                cfpLoadingBar.start();
                pokerHubProxy.server.getTable(tableId).done(function (t) {
                    console.log("Table Loaded");
                    console.log(t);
                    cfpLoadingBar.complete();
                    deferred.resolve(t);
                });
            } catch(e) {
                deferred.reject(e);
            } 

            return deferred.promise;
        },
        addPlayerToSeat: function(tableId, seatId, playerId) {
            var deferred = $q.defer();

            try {
                console.log("Adding Player to Seat");
                cfpLoadingBar.start();
                pokerHubProxy.server.addPlayerToSeat(tableId, seatId, playerId).done(function (data) {
                    console.log("Player added to Seat");
                    console.log(data);
                    cfpLoadingBar.complete();
                    deferred.resolve(data);
                });
            } catch(e) {
                deferred.reject(e);
            } 

            return deferred.promise;
        },
        removePlayerFromSeat: function (tableId, seatId) {
            var deferred = $q.defer();

            try {
                console.log("Removing Player from Seat");
                cfpLoadingBar.start();
                pokerHubProxy.server.removePlayerFromSeat(tableId, seatId).done(function (data) {
                    console.log("Player removed from Seat");
                    console.log(data);
                    cfpLoadingBar.complete();
                    deferred.resolve(data);
                });
            } catch (e) {
                deferred.reject(e);
            }

            return deferred.promise;
        },
        setDealer: function (tableId, seatId) {
            var deferred = $q.defer();

            try {
                console.log("Setting Dealer");
                cfpLoadingBar.start();
                pokerHubProxy.server.setDealer(tableId, seatId).done(function (data) {
                    console.log("Dealer Set");
                    console.log(data);
                    cfpLoadingBar.complete();
                    deferred.resolve(data);
                });
            } catch (e) {
                deferred.reject(e);
            }

            return deferred.promise;
        },
        dealPlayers: function(tableId) {
            var deferred = $q.defer();

            try {
                console.log("Dealing Players");
                cfpLoadingBar.start();
                pokerHubProxy.server.dealPlayers(tableId).done(function (data) {
                    console.log("Players Dealt");
                    console.log(data);
                    cfpLoadingBar.complete();
                    deferred.resolve(data);
                });
            } catch (e) {
                deferred.reject(e);
            }

            return deferred.promise;
        },
        dealFlop: function(tableId) {
            var deferred = $q.defer();

            try {
                console.log("Dealing Flop");
                cfpLoadingBar.start();
                pokerHubProxy.server.dealFlop(tableId).done(function (data) {
                    console.log("Flop Dealt");
                    console.log(data);
                    cfpLoadingBar.complete();
                    deferred.resolve(data);
                });
            } catch (e) {
                deferred.reject(e);
            }

            return deferred.promise;
        },
        dealTurn: function(tableId) {
            var deferred = $q.defer();

            try {
                console.log("Dealing Turn");
                cfpLoadingBar.start();
                pokerHubProxy.server.dealTurn(tableId).done(function (data) {
                    console.log("Turn Dealt");
                    console.log(data);
                    cfpLoadingBar.complete();
                    deferred.resolve(data);
                });
            } catch (e) {
                deferred.reject(e);
            }

            return deferred.promise;
        },
        dealRiver: function(tableId) {
            var deferred = $q.defer();

            try {
                console.log("Dealing River");
                cfpLoadingBar.start();
                pokerHubProxy.server.dealRiver(tableId).done(function (data) {
                    console.log("River Dealt");
                    console.log(data);
                    cfpLoadingBar.complete();
                    deferred.resolve(data);
                });
            } catch (e) {
                deferred.reject(e);
            }

            return deferred.promise;
        },
        resetTable: function(tableId) {
            var deferred = $q.defer();

            try {
                console.log("Reseting the table");
                cfpLoadingBar.start();
                pokerHubProxy.server.resetTable(tableId).done(function (data) {
                    console.log("Table Reset");
                    console.log(data);
                    cfpLoadingBar.complete();
                    deferred.resolve(data);
                });
            } catch (e) {
                deferred.reject(e);
            }

            return deferred.promise;
        }
    };
});