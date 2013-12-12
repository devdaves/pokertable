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
                        deferred.resolve(data.tableId, data.PlayerId);
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
        }
    };
});