//Leer Archivos de de fisico a binario
var archivosSelecionados = [];
let TipoMensaje = "warning";
function ReadFileToBinary(control) {
    for (var i = 0, f; f = control.files[i]; i++) {
        let files = f;
        var reader = new FileReader();
        reader.onloadend = function (e) {
            console.log(files);
            archivosSelecionados.push({
                NombreArchivo: files.name,
                TamanoArchivo: files.size,
                TipoArchivo: files.type,
                RutaBinaria: e.target.result
            });
        }
        reader.readAsBinaryString(f);
    }
}
(function () {
    'use strict';

    angular.module('app').controller('mesavirtual_controller', mesavirtual_controller);

    
    mesavirtual_controller.$inject = ['$location', 'app_factory', 'appService'];

    function mesavirtual_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        let TipoDocumento = "012";
        let PrioridadAtencion = "005";
        let TipoAcceso = "002";
        let TipoComunicacion = "022";
        let TipoMesaVirtual = "011";

        let UsuarioOrganizado = "05";
        let UsuarioInvitado = "02";
        var context = this;
        context.operacion = {};
        context.operacion.FechaVigente = new Date();
        context.operacion.FechaCierre = new Date();
        context.visible = "CreateAndEdit";
        context.listaUsuarioGrupo = [];

        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioInvitados = [];
        context.usuarioOrganizador = [];
        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario = {};


        LlenarConcepto(TipoDocumento);
        LlenarConcepto(TipoAcceso);
        LlenarConcepto(TipoMesaVirtual);
        LlenarConcepto(PrioridadAtencion);


        //COMIENZO
        context.operacion = {
            TipoDocumento: '02',
            PrioridadOperacion: '02',
            AccesoOperacion: '2',
            NotificacionOperacion: '0'
        };

        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NumeroOperacion', displayName: 'Nº Documento' },
                { field: 'TipoDoc.DescripcionConcepto', displayName: 'Tipo de Documento' },
                { field: 'DescripcionOperacion', displayName: '	Asunto' },
                { field: 'FechaRegistro', displayName: 'Fecha Emisión', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy"' },
                { field: 'FechaVigente', displayName: '	Fecha Recepción', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy"' },
                { field: 'Estado.DescripcionConcepto', displayName: 'Estado' },
                {
                    name: 'Acciones',
                    cellTemplate: '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                '<i ng-click="grid.appScope.eliminarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-times" data-placement="top" data-toggle="tooltip" title="" data-original-title="Cerrar"></i>'
                }
            ]
        };
        //Eventos
        context.grabar = function (numeroboton) {
            if (archivosSelecionados == undefined || archivosSelecionados == "" || archivosSelecionados == null) {
                return appService.mostrarAlerta("Advertencia", "Debe seleccionar por lo menos un archivo", "warning");
            }
            if (context.usuarioOrganizador == undefined || context.usuarioOrganizador == "") {
                return appService.mostrarAlerta("Falta el Organizador", "Agregue al Organizador", "warning");
            }
            if (context.usuarioInvitados == undefined || context.usuarioInvitados == "") {
                return appService.mostrarAlerta("Falta los Invitados", "Agregue a los invitados", "warning");
            }
            console.log(context.operacion);
            let Operacion = context.operacion;
            let listEUsuarioGrupo = [];

            for (var ind in context.usuarioOrganizador) {
                context.usuarioOrganizador[ind].TipoParticipante = UsuarioOrganizado;
                listEUsuarioGrupo.push(context.usuarioOrganizador[ind]);
            }
            for (var ind in context.usuarioInvitados) {
                context.usuarioInvitados[ind].TipoParticipante = UsuarioInvitado;
                listEUsuarioGrupo.push(context.usuarioInvitados[ind]);
            }
            if (numeroboton == 1)
                Operacion.EstadoOperacion = 0
            else if (numeroboton == 2)
                Operacion.EstadoOperacion = 1

            function enviarFomularioOK() {
                console.log(context.DocumentoElectronicoOperacion);
                dataProvider.postData("MesaVirtual/Grabar", { Operacion: Operacion, listEUsuarioGrupo: listEUsuarioGrupo }).success(function (respuesta) {
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    appService.mostrarAlerta("Información", "Se grabo correctamente", TipoMensaje);
                    console.log(respuesta);
                }).error(function (error) {
                    //MostrarError();
                });
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);
            
        }

        context.CambiarVentana = function (mostrarVentana) {
            context.visible = mostrarVentana;
            if (context.visible != "List") {
            }
        }
        ////
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

        function LlenarConcepto(tipoConcepto) {
            var concepto = { TipoConcepto: tipoConcepto };
            appService.listarConcepto(concepto).success(function (respuesta) {
                if (concepto.TipoConcepto == TipoDocumento)
                    context.listTipoDocumento = respuesta;
                else if (concepto.TipoConcepto == PrioridadAtencion)
                    context.listPrioridadAtencion = respuesta;
                else if (concepto.TipoConcepto == TipoAcceso)
                    context.listTipoAcceso = respuesta;
                else if (concepto.TipoConcepto == TipoComunicacion)
                    context.listTipoComunicacion = respuesta;
                else if (concepto.TipoConcepto == TipoMesaVirtual)
                    context.listTipoMesaVirtual = respuesta;
            });
        }

        function listarOperacion() {
            dataProvider.getData("MesaVirtual/ListarOperacion").success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        listarOperacion();
    }
})();
