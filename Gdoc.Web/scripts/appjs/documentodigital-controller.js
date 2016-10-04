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
        let UsuarioRemitente = "07";//falta
        let UsuarioDestinatario = "08";
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
        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario = {};
        //var
        let listEUsuarioGrupo = [];
        var listRemitentes = [];
        var listDestinatarios = [];
        var listDocumentoDigitaloOperacion = [];
        //ng-visible
        context.eliminar = true;
        context.agregar = true;

        LlenarConcepto(TipoDocumento);
        LlenarConcepto(TipoAcceso);
        LlenarConcepto(PrioridadAtencion);
        LlenarConcepto(Estado);

        //COMIENZO
        context.operacion = {
            TipoDocumento: '02',
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
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,//FILTRO
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NumeroOperacion', width : '15%', displayName: 'Nº Documento' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha Emisión', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'TipoDoc.DescripcionCorta', width: '5%', displayName: 'T.Doc' },
                { field: 'TituloOperacion', width: '55%', displayName: '	Titulo' },
                { field: 'Estado.DescripcionConcepto', width: '8%', displayName: 'Estado' },
                {
                    name: 'Acciones', width: '7%',
                    cellTemplate: '<i ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-file-pdf-o" data-placement="top" data-toggle="tooltip" title="Ver"></i>' +
                                '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                '<i ng-click="grid.appScope.eliminarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-times" data-placement="top" data-toggle="tooltip" title="" data-original-title="Borrar"></i>'
                }
            ]
        };
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
                    RutaFisica: context.listDocumentoAdjunto[index].RutaArchivo,
                    NombreOriginal: context.listDocumentoAdjunto[index].NombreOriginal,
                    TamanoDocto: context.listDocumentoAdjunto[index].TamanoArchivo,
                    TipoArchivo: context.listDocumentoAdjunto[index].TipoArchivo,
                    DerivarDocto: context.operacion.DocumentoDigitalOperacion.DerivarDocto,
                });
                console.log(listDocumentoDigitaloOperacion);
            }
            console.log(listDocumentoDigitaloOperacion);
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
            dataProvider.postData("DocumentoDigital/EliminarOperacion", operacion).success(function (respuesta) {
                console.log(respuesta);
                listarOperacion();
            }).error(function (error) {
                //MostrarError();
            });
        };
        context.CambiarVentana = function (mostrarVentana) {        
            context.visible = mostrarVentana;
            if (context.visible == "List") {
                limpiarFormulario();
                listarOperacion();
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
        function limpiarFormulario() {
            context.eliminar = true;
            context.agregar = true;
            context.operacion = {};
            context.usuarioDestinatarios = [];
            context.usuarioRemitentes = [];
            context.DocumentoDigitalOperacion = {};
            context.referencia = {};
            context.listaReferencia = [];
            context.listDocumentoAdjunto = [];
            context.operacion = {
                TipoDocumento: '02',
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
            listRemitentes = [];
            listDestinatarios = [];
            $('.nav-tabs a[href="#Datos"]').tab('show')
        }
        
        obtenerUsuarioSession();
    }
})();
