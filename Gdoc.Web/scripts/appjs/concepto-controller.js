(function () {
    'use strict';

    angular.module('app').controller('concepto_controller', concepto_controller);
    concepto_controller.$inject = ['$location', 'app_factory'];

    function concepto_controller($location, dataProvider) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.listConcepto = [];
        context.concepto = {};
        //Eventos
        context.grabar = function () {
            console.log(context.concepto);
            dataProvider.postData("Concepto/GrabarConcepto", context.concepto).success(function (respuesta) {
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }

        //Metodos
        function listarConcepto() {
            dataProvider.getData("Concepto/ListarConcepto").success(function (respuesta) {
                context.listConcepto = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
        listarConcepto();
    }
})();
