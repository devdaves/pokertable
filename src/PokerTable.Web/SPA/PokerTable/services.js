pokerApp.factory('PokerService', function ($http) {
    return {
        createTable: function (tableName) {
            var postData = {
                TableName: tableName
            };

            return $http.post('/Poker/CreateTable', postData).then(function (result) {
                return result.data;
            });
        },
        joinTable: function(tableCode, playerName) {
            var postData = {
                TableCode: tableCode,
                PlayerName: playerName
            };
            
            return $http.post('/Poker/JoinTable', postData).then(function (result) {
                return result.data;
            });
        }
    };
});