let TipoMensaje = "warning";
(function () {
    'use strict';

    angular.module('app').controller('empresa_controller', empresa_controller);
    empresa_controller.$inject = ['$location', 'app_factory', 'appService'];

    function empresa_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.empresa = {};
        context.codigodepartamento = {};
        context.codigoprovincia = {};
        context.codigodistrito = {};
        //comienzo
        context.empresa.EstadoEmpresa = '0';
        //Eventos
        context.editarEmpresa = function (rowIndex) {
            context.empresa = context.gridOptions.data[rowIndex];
            context.codigodepartamento = parseInt(context.empresa.CodigoUbigeo.substring(0, 2));
            context.obtenerProvincia(context.codigodepartamento);
            context.codigoprovincia = parseInt(context.empresa.CodigoUbigeo.substring(2, 4));
            context.obtenerDistrito(context.codigodepartamento, context.codigoprovincia);
            context.codigodistrito = parseInt(context.empresa.CodigoUbigeo.substring(4, 6));

            context.empresa.EstadoEmpresa = context.empresa.EstadoEmpresa.toString();
            $("#modal_contenido").modal("show");
        };

        context.eliminarEmpresa = function (rowIndex) {
            var empresa = context.gridOptions.data[rowIndex];
            dataProvider.postData("Empresa/EliminarEmpresa", empresa).success(function (respuesta) {
                listarEmpresa();
            }).error(function (error) {
                //MostrarError();
            });
        };

        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableSorting: true,
            //enableFiltering: true,
            data: [],
            appScopeProvider : context,
            columnDefs: [
                {
                    name: 'Acciones',
                    width: '10%',
                    cellTemplate: '<i ng-click="grid.appScope.editarEmpresa(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Editar"></i>' 
                    // '<i ng-click="grid.appScope.eliminarEmpresa(grid.renderContainers.body.visibleRowCache.indexOf(row))"class="fa fa-times"  style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Borrar"></i>'
                },
                { field: 'RucEmpresa', width: '10%', displayName: 'Ruc Empresa' },
                { field: 'RazonSocial', width: '60%', displayName: 'Razon Social' },
                { field: 'Estado.DescripcionConcepto', width: '10%', displayName: 'Estado' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha de Registro', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm"' }
                

            ],

        };

        context.grabar = function () {
            var empresa = context.empresa;
            var departamento, provincia, distrito;

            departamento = (context.codigodepartamento < 10) ? "0" + context.codigodepartamento : context.codigodepartamento.toString();
            provincia = (context.codigoprovincia < 10) ? "0" + context.codigoprovincia : context.codigoprovincia.toString();
            distrito = (context.codigodistrito < 10) ? "0" + context.codigodistrito : context.codigodistrito.toString();

            //if (numeroboton == 1)
            //    empresa.EstadoEmpresa = 0
            //else if(numeroboton == 2)
            //empresa.EstadoEmpresa = 1

            empresa.CodigoUbigeo = (departamento + provincia + distrito);

            function enviarFomularioOK() {
                dataProvider.postData("Empresa/GrabarEmpresa", empresa).success(function (respuesta) {
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);
                    limpiarFormulario();
                    listarEmpresa();
                    $("#modal_contenido").modal("hide");
                }).error(function (error) {
                    //MostrarError();
                });
            }
            function cancelarFormulario() {
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK, cancelarFormulario);
 
            
        }

        //Metodos
        context.obtenerProvincia = function (CodigoDepartamento) {
            dataProvider.postData("Ubigeo/ListarProvincias", {CodigoDepartamento:CodigoDepartamento}).success(function (respuesta) {
                context.listPronvincia = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.obtenerDistrito = function (CodigoDepartamento, CodigoProvincia) {
            dataProvider.postData("Ubigeo/ListarDistritos", { CodigoDepartamento: CodigoDepartamento , CodigoProvincia: CodigoProvincia}).success(function (respuesta) {
                context.listDistrito = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        
        context.CambiarVentana = function () {
            limpiarFormulario();
            $("#modal_contenido").modal("show");
        }
        function listarEmpresa() {
            dataProvider.getData("Empresa/ListarEmpresa").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                context.listEmpresa = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarDepartamento(){
            dataProvider.getData("Ubigeo/ListarDepartamento").success(function (respuesta) {
                context.listDepartamento = respuesta;
            }).error(function(error){

            });
        }
        function limpiarFormulario() {
            context.empresa = {};
            context.codigodepartamento = {};
            context.codigoprovincia = {};
            context.codigodistrito = {};
            context.empresa.EstadoEmpresa = '0';
        }

        //Carga
        listarEmpresa();
        listarDepartamento();
    }
})();
