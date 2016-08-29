(function () {
    'use strict';

    angular.module('app').controller('usuario_controller', usuario_controller);
    usuario_controller.$inject = ['$location', 'app_factory'];

    function usuario_controller($location, dataProvider) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.listUsuario = [];
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
        function listarUsuario() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.listUsuario = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
        listarUsuario();
    }
})();
