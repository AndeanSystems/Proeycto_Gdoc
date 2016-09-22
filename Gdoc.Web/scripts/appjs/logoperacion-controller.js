(function () {
    'use strict';
    angular.module('app').controller('logoperacion_controller', logoperacion_controller);
    logoperacion_controller.$inject = ['$location', 'app_factory', 'appService'];
    function logoperacion_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        let TipoOperacion = "003";
        context.operacion = {};

        LlenarConcepto(TipoOperacion);
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'Usuario.NombreUsuario', width: '25%', displayName: 'Usuario' },
                { field: 'Evento.DescripcionConcepto', width: '75%', displayName: 'Comentario' }
            ]
        };
        //Eventos

        //Metodos
        context.buscarLogOperacion=function (operacion) {
            dataProvider.postData("LogOperacion/ListarLogOperacion", operacion).success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        function LlenarConcepto(tipoConcepto) {
            var concepto = { TipoConcepto: tipoConcepto };
            appService.listarConcepto(concepto).success(function (respuesta) {
                if (concepto.TipoConcepto == TipoOperacion)
                    context.listTipoOperacion = respuesta;
            });
        }
        //Carga
    }
})();
