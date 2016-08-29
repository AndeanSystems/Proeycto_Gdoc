(function () {
    'use strict';

    angular.module('app').controller('concepto_controller', concepto_controller);
    concepto_controller.$inject = ['$location', 'app_factory'];

    function concepto_controller($location, dataProvider) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.concepto = {};
        context.gridOptions = {
            data: [],
            columnDefs: [
                { field: 'CodiConcepto', displayName: 'Codigo' },
                { field: 'DescripcionConcepto', displayName: 'Descripcion' },
                { field: 'DescripcionCorta', displayName: 'Abreviatura' },
                { field: 'ValorUno', displayName: 'Valor1' },
                { field: 'ValorDos', displayName: 'Valor2' },
                { field: 'TextoUno', displayName: 'Texto1' },
                { field: 'TextoDos', displayName: 'Texto2' },
                { field: 'EstadoConcepto', displayName: 'Estado' }
            ]
        };
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
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
        listarConcepto();
    }
})();
