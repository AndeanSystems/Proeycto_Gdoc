let TipoMensaje = "warning";    
(function () {
    'use strict';

    angular.module('app').controller('grupo_controller', grupo_controller);
    grupo_controller.$inject = ['$location', 'app_factory', 'appService'];

    function grupo_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.listGrupo = [];
        context.grupo = {};
        context.listaUsuariosGrupo = [];
        context.listaUsuarioGrupo = [];
        context.participantesGrupo = [];
        context.visible = "List";

        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioDestinatarios = [];
        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario = {};
        var listUsuariosGrupo = [];
        var listEUsuarioGrupo = [];


        context.editarGrupo = function (rowIndex) {
            context.grupo = context.gridOptions.data[rowIndex];
            ObtenerUsuariosGrupo(context.grupo)
            context.participantesGrupo = listUsuariosGrupo;
            context.CambiarVentana('CreateAndEdit');
        };

        context.eliminarGrupo = function (rowIndex) {
            var grupo = context.gridOptions.data[rowIndex];
            dataProvider.postData("Grupo/EliminarGrupo", grupo).success(function (respuesta) {
                console.log(grupo);
                listarGrupo();
            }).error(function (error) {
                //MostrarError();
            });
        };

        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NombreGrupo', displayName: 'Grupo' },
                { field: 'CantidadUsuarios', displayName: 'Participantes' },
                { field: 'ComentarioGrupo', displayName: 'Comentario' },
                { field: 'Estado.DescripcionConcepto', displayName: 'Estado' },

                {
                    name: 'Acciones',
                    cellTemplate: '<i ng-click="grid.appScope.editarGrupo(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-pencil-square-o  " style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                  '<i ng-click="grid.appScope.eliminarGrupo(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-times  " style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Borrar"></i>'
                }

            ],
            multiSelect: false,
            modifierKeysToMultiSelect: false,
        };

        context.CambiarVentana = function (mostrarVentana) {
            context.visible = mostrarVentana;
            if (context.visible == "List") {
                listarGrupo();
                limpiarFormulario();
            }
        }

        //Eventos

        context.buscarGrupo = function (NombreGrupo) {
            if (NombreGrupo == "") {
                listarGrupo();
            }
            else {
                dataProvider.postData("Grupo/BuscarGrupo", { NombreGrupo: NombreGrupo }).success(function (respuesta) {
                    console.log(respuesta);
                    context.gridOptions.data = respuesta;
                }).error(function (error) {
                    //MostrarError();
                });
            }

            
        }

        context.agregar = function (IDUsuario) {
            if (context.grupo.CodigoGrupo == undefined)
                alert("vacio");
            else {
                context.listaUsuariosGrupo.push(context.grupo);
                context.grupo = {};
            }
            //dataProvider.postData("Usuario/ListarUsuarioGrupo", { IDUsuario: IDUsuario }).success(function (respuesta) {
            //    console.log(respuesta);
            //    context.listaUsuariosGrupo.push(respuesta);
            //}).error(function (error) {
            //    //MostrarError();
            //});
        }
        context.grabar = function (numeroboton) {

            var grupo = context.grupo;

            
            for (var ind in context.participantesGrupo) {
                listEUsuarioGrupo.push(context.participantesGrupo[ind]);
            }

            if (numeroboton == 1)
                grupo.EstadoGrupo = 0
            else if (numeroboton == 2)
                grupo.EstadoGrupo = 1

            function enviarFomularioOK() {
                console.log(context.grupo);
                dataProvider.postData("Grupo/GrabarGrupoUsuarios", { grupo: grupo, listUsuarioGrupo: listEUsuarioGrupo }).success(function (respuesta) {
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);
                    listarGrupo();
                }).error(function (error) {
                    //MostrarError();
                });
                context.CambiarVentana("List");
                limpiarFormulario();
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);
        }

        //Metodos
        function listarUsuarioGrupoAutoComplete(Nombre, tipo) {
            var UsuarioGrupo = { Nombre: Nombre, Tipo: tipo };
            appService.buscarUsuarioGrupoAutoComplete(UsuarioGrupo).success(function (respuesta) {
                //context.listaUsuario = respuesta;
                context.listaUsuarioGrupo = respuesta;
            });
        }

        function querySearch(criteria, tipo) {
            listarUsuarioGrupoAutoComplete(criteria, tipo);
            cachedQuery = cachedQuery || criteria;
            return cachedQuery ? context.listaUsuarioGrupo : [];
        }

        function listarGrupo() {
            dataProvider.getData("Grupo/ListarGrupo").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                context.listGrupo = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        function ObtenerUsuariosGrupo(grupo) {
            dataProvider.postData("Grupo/ListarUsuarioGrupo", grupo).success(function (respuesta) {
                for (var ind in respuesta) {
                    listUsuariosGrupo.push(respuesta[ind]);
                }
            }).error(function (error) {
                //MostrarError();
            });

        }

        function limpiarFormulario() {
            context.grupo = {};
            context.participantesGrupo = [];
            listEUsuarioGrupo = [];
            listUsuariosGrupo = [];
        }
        
        //Carga
        listarGrupo();
    }
})();
