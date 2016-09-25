//Leer Archivos de de fisico a binario
var archivosSelecionados = [];
let TipoMensaje = "warning";
function ReadFileToBinary(control) {
    archivosSelecionados = [];
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
//try {
//    CKEDITOR.instances['editor1'].destroy(true);
//} catch (e) { }
//CKEDITOR.replace('editor1');
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
        context.visible = "CreateAndEdit";
        context.listaUsuarioGrupo = [];
        
        //Variable de autocomplete
        var Operacion = {};
        let listEUsuarioGrupo = [];
        let listERemitente = [];
        let listEDestinatario = [];
        var listDocumentosAdjuntos = [];
        //
        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioRemitentes = [];
        context.usuarioDestinatarios = [];
        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario = {};


        //var
        var listRemitentes = [];
        var listDestinatarios = [];

        LlenarConcepto(TipoDocumento);
        LlenarConcepto(PrioridadAtencion);
        LlenarConcepto(TipoAcceso);
        LlenarConcepto(TipoComunicacion);
        
        context.operacion = {
            TipoDocumento: '02',
            TipoComunicacion: '1',
            PrioridadOperacion: '02',
            AccesoOperacion: '2',
            FechaVigente: sumarDias(new Date(), 5),
            FechaEnvio: new Date(),
            FechaRegistro: new Date()
        };

        
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NumeroOperacion', displayName: 'Nº Documento' },
                { field: 'FechaRegistro', displayName: 'Fecha Registro', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'TipoDoc.DescripcionCorta', displayName: 'T.Doc' },
                { field: 'DescripcionOperacion', displayName: '	Asunto' },
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
            context.DocumentoElectronicoOperacion.Memo = CKEDITOR.instances.editor1.getData();
            console.log(CKEDITOR.instances.editor1.getData());
            console.log(context.DocumentoElectronicoOperacion.Memo);
            Operacion = context.operacion;
            let usuarioRemitenteLogueado = appService.obtenerUsuarioId();

            if (Operacion.EstadoOperacion == "ACTIVO") {
                return appService.mostrarAlerta("No se puede modificar Documento", "El documento ya ha sido enviado", "warning");
            }
            //if (archivosSelecionados == undefined || archivosSelecionados == "" || archivosSelecionados == null) {
            //    return appService.mostrarAlerta("Advertencia", "Debe seleccionar por lo menos un archivo", "warning");
            //}
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
            if (numeroboton == 1){
                Operacion.EstadoOperacion = 0;
            }
            else if (numeroboton == 2) {
                if (!usuarioRemitenteEnSession) {
                    return appService.mostrarAlerta("Advertencia", "El usuario no es remitente", "warning");
                }
                Operacion.EstadoOperacion = 1;
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
                dataProvider.postData("DocumentoElectronico/Grabar", { Operacion: Operacion, listDocumentosAdjuntos: listDocumentosAdjuntos, eDocumentoElectronicoOperacion: context.DocumentoElectronicoOperacion, listEUsuarioGrupo: listEUsuarioGrupo }).success(function (respuesta) {
                    console.log(Operacion);
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

        context.nuevo = function () {
            limpiarFormulario();
            obtenerUsuarioSession();
        }

        context.editarOperacion = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            context.DocumentoElectronicoOperacion = context.operacion.DocumentoElectronicoOperacion;
            context.operacion.TipoComunicacion = context.operacion.TipoComunicacion.substring(0, 1);
            //context.codigodepartamento = parseInt(context.empresa.CodigoUbigeo.substring(0, 2));
            context.operacion.AccesoOperacion = context.operacion.AccesoOperacion.substring(0, 1)

            //falta corregir fecha
            context.operacion.FechaVigente = appService.setFormatDate(context.operacion.FechaVigente);
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            context.operacion.FechaRegistro = appService.setFormatDate(context.operacion.FechaRegistro);

            //Estados
            if (context.operacion.EstadoOperacion==0)
                context.operacion.EstadoOperacion = 'CREADO'
            else if (context.operacion.EstadoOperacion == 1)
                context.operacion.EstadoOperacion = 'ACTIVO'
            else
                context.operacion.EstadoOperacion = 'INACTIVO'

            ObtenerUsuariosParticipantes(context.operacion)

            context.usuarioRemitentes = listRemitentes;
            context.usuarioDestinatarios = listDestinatarios;

            context.CambiarVentana('CreateAndEdit');
            window.setTimeout(function () {
                CKEDITOR.instances.editor1.setData(context.DocumentoElectronicoOperacion.Memo);
            }, 2000);
        };
        context.CambiarVentana = function (mostrarVentana) {
           
            context.visible = mostrarVentana;
            if (context.visible == "List") {
                limpiarFormulario();
                listarOperacion();
            } else {
                obtenerUsuarioSession();
            }
        }
        context.listarAdjunto = function () {
            //listarAdjunto
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
                AccesoOperacion: '2',
                FechaVigente: sumarDias(new Date(), 5),
                FechaEnvio: new Date(),
                FechaRegistro: new Date()
            }
            document.getElementById("input_file").value = "";
            listRemitentes = [];
            listDestinatarios = [];
            obtenerUsuarioSession();
            archivosSelecionados = [];
            listDocumentosAdjuntos = [];
            CKEDITOR.instances.editor1.setData("");
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
                console.log(respuesta);
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
        //sumar dias
        function sumarDias(fecha, dias) {
            fecha.setDate(fecha.getDate() + dias);
            return fecha;
        }

        function ObtenerUsuariosParticipantes(operacion) {
            dataProvider.postData("DocumentoElectronico/ListarUsuarioParticipanteDE", operacion).success(function (respuesta) {
                for (var ind in respuesta) {
                    respuesta[ind].Usuario.Nombre = respuesta[ind].Usuario.NombreUsuario;
                    if (respuesta[ind].TipoParticipante == UsuarioRemitente)
                        listRemitentes.push(respuesta[ind].Usuario);
                    else
                        listDestinatarios.push(respuesta[ind].Usuario);
                }
                console.log(listDestinatarios);
                console.log(listRemitentes);
            }).error(function (error) {
                //MostrarError();
            });

        }

        obtenerUsuarioSession();
        //context.usuarioDestinatarios=
        //ObtenerUsuariosParticipantes();
    }
})();
