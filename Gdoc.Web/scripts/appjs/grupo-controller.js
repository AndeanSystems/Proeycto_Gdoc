(function () {
    'use strict';

    angular.module('app').controller('grupo_controller', grupo_controller);
    grupo_controller.$inject = ['$location', 'app_factory'];

    function grupo_controller($location, dataProvider) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.listGrupo = [];
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
        function listarGrupo() {
            dataProvider.getData("Grupo/ListarGrupo").success(function (respuesta) {
                context.listGrupo = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
        listarGrupo();
    }
})();
