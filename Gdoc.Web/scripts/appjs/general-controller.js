﻿(function () {
    'use strict';

    angular.module('app').controller('general_controller', general_controller);
    general_controller.$inject = ['$location', 'app_factory', 'appService'];

    function general_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.general = {};

        //Eventos
        context.editar= function () {
            var general = context.general;

           
            console.log(general);
            dataProvider.postData("General/EditarGeneralParametros", general).success(function (respuesta) {
                console.log(respuesta);
                listarGeneral();
            }).error(function (error) {
                //MostrarError();
            });
        }

        //Metodos
        context.obtenerParametros = function (idempresa) {
            console.log(idempresa);
            dataProvider.postData("General/ListarGeneralParametros", { IDEmpresa: idempresa }).success(function (respuesta) {
                console.log(respuesta);
                context.general = respuesta[0];
            }).error(function (error) {
                //MostrarError();
            });
        }
        function listarGeneral() {
            dataProvider.getData("General/ListarGeneralParametros").success(function (respuesta) {
                context.listGeneral = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarEmpresa() {
            dataProvider.getData("Empresa/ListarEmpresa").success(function (respuesta) {
                context.listEmpresa = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        
        //Carga
        //listarGeneral();
        listarEmpresa();
    }
})();