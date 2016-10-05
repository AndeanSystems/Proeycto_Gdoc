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

            console.log(archivosSelecionados);
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
        let Estado = "001";

        var context = this;
        context.operacion = {};
        context.DocumentoElectronicoOperacion = {};
        context.visible = "CreateAndEdit";
        context.listaUsuarioGrupo = [];
        context.listDocumentoAdjunto = [];

        var Operacion = {};
        var listRemitentes = [];
        var listDestinatarios = [];
        let listEUsuarioGrupo = [];
        var listDocumentosAdjuntos = [];

        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioRemitentes = [];
        context.usuarioDestinatarios = [];
        context.filterSelected = true;
        context.querySearch = querySearch;

        //ng-visible
        context.eliminar = true;
        context.agregar = true;

        LlenarConcepto(TipoDocumento);
        LlenarConcepto(PrioridadAtencion);
        LlenarConcepto(TipoAcceso);
        LlenarConcepto(TipoComunicacion);
        LlenarConcepto(Estado);

        context.operacion = {
            TipoDocumento: '02',
            TipoComunicacion: '1',
            PrioridadOperacion: '02',
            EstadoOperacion: '0',
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
                { field: 'NumeroOperacion', width: '15%', displayName: 'Nº Documento' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha Registro', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'TipoDoc.DescripcionCorta', width: '5%', displayName: 'T.Doc' },
                { field: 'TituloOperacion', width: '55%', displayName: 'Titulo' },
                { field: 'Estado.DescripcionConcepto', width: '8%', displayName: 'Estado' },
                {
                    name: 'Acciones', width: '7%',
                    cellTemplate: '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="bottom" data-toggle="tooltip" title="Editar"></i>' +
                                '<i ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-file-pdf-o" data-placement="bottom" data-toggle="tooltip"  data-original-title="Ver Documento"></i>' +
                                '<i ng-click="grid.appScope.eliminarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-times" data-placement="bottom" data-toggle="tooltip" title="" data-original-title="Desactivar"></i>'
                }
            ]
        };
        //Eventos
        //Visualizacion
        context.mostrarPDF = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            if (context.operacion.EstadoOperacion == 1) {
                dataProvider.postData("DocumentosRecibidos/ListarDocumentoPDF", context.operacion).success(function (respuesta) {
                    console.log(respuesta)
                    window.open(respuesta, "mywin", "resizable=1");
                }).error(function (error) {
                    //MostrarError();
                });
            }
            else {
                appService.mostrarAlerta("Información","La operacion no ha sido enviada")
            }
        }
        context.mostrarAdjuntoWindows = function (archivo) {
            console.log(archivo);
            dataProvider.postData("DocumentosRecibidos/ListarAdjuntos", archivo).success(function (respuesta) {
                console.log(respuesta)
                window.open(respuesta, "mywin", "resizable=1");
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Mantenimiento
        context.grabar = function (numeroboton) {
            context.DocumentoElectronicoOperacion.Memo = CKEDITOR.instances.editor1.getData();
            console.log(CKEDITOR.instances.editor1.getData());
            console.log(context.DocumentoElectronicoOperacion.Memo);
            console.log(context.usuarioRemitentes);
            console.log(context.usuarioDestinatarios);
            Operacion = context.operacion;
            let usuarioRemitenteLogueado = appService.obtenerUsuarioId();

            if (Operacion.EstadoOperacion == 1) {
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
            if (context.DocumentoElectronicoOperacion.Memo == undefined || context.DocumentoElectronicoOperacion.Memo == "") {
                return appService.mostrarAlerta("Atención", "Agregue Contenido al Documento", "warning");
            }
            var usuarioRemitenteEnSession = false;
            for (var ind in context.usuarioRemitentes) {
                console.log(context.usuarioRemitentes[ind]);
                if (context.usuarioRemitentes[ind].IDUsuarioGrupo == usuarioRemitenteLogueado)
                    usuarioRemitenteEnSession = true;
                context.usuarioRemitentes[ind].TipoParticipante = UsuarioRemitente;
                listEUsuarioGrupo.push(context.usuarioRemitentes[ind]);
            }
            for (var ind in context.usuarioDestinatarios) {
                console.log(context.usuarioDestinatarios[ind]);
                context.usuarioDestinatarios[ind].TipoParticipante = UsuarioDestinatario;
                listEUsuarioGrupo.push(context.usuarioDestinatarios[ind]);
            }
            console.log(listEUsuarioGrupo);
            if (numeroboton == 1) {
                Operacion.EstadoOperacion = 0;
            }
            else if (numeroboton == 2) {
                if (!usuarioRemitenteEnSession) {
                    return appService.mostrarAlerta("Advertencia", "El usuario no es remitente", "warning");
                }
                Operacion.EstadoOperacion = 1;
            }
            //for (var index in archivosSelecionados) {
            //    listDocumentosAdjuntos.push({
            //        RutaArchivo: archivosSelecionados[index].RutaBinaria,
            //        NombreOriginal: archivosSelecionados[index].NombreArchivo,
            //        TamanoArchivo: archivosSelecionados[index].TamanoArchivo,
            //        TipoArchivo: archivosSelecionados[index].TipoArchivo,
            //    });
            //    console.log(listDocumentosAdjuntos);
            //}
            for (var index in context.listDocumentoAdjunto) {
                listDocumentosAdjuntos.push({
                    RutaArchivo: context.listDocumentoAdjunto[index].RutaArchivo,
                    NombreOriginal: context.listDocumentoAdjunto[index].NombreOriginal,
                    TamanoArchivo: context.listDocumentoAdjunto[index].TamanoArchivo,
                    TipoArchivo: context.listDocumentoAdjunto[index].TipoArchivo,
                    EstadoAdjunto: context.listDocumentoAdjunto[index].EstadoAdjunto,
                });
                console.log(context.listDocumentoAdjunto);
                console.log(listDocumentosAdjuntos);
            }
            function enviarFomularioOK() {
                dataProvider.postData("DocumentoElectronico/Grabar", { Operacion: Operacion, listDocumentosAdjuntos: listDocumentosAdjuntos, eDocumentoElectronicoOperacion: context.DocumentoElectronicoOperacion, listEUsuarioGrupo: listEUsuarioGrupo }).success(function (respuesta) {
                    console.log(Operacion);
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    else
                        TipoMensaje = "error";
                    appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);
                }).error(function (error) {
                    console.log(error);
                    //MostrarError();
                });
                limpiarFormulario();
            }
            function cancelarFormulario() {
                Operacion.EstadoOperacion = 0;
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK, cancelarFormulario);
        }
        context.nuevo = function () {
            limpiarFormulario();
            obtenerUsuarioSession();
        }
        context.editarOperacion = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            console.log(context.operacion);
            context.DocumentoElectronicoOperacion = context.operacion.DocumentoElectronicoOperacion;
            context.operacion.TipoComunicacion = context.operacion.TipoComunicacion.substring(0, 1);
            context.operacion.AccesoOperacion = context.operacion.AccesoOperacion.substring(0, 1)
            context.operacion.EstadoOperacion = context.operacion.EstadoOperacion.toString();

            listarAdjuntos(context.operacion);
            if (context.operacion.EstadoOperacion == 1) {
                context.eliminar = false;
                context.agregar = false;
            }
            //falta corregir fecha
            context.operacion.FechaVigente = appService.setFormatDate(context.operacion.FechaVigente);
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            context.operacion.FechaRegistro = appService.setFormatDate(context.operacion.FechaRegistro);

            ObtenerUsuariosParticipantes(context.operacion)

            context.usuarioRemitentes = listRemitentes;
            context.usuarioDestinatarios = listDestinatarios;

            console.log(listDestinatarios);
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
                //obtenerUsuarioSession();
            }
        }
        //Adjuntos
        context.agregaradjunto = function () {
            for (var ind in archivosSelecionados) {
                var hola = true;
                console.log(archivosSelecionados[ind].NombreArchivo);
                for (var index in context.listDocumentoAdjunto) {
                    if (archivosSelecionados[ind].NombreArchivo == context.listDocumentoAdjunto[index].NombreOriginal)
                        hola = false;
                }
                if (hola == true) {
                    context.listDocumentoAdjunto.push({
                        RutaArchivo: archivosSelecionados[ind].RutaBinaria,
                        NombreOriginal: archivosSelecionados[ind].NombreArchivo,
                        TamanoArchivo: archivosSelecionados[ind].TamanoArchivo,
                        TipoArchivo: archivosSelecionados[ind].TipoArchivo,
                        EstadoAdjunto:0
                    });
                }
            }
            console.log(context.listDocumentoAdjunto);
            archivosSelecionados = [];
            document.getElementById("input_file").value = "";
        }
        context.eliminarAdjunto = function (indexAdjunto) {
            context.listDocumentoAdjunto.splice(indexAdjunto, 1);
        }
        context.listarAdjunto = function () {
            $("#modal_adjuntos").modal("show");
        }
        ////
        function listarAdjuntos(operacion) {
            //dataProvider.postData("DocumentosRecibidos/ListarDocumentoAdjunto", operacion).success(function (respuesta) {
            dataProvider.postData("DocumentosRecibidos/ListarAdjunto", operacion).success(function (respuesta) {
                context.listDocumentoAdjunto = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        function limpiarFormulario() {
            context.eliminar = true;
            context.agregar = true;
            context.operacion = {};
            context.DocumentoElectronicoOperacion = {};
            context.usuarioRemitentes = [];
            context.usuarioDestinatarios = [];
            context.listDocumentoAdjunto = [];
            context.operacion = {
                TipoDocumento: '02',
                TipoComunicacion: '1',
                PrioridadOperacion: '02',
                AccesoOperacion: '2',
                EstadoOperacion: '0',
                FechaVigente: sumarDias(new Date(), 5),
                FechaEnvio: new Date(),
                FechaRegistro: new Date()
            }
            document.getElementById("input_file").value = "";
            listRemitentes = [];
            listDestinatarios = [];
            obtenerUsuarioSession();
            archivosSelecionados = [];
            listEUsuarioGrupo = [];
            listDocumentosAdjuntos = [];
            CKEDITOR.instances.editor1.setData("");
            $('.nav-tabs a[href="#Datos"]').tab('show')
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
                else if (concepto.TipoConcepto == Estado)
                    context.listEstado = respuesta;
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
                    if (respuesta[ind].TipoParticipante == UsuarioRemitente)
                        listRemitentes.push(respuesta[ind]);
                    else
                        listDestinatarios.push(respuesta[ind]);
                }
                console.log(listDestinatarios);
                console.log(listRemitentes);
            }).error(function (error) {
                //MostrarError();
            });

        }
        obtenerUsuarioSession();
    }
})();
