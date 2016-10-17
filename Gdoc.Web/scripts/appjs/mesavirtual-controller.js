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
        context.listDocumentoAdjunto = [];
        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioInvitados = [];
        context.usuarioOrganizador = [];
        context.filterSelected = true;
        context.querySearch = querySearch;
        var listDocumentosAdjuntos = [];
        var listOrganizador = [];
        var listInvitados = [];
        let listEUsuarioGrupo = [];
        //ng-visible
        context.mostrar = false;
        context.eliminar = true;
        context.agregar = true;

        //LlenarConcepto(TipoDocumento);
        LlenarConcepto(TipoAcceso);
        //LlenarConcepto(TipoMesaVirtual);
        LlenarConcepto(PrioridadAtencion);
        LlenarConcepto(Estado);

        //COMIENZO
        context.operacion = {
            TipoDocumento: '81',
            PrioridadOperacion: '02',
            AccesoOperacion: '2',
            NotificacionOperacion: 'S',
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
            enableSorting: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                {
                    name: 'Acciones', width: '7%',
                    cellTemplate: '<i ng-click="grid.appScope.editarMiOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="bottom" data-toggle="tooltip" title="Editar"></i>' +
                                '<i ng-click="grid.appScope.eliminarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-times" data-placement="top" data-toggle="tooltip" title="" data-original-title="Cerrar"></i>'
                },
                { field: 'NumeroOperacion', width: '15%', displayName: 'Nº Documento' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha Registro', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'TipoDoc.DescripcionCorta', width: '7%', displayName: 'Tipo' },
                { field: 'TituloOperacion', width: '53%', displayName: 'Titulo' },
                { field: 'Estado.DescripcionConcepto', width: '8%', displayName: 'Estado' }
                
            ]
        };
        //Eventos
        //Mantenimiento
        context.editarMiOperacion = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            console.log(context.operacion);
            context.operacion.NotificacionOperacion = context.operacion.NotificacionOperacion.substring(0, 1);
            context.operacion.AccesoOperacion = context.operacion.AccesoOperacion.substring(0, 1);
            context.operacion.EstadoOperacion = context.operacion.EstadoOperacion.toString();
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            context.operacion.FechaRegistro = appService.setFormatDate(context.operacion.FechaRegistro);
            context.operacion.FechaVigente = appService.setFormatDate(context.operacion.FechaVigente);
            //context.operacion.FechaCierre = appService.setFormatDate(context.operacion.FechaCierre);
            listarAdjuntos(context.operacion);
            if (context.operacion.EstadoOperacion == 1) {
                context.eliminar = false;
                context.agregar = false;
                context.mostrar = true;
            }
            ObtenerUsuariosParticipantes(context.operacion)
            context.usuarioOrganizador = listOrganizador;
            context.usuarioInvitados = listInvitados;
            if (context.operacion.EstadoOperacion == 1)
                context.CambiarVentana('Commentario');
            else
                context.CambiarVentana('CreateAndEdit');
        }
        context.eliminarOperacion = function (rowIndex) {
            var operacion = context.gridOptions.data[rowIndex];
            operacion.FechaEnvio = appService.setFormatDate(operacion.FechaEnvio);
            //operacion.FechaCierre = appService.setFormatDate(operacion.FechaCierre);
            operacion.FechaRegistro = appService.setFormatDate(operacion.FechaRegistro);
            operacion.FechaVigente = appService.setFormatDate(operacion.FechaVigente);
            console.log(operacion);


            function enviarFomularioOK() {
                dataProvider.postData("MesaVirtual/EliminarOperacion", operacion).success(function (respuesta) {
                    console.log(respuesta);
                    appService.mostrarAlerta("Información", "Grupo de Trabajo Cerrado", "warning")
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
                console.log(listDocumentosAdjuntos);
            }

            function enviarFomularioOK() {
                dataProvider.postData("MesaVirtual/Grabar", { Operacion: Operacion, listAdjuntos: listDocumentosAdjuntos, listEUsuarioGrupo: listEUsuarioGrupo }).success(function (respuesta) {
                    if (respuesta.Exitoso)
                        TipoMensaje = "success";
                    else
                        TipoMensaje = "error";
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
                //limpiarFormulario()
            }
            else if (context.visible == "Commentario") {
                listarComentarioMesaVirtual(context.operacion);
            }
        }
        function LlenarConceptoTipoDocumento() {
            var concepto = { TipoConcepto: TipoDocumento, TextoUno: "MV" }
            dataProvider.postData("Concepto/ListarConceptoTipoDocumento", concepto).success(function (respuesta) {
                console.log(respuesta);
                context.listTipoDocumento = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Adjuntos
        context.agregaradjunto = function () {
            for (var ind in archivosSelecionados) {
                var hola = true;
                console.log(archivosSelecionados[ind].NombreArchivo);
                for (var index in context.listDocumentoAdjunto) {
                    if (archivosSelecionados[ind].NombreArchivo == context.listDocumentoAdjunto[index].NombreOriginal) {
                        hola = false;
                    }
                }
                if (hola == true) {
                    context.listDocumentoAdjunto.push({
                        RutaArchivo: archivosSelecionados[ind].RutaBinaria,
                        NombreOriginal: archivosSelecionados[ind].NombreArchivo,
                        TamanoArchivo: archivosSelecionados[ind].TamanoArchivo,
                        TipoArchivo: archivosSelecionados[ind].TipoArchivo,
                        EstadoAdjunto: 0
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
        function listarAdjuntos(operacion) {
            dataProvider.postData("DocumentosRecibidos/ListarAdjunto", operacion).success(function (respuesta) {
                context.listDocumentoAdjunto = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
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
        context.gridMesaTrabajo = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,
            enableSorting: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                {
                    name: 'Acciones', width: '6%',
                    cellTemplate: '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-pencil-square-o" data-placement="bottom" data-toggle="tooltip" title="Editar"></i>'
                },
                { field: 'NumeroOperacion', width: '15%', displayName: 'Nº Documento' },
                { field: 'OrganizadorMV', width: '10%', displayName: 'Organizador' },
                { field: 'TipoDoc.DescripcionCorta', width: '41%', displayName: 'Tipo' },
                { field: 'TituloOperacion', width: '9%', displayName: 'Titulo' },
                { field: 'FechaRegistro', width: '10%', displayName: 'Fecha Emisión', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'Prioridad.DescripcionCorta', width: '9%', displayName: 'Prioridad' }
                
            ]
        };

        context.gridComentarios = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'FechaPublicacion', width: '9%', displayName: 'Fecha', type: 'date', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm"' },
                { field: 'Usuario.NombreUsuario', width: '10%', displayName: 'Participante' },
                { field: 'ComentarioMesaVirtual', width: '74%', displayName: 'Comentario' },
                {
                    name: 'Adjuntos', width: '7%',
                    cellTemplate: '<i ng-click="grid.appScope.mostrarAdjuntos(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-paperclip" style="padding: 4px;font-size: 1.4em;" data-placement="bottom" data-toggle="tooltip" title="Ver"></i>'
                }
            ]
        };
        //Adjuntos Mesa Virtual
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
            dataProvider.postData("DocumentosRecibidos/ListarAdjuntos", archivo).success(function (respuesta) {
                console.log(respuesta)
                window.open(respuesta, "mywin", "resizable=1");
                //window.open(respuesta, '_blank');
            }).error(function (error) {
                //MostrarError();
            });
        }
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
        context.editarOperacion = function (rowIndex) {

            if (context.gridMesaTrabajo.data[rowIndex] == undefined || context.gridMesaTrabajo.data[rowIndex] == null)
                context.operacion = context.gridOptions.data[rowIndex];
            else
                context.operacion = context.gridMesaTrabajo.data[rowIndex];

            console.log(context.operacion);
            context.operacion.FechaEnvio = appService.setFormatDate(context.operacion.FechaEnvio);
            //context.operacion.FechaCierre = appService.setFormatDate(context.operacion.FechaCierre);
            listarDocumentoAdjuntoMesa(context.operacion);
            ObtenerUsuariosParticipantes(context.operacion)
            context.usuarioOrganizador = listOrganizador;
            context.usuarioInvitados = listInvitados;
            context.CambiarVentana2('Commentario');
        }
        //Mantenimiento Comentario
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
                dataProvider.postData("MesaVirtual/GrabarMesaVirtualComentario", { Operacion: Operacion, listAdjuntos: listDocumentosAdjuntos, mesaVirtualComentario: MesaVirtualComentario }).success(function (respuesta) {
                    //if (respuesta.Exitoso)
                    //    TipoMensaje = "success";
                    //appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);

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
        //Metodos
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
        function listarDocumentoAdjunto(mesaVirtualComentario) {
            dataProvider.postData("MesaVirtual/ListarAdjuntoComentario", mesaVirtualComentario).success(function (respuesta) {
                context.listDocumentoAdjunto = respuesta;
            }).error(function (error) {
                //MostrarError();
            });

        }
        function listarDocumentoAdjuntoMesa(operacion) {
            dataProvider.postData("MesaVirtual/ListarAdjuntoOperacion", operacion).success(function (respuesta) {
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
                    if (respuesta[ind].TipoParticipante == UsuarioOrganizado)
                        listOrganizador.push(respuesta[ind]);
                    else
                        listInvitados.push(respuesta[ind]);
                }
                console.log(listInvitados);
                console.log(listOrganizador);
            }).error(function (error) {
                //MostrarError();
            });

        }
        function limpiarFormulario() {
            context.eliminar = true;
            context.agregar = true;
            context.mostrar = false;
            context.operacion = {};
            context.mesavirtualComentario = {};
            context.usuarioInvitados = [];
            context.usuarioOrganizador = [];
            context.listDocumentoAdjunto = [];
            context.operacion = {
                TipoDocumento: '81',
                PrioridadOperacion: '02',
                AccesoOperacion: '2',
                NotificacionOperacion: 'S',
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
            listEUsuarioGrupo = [];
            archivosSelecionados = [];
            listDocumentosAdjuntos = [];
            listOrganizador = [];
            listInvitados = [];
        }
        listarMesaTrabajo();
        obtenerUsuarioSession();
        LlenarConceptoTipoDocumento();
    }
})();
