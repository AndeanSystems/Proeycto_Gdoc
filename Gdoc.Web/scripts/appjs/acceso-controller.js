(function () {
    'use strict';

    angular.module('app').controller('acceso_controller', acceso_controller);
    acceso_controller.$inject = ['$location', 'app_factory', 'appService', '$timeout', '$q', '$log'];

    function acceso_controller($location, dataProvider, appService, $timeout, $q, $log) {
        /* jshint validthis:true */
        ///Variables

        var context = this;

        context.usuario = {};
        context.accesosistema = {};
        context.listUsuario = [];
        context.listarAccesoSistema = [];
        //AUTOCOMPLETE //FALTA CORREGIR ERROR EN selectedItemChange
        context.simulateQuery = false;
        context.isDisabled = false;

        context.allStates = [];

        // list of `state` value/display objects
        context.repos = loadAll();
        context.querySearch = querySearch;
        context.selectedItemChange = selectedItemChange;
        context.searchTextChange = searchTextChange;

        function querySearch(query) {
            var results = query ? context.repos.filter(createFilterFor(query)) : context.repos,
                deferred;
            if (context.simulateQuery) {
                deferred = $q.defer();
                $timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
                return deferred.promise;
            } else {
                return results;
            }
        }

        function searchTextChange(text) {
            $log.info('Text changed to ' + text);
            context.usuario.NombreUsuario = text;
            context.usuario.NombreCompleto = text.NombreCompleto;
        }

        function selectedItemChange(item) {
            $log.info('Item changed to ' + JSON.stringify(item));
            console.log(item);
            if (item == 'undefined') {
                context.usuario.NombreUsuario = '';
            } else {
                context.usuario.NombreUsuario = item.NombreUsuario;
            }
            context.usuario.Personal.NombrePers = item.Personal.NombrePers;
        }

        /**
         * Build `states` list of key/value pairs
         */
        function loadAll() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.repos = respuesta;
                console.log(respuesta);

                return context.repos.map(function (repo) {
                    //return {
                    //    value: state.toLowerCase(),
                    //    display: state
                    //};

                    repo.value = repo.NombreUsuario.toLowerCase();

                    console.log(repo.value);
                    return repo.value;
                });
            }).error(function (error) {
                //MostrarError();
            });


        }

        /**
         * Create filter function for a query string
         */
        function createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(state) {
                return (state.value.indexOf(lowercaseQuery) === 0);
            };

        }
        //FIN AUTOCOMPLETE
        context.editarRoles = function (rowIndex) {
            context.accesosistema = context.gridAccesos.data[rowIndex];
            $("#modal_contenido").modal("show");
        };

        context.activarAcceso = function (rowIndex) {
            var acceso = context.gridAccesos.data[rowIndex];

            dataProvider.postData("Acceso/ActivarAcceso", acceso).success(function (respuesta) {
                console.log(respuesta);
                context.buscarAccesoSistema(acceso);
            }).error(function (error) {
                //MostrarError();
            });
        };
            
        context.desactivarAcceso = function (rowIndex) {
            var acceso = context.gridAccesos.data[rowIndex];

            dataProvider.postData("Acceso/DesactivarAcceso", acceso).success(function (respuesta) {
                console.log(respuesta);
                context.buscarAccesoSistema(acceso);
            }).error(function (error) {
                //MostrarError();
            });
        
        };
        context.gridAccesos = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,

            columnDefs: [
                { field: 'ModuloPaginaUrl.ModuloSistema', displayName: 'Modulo de Sistema' },
                { field: 'ModuloPaginaUrl.NombrePagina', displayName: 'Nombre de Pagina' },
                { field: 'ModuloPaginaUrl.DireccionFisicaPagina', displayName: 'Direccion de la Pagina' },
                { field: 'FechaModificacion', displayName: 'Fecha Actualizacion', type: 'date', cellFilter: 'toDateTime | date:"mediumDate"' },
                { field: 'ModuloPaginaUrl.CodigoPaginaPadre', displayName: 'Pagina Origen' },
                { field: 'EstadoAcceso', displayName: 'Estado' },
                {
                    name: 'Acciones', cellTemplate: '<i ng-click="grid.appScope.activarAcceso(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-check" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Activar"></i> ' +
                                                    '<i ng-click="grid.appScope.desactivarAcceso(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-times" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Desactivar"></i> ' +
                                                    '<i ng-click="grid.appScope.editarRoles(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="glyphicon glyphicon-list-alt" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Roles"></i> '
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
        context.buscarAccesoSistema = function (usuario) {
            if (usuario == null) {
                alert("Ingrese el Nombre Usuario");
            }
            else {
                dataProvider.postData("Acceso/ListarAccesoSistema", usuario).success(function (respuesta) {
                    console.log(respuesta);
                    context.accesosistema = respuesta[0];
                    context.gridAccesos.data = respuesta;
                }).error(function (error) {
                    //MostrarError();
                });
            }

            

        }


        //Metodos
        function listarUsuario() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.gridAccesos.data = respuesta;
                context.listUsuario = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarAccesoSistema() {
            dataProvider.getData("Acceso/ListarAccesoSistema").success(function (respuesta) {
                context.gridUsuarios.data = respuesta;
                context.listAccesoSistema = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        
        //Carga
        //listarAccesoSistema();
    }
})();