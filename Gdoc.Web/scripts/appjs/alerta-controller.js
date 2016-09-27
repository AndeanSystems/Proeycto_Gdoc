﻿(function () {
    'use strict';

    angular.module('app').controller('alerta_controller', alerta_controller);
    alerta_controller.$inject = ['$location', 'app_factory', 'appService'];

    function alerta_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.alerta = {};

        //Eventos
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'Operacion.NumeroOperacion', displayName: 'Numero Operacion' },
                { field: 'FechaAlerta', displayName: 'Fecha Emision', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                //{ field: 'Usuario.NombreUsuario', displayName: 'Destinatario' },
                //{ field: 'Remitente', displayName: 'Remitente' },
                { field: 'TipoOperacion.DescripcionCorta', displayName: 'T.Ope' },
                { field: 'TipoDocumento.DescripcionCorta', displayName: 'Tipo' },
                {
                    name: 'Acciones',
                    cellTemplate: '<i class="fa fa-paperclip" ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Ver Operacion"></i>'
                }

            ],

        };

        //Metodos

        context.mostrarPDF = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            console.log(context.operacion);
            if (context.operacion.Operacion.TipoOperacion == "04") {

                return appService.mostrarAlerta("","","error")
            }
            else {
                dataProvider.postData("DocumentosRecibidos/ListarDocumentoPDF", context.operacion).success(function (respuesta) {
                    console.log(respuesta)
                    //window.open(respuesta, '_blank');
                    window.open(respuesta, "mywin", "resizable=0");
                }).error(function (error) {
                    //MostrarError();
                });
            }
        }

        function listarMensajeAlerta() {
            dataProvider.getData("Alertas/ListarMensajeAlerta").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                //context.listEmpresa = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        ////Carga
        listarMensajeAlerta();
    }
})();
