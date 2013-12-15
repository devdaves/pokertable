pokerApp.factory('PokerService', function ($q) {
    return {
        createTable: function (tableName) {
            var deferred = $q.defer();

            try {
                pokerHubProxy.server.createTable(tableName).done(function (data) {
                    console.log(data);
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
                pokerHubProxy.server.joinTable(tableCode, playerName).then(function (data) {
                    console.log(data);

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
                pokerHubProxy.server.getTable(tableId).done(function (t) {
                    console.log("Table Loaded");
                    console.log(t);
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
                pokerHubProxy.server.addPlayerToSeat(tableId, seatId, playerId).done(function (data) {
                    console.log("Player added to Seat");
                    console.log(data);
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
                pokerHubProxy.server.removePlayerFromSeat(tableId, seatId).done(function (data) {
                    console.log("Player removed from Seat");
                    console.log(data);
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
                pokerHubProxy.server.setDealer(tableId, seatId).done(function (data) {
                    console.log("Dealer Set");
                    console.log(data);
                    deferred.resolve(data);
                });
            } catch (e) {
                deferred.reject(e);
            }

            return deferred.promise;
        }
    };
});