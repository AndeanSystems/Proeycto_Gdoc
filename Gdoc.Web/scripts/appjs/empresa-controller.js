(function () {
    'use strict';

    angular.module('app').controller('empresa_controller', empresa_controller);
    empresa_controller.$inject = ['$location', 'app_factory'];

    function empresa_controller($location, dataProvider) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.listEmpresa = [];
        context.listDepartamento = [];
        context.listProvincia = [];
        context.empresa = {};
        context.ubigeo = {};
        


        context.editarEmpresa = function () {
            $("#modal_contenido").modal("show");
            alert("Falta terminar: llenar modal con registro empresa para actualizar")
        };

        context.eliminarEmpresa = function (idempresa) {
            alert("Falta terminar: cambiar estado de empresa a 2:inactivo")
            //dataProvider.postData("Empresa/EliminarEmpresa", { IDEmpresa: idempresa }).success(function (respuesta) {
            //    console.log(respuesta);
            //    context.listEmpresa = respuesta;
            //}).error(function (error) {
            //    //MostrarError();
            //});
        };

        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider : context,

            columnDefs: [
                { field: 'RucEmpresa', displayName: 'Ruc Empresa' },
                { field: 'RazonSocial', displayName: 'Razon Social' },
                { field: 'EstadoEmpresa', displayName: 'Estado' },
                {
                    name: 'Acciones',
                    cellTemplate: '<i ng-click="grid.appScope.editarEmpresa()" class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                  '<i ng-click="grid.appScope.eliminarEmpresa()"class="fa fa-times"  style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Borrar"></i>'
                }

            ],
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            //onRegisterApi : function( gridApi ) {
            //    context.gridApi = gridApi;
            //    gridApi.selection.on.rowSelectionChanged(context, function (row) {
            //        var msg = 'row selected ' + row.isSelected;
            //        console.log(msg);
            //    });
            //}
        };

        //Eventos
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
        context.listPronvincia = function (CodigoDepartamento) {
            dataProvider.postData("Ubigeo/ListarProvincias", {CodigoDepartamento:CodigoDepartamento}).success(function (respuesta) {
                console.log(respuesta);
                context.listPronvincia = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        context.listDistrito = function (CodigoDepartamento, CodigoProvincia) {
            dataProvider.postData("Ubigeo/ListarDistritos", { CodigoDepartamento: CodigoDepartamento , CodigoProvincia: CodigoProvincia}).success(function (respuesta) {
                console.log(respuesta);
                context.listDistrito = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        
        //Metodos
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

        function listarProvincia() {
            dataProvider.postData("Ubigeo/ListarProvincias", context.ubigeo).success(function (respuesta) {
                context.listProvincia = respuesta;
            }).error(function (error) {

            });
        }

        //Carga
        listarEmpresa();
        listarDepartamento()
        listarProvincia()
    }
})();
