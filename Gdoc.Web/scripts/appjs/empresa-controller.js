(function () {
    'use strict';

    angular.module('app').controller('empresa_controller', empresa_controller);
    empresa_controller.$inject = ['$location', 'app_factory'];

    function empresa_controller($location, dataProvider) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.listEmpresa = [];
        //context.Usuario = {};
        //Eventos

        //context.grabar = function () {
        //    console.log(context.Usuario);
        //    dataProvider.postData("Concepto/GrabarConcepto", context.Usuario).success(function (respuesta) {
        //        console.log(respuesta);
        //    }).error(function (error) {
        //        //MostrarError();
        //    });
        //}

        //Metodos
        function listarEmpresa() {
            dataProvider.getData("Empresa/ListarEmpresa").success(function (respuesta) {
                context.listEmpresa = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
        listarEmpresa();
    }
})();
