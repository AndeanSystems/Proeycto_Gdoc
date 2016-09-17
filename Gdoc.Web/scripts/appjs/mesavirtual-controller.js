(function () {
    'use strict';

    angular.module('app').controller('mesavirtual_controller', mesavirtual_controller)

    .config(function ($mdDateLocaleProvider) {
        $mdDateLocaleProvider.formatDate = function (date) {
            return moment(date).format('DD/MM/YYYY');
        };
    });
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
        context.visible = "List";
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
            //TipoDocumento: '02',
            PrioridadOperacion: '02',
            AccesoOperacion: '2',
            NotificacionOperacion: '0'
        };

        //context.gridOptions = {
        //    paginationPageSizes: [25, 50, 75],
        //    paginationPageSize: 25,
        //    enableFiltering: true,
        //    data: [],
        //    columnDefs: [
        //        { field: 'CodiConcepto', displayName: 'Codigo' },
        //        { field: 'DescripcionConcepto', displayName: 'Descripcion' },
        //        { field: 'DescripcionCorta', displayName: 'Abreviatura' },
        //        { field: 'ValorUno', displayName: 'Valor1' },
        //        { field: 'ValorDos', displayName: 'Valor2' },
        //        { field: 'TextoUno', displayName: 'Texto1' },
        //        { field: 'TextoDos', displayName: 'Texto2' },
        //        { field: 'EstadoConcepto', displayName: 'Estado' },
        //        { field: 'Empresa.DireccionEmpresa', displayName: 'Direción Empresa' }
        //    ]
        //};
        //Eventos
        context.grabar = function (numeroboton) {
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

            console.log(context.DocumentoElectronicoOperacion);
            dataProvider.postData("MesaVirtual/Grabar", { Operacion: Operacion, listEUsuarioGrupo: listEUsuarioGrupo }).success(function (respuesta) {
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
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
    }
})();
