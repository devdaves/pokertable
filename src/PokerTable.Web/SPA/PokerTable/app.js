var pokerApp = angular.module('pokerApp', ['ngRoute']);

pokerApp.config(function ($routeProvider) {
    $routeProvider
        .when("/", { templateUrl: '/SPA/PokerTable/PartialViews/Start.html', controller: "StartCtrl" })
        .when("/tableview/:tableId", { templateUrl: '/SPA/PokerTable/PartialViews/TableView.html', controller: "TableCtrl" })
        .otherwise({ redirectTo: "/" });
});