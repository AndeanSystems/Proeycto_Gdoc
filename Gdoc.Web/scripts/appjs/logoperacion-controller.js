(function () {
    'use strict';
    angular.module('app').controller('logoperacion_controller', logoperacion_controller);
    logoperacion_controller.$inject = ['$location', 'app_factory', 'appService'];
    function logoperacion_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables

        let TipoDocumento = "012";
        var context = this;
        let TipoOperacion = "003";
        context.operacion = {};
        context.FechaBusqueda = new Date();

        LlenarConcepto(TipoOperacion);
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableSorting: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'Usuario.NombreUsuario', width: '20%', displayName: 'Usuario' },
                { field: 'Evento.DescripcionConcepto', width: '60%', displayName: 'Evento' },
                { field: 'FechaEvento', width: '20%', displayName: 'Fecha Evento', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm"' }
            ]
        };
        //Eventos

        //Metodos
        context.obtenerTipoDocumento = function (tipooperacion) {
            var texto = "";
            if (tipooperacion == "02") {
                texto = "DD";
                context.operacion.TipoDocumento = "41";
            }
            else if (tipooperacion == "03") {
                texto = "DE";
                context.operacion.TipoDocumento = "01";
            }
            else if (tipooperacion == "04") {
                texto = "MV";
                context.operacion.TipoDocumento = "81";
            }
            var concepto = { TipoConcepto: TipoDocumento, TextoUno: texto }
            console.log(concepto);
            dataProvider.postData("Concepto/ListarConceptoTipoDocumento", concepto).success(function (respuesta) {
                console.log(respuesta);
                context.listTipoDocumento = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        context.buscarLogOperacion = function (operacion) {
            console.log(operacion);
            dataProvider.postData("LogOperacion/ListarLogOperacion", operacion).success(function (respuesta) {
                console.log(respuesta);
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
        function listarAlertaPorFecha(fecha) {
            dataProvider.postData("LogOperacion/ListarLogOperacionPorFechas", { fecha: fecha }).success(function (respuesta) {
                context.gridOptions.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
    }
})();
