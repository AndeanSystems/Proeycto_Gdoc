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
        let Estado = "001";

        let UsuarioOrganizado = "05";
        let UsuarioInvitado = "02";
        var context = this;
        context.operacion = {};
        context.mesavirtualComentario = {};
        context.visible = "CreateAndEdit";
        context.visible2 = "ListWork";
        context.listaUsuarioGrupo = [];

        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioInvitados = [];
        context.usuarioOrganizador = [];
        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario = {};
        var listDocumentosAdjuntos = [];

        var listOrganizador = [];
        var listInvitados = [];

        LlenarConcepto(TipoDocumento);
        LlenarConcepto(TipoAcceso);
        LlenarConcepto(TipoMesaVirtual);
        LlenarConcepto(PrioridadAtencion);
        LlenarConcepto(Estado);

        var hoy = new Date()
        
        //COMIENZO
        context.operacion = {
            TipoDocumento: '90',
            PrioridadOperacion: '02',
            AccesoOperacion: '2',
            NotificacionOperacion: '0',
            EstadoOperacion: '0',
            FechaVigente: new Date(),
            //FechaCierre: new Date(),
            FechaRegistro: new Date(),
            FechaEnvio: new Date()
        };

        //MESA VIRTUAL
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NumeroOperacion', width:'15%', displayName: 'Nº Documento' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha Registro', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'TipoDoc.DescripcionCorta', width: '7%', displayName: 'Tipo' },
                { field: 'TituloOperacion', width: '55%', displayName: 'Titulo' },
                { field: 'Estado.DescripcionConcepto', width: '8%', displayName: 'Estado' },
                {
                    name: 'Acciones', width: '5%',
                    cellTemplate: '<i ng-click="grid.appScope.editarMiOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="bottom" data-toggle="tooltip" title="Editar"></i>' +
                                '<i ng-click="grid.appScope.eliminarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-times" data-placement="top" data-toggle="tooltip" title="" data-original-title="Cerrar"></i>'
                }
            ]
        };
        //Eventos
        context.editarMiOperacion = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            console.log(context.operacion);
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            //context.operacion.FechaCierre = appService.setFormatDate(context.operacion.FechaCierre);

            ObtenerUsuariosParticipantes(context.operacion)
            context.usuarioOrganizador = listOrganizador;
            context.usuarioInvitados = listInvitados;
            context.CambiarVentana('Commentario');
        }
        context.eliminarOperacion = function (rowIndex) {
            var operacion = context.gridOptions.data[rowIndex];

            operacion.FechaEnvio = appService.setFormatDate(operacion.FechaEnvio);
            //operacion.FechaCierre = appService.setFormatDate(operacion.FechaCierre);
            operacion.FechaRegistro = appService.setFormatDate(operacion.FechaRegistro);
            operacion.FechaVigente = appService.setFormatDate(operacion.FechaVigente);

            console.log(operacion);


            dataProvider.postData("MesaVirtual/EliminarOperacion", operacion).success(function (respuesta) {
                console.log(respuesta);
                appService.mostrarAlerta("Información","Grupo de Trabajo Cerrado","warning")
                listarOperacion();
            }).error(function (error) {
                //MostrarError();
            });
        };
        context.grabar = function (numeroboton) {

            let Operacion = context.operacion;
            if (Operacion.EstadoOperacion == "ACTIVO") {
                return appService.mostrarAlerta("No se puede modificar Documento", "El documento ya ha sido enviado", "warning");
            }
            //if (archivosSelecionados == undefined || archivosSelecionados == "" || archivosSelecionados == null) {
            //    return appService.mostrarAlerta("Advertencia", "Debe seleccionar por lo menos un archivo", "warning");
            //}
            if (context.usuarioOrganizador == undefined || context.usuarioOrganizador == "") {
                return appService.mostrarAlerta("Falta el Organizador", "Agregue al Organizador", "warning");
            }
            if (context.usuarioInvitados == undefined || context.usuarioInvitados == "") {
                return appService.mostrarAlerta("Falta los Invitados", "Agregue a los invitados", "warning");
            }
            console.log(context.operacion);
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
                dataProvider.postData("MesaVirtual/Grabar", { Operacion: Operacion, listAdjuntos: listDocumentosAdjuntos, listEUsuarioGrupo: listEUsuarioGrupo }).success(function (respuesta) {
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);
                    console.log(respuesta);
                }).error(function (error) {
                    //MostrarError();
                });
                limpiarFormulario();
                document.getElementById("input_file").value = "";
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

        context.CambiarVentana = function (mostrarVentana) {
            context.visible = mostrarVentana;
            if (context.visible == "List") {
                limpiarFormulario();
                listarOperacion();
            }
            else if (context.visible == "CreateAndEdit") {
                limpiarFormulario()
            }
            else if (context.visible == "Commentario") {
                listarComentarioMesaVirtual(context.operacion);
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
        function obtenerUsuarioSession() {
            var usuarioGrupo = { IDUsuarioGrupo: appService.obtenerUsuarioId() };
            appService.buscarUsuarioGrupoAutoComplete(usuarioGrupo).success(function (respuesta) {
                context.usuarioOrganizador = respuesta;
            });
        }
        function querySearch(criteria, tipo) {
            listarUsuarioGrupoAutoComplete(criteria, tipo);
            cachedQuery = cachedQuery || criteria;
            return cachedQuery ? context.listaUsuarioGrupo : [];
        }
        function listarOperacion() {
            dataProvider.getData("MesaVirtual/ListarOperacion").success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        //MESA VIRTUAL COMENTARIO
        context.mostrarAdjuntos = function (rowIndex) {
            context.mesavirtualComentario = context.gridComentarios.data[rowIndex];
            listarDocumentoAdjunto(context.mesavirtualComentario)
            context.mesavirtualComentario.ComentarioMesaVirtual = "";
            //if (context.listDocumentoAdjunto == undefined || context.listDocumentoAdjunto == undefined) {
            //    appService.mostrarAlerta("Informacion", "No tiene Documentos Adjuntos", "warning")
            //    return;
            //}
            //else {
                
                listarComentarioMesaVirtual(context.operacion);
                $("#modal_adjuntos").modal("show");
            //}
                
            
            
        }
        context.mostrarAdjuntosMesa = function (operacion) {

            listarDocumentoAdjuntoMesa(operacion);
            $("#modal_adjuntos").modal("show");
        }

        context.mostrarAdjuntoWindows = function (archivo) {
            console.log(archivo);
            window.open("http://192.168.100.29:85/ADJUNTOS/"+archivo, "mywin", "resizable=0");
        }
        context.editarOperacion = function (rowIndex) {

            if (context.gridMesaTrabajo.data[rowIndex] == undefined || context.gridMesaTrabajo.data[rowIndex] == null)
                context.operacion = context.gridOptions.data[rowIndex];
            else
                context.operacion = context.gridMesaTrabajo.data[rowIndex];

            console.log(context.operacion);
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            //context.operacion.FechaCierre = appService.setFormatDate(context.operacion.FechaCierre);

            ObtenerUsuariosParticipantes(context.operacion)
            context.usuarioOrganizador = listOrganizador;
            context.usuarioInvitados = listInvitados;
            context.CambiarVentana2('Commentario');
        }

        context.gridMesaTrabajo = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NumeroOperacion', width: '15%', displayName: 'Nº Documento' },
                { field: 'OrganizadorMV', width: '10%', displayName: 'Organizador' },
                { field: 'TipoDoc.DescripcionCorta', width: '41%', displayName: 'Tipo' },
                { field: 'TituloOperacion', width: '9%', displayName: 'Titulo' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha Emisión', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'Prioridad.DescripcionCorta', width: '9%', displayName: 'Prioridad' },
                {
                    name: 'Acciones', width: '6%',
                    cellTemplate: '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="bottom" data-toggle="tooltip" title="Editar"></i>'
                }
            ]
        };

        context.gridComentarios = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'FechaPublicacion', width:'9%', displayName: 'Fecha', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm"' },
                { field: 'Usuario.NombreUsuario', width: '10%', displayName: 'Participante' },
                { field: 'ComentarioMesaVirtual', width: '74%', displayName: 'Comentario' },
                {
                    name: 'Adjuntos', width: '7%',
                    cellTemplate: '<i ng-click="grid.appScope.mostrarAdjuntos(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-paperclip" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Ver"></i>'
                }
            ]
        };
        context.CambiarVentana2 = function (mostrarVentana) {
            context.visible2 = mostrarVentana;
            if (context.visible2 == "ListWork") {
                limpiarFormulario();
                listarMesaTrabajo();
            }
            else if (context.visible2 == "Commentario") {
                listarComentarioMesaVirtual(context.operacion);
            }
        }

        context.recargarComentario = function () {
            listarComentarioMesaVirtual(context.operacion);
        }
        context.grabarComentarioMesa = function () {
            let Operacion = context.operacion;
            let MesaVirtualComentario = context.mesavirtualComentario;

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
                dataProvider.postData("MesaVirtual/GrabarMesaVirtualComentario", { Operacion: Operacion, listAdjuntos: listDocumentosAdjuntos,mesaVirtualComentario:MesaVirtualComentario }).success(function (respuesta) {
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);

                    listarComentarioMesaVirtual(context.operacion);
                    console.log(respuesta);
                }).error(function (error) {
                    //MostrarError();
                });

                context.mesavirtualComentario = {};
                document.getElementById("input_file2").value = "";
            }
            appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);
        }
        
        function listarMesaTrabajo() {
            dataProvider.getData("MesaVirtual/ListarMesaTrabajoVirtual").success(function (respuesta) {
                context.gridMesaTrabajo.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
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
                else if (concepto.TipoConcepto == Estado)
                    context.listEstado = respuesta;
            });
        }
        function listarDocumentoAdjunto(mesavirtualcomentario) {
            dataProvider.postData("MesaVirtual/ListarDocumentoAdjunto", mesavirtualcomentario).success(function (respuesta) {
                context.listDocumentoAdjunto = respuesta;
                console.log(respuesta[0].IDAdjunto);
            }).error(function (error) {
                //MostrarError();
            });

        }
        function listarDocumentoAdjuntoMesa(operacion) {
            dataProvider.postData("MesaVirtual/ListarDocumentoAdjuntoOperacion", operacion).success(function (respuesta) {
                console.log(respuesta);
                context.listDocumentoAdjunto = respuesta;
                //context.gridDocumentosAdjuntos.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        function listarComentarioMesaVirtual(operacion) {
            dataProvider.postData("MesaVirtual/ListarComentarioMesaVirtual", operacion).success(function (respuesta) {
                context.gridComentarios.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }

        function ObtenerUsuariosParticipantes(operacion) {
            dataProvider.postData("MesaVirtual/ListarUsuarioParticipanteMV", operacion).success(function (respuesta) {
                console.log(respuesta);
                for (var ind in respuesta) {
                    respuesta[ind].Usuario.Nombre = respuesta[ind].Usuario.NombreUsuario;
                    if (respuesta[ind].TipoParticipante == UsuarioOrganizado)
                        listOrganizador.push(respuesta[ind].Usuario.Nombre);
                    else
                        listInvitados.push(respuesta[ind].Usuario.Nombre);
                }
                console.log(listInvitados);
                console.log(listOrganizador);
            }).error(function (error) {
                //MostrarError();
            });

        }
        function limpiarFormulario() {
            context.operacion = {};
            context.mesavirtualComentario = {};
            context.usuarioInvitados = [];
            context.usuarioOrganizador = [];
            context.operacion = {
                TipoDocumento: '90',
                PrioridadOperacion: '02',
                AccesoOperacion: '2',
                NotificacionOperacion: '0',
                EstadoOperacion: '0',
                FechaVigente: new Date(),
                //FechaCierre: new Date(),
                FechaRegistro: new Date(),
                FechaEnvio: new Date()
            };
            archivosSelecionados = [];
            //document.getElementById("input_file2").value = "";
            //document.getElementById("input_file").value = "";
            obtenerUsuarioSession();
            archivosSelecionados = [];
            listDocumentosAdjuntos = [];
            listOrganizador = [];
            listInvitados = [];
        }
        listarMesaTrabajo();
        obtenerUsuarioSession();
    }
})();
