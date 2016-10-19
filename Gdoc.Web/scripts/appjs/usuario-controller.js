(function () {
    'use strict';

    angular.module('app').controller('usuario_controller', usuario_controller);
    usuario_controller.$inject = ['$location', 'app_factory', 'appService', '$timeout', '$q', '$log'];

    function usuario_controller($location, dataProvider, appService, $timeout, $q, $log) {
        /* jshint validthis:true */
        ///Variables
        
        var context = this;
        //LLENAR CONCEPTOS
        LlenarConcepto("010");
        LlenarConcepto("007");
        LlenarConcepto("013");
        LlenarConcepto("021");
        LlenarConcepto("009");
        LlenarConcepto("015");
        LlenarConcepto("024");

        var usuario = {};
        context.usuario = {};
        context.usuario = {
            ClaseUsuario:'0',
            CodigoTipoUsua:'04'

        };
        context.listDepartamento = [];

        context.editar = false;

        context.usuario.ExpiraFirma=0;
        //AUTOCOMPLETE //
        context.simulateQuery = false;
        context.isDisabled = false;

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
            context.usuario.NombreUsuario = text;
            context.usuario.NombreCompleto = text.NombreCompleto;
        }

        function selectedItemChange(item) {
            if (item != undefined) {
                context.usuario.NombreUsuario = item.NombreUsuario;
                context.usuario.NombreCompleto = item.NombreCompleto;
            }
        }

        function loadAll() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.repos = respuesta;
                console.log(respuesta);
                return context.repos.map(function (repo) {
                    repo.value = repo.NombreUsuario.toLowerCase();
                    console.log(repo.value);
                    return repo.value;
                });
            }).error(function (error) {
                //MostrarError();
            });

            
        }

        function createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(state) {
                return (state.value.indexOf(lowercaseQuery) === 0);
            };

        }
        //FIN AUTOCOMPLETE

        context.editarUsuario = function (rowIndex) {
            context.usuario = context.gridOptions.data[rowIndex];
            console.log(context.usuario);
            console.log(context.usuario.Personal.Cargo.DescripcionConcepto)
            context.usuario.Personal.NombrePers = context.usuario.Personal.NombrePers + " " + context.usuario.Personal.ApellidoPersonal;
            context.editar = true;
            $("#modal_contenido").modal("show");
        };

        context.buscarUsuarioPersonal = function (user, documento, idetificacion) {
            
            var personal = context.personal;

            personal = { NumeroIdentificacion: documento };
            console.log(personal);
            dataProvider.postData("Usuario/BuscarUsuarioPersonal", { NombreUsuario: user, Personal: personal }).success(function (respuesta) {

                context.usuario.Personal = respuesta[0];
                context.usuario.NombreUsuario = context.usuario.Personal.EmailTrabrajo;
                context.usuario.Personal.NombrePers = context.usuario.Personal.NombrePers + " " + context.usuario.Personal.ApellidoPersonal;
                console.log(respuesta);
                //context.usuario = respuesta[0];
                console.log(context.usuario)
            }).error(function (error) {
                //MostrarError();
            });
        };

        context.eliminarUsuario = function (rowIndex) {
            var usuario = context.gridOptions.data[rowIndex];
            dataProvider.postData("Usuario/EliminarUsuario", usuario).success(function (respuesta) {
                console.log(usuario);
                listarUsuario();
            }).error(function (error) {
                //MostrarError();
            });
        };

        context.abrirAcceso = function (rowIndex) {
            location.href = "/Acceso/Index";
            //var usuario = context.gridOptions.data[rowIndex];
            //dataProvider.postData("Acceso/ListarAccesoSistema", usuario).success(function (respuesta) {
            //    console.log(usuario);
            //    context.accesosistema = respuesta[0];
            //    context.gridAccesos.data = respuesta;
            //}).error(function (error) {
            //    //MostrarError();
            //});
            //@Url.Action("Index", "Acceso")
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
                    name: 'Acciones', cellTemplate: '<i ng-click="grid.appScope.editarUsuario(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                                  '<i ng-click="grid.appScope.eliminarUsuario(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-times" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Desactivar"></i> ' 
                    //'<i ng-click="grid.appScope.abrirAcceso(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="glyphicon glyphicon-list-alt" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Accesos"></i> '
                },
                { field: 'NombreUsuario', displayName: 'ID Usuario' },
                { field: 'NombreCompleto', displayName: 'Apellidos y Nombres' },
                { field: 'TipoUsuario.DescripcionConcepto', displayName: 'Tipo Usuario' },
                //{ field: 'Cargo.DescripcionConcepto', displayName: 'Cargo' },
                //{ field: 'Area.DescripcionConcepto', displayName: 'Área' },
                { field: 'Personal.EmailTrabrajo', displayName: 'Email Trabajo' },
                { field: 'Estado.DescripcionConcepto', displayName: 'Estado' }
                //{ field: 'Personal.TelefonoPersonal', displayName: 'Telefono Personal' },
                //{ field: 'ClaseUsu.DescripcionConcepto', displayName: 'Clase Usuario' },
                

            ],
        };

        //Eventos
        context.grabar = function (numeroboton) {

            usuario = context.usuario;
            if (numeroboton == 1)
                usuario.EstadoUsuario = 0
            else if (numeroboton == 2)
                usuario.EstadoUsuario = 1

            function enviarFomularioOK() {
                dataProvider.postData("Usuario/GrabarUsuario", usuario).success(function (respuesta) {
                    console.log(respuesta);
                    swal("¡Bien!", "Usuario Registrado Correctamente", "success");
                    listarUsuario();
                    $("#modal_contenido").modal("hide");
                }).error(function (error) {
                    //MostrarError();
                });
            }
            function cancelarFormulario() {
                context.usuario = {};
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK, cancelarFormulario);
        }

        context.listPronvincia = function (CodigoDepartamento) {
            dataProvider.postData("Ubigeo/ListarProvincias", { CodigoDepartamento: CodigoDepartamento }).success(function (respuesta) {
                console.log(respuesta);
                context.listPronvincia = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.listDistrito = function (CodigoDepartamento, CodigoProvincia) {
            dataProvider.postData("Ubigeo/ListarDistritos", { CodigoDepartamento: CodigoDepartamento, CodigoProvincia: CodigoProvincia }).success(function (respuesta) {
                console.log(respuesta);
                context.listDistrito = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.buscarUsuario = function (usuario) {
            dataProvider.postData("Usuario/BuscarUsuarioNombre", usuario).success(function (respuesta) {
                //context.usuario = respuesta[0];
                console.log(usuario)
                console.log(respuesta);
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.nuevoUsuario = function () {
            limpiarFormulario();
            $("#modal_contenido").modal("show");
        }
        //Metodos
        
        function limpiarFormulario() {
            context.usuario = {};
            context.usuario.Personal = {};
            context.listDepartamento = [];
            context.usuario.ExpiraFirma = 0;
            context.listDepartamento = [];
            context.listPronvincia = [];
            context.listDistrito = [];
            context.usuario = {
                ClaseUsuario: '0',
                CodigoTipoUsua: '04'
            };
            context.editar = false;
        }

        function listarUsuario() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                //context.usuario = respuesta[0];
                context.listUsuario = respuesta;
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

        function LlenarConcepto(tipoConcepto) {
            var concepto = { TipoConcepto: tipoConcepto };
            appService.listarConcepto(concepto).success(function (respuesta) {
                if (concepto.TipoConcepto == "010")
                    context.listTipoUsuario = respuesta;
                else if (concepto.TipoConcepto == "007")
                    context.listTipoCargo = respuesta;
                else if (concepto.TipoConcepto == "013")
                    context.listArea = respuesta;
                else if (concepto.TipoConcepto == "021")
                    context.listClaseUsuario = respuesta;
                else if (concepto.TipoConcepto == "009")
                    context.listTipoRol = respuesta;
                else if (concepto.TipoConcepto == "015")
                    context.listTipoPersonal = respuesta;
                else if (concepto.TipoConcepto == "024")
                    context.listTipoIdentificacion = respuesta;
            });
        }
        //Carga
        listarUsuario();
        listarDepartamento();
    }
})();
