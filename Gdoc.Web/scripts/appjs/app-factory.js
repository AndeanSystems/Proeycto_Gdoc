(function () {

    'use strict';

    angular.module('app', ['ui.grid']).factory('app_factory', app_factory);

    app_factory.$inject = ['$http'];

    function app_factory($http) {
        var service = {
            getData: getData,
            postData: postData,
            putData: putData,
            deleteData: deleteData
        };

        return service;

        function getData(url) {
            return $http.get(url);
        }
        function postData(url, data) {
            return $http.post(url, data);
        }
        function putData(url, data) {
            return $http.put(url, data);
        }
        function deleteData(url, data) {
            return $http.delete(url, data);
        }

    }
})();