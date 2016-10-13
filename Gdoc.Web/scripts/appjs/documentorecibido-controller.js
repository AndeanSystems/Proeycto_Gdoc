(function () {
    'use strict';
    angular.module('app').controller('documentorecibido_controller', documentorecibido_controller);
    documentorecibido_controller.$inject = ['$location', 'app_factory', 'appService'];
    function documentorecibido_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;

        let UsuarioRemitente = "06";
        let UsuarioDestinatario = "03";
        var listRemitente = [];
        var listDestinatarios = [];
        context.operacion = {};
        context.visible = "List";
        context.FechaBusqueda=new Date();
        context.mostrarPDF = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            dataProvider.postData("DocumentosRecibidos/ListarDocumentoPDF", context.operacion).success(function (respuesta) {
                console.log(respuesta)
                window.open(respuesta,"mywin","resizable=1");
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.mostrarAdjuntos = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            if(context.operacion.TipoOperacion!="02"){
                listarDocumentoAdjunto(context.operacion);
                $("#modal_adjuntos").modal("show");
            }
            else {
                appService.mostrarAlerta("Información","Esta Operación no tiene Adjuntos","warning")
            }
            
        }

        context.mostrarAdjuntoWindows = function (archivo) {
            console.log(archivo);
            dataProvider.postData("DocumentosRecibidos/ListarAdjuntos", archivo).success(function (respuesta) {
                console.log(respuesta)
                window.open(respuesta, "mywin", "resizable=1");
                //window.open(respuesta, '_blank');
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.FechaAnterior = function () {
            context.FechaBusqueda.setDate(context.FechaBusqueda.getDate() - 1);
            context.gridOptions.data = [];
            listarOperacionPorFecha(context.FechaBusqueda);
        }

        context.FechaPosterior = function () {
            context.FechaBusqueda.setDate(context.FechaBusqueda.getDate() + 1);
            context.gridOptions.data = [];
            listarOperacionPorFecha(context.FechaBusqueda);
        }

        context.CambiarVentana = function (mostrarVentana) {
            context.visible = mostrarVentana;
            if (context.visible == "List") {
                listarOperacion();
                limpiarFormulario();
            } else {
                //obtenerUsuarioSession();
            }
        }
        context.ComentarioProveido = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            ObtenerUsuariosParticipantes(context.operacion);
            context.usuarioOrganizador = listRemitente;
            context.usuarioInvitados = listDestinatarios;
            context.CambiarVentana("ComentarioProveido");
        }
        //var OrganizadorMV = "";
        //context.prioridad = '<i class="fa fa-certificate" style="padding: 4px;font-size: 1.4em; color:red;"></i>';
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,
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
                    
                    name: 'Acciones',
                    cellTemplate: '<i class="fa fa-file-pdf-o" ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Ver Documento pdf"></i>' +
                            '<i class="fa fa-paperclip" ng-click="grid.appScope.mostrarAdjuntos(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;"  data-placement="bottom" data-toggle="tooltip" title="Ver Adjuntos"></i>' +
                             '<i class="fa fa-commenting-o" ng-click="grid.appScope.ComentarioProveido(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Proveido"></i>' 
                            // '<i class="fa fa-forward" ng-click="grid.appScope.ReenvioReferencia(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Reenvio con Referencia"></i>'
                            ////'<i class="fa fa-commenting-o" ng-click="grid.appScope.comentario(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;"  data-placement="bottom" data-toggle="tooltip" title="Ver Adjuntos"></i>'
                },
                { field: 'NumeroOperacion', displayName: 'Nº Documento' },
                { field: 'Remitente', displayName: 'Remitente' },
                //{ field: 'TipoOpe.DescripcionCorta', width: '10%', displayName: 'T.Oper' },
                { field: 'TipoDoc.DescripcionCorta', displayName: 'T.Doc' },
                { field: 'TituloOperacion', displayName: '	Titulo' },
                { field: 'FechaRegistro', displayName: 'Fecha Emisión', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'FechaVigente', displayName: 'Fecha Vigencia', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                //{
                //    field: 'Prioridoc',
                //    cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                //        if (grid.getCellValue(row, col) === 'verde') 
                //            return 'green';
                //        else if (grid.getCellValue(row, col) === 'amarillo') 
                //            return 'yellow';
                //        else if (grid.getCellValue(row, col) === 'rojo') 
                //            return 'red';
                //    },
                //    displayName: 'Prioridad'
                //}
            ]
        };
        //Eventos

        //Metodos
        function ObtenerUsuariosParticipantes(operacion) {
            dataProvider.postData("DocumentosRecibidos/ListarUsuarioParticipanteDE", operacion).success(function (respuesta) {
                console.log(respuesta);
                for (var ind in respuesta) {
                    if (respuesta[ind].TipoParticipante == UsuarioRemitente)
                        listRemitente.push(respuesta[ind]);
                    else
                        listDestinatarios.push(respuesta[ind]);
                }
                console.log(listRemitente);
                console.log(listDestinatarios);
            }).error(function (error) {
                //MostrarError();
            });

        }
        function listarOperacion() {
            dataProvider.getData("DocumentosRecibidos/ListarOperacion").success(function (respuesta) {
                
                for (var ind in respuesta) {
                    if (respuesta[ind].PrioridadOperacion == "02") 
                        respuesta[ind].Prioridoc = 'verde';
                    else if (respuesta[ind].PrioridadOperacion == "03")
                        respuesta[ind].Prioridoc = 'amarillo';
                    else if (respuesta[ind].PrioridadOperacion == "04")
                        respuesta[ind].Prioridoc = 'rojo';
                }
                context.gridOptions.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarDocumentoAdjunto(operacion) {
            dataProvider.postData("DocumentosRecibidos/ListarAdjunto", operacion).success(function (respuesta) {
                context.listDocumentoAdjunto = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarOperacionPorFecha(fecha) {
            dataProvider.postData("DocumentosRecibidos/ListarOperacionPorFecha", { fecha: fecha }).success(function (respuesta) {

                for (var ind in respuesta) {
                    if (respuesta[ind].PrioridadOperacion == "02")
                        respuesta[ind].Prioridoc = 'verde';
                    else if (respuesta[ind].PrioridadOperacion == "03")
                        respuesta[ind].Prioridoc = 'amarillo';
                    else if (respuesta[ind].PrioridadOperacion == "04")
                        respuesta[ind].Prioridoc = 'rojo';
                }
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
