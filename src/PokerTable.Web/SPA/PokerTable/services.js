pokerApp.factory('PokerService', function ($http) {
    return {
        createTable: function (tableName) {
            var postData = {
                TableName: tableName
            };

            return $http.post('/Poker/CreateTable', postData).then(function (result) {
                return result.data;
            });
        }
    };
});