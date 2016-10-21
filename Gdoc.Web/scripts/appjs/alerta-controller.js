(function () {
    'use strict';
    angular.module('app').controller('alerta_controller', alerta_controller);
    alerta_controller.$inject = ['$location', 'app_factory', 'appService'];
    function alerta_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.alerta = {};
        context.operacion = {};
        context.visible = "List";
        context.FechaBusqueda = new Date();

        context.CambiarVentana = function (mostrarVentana) {
            context.visible = mostrarVentana;
            if (context.visible == "List") {
                listarMensajeAlerta();
                limpiarFormulario();
            } else {

                //listarComentarioProveido();
            }
        }
        
        context.comentarioProveido = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            console.log(context.operacion);
            context.operacion.FechaAlerta = appService.setFormatDate(context.operacion.FechaAlerta);
            if (context.operacion.TipoAlerta == 1)
                appService.mostrarAlerta("Información", "La alerta no posee Comentario", "warning");
            else
            {
                listarComentarioProveido(context.operacion);
                context.CambiarVentana("ListComentarioProveido");
            }
        }
        //Eventos
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableSorting: true,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                {
                    field: 'Prioridoc',
                    width: '3%',
                    cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                        if (grid.getCellValue(row, col) === 'verde')
                            return 'green';
                        else if (grid.getCellValue(row, col) === 'amarillo')
                            return 'yellow';
                        else if (grid.getCellValue(row, col) === 'rojo')
                            return 'red';
                    },
                    name: ' ',
                },
                {
                    name: 'Acción', width: '5%',
                    cellTemplate: '<i class="fa fa-file-pdf-o" ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Ver Operacion"></i>' +
                        '<i class="fa fa-commenting" ng-click="grid.appScope.comentarioProveido(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Proveido"></i>'
                },
                { field: 'Operacion.NumeroOperacion',width:'14%', displayName: 'Nº Operación' },
                { field: 'FechaAlerta', width: '12%', displayName: 'Fecha Alerta', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'Remitente', width: '13%', displayName: 'Remitente' },
                { field: 'TipoDocumento.DescripcionCorta', width: '6%', displayName: 'T.Doc.' },
                { field: 'Evento.DescripcionConcepto', width: '50%', displayName: 'Detalle' }
                

            ],

        };

        context.gridComentarios = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableSorting: true,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'FechaPublicacion', displayName: 'Fecha', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm"' },
                { field: 'Usuario.NombreUsuario', displayName: 'Remitente' },
                { field: 'ComentarioMesaVirtual', displayName: 'Comentario' },
                { field: 'Destinatarios', displayName: 'Destinatarios' }
                //{
                //    name: 'Adjuntos', width: '7%',
                //    cellTemplate: '<i ng-click="grid.appScope.mostrarAdjuntos(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-paperclip" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Ver"></i>'
                //}
            ]
        };
        //Metodos

        context.mostrarPDF = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            console.log(context.operacion);
            if (context.operacion.Operacion.TipoOperacion == "04") {

                return appService.mostrarAlerta("Información","La operación no Posee Documento PDF","warning")
            }
            else {
                dataProvider.postData("DocumentosRecibidos/ListarDocumentoPDF", context.operacion).success(function (respuesta) {
                    console.log(respuesta)
                    window.open(respuesta, "mywin", "resizable=0");
                }).error(function (error) {
                    //MostrarError();
                });
            }
        }

        context.FechaAnterior = function () {
            context.FechaBusqueda.setDate(context.FechaBusqueda.getDate() - 1);
            context.gridOptions.data = [];
            listarAlertaPorFecha(context.FechaBusqueda);
        }

        context.FechaPosterior = function () {
            context.FechaBusqueda.setDate(context.FechaBusqueda.getDate() + 1);
            context.gridOptions.data = [];
            listarAlertaPorFecha(context.FechaBusqueda);
        }
        function listarMensajeAlerta() {
            dataProvider.getData("Alertas/ListarMensajeAlerta").success(function (respuesta) {
                for (var ind in respuesta) {
                    if (respuesta[ind].Operacion.PrioridadOperacion == "02")
                        respuesta[ind].Prioridoc = 'verde';
                    else if (respuesta[ind].Operacion.PrioridadOperacion == "03")
                        respuesta[ind].Prioridoc = 'amarillo';
                    else if (respuesta[ind].Operacion.PrioridadOperacion == "04")
                        respuesta[ind].Prioridoc = 'rojo';
                }
                context.gridOptions.data = respuesta;
                console.log(respuesta);
                //context.listEmpresa = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarComentarioProveido(operacion) {
            dataProvider.postData("Alertas/ListarComentarioProveido", operacion).success(function (respuesta) {
                context.gridComentarios.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        function limpiarFormulario(){
            context.alerta = {};
            context.operacion = {};
        }
        function listarAlertaPorFecha(fecha) {
            dataProvider.postData("Alertas/ListarAlertasPorFecha", { fecha: fecha }).success(function (respuesta) {

                for (var ind in respuesta) {
                    if (respuesta[ind].Operacion.PrioridadOperacion == "02")
                        respuesta[ind].Prioridoc = 'verde';
                    else if (respuesta[ind].Operacion.PrioridadOperacion == "03")
                        respuesta[ind].Prioridoc = 'amarillo';
                    else if (respuesta[ind].Operacion.PrioridadOperacion == "04")
                        respuesta[ind].Prioridoc = 'rojo';
                }
                context.gridOptions.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        ////Carga
        //listarComentarioProveido();
        listarMensajeAlerta();
    }
})();
