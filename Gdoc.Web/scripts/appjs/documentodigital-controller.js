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

        let UsuarioRemitente = "07";//falta
        let UsuarioDestinatario = "08";
        var context = this;
        context.operacion = {};
        context.operacion.FechaEmision = new Date();
        context.operacion.FechaCierre = new Date();
        context.DocumentoDigitaloOperacion = {};
        context.IndexacionDocumento = [];
        context.referencia = {};
        context.visible = "CreateAndEdit";
        context.listaReferencia = [];
        context.listaUsuarioGrupo = [];

        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioRemitentes = [];
        context.usuarioDestinatarios = [];
        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario = {};


        LlenarConcepto(TipoDocumento);
        LlenarConcepto(TipoAcceso);
        LlenarConcepto(PrioridadAtencion);
        //LlenarConcepto(TipoComunicacion);

        //COMIENZO
        context.operacion = {
            TipoDocumento: '02',
            AccesoOperacion: '2',
            PrioridadOperacion: '02',
            TipoComunicacion: '1',
            FechaEmision: new Date(),
            FechaCierre: new Date()
        };
        context.DocumentoDigitaloOperacion = {
            DerivarDocto: 'S'
        };
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,//FILTRO
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
                    cellTemplate: '<i style="padding: 4px;font-size: 1.4em;" class="fa fa-eye" data-placement="top" data-toggle="tooltip" title="Ver"></i>' +
                                '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                '<i ng-click="grid.appScope.eliminarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-times" data-placement="top" data-toggle="tooltip" title="" data-original-title="Borrar"></i>'
                }
            ]
        };
        //Eventos
        context.agregarreferencia = function (referencia) {
            if (context.referencia.DescripcionIndice == undefined)
                alert("Ingrese Referencia");
            else {
                context.listaReferencia.push(context.referencia);
                console.log(context.listaReferencia);
                context.referencia = {};
            }
        }

        context.eliminarreferencia = function (indexReferencia) {
            context.listaReferencia.splice(indexReferencia,1);
        }
        context.ObtenerImagen = function (element) {
            console.log(element);
        }
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

            let Operacion = context.operacion;
            let DocumentoDigitalOperacion = context.DocumentoDigitaloOperacion;
            let listIndexacionDocumento = context.listaReferencia;

            let listEUsuarioGrupo = [];
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
            else if (numeroboton == 2)
                Operacion.EstadoOperacion = 1

            console.log(listIndexacionDocumento);
            console.log(listEUsuarioGrupo);
            var listDocumentoDigitaloOperacion = [];

            console.log(archivosSelecionados);
            for (var index in archivosSelecionados) {
                listDocumentoDigitaloOperacion.push({
                    RutaFisica: archivosSelecionados[index].RutaBinaria,
                    NombreOriginal: archivosSelecionados[index].NombreArchivo,
                    TamanoDocto: archivosSelecionados[index].TamanoArchivo,
                    TipoArchivo: archivosSelecionados[index].TipoArchivo,
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
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);
                      
        }

        context.editarOperacion = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            context.DocumentoDigitaloOperacion = context.operacion.DocumentoDigitalOperacion;
            //context.codigodepartamento = parseInt(context.empresa.CodigoUbigeo.substring(0, 2));
            context.operacion.AccesoOperacion = context.operacion.AccesoOperacion.substring(0,1)
            console.log(context.operacion);
            console.log(context.DocumentoDigitaloOperacion);
            context.CambiarVentana('CreateAndEdit');
        };

        context.eliminarOperacion = function (rowIndex) {
            var empresa = context.gridOptions.data[rowIndex];
            dataProvider.postData("DocumentoDigital/EliminarOperacion", empresa).success(function (respuesta) {
                console.log(respuesta);
                listarOperacion();
            }).error(function (error) {
                //MostrarError();
            });
        };

        context.CambiarVentana = function (mostrarVentana) {
            limpiarFormulario();
            context.visible = mostrarVentana;
            if (context.visible == "List") {
                listarOperacion();
            }
        }
        ////
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
            });
        }

        function listarOperacion() {
            dataProvider.getData("DocumentoDigital/ListarOperacion").success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function limpiarFormulario() {
            context.operacion = {};
            context.usuarioDestinatarios = [];
            context.usuarioRemitentes = [];
            context.DocumentoDigitaloOperacion = {};
            context.referencia = {};
            context.listaReferencia = [];
            context.operacion = {
                TipoDocumento: '02',
                AccesoOperacion: '2',
                PrioridadOperacion: '02',
                TipoComunicacion: '1',
                FechaEmision: new Date(),
                FechaCierre: new Date()
            };
            context.DocumentoDigitaloOperacion = {
                DerivarDocto: 'S'
            };
            obtenerUsuarioSession();
            archivosSelecionados = [];
        }
        
        obtenerUsuarioSession();
    }
})();
