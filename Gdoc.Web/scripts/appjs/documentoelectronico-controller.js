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

    angular.module('app').controller('documentoelectronico_controller', documentoelectronico_controller);
    documentoelectronico_controller.$inject = ['$location', 'app_factory', 'appService'];

    function documentoelectronico_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        let TipoDocumento = "012";
        let PrioridadAtencion = "005";
        let TipoAcceso = "002";
        let TipoComunicacion = "022";
        let UsuarioRemitente = "06";
        let UsuarioDestinatario = "03";
        var context = this;
        context.operacion = {};
        context.DocumentoElectronicoOperacion = {};
        context.visible = "List";
        context.listaUsuarioGrupo = [];

        //Variable de autocomplete
        let Operacion = context.operacion;
        let DocumentoElectronicoOperacion = context.DocumentoElectronicoOperacion;
        let listEUsuarioGrupo = [];
        let listERemitente = [];
        let listEDestinatario = [];
        let listDocumentosAdjuntos = [];
        //
        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioRemitentes = [];
        context.usuarioDestinatarios = [];
        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario = {};



        LlenarConcepto(TipoDocumento);
        LlenarConcepto(PrioridadAtencion);
        LlenarConcepto(TipoAcceso);
        LlenarConcepto(TipoComunicacion);
        
        context.operacion = {
            TipoDocumento: '02',
            TipoComunicacion: '1',
            PrioridadOperacion: '02',
            AccesoOperacion: '2'
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
                                '<i ng-click="grid.appScope.eliminarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-times" data-placement="top" data-toggle="tooltip" title="" data-original-title="Borrar"></i>'
                }
            ]
        };
        //Eventos
        context.grabar = function (numeroboton) {
            let usuarioRemitenteLogueado = appService.obtenerUsuarioId();
            if (archivosSelecionados == undefined || archivosSelecionados == "" || archivosSelecionados == null) {
                return appService.mostrarAlerta("Advertencia", "Debe seleccionar por lo menos un archivo", "warning");
            }
            if (context.usuarioRemitentes == undefined || context.usuarioRemitentes == "") {
                return appService.mostrarAlerta("Falta los Remitentes", "Agregue a los Remitentes", "warning");
            }
            if (context.usuarioDestinatarios == undefined || context.usuarioDestinatarios == "") {
                return appService.mostrarAlerta("Falta los Destinatarios", "Agregue a los destinatarios", "warning");
            }
            let usuarioRemitenteEnSession = false;
            for (var ind in context.usuarioRemitentes) {
                if (context.usuarioRemitentes[ind].IDUsuarioGrupo == usuarioRemitenteLogueado)
                    usuarioRemitenteEnSession = true;
                context.usuarioRemitentes[ind].TipoParticipante = UsuarioRemitente;
                listEUsuarioGrupo.push(context.usuarioRemitentes[ind]);
            }
            for (var ind in context.usuarioDestinatarios) {
                context.usuarioDestinatarios[ind].TipoParticipante = UsuarioDestinatario;
                listEUsuarioGrupo.push(context.usuarioDestinatarios[ind]);
            }
            if (numeroboton == 1)
                Operacion.EstadoOperacion = 0
            else if (numeroboton == 2) {
                if (!usuarioRemitenteEnSession) {
                    return appService.mostrarAlerta("Advertencia", "El usuario no es remitente", "warning");
                }
                Operacion.EstadoOperacion = 1
            }
            for (var index in archivosSelecionados) {
                listDocumentosAdjuntos.push({
                    RutaArchivo: archivosSelecionados[index].RutaBinaria,
                    NombreOriginal: archivosSelecionados[index].NombreArchivo,
                    TamanoArchivo: archivosSelecionados[index].TamanoArchivo,
                    TipoArchivo: archivosSelecionados[index].TipoArchivo,
                });
                console.log(listDocumentosAdjuntos);
            }

            function enviarFomularioOK() {
                dataProvider.postData("DocumentoElectronico/Grabar", { Operacion: Operacion, listDocumentosAdjuntos: listDocumentosAdjuntos, eDocumentoElectronicoOperacion: DocumentoElectronicoOperacion, listEUsuarioGrupo: listEUsuarioGrupo }).success(function (respuesta) {
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);
                }).error(function (error) {
                    console.log(error);
                    //MostrarError();
                });
                limpiarFormulario();
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);
        }

        context.editarOperacion = function (rowIndex) {
           
            context.operacion = context.gridOptions.data[rowIndex];
            context.DocumentoElectronicoOperacion = context.operacion.DocumentoElectronicoOperacion;
            context.operacion.TipoComunicacion = context.operacion.TipoComunicacion.substring(0, 1);
            //context.codigodepartamento = parseInt(context.empresa.CodigoUbigeo.substring(0, 2));
            context.operacion.AccesoOperacion = context.operacion.AccesoOperacion.substring(0, 1)
            console.log(context.operacion);
            console.log(context.DocumentoElectronicoOperacion);
            context.CambiarVentana('CreateAndEdit');
        };
        context.CambiarVentana = function (mostrarVentana) {
            context.visible = mostrarVentana;
            if (context.visible == "List") {
                limpiarFormulario();
            } else {
                obtenerUsuarioSession();
            }
        }
        ////
        function limpiarFormulario() {
            context.operacion = {};
            context.DocumentoElectronicoOperacion = {};
            context.usuarioRemitentes = [];
            context.usuarioDestinatarios = [];
            context.operacion = {
                TipoDocumento: '02',
                TipoComunicacion: '1',
                PrioridadOperacion: '02',
                AccesoOperacion: '2'
            }
            archivosSelecionados = [];
            document.getElementById("input_file").value = "";
        }
        function listarUsuarioGrupoAutoComplete(Nombre) {
            var UsuarioGrupo = { Nombre: Nombre };
            appService.buscarUsuarioGrupoAutoComplete(UsuarioGrupo).success(function (respuesta) {
                //context.listaUsuario = respuesta;
                //console.log(respuesta);
                context.listaUsuarioGrupo = respuesta;
            });
        }
        function obtenerUsuarioSession() {
            var usuarioGrupo = { IDUsuarioGrupo: appService.obtenerUsuarioId() };
            appService.buscarUsuarioGrupoAutoComplete(usuarioGrupo).success(function (respuesta) {
                context.usuarioRemitentes = respuesta;
            });
        }
        function querySearch(criteria) {

            listarUsuarioGrupoAutoComplete(criteria);
            cachedQuery = cachedQuery || criteria;
            console.log(context.listaUsuarioGrupo);
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
            });
        }

        function listarOperacion() {
            dataProvider.getData("DocumentoElectronico/ListarOperacion").success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        listarOperacion();
        //context.usuarioDestinatarios=
    }
})();
