﻿//Leer Archivos de de fisico a binario
var archivosSelecionados = [];
let TipoMensaje = "warning";
function ReadFileToBinary(control) {
    archivosSelecionados = [];
    for (var i = 0, f; f = control.files[i]; i++) {
        let files = f;
        var reader = new FileReader();
        reader.onloadend = function (e) {
            console.log(files);
            console.log(files.name.length)
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
//Angular JS
(function () {
    'use strict';
    angular.module('app').controller('documentodigital_controller', documentodigital_controller);
    documentodigital_controller.$inject = ['$location', 'app_factory', 'appService'];
    function documentodigital_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        let TipoDocumento = "012";
        let PrioridadAtencion = "005";
        let TipoAcceso = "002";
        let TipoComunicacion = "022";
        let Estado = "001";
        let UsuarioRemitente = "07";
        let UsuarioDestinatario = "08";
        let UsuarioRemitenteProveido = "09";
        let UsuarioDestinatarioProveido = "10";
        var context = this;
        context.operacion = {};
        context.operacion.FechaEmision = new Date();
        context.operacion.FechaCierre = new Date();
        context.DocumentoDigitalOperacion = {};
        context.referencia = {};
        context.visible = "CreateAndEdit";
        context.listaReferencia = [];
        context.listaUsuarioGrupo = [];
        context.listDocumentoAdjunto = [];
        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioRemitentes = [];
        context.usuarioDestinatarios = [];
        context.destinatariosProveidos = [];
        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario = {};
        //var
        var listEUsuarioGrupo = [];
        var listRemitentes = [];
        var listDestinatarios = [];
        var listDocumentoDigitaloOperacion = [];
        //ng-visible
        context.mostrar = false;
        context.eliminar = true;
        context.agregar = true;

        //LlenarConcepto(TipoDocumento);
        LlenarConcepto(TipoAcceso);
        LlenarConcepto(PrioridadAtencion);
        LlenarConcepto(Estado);

        //COMIENZO
        context.operacion = {
            TipoDocumento: '41',
            AccesoOperacion: '2',
            PrioridadOperacion: '02',
            TipoComunicacion: '1',
            EstadoOperacion: '0',
            FechaEmision: new Date(),
            FechaCierre: new Date(),
            FechaRegistro: new Date(),
            FechaEnvio: new Date(),
            FechaVigente: new Date()
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
                { field: 'ComentarioMesaVirtual', displayName: 'Comentario' },
                { field: 'Destinatarios', displayName: 'Destinatarios' }
                //{
                //    name: 'Adjuntos', width: '7%',
                //    cellTemplate: '<i ng-click="grid.appScope.mostrarAdjuntos(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-paperclip" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Ver"></i>'
                //}
            ]
        };
        context.operacion.DocumentoDigitalOperacion = {
            DerivarDocto: 'S'
        };
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableSorting: true,
            enableFiltering: true,//FILTRO
            data: [],
            appScopeProvider: context,
            columnDefs: [
                {
                    name: 'Acciones', width: '7%',
                    cellTemplate: '<i ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-file-pdf-o" data-placement="top" data-toggle="tooltip" title="Ver"></i>' +
                                '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                '<i class="fa fa-commenting-o" ng-click="grid.appScope.ComentarioProveido(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Proveido"></i>' +
                                '<i ng-click="grid.appScope.eliminarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-times" data-placement="top" data-toggle="tooltip" title="" data-original-title="Borrar"></i>'
                },
                { field: 'NumeroOperacion', width : '15%', displayName: 'Nº Documento' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha Emisión', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'TipoDoc.DescripcionCorta', width: '5%', displayName: 'T.Doc' },
                { field: 'TituloOperacion', width: '55%', displayName: '	Titulo' },
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
            function cancelarFormulario() {
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK, cancelarFormulario);
        }
        //Eventos
        context.mostrarPDF = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            dataProvider.postData("DocumentosRecibidos/ListarDocumentoPDF", context.operacion).success(function (respuesta) {
                console.log(respuesta)
                window.open(respuesta, "mywin", "resizable=1");
            }).error(function (error) {
                //MostrarError();
            });                     
        }
        //Reeferencias
        context.agregarreferencia = function (referencia) {
            if (context.referencia.DescripcionIndice == undefined)
                appService.mostrarAlerta("Informacion", "Ingrese Referencia");
            else {
                for (var ind in context.listaReferencia) {
                    if (context.listaReferencia[ind].DescripcionIndice == context.referencia.DescripcionIndice) {
                        context.referencia = {};
                        return;
                    }   
                }
                context.listaReferencia.push(context.referencia);
                console.log(context.listaReferencia);
                context.referencia = {};
            }
        }
        context.eliminarreferencia = function (indexReferencia) {
            context.listaReferencia.splice(indexReferencia,1);
        }
        //Mantenimiento
        context.grabar = function (numeroboton) {
            let usuarioRemitenteLogueado = appService.obtenerUsuarioId();
            let Operacion = context.operacion;
            if (Operacion.EstadoOperacion == 1) {
                return appService.mostrarAlerta("Atención", "No se puede modificar el documento, ya ha sido enviado", "warning");
            }
            if (Operacion.EstadoOperacion == 2) {
                return appService.mostrarAlerta("No se puede modificar Documento", "El documento ya ha sido eliminado", "warning");
            }
            if (context.listDocumentoAdjunto == undefined || context.listDocumentoAdjunto == "" || context.listDocumentoAdjunto == null) {
                return appService.mostrarAlerta("Advertencia", "Debe seleccionar por lo menos un archivo", "warning");
            }
            if (context.usuarioRemitentes == undefined || context.usuarioRemitentes == "") {
                return appService.mostrarAlerta("Falta los Remitentes", "Agregue a los Remitentes", "warning");
            }
            if (context.usuarioDestinatarios == undefined || context.usuarioDestinatarios == "") {
                return appService.mostrarAlerta("Falta los Destinatarios", "Agregue a los destinatarios", "warning");
            }
            if (context.listaReferencia == undefined || context.listaReferencia == "") {
                return appService.mostrarAlerta("Atención", "Debe adicionar referencia", "warning");
            }
            if (context.listDocumentoAdjunto[0].NombreOriginal.length > 200) {
                return appService.mostrarAlerta("Atención", "El nombre del documento adjunto supera el maximo permitido", "warning");
            }
            let DocumentoDigitalOperacion = context.DocumentoDigitaloOperacion;
            var listIndexacionDocumento = context.listaReferencia;

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
                
            listDocumentoDigitaloOperacion = [];
            console.log(listEUsuarioGrupo);

            console.log(archivosSelecionados);
            //for (var index in archivosSelecionados) {
            //    listDocumentoDigitaloOperacion.push({
            //        RutaFisica: archivosSelecionados[index].RutaBinaria,
            //        NombreOriginal: archivosSelecionados[index].NombreArchivo,
            //        TamanoDocto: archivosSelecionados[index].TamanoArchivo,
            //        TipoArchivo: archivosSelecionados[index].TipoArchivo,
            //        DerivarDocto: context.operacion.DocumentoDigitalOperacion.DerivarDocto,
            //    });
            //    console.log(listDocumentoDigitaloOperacion);
            //}
            for (var index in context.listDocumentoAdjunto) {
                listDocumentoDigitaloOperacion.push({
                    RutaFisica: context.listDocumentoAdjunto[index].RutaFisica,
                    NombreOriginal: context.listDocumentoAdjunto[index].NombreOriginal,
                    TamanoDocto: context.listDocumentoAdjunto[index].TamanoDocto,
                    TipoArchivo: context.listDocumentoAdjunto[index].TipoArchivo,
                    DerivarDocto: context.operacion.DocumentoDigitalOperacion.DerivarDocto,
                });
                console.log(listDocumentoDigitaloOperacion);
            }
            console.log(listDocumentoDigitaloOperacion);

            //validar tamaño maximo total de archivos
            var size = 0;
            for (var index in listDocumentoDigitaloOperacion) {
                size = size + listDocumentoDigitaloOperacion[index].TamanoArchivo;
            }
            if (size >= context.listParametros[0].TamanoMaxArchivos)
                return appService.mostrarAlerta("Información", "Se excedio el tamaño maximo en archivos adjuntos", "warning")

            function enviarFomularioOK() {
                dataProvider.postData("DocumentoDigital/Grabar", { Operacion: Operacion, listDocumentoDigitalOperacion: listDocumentoDigitaloOperacion, listEUsuarioGrupo: listEUsuarioGrupo, listIndexacion: listIndexacionDocumento }).success(function (respuesta) {
                    console.log(respuesta);
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);
                }).error(function (error) {
                    //MostrarError();
                });
                
                listDocumentoDigitaloOperacion = {};
                limpiarFormulario();

                document.getElementById("input_file").value = "";
            }
            function cancelarFormulario() {
                Operacion.EstadoOperacion = 0;
                listEUsuarioGrupo = [];
                listIndexacionDocumento = [];
                listDocumentoDigitaloOperacion = {};
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK, cancelarFormulario);                    
        }
        context.editarOperacion = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            context.operacion.DocumentoDigitalOperacion.DerivarDocto = context.operacion.DocumentoDigitalOperacion.DerivarDocto.substring(0, 1);
            context.operacion.TipoComunicacion = context.operacion.TipoComunicacion.substring(0, 1);
            context.operacion.AccesoOperacion = context.operacion.AccesoOperacion.substring(0, 1);
            context.operacion.EstadoOperacion = context.operacion.EstadoOperacion.toString();

            listarAdjuntos(context.operacion);
            if (context.operacion.EstadoOperacion == 1) {
                context.eliminar = false;
                context.agregar = false;
                context.mostrar = true;
            }
            //falta corregir fecha
            context.operacion.FechaEmision = appService.setFormatDate(context.operacion.FechaEmision);
            context.operacion.FechaVigente = appService.setFormatDate(context.operacion.FechaVigente);
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            context.operacion.FechaRegistro = appService.setFormatDate(context.operacion.FechaRegistro);
            context.operacion.FechaCierre = appService.setFormatDate(context.operacion.FechaCierre);

            ObtenerUsuariosParticipantes(context.operacion)

            context.usuarioRemitentes = listRemitentes;
            context.usuarioDestinatarios = listDestinatarios;

            listarReferencia(context.operacion);
            console.log(context.operacion);
            context.CambiarVentana('CreateAndEdit');
        };
        context.nuevo = function () {
            limpiarFormulario();
            obtenerUsuarioSession();
        }
        context.eliminarOperacion = function (rowIndex) {
            var operacion = context.gridOptions.data[rowIndex];

            function enviarFomularioOK() {
                dataProvider.postData("DocumentoDigital/EliminarOperacion", operacion).success(function (respuesta) {
                    console.log(respuesta);
                    appService.mostrarAlerta("Información", "Documento Digital Inactivo", "warning")
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
            } else if (context.visible == "CreateAndEdit") {
                //limpiarFormulario();
            }
        }
        //Adjuntos
        context.agregaradjunto = function () {
            context.listDocumentoAdjunto = [];
            for (var ind in archivosSelecionados) {
                var hola = true;
                console.log(archivosSelecionados[ind].NombreArchivo);
                for (var index in context.listDocumentoAdjunto) {
                    if (archivosSelecionados[ind].NombreArchivo == context.listDocumentoAdjunto[index].NombreOriginal)
                        hola = false;
                }
                if (hola == true) {
                    if (archivosSelecionados[ind].TamanoArchivo <= context.listParametros[0].TamanoMaxArchivo) {
                        context.listDocumentoAdjunto.push({
                            RutaFisica: archivosSelecionados[ind].RutaBinaria,
                            NombreOriginal: archivosSelecionados[ind].NombreArchivo,
                            TamanoDocto: archivosSelecionados[ind].TamanoArchivo,
                            TipoArchivo: archivosSelecionados[ind].TipoArchivo,
                        });
                    }
                    else
                        appService.mostrarAlerta("Información", "El archivo seleccionado se excedio del tamaño maximo", "warning");
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
        function listarComentarioProveido(operacion) {
            dataProvider.postData("DocumentosRecibidos/ListarComentarioProveido", operacion).success(function (respuesta) {
                context.gridComentarios.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        function listarAdjuntos(operacion) {
            dataProvider.postData("DocumentoDigital/ListarDocumentoDigitalOperacion", operacion).success(function (respuesta) {
                context.listDocumentoAdjunto = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        function listarUsuarioGrupoAutoComplete(Nombre) {
            var UsuarioGrupo = { Nombre: Nombre };
            appService.buscarUsuarioGrupoAutoComplete(UsuarioGrupo).success(function (respuesta) {
                //context.listaUsuario = respuesta;
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
            var concepto = { TipoConcepto: TipoDocumento, TextoUno: "DD" }
            dataProvider.postData("Concepto/ListarConceptoTipoDocumento", concepto).success(function (respuesta) {
                console.log(respuesta);
                context.listTipoDocumento = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        function listarOperacion() {
            dataProvider.getData("DocumentoDigital/ListarOperacion").success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
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
        function listarReferencia(operacion) {
            dataProvider.postData("DocumentoDigital/ListarReferencia", operacion).success(function (respuesta) {
                for (var ind in respuesta) {
                    context.listaReferencia.push(respuesta[ind]);
                }
                console.log(context.listaReferencia);
            }).error(function (error){
                //MostrarError();
            });
        }
        function CargarParametros() {
            var empresa = { IDEmpresa: 1001 };
            appService.listarParametros(empresa).success(function (respuesta) {
                context.listParametros = respuesta;
            });
        }
        function limpiarFormulario() {
            context.eliminar = true;
            context.agregar = true;
            context.mostrar = false;
            context.operacion = {};
            context.usuarioDestinatarios = [];
            context.usuarioRemitentes = [];
            context.DocumentoDigitalOperacion = {};
            context.referencia = {};
            context.listaReferencia = [];
            context.listDocumentoAdjunto = [];
            context.destinatariosProveidos = [];
            context.operacion = {
                TipoDocumento: '41',
                AccesoOperacion: '2',
                PrioridadOperacion: '02',
                TipoComunicacion: '1',
                EstadoOperacion: '0',
                FechaEmision: new Date(),
                FechaCierre: new Date(),
                FechaRegistro: new Date(),
                FechaEnvio: new Date(),
                FechaVigente: new Date()
            };
            context.operacion.DocumentoDigitalOperacion = {
                DerivarDocto: 'S'
            };
            obtenerUsuarioSession();
            archivosSelecionados = [];
            listEUsuarioGrupo = [];
            listRemitentes = [];
            listDestinatarios = [];
            $('.nav-tabs a[href="#Datos"]').tab('show')
        }
        CargarParametros();
        obtenerUsuarioSession();
        LlenarConceptoTipoDocumento();
    }
})();
