(function () {
    'use strict';

    angular.module('app').controller('empresa_controller', empresa_controller);
    empresa_controller.$inject = ['$location', 'app_factory'];

    function empresa_controller($location, dataProvider) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.empresa = {};
        
        //Eventos
        context.editarEmpresa = function (rowIndex) {
            context.empresa = context.gridOptions.data[rowIndex];
            context.codigodepartamento = parseInt(context.empresa.CodigoUbigeo.substring(0, 2));
            context.obtenerProvincia(context.codigodepartamento);
            context.codigoprovincia = parseInt(context.empresa.CodigoUbigeo.substring(2, 4));
            context.obtenerDistrito(context.codigodepartamento, context.codigoprovincia);
            context.codigodistrito = parseInt(context.empresa.CodigoUbigeo.substring(4, 6));
            $("#modal_contenido").modal("show");
        };

        context.conformifad = function () {
            $("#modal_conformidad").modal("show");
        };

        context.clave = function () {
            $("#modal_clave").modal("show");
        };

        context.eliminarEmpresa = function (rowIndex) {
            var empresa = context.gridOptions.data[rowIndex];
            dataProvider.postData("Empresa/EliminarEmpresa", empresa).success(function (respuesta) {
                console.log(respuesta);
                listarEmpresa();
            }).error(function (error) {
                //MostrarError();
            });
        };

        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider : context,
            columnDefs: [
                { field: 'RucEmpresa', width: '10%', displayName: 'Ruc Empresa' },
                { field: 'RazonSocial', width: '60%', displayName: 'Razon Social' },
                { field: 'Estado.DescripcionConcepto', width: '10%', displayName: 'Estado' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha de Registro', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy"' },
                {
                    name: 'Acciones',
                    width: '10%',
                    cellTemplate: '<i ng-click="grid.appScope.editarEmpresa(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                  '<i ng-click="grid.appScope.eliminarEmpresa(grid.renderContainers.body.visibleRowCache.indexOf(row))"class="fa fa-times"  style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Borrar"></i>'
                }

            ],

        };

        context.grabar = function (numeroboton) {
            console.log(context.empresa);
            var empresa = context.empresa;
            var departamento, provincia, distrito;

            departamento = (context.codigodepartamento < 10) ? "0" + context.codigodepartamento : context.codigodepartamento.toString();
            provincia = (context.codigoprovincia < 10) ? "0" + context.codigoprovincia : context.codigoprovincia.toString();
            distrito = (context.codigodistrito < 10) ? "0" + context.codigodistrito : context.codigodistrito.toString();

            if (numeroboton == 1)
                empresa.EstadoEmpresa = 0
            else if(numeroboton == 2)
                empresa.EstadoEmpresa = 1
            empresa.CodigoUbigeo = (departamento + provincia + distrito);
            console.log(empresa);
            dataProvider.postData("Empresa/GrabarEmpresa", empresa).success(function (respuesta) {
                console.log(respuesta);
                listarEmpresa();
                context.empresa = {};
                $("#modal_contenido").modal("hide");
            }).error(function (error) {
                //MostrarError();
            });
        }

        //Metodos
        context.obtenerProvincia = function (CodigoDepartamento) {
            console.log(CodigoDepartamento);
            dataProvider.postData("Ubigeo/ListarProvincias", {CodigoDepartamento:CodigoDepartamento}).success(function (respuesta) {
                console.log(respuesta);
                context.listPronvincia = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.obtenerDistrito = function (CodigoDepartamento, CodigoProvincia) {
            dataProvider.postData("Ubigeo/ListarDistritos", { CodigoDepartamento: CodigoDepartamento , CodigoProvincia: CodigoProvincia}).success(function (respuesta) {
                console.log(respuesta);
                context.listDistrito = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
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

        //Carga
        listarEmpresa();
        listarDepartamento();
    }
})();
