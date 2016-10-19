(function () {
    'use strict';

    angular.module('app').controller('sede_controller', sede_controller);
    sede_controller.$inject = ['$location', 'app_factory', 'appService'];

    function sede_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.sede = {};

        //comienzo
        context.sede.EstadoSede = '0';
        //Eventos
        context.editarSede = function (rowIndex) {
            context.sede = context.gridOptions.data[rowIndex];
            context.codigodepartamento = parseInt(context.sede.CodigoUbigeo.substring(0, 2));
            context.obtenerProvincia(context.codigodepartamento);
            context.codigoprovincia = parseInt(context.sede.CodigoUbigeo.substring(2, 4));
            context.obtenerDistrito(context.codigodepartamento, context.codigoprovincia);
            context.codigodistrito = parseInt(context.sede.CodigoUbigeo.substring(4, 6));

            context.sede.EstadoSede = context.sede.EstadoSede.toString();
            $("#modal_contenido").modal("show");
        };

        context.eliminarSede = function (rowIndex) {
            var sede = context.gridOptions.data[rowIndex];
            dataProvider.postData("Sede/EliminarSede", sede).success(function (respuesta) {
                console.log(respuesta);
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
            appScopeProvider: context,
            columnDefs: [
                {
                    name: 'Acciones',
                    width: '10%',
                    cellTemplate: '<i ng-click="grid.appScope.editarSede(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Editar"></i>' 
                    //'<i ng-click="grid.appScope.eliminarSede(grid.renderContainers.body.visibleRowCache.indexOf(row))"class="fa fa-times"  style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Borrar"></i>'
                },
                { field: 'CodigoSede', width: '10%', displayName: 'Ruc Empresa' },
                { field: 'NombreSede', width: '60%', displayName: 'Razon Social' },
                { field: 'EstadoSede', width: '10%', displayName: 'Estado' },
                { field: 'FechaModifica', width: '10%', displayName: 'Fecha de Registro', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy"' }
                

            ],

        };

        context.grabar = function () {
            
            console.log(context.sede);
            var sede = context.sede;
            var departamento, provincia, distrito;

            departamento = (context.codigodepartamento < 10) ? "0" + context.codigodepartamento : context.codigodepartamento.toString();
            provincia = (context.codigoprovincia < 10) ? "0" + context.codigoprovincia : context.codigoprovincia.toString();
            distrito = (context.codigodistrito < 10) ? "0" + context.codigodistrito : context.codigodistrito.toString();

            sede.CodigoUbigeo = (departamento + provincia + distrito);

            console.log(sede);

            function enviarFomularioOK() {
                dataProvider.postData("Sede/GrabarSede", sede).success(function (respuesta) {
                    console.log(respuesta);
                    appService.mostrarAlerta("Información", "Sede Registrada Correctamente", "success");
                    listarEmpresa();
                    listarSede();
                    context.sede = {};
                    $("#modal_contenido").modal("hide");
                }).error(function (error) {
                    //MostrarError();
                });
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);

        }

        //Metodos
        context.obtenerProvincia = function (CodigoDepartamento) {
            console.log(CodigoDepartamento);
            dataProvider.postData("Ubigeo/ListarProvincias", { CodigoDepartamento: CodigoDepartamento }).success(function (respuesta) {
                console.log(respuesta);
                context.listPronvincia = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.obtenerDistrito = function (CodigoDepartamento, CodigoProvincia) {
            dataProvider.postData("Ubigeo/ListarDistritos", { CodigoDepartamento: CodigoDepartamento, CodigoProvincia: CodigoProvincia }).success(function (respuesta) {
                console.log(respuesta);
                context.listDistrito = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.CambiarVentana = function () {
            context.sede = {};
            context.codigodepartamento = {};
            context.codigoprovincia = {};
            context.codigodistrito = {};
            $("#modal_contenido").modal("show");
        }
        function listarEmpresa() {
            dataProvider.getData("Empresa/ListarEmpresa").success(function (respuesta) {
                //context.gridOptions.data = respuesta;
                context.listEmpresa = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        function listarSede() {
            dataProvider.getData("Sede/ListarSede").success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarDepartamento() {
            dataProvider.getData("Ubigeo/ListarDepartamento").success(function (respuesta) {
                context.listDepartamento = respuesta;
            }).error(function (error) {

            });
        }

        //Carga
        listarEmpresa();
        listarSede();
        listarDepartamento();
    }
})();
