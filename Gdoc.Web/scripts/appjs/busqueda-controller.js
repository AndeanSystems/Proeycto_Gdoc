(function () {
    'use strict';

    angular.module('app').controller('busqueda_controller', busqueda_controller);
    busqueda_controller.$inject = ['$location', 'app_factory', 'appService'];

    function busqueda_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        let TipoOperacion = "003";
        let TipoDocumento = "012";
        let PrioridadAtencion = "005";
        let TipoAcceso = "002";
        let TipoComunicacion = "022";
        context.operacion = {};

        LlenarConcepto(TipoOperacion);
        LlenarConcepto(TipoDocumento);
     
        context.buscarOperacion = function (operacion) {
            dataProvider.postData("Busqueda/ListarOperacionBusqueda", operacion).success(function (respuesta) {
                console.log(respuesta);
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        context.exportarExcel = function () {
            dataProvider.postData("Busqueda/ListToExcel", context.gridOptions.data).success(function (respuesta) {
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'TipoOpe.DescripcionCorta', displayName: 'Tipo Operacion' },
                { field: 'NumeroOperacion', displayName: 'Numero Operacion' },
                { field: 'FechaEnvio', displayName: 'Fecha Envio', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy"' },
                { field: 'TipoDoc.DescripcionCorta', displayName: 'Tipo Documento/Mesa' },
                {
                    name: 'Acciones',
                    cellTemplate: '<i  class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                '<i  class="fa fa-user-plus" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Compartir"></i> ' +
                                '<i  class="fa fa-times" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Eliminar"></i> '
                }
            ]
        };
        //Eventos
        //Metodos
        function LlenarConcepto(tipoConcepto) {
            var concepto = { TipoConcepto: tipoConcepto };
            appService.listarConcepto(concepto).success(function (respuesta) {
                if (concepto.TipoConcepto == TipoDocumento)
                    context.listTipoDocumento = respuesta;
                else if (concepto.TipoConcepto == PrioridadAtencion)
                    context.listPrioridadAtencion = respuesta;
                else if (concepto.TipoConcepto == TipoAcceso)
                    context.listTipoAcceso = respuesta;
                else if (concepto.TipoConcepto == TipoComunicacion)
                    context.listTipoComunicacion = respuesta;
                else if (concepto.TipoConcepto == TipoOperacion)
                    context.listTipoOperacion = respuesta;
            });
        }
        //Carga
    }
})();
