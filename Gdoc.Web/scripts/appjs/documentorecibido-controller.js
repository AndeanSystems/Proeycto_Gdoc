(function () {
    'use strict';
    angular.module('app').controller('documentorecibido_controller', documentorecibido_controller);
    documentorecibido_controller.$inject = ['$location', 'app_factory', 'appService'];
    function documentorecibido_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.operacion = {};

        context.mostrarPDF = function (rowIndex) {
            try {
                context.operacion = context.gridOptions.data[rowIndex];
                var ruta = "http://localhost:99/PDF/" + context.operacion.NumeroOperacion + ".pdf";
                window.open(ruta, '_blank');
            } catch (e) {
                //return appService.mostrarAlerta("No encontrado", "El Documento no se ha encontrado o ha sido eliminado", "error");
                swal("No encontrado", "El Documento no se ha encontrado o ha sido eliminado", "error");
            }
            
        }
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NumeroOperacion', displayName: 'Nº Documento' },
                { field: 'TipoOpe.DescripcionConcepto', displayName: 'Tipo Operacion' },
                { field: 'TipoDoc.DescripcionConcepto', displayName: 'Tipo de Documento' },
                { field: 'DescripcionOperacion', displayName: '	Asunto' },
                { field: 'FechaRegistro', displayName: 'Fecha Emisión', cellFilter: 'toDateTime | date:"dd/MM/yyyy"' },
                { field: 'FechaVigente', displayName: '	Fecha Recepción', cellFilter: 'toDateTime | date:"dd/MM/yyyy"' },
                { field: 'Estado.DescripcionConcepto', displayName: 'Estado' },
                {
                    name: 'Ver',
                    cellTemplate: '<i class="fa fa-paperclip" ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" target="_blank" data-placement="bottom" data-toggle="tooltip" title="Abrir pdf"></i>' +
                            '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-eye" data-placement="bottom" data-toggle="tooltip" title="Adjuntos"></i>'
                }
            ]
        };
        //Eventos

        //Metodos
        context.buscarLogOperacion = function (operacion) {
            dataProvider.postData("LogOperacion/ListarLogOperacion", operacion).success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarOperacion() {
            dataProvider.getData("DocumentosRecibidos/ListarOperacion").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
        listarOperacion();
    }
})();
