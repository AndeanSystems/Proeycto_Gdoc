(function () {
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

            function enviarFomularioOK() {
                dataProvider.postData("General/EditarGeneralParametros", general).success(function (respuesta) {
                    console.log(respuesta);
                    listarGeneral();
                    appService.mostrarAlerta("Información", "Se grabo correctamente", "success");
                }).error(function (error) {
                    //MostrarError();
                });
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);
        }

        //Metodos
        context.obtenerParametros = function (idempresa) {
            dataProvider.postData("General/ListarGeneralParametros", { IDEmpresa: idempresa }).success(function (respuesta) {
                console.log(respuesta);
                context.general = respuesta[0];
                context.general.HoraActualizaEstadoOperacion = appService.setFormatDate(context.general.HoraActualizaEstadoOperacion);
                context.general.HoraCierreLabores = appService.setFormatDate(context.general.HoraCierreLabores);
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
