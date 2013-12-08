var pokerApp = angular.module('pokerApp', ['ngRoute']);

pokerApp.config(function ($routeProvider) {
    $routeProvider
        .when("/", { templateUrl: '/SPA/PokerTable/PartialViews/Start.html', controller: "StartCtrl" })
        .otherwise({ redirectTo: "/" });
});