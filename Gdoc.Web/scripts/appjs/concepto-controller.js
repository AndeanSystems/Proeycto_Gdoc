let TipoMensaje = "warning";
(function () {
    'use strict';

    angular.module('app').controller('concepto_controller', concepto_controller);
    concepto_controller.$inject = ['$location', 'app_factory', 'appService'];

    function concepto_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.concepto = {};
        LlenarConcepto("999");

        //var concepto = { TipoConcepto: "012" };
        context.acciones = '<i ng-click="grid.appScope.editarConcepto(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Editar"></i>';

        //appService.listarConcepto(concepto).success(function (respuesta) {
        //    console.log(respuesta);
        //});

        context.registrarConcepto = function () {
            context.concepto = {};
            $("#modal_contenido").modal("show");
            context.concepto.EstadoConcepto = '0';
        };

        context.editarConcepto = function (rowIndex) {
            context.concepto = context.gridOptions.data[rowIndex];
            console.log(context.concepto);
            context.concepto.EstadoConcepto = context.concepto.EstadoConcepto.toString();
            if (context.concepto.EditarRegistro == 0){
                return appService.mostrarAlerta("Informacion", "El concepto no se puede editar", "info");
            }
            else
                $("#modal_contenido").modal("show");
        };

        context.buscarConcepto = function (CodiConcepto, EstadoConcepto) {
            if (CodiConcepto == null) {
                return appService.mostrarAlerta("Advertencia", "Seleccione el tipo de concepto para la consultar", "info");
            }
            else {
                if (EstadoConcepto == null) {
                    dataProvider.postData("Concepto/BuscarConceptoEstado", { CodiConcepto: CodiConcepto }).success(function (respuesta) {
                        console.log(respuesta);
                        context.concepto = respuesta[0];
                        context.gridOptions.data = respuesta;
                    }).error(function (error) {
                        //MostrarError();
                    });
                }
                else {
                    dataProvider.postData("Concepto/BuscarConceptoEstado", { CodiConcepto: CodiConcepto, EstadoConcepto: EstadoConcepto }).success(function (respuesta) {
                        console.log(respuesta);
                        context.concepto = respuesta[0];
                        context.gridOptions.data = respuesta;
                    }).error(function (error) {
                        //MostrarError();
                    });
                }

            }

        }

        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            //multiSelect: false,
            //enableSelectAll: false,
            //modifierKeysToMultiSelect: false,
            //noUnselect: true,
            //enableRowHeaderSelection: false, 
            //enableRowSelection: true, 
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'CodiConcepto', displayName: 'Codigo' },
                { field: 'DescripcionConcepto', width: '20%', displayName: 'Descripcion' },
                { field: 'DescripcionCorta', displayName: 'Abreviatura' },
                { field: 'ValorUno', width: '8%', displayName: 'Valor1' },
                { field: 'ValorDos', width: '8%', displayName: 'Valor2' },
                { field: 'TextoUno', width: '8%', displayName: 'Texto1' },
                { field: 'TextoDos', width: '8%', displayName: 'Texto2' },
                { field: 'EstadoConcepto', displayName: 'Estado' },
                {
                    name: 'Acciones',
                    cellTemplate: context.acciones
                }
            ]
        };
        //Eventos
        context.grabar = function () {
            var concepto = context.concepto;

            //if (numeroboton == 1)
            //    concepto.EstadoConcepto = 0
            //else if (numeroboton == 2)
            //    concepto.EstadoConcepto = 1

            console.log(context.concepto);

           
            function enviarFomularioOK() {
                dataProvider.postData("Concepto/GrabarConcepto", context.concepto).success(function (respuesta) {
                    console.log(respuesta);
                    //listarConcepto();
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);
                    context.concepto = {};
                    $("#modal_contenido").modal("hide");
                }).error(function (error) {
                    //MostrarError();
                });
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);
        }

        //Metodos
        function listarConcepto() {
            //context.concepto.TipoConcepto == "999";
            var concepto = { TipoConcepto: "999" };
            dataProvider.postData("Concepto/ListarConceptoEditables", concepto).success(function (respuesta) {
                context.listTipoConceptoEditable = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        function LlenarConcepto(tipoConcepto) {
            var concepto = { TipoConcepto: tipoConcepto };
            appService.listarConcepto(concepto).success(function (respuesta) {
                if (concepto.TipoConcepto == "999")
                    context.listTipoConceptoTitulo = respuesta;
            });
        }
        //Carga
        listarConcepto();
    }
})();
