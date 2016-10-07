(function () {
    'use strict';
    angular.module('app').controller('documentorecibido_controller', documentorecibido_controller);
    documentorecibido_controller.$inject = ['$location', 'app_factory', 'appService'];
    function documentorecibido_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.operacion = {};

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

        context.FechaAnterior = function (archivo) {
            context.FechaBusqueda.setDate(context.FechaBusqueda.getDate() - 1);
            listarOperacionPorFecha(context.FechaBusqueda);
        }

        context.FechaPosterior = function (archivo) {
            context.FechaBusqueda.setDate(context.FechaBusqueda.getDate() + 1);
            listarOperacionPorFecha(context.FechaBusqueda);
        }
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NumeroOperacion', width: '20%', displayName: 'Nº Documento' },
                { field: 'Remitente', width: '10%', displayName: 'Remitente' },
                { field: 'TipoOpe.DescripcionCorta', width: '10%', displayName: 'T.Oper' },
                { field: 'TipoDoc.DescripcionCorta', width: '10%', displayName: 'T.Doc' },
                { field: 'TituloOperacion', width: '23%', displayName: '	Titulo' },
                { field: 'FechaRegistro', width: '15%', displayName: 'Fecha Emisión', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'FechaVigente', width: '15%', displayName: '	Fecha Vigencia', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                {
                    name: 'Acciones', width: '7%',
                    cellTemplate: '<i class="fa fa-file-pdf-o" ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Ver Documento pdf"></i>' +
                            '<i class="fa fa-paperclip" ng-click="grid.appScope.mostrarAdjuntos(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;"  data-placement="bottom" data-toggle="tooltip" title="Ver Adjuntos"></i>'
                }
            ]
        };
        //Eventos

        //Metodos

        function listarOperacion() {
            dataProvider.getData("DocumentosRecibidos/ListarOperacion").success(function (respuesta) {
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
            console.log(fecha);
            var dia = fecha.getDate();
            var mes = fecha.getMonth();
            var año = fecha.getFullYear();

            var fechaformato = dia + "/" + mes + "/" + año;
            console.log(fechaformato);
            dataProvider.postData("DocumentosRecibidos/ListarOperacionPorFecha", fecha).success(function (respuesta) {
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
