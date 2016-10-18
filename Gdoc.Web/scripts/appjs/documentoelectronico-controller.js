//Leer Archivos de de fisico a binario
var archivosSelecionados = [];
let TipoMensaje = "warning";

function ReadFileToBinary(control) {
    archivosSelecionados = [];
    for (var i = 0, f; f = control.files[i]; i++) {
        let files = f;
        var reader = new FileReader();
        reader.onloadend = function (e) {
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
        let UsuarioRemitenteProveido = "09";
        let UsuarioDestinatarioProveido = "10";
        let Estado = "001";

        var context = this;
        context.operacion = {};
        context.DocumentoElectronicoOperacion = {};
        context.visible = "CreateAndEdit";
        context.listaUsuarioGrupo = [];
        context.listDocumentoAdjunto = [];
        context.listDocumentoAdjuntoR = [];

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
        context.destinatariosProveidos = [];
        context.filterSelected = true;
        context.querySearch = querySearch;

        //ng-visible
        context.eliminar = true;
        context.agregar = true;
        context.mostrar = false
        context.referencia = true;

        //LlenarConcepto(TipoDocumento);
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
        context.gridComentarios = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableSorting: true,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'FechaPublicacion', displayName: 'Fecha', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm"' },
                { field: 'Usuario.NombreUsuario', displayName: 'Participante' },
                { field: 'ComentarioMesaVirtual', displayName: 'Comentario' }
                //{
                //    name: 'Adjuntos', width: '7%',
                //    cellTemplate: '<i ng-click="grid.appScope.mostrarAdjuntos(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-paperclip" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Ver"></i>'
                //}
            ]
        };
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableSorting: true,
            enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                {
                    name: 'Acciones', width: '7%',
                    cellTemplate: '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="bottom" data-toggle="tooltip" title="Editar"></i>' +
                                '<i ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-file-pdf-o" data-placement="bottom" data-toggle="tooltip"  data-original-title="Ver Documento"></i>' +
                                '<i class="fa fa-commenting-o" ng-click="grid.appScope.ComentarioProveido(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Proveido"></i>' +
                                '<i ng-click="grid.appScope.eliminarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-times" data-placement="bottom" data-toggle="tooltip" title="" data-original-title="Desactivar"></i>'
                },
                { field: 'NumeroOperacion', width: '15%', displayName: 'Nº Documento' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha Registro', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'TipoDoc.DescripcionCorta', width: '5%', displayName: 'T.Doc' },
                { field: 'TituloOperacion', width: '55%', displayName: 'Titulo' },
                { field: 'Estado.DescripcionConcepto', width: '8%', displayName: 'Estado' }
                
            ]
        };
        //Comentario Proveido
        context.ComentarioProveido = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            console.log(context.operacion);
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            ObtenerUsuariosParticipantes(context.operacion);
            context.usuarioOrganizador = listRemitentes;
            context.usuarioInvitados = listDestinatarios;
            listarComentarioProveido(context.operacion);
            context.CambiarVentana("ComentarioProveido");
        }
        context.recargarComentario = function () {
            listarComentarioProveido(context.operacion);
        }
        context.grabarComentarioProveido = function () {
            let Operacion = context.operacion;
            let MesaVirtualComentario = context.mesavirtualComentario;

            if (context.destinatariosProveidos == undefined || context.destinatariosProveidos == "") {
                return appService.mostrarAlerta("Falta los Destinatarios", "Agregue a los destinatarios", "warning");
            }

            for (var ind in context.destinatariosProveidos) {
                console.log(context.destinatariosProveidos[ind]);
                context.destinatariosProveidos[ind].TipoParticipante = UsuarioDestinatarioProveido;
                listEUsuarioGrupo.push(context.destinatariosProveidos[ind]);
            }
            console.log(listEUsuarioGrupo);

            function enviarFomularioOK() {
                dataProvider.postData("DocumentosRecibidos/GrabarComentarioProveido", { Operacion: Operacion, mesaVirtualComentario: MesaVirtualComentario, listUsuariosDestinatarios: listEUsuarioGrupo }).success(function (respuesta) {
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);
                    //listarComentarioProveido();
                    //limpiarFormulario();

                    context.CambiarVentana("List");
                    //listarComentarioMesaVirtual(context.operacion);

                    console.log(respuesta);
                }).error(function (error) {
                    //MostrarError();
                });

                context.mesavirtualComentario = {};
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);
        }
        //Eventos
        //Visualizacion
        context.mostrarPDF = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            if (context.operacion.EstadoOperacion == 1) {
                dataProvider.postData("DocumentosRecibidos/ListarDocumentoPDF", context.operacion).success(function (respuesta) {
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
            dataProvider.postData("DocumentosRecibidos/ListarAdjuntos", archivo).success(function (respuesta) {
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
            for (var index in context.listDocumentoAdjuntoR) {
                listDocumentosAdjuntos.push({
                    RutaArchivo: context.listDocumentoAdjuntoR[index].RutaArchivo,
                    NombreOriginal: context.listDocumentoAdjuntoR[index].NombreOriginal,
                    TamanoArchivo: context.listDocumentoAdjuntoR[index].TamanoArchivo,
                    TipoArchivo: context.listDocumentoAdjuntoR[index].TipoArchivo,
                    EstadoAdjunto: context.listDocumentoAdjuntoR[index].EstadoAdjunto,
                });
                console.log(context.listDocumentoAdjuntoR);
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
                document.getElementById("input_file").value = "";
                CKEDITOR.instances.editor1.setData("");
            }
            function cancelarFormulario() {
                Operacion.EstadoOperacion = 0;
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK, cancelarFormulario);
        }
        context.nuevo = function () {
            limpiarFormulario();
            document.getElementById("input_file").value = "";
            CKEDITOR.instances.editor1.setData("");
            obtenerUsuarioSession();
        }
        context.editarOperacion = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            context.DocumentoElectronicoOperacion = context.operacion.DocumentoElectronicoOperacion;
            context.operacion.TipoComunicacion = context.operacion.TipoComunicacion.substring(0, 1);
            context.operacion.AccesoOperacion = context.operacion.AccesoOperacion.substring(0, 1)
            context.operacion.EstadoOperacion = context.operacion.EstadoOperacion.toString();

            listarAdjuntos(context.operacion);
            if (context.operacion.EstadoOperacion == 1) {
                context.eliminar = false;
                context.agregar = false;
                context.mostrar = true;
            }

            context.referencia = false;
            //falta corregir fecha
            context.operacion.FechaVigente = appService.setFormatDate(context.operacion.FechaVigente);
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            context.operacion.FechaRegistro = appService.setFormatDate(context.operacion.FechaRegistro);

            ObtenerUsuariosParticipantes(context.operacion)

            context.usuarioRemitentes = listRemitentes;
            context.usuarioDestinatarios = listDestinatarios;

            context.CambiarVentana('CreateAndEdit');
            window.setTimeout(function () {
                CKEDITOR.instances.editor1.setData(context.DocumentoElectronicoOperacion.Memo);
            }, 2000);
        };
        context.eliminarOperacion = function (rowIndex) {
            var operacion = context.gridOptions.data[rowIndex];
            function enviarFomularioOK() {
                dataProvider.postData("DocumentoElectronico/EliminarOperacion", operacion).success(function (respuesta) {
                    appService.mostrarAlerta("Información", "Documento Electronico Inactivo", "warning")
                    listarOperacion();
                }).error(function (error) {
                    //MostrarError();
                });
            }
            function cancelarFormulario() {
                //Operacion.EstadoOperacion = 0;
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK, cancelarFormulario);

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
            archivosSelecionados = [];
            document.getElementById("input_file").value = "";
        }
        context.agregaradjuntoR = function (operacion) {        
            obtenerDocumentoReferencia(operacion);      
        }
        context.eliminarAdjunto = function (indexAdjunto) {
            context.listDocumentoAdjunto.splice(indexAdjunto, 1);
        }
        context.eliminarAdjuntoR = function (indexAdjunto) {
            context.listDocumentoAdjuntoR.splice(indexAdjunto, 1);
        }
        context.listarAdjunto = function () {
            $("#modal_adjuntos").modal("show");
        }
        context.listarAdjuntoR = function () {
            $("#modal_adjuntosR").modal("show");
        }
        ////
        function listarComentarioProveido(operacion) {
            dataProvider.postData("DocumentosRecibidos/ListarComentarioProveido", operacion).success(function (respuesta) {
                context.gridComentarios.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        function listarAdjuntos(operacion) {
            //dataProvider.postData("DocumentosRecibidos/ListarDocumentoAdjunto", operacion).success(function (respuesta) {
            dataProvider.postData("DocumentosRecibidos/ListarAdjunto", operacion).success(function (respuesta) {
                context.listDocumentoAdjunto = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        function obtenerDocumentoReferencia(operacion) {

            console.log(operacion);
            dataProvider.postData("DocumentoElectronico/ListarOperacionReferencia", operacion).success(function (respuesta) {
                console.log(respuesta);
                context.listDocumentoAdjuntoR = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        function limpiarFormulario() {
            context.eliminar = true;
            context.agregar = true;
            context.mostrar = false;
            context.referencia = true;
            context.operacion = {};
            context.DocumentoElectronicoOperacion = {};
            context.usuarioRemitentes = [];
            context.usuarioDestinatarios = [];
            context.listDocumentoAdjunto = [];
            context.listDocumentoAdjuntoR = [];
            context.destinatariosProveidos = [];
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
            //document.getElementById("input_file").value = "";
            listRemitentes = [];
            listDestinatarios = [];
            obtenerUsuarioSession();
            archivosSelecionados = [];
            listEUsuarioGrupo = [];
            listDocumentosAdjuntos = [];
            //CKEDITOR.instances.editor1.setData("");
            $('.nav-tabs a[href="#Datos"]').tab('show')
        }
        function listarUsuarioGrupoAutoComplete(Nombre) {
            var UsuarioGrupo = { Nombre: Nombre };
            appService.buscarUsuarioGrupoAutoComplete(UsuarioGrupo).success(function (respuesta) {
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
        function LlenarConceptoTipoDocumento() {
            var concepto = { TipoConcepto: TipoDocumento, TextoUno: "DE" }
            dataProvider.postData("Concepto/ListarConceptoTipoDocumento", concepto).success(function (respuesta) {
                context.listTipoDocumento = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        function listarOperacion() {
            dataProvider.getData("DocumentoElectronico/ListarOperacion").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                console.log(respuesta);
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
            }).error(function (error) {
                //MostrarError();
            });

        }
        obtenerUsuarioSession();
        LlenarConceptoTipoDocumento();
    }
})();
