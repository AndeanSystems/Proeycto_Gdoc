(function () {
    'use strict';

    angular.module('app').controller('busqueda_controller', busqueda_controller);
    busqueda_controller.$inject = ['$location', 'app_factory', 'appService'];

    function busqueda_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        let TipoOperacion = "003";
        let TipoDocumento = "012";
        let PrioridadAtencion = "005";
        let TipoAcceso = "002";
        let TipoComunicacion = "022";
        context.busqueda = {};

        LlenarConcepto(TipoOperacion);
        LlenarConcepto(TipoDocumento);

        //var concepto = { TipoConcepto: "012" };
        
        //appService.listarConcepto(concepto).success(function (respuesta) {
        //    console.log(respuesta);
        //});

     
        context.buscarConcepto = function (CodiConcepto, EstadoConcepto) {
            if (CodiConcepto == null) {
                alert("Seleccione el tipo de concepto para la consulta");
            }
            else {
                if (EstadoConcepto == null) {
                    dataProvider.postData("Concepto/BuscarConceptoEstado", { CodiConcepto: CodiConcepto }).success(function (respuesta) {
                        console.log(respuesta);
                        context.concepto = respuesta[0];
                        //if (context.concepto.EditarRegistro == 0) {
                        //    context.acciones = '<i>a</i>';//por terminar
                        //    console.log(context.acciones);
                        //}
                        context.gridOptions.data = respuesta;
                    }).error(function (error) {
                        //MostrarError();
                    });
                }
                else {
                    dataProvider.postData("Concepto/BuscarConceptoEstado", { CodiConcepto: CodiConcepto, EstadoConcepto: EstadoConcepto }).success(function (respuesta) {
                        console.log(respuesta);
                        context.concepto = respuesta[0];
                        //if (context.concepto.EditarRegistro == 0) {
                        //    context.acciones = ' a';
                        //    console.log(context.acciones);
                        //}
                        context.gridOptions.data = respuesta;
                    }).error(function (error) {
                        //MostrarError();
                    });
                }

            }

        }

        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            //multiSelect: false,
            //enableSelectAll: false,
            //modifierKeysToMultiSelect: false,
            //noUnselect: true,
            //enableRowHeaderSelection: false, 
            //enableRowSelection: true, 
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'TipoConcepto', displayName: 'Tipo Concepto' },
                { field: 'CodiConcepto', displayName: 'Codigo' },
                { field: 'DescripcionConcepto', width: '20%', displayName: 'Descripcion' },
                { field: 'DescripcionCorta', displayName: 'Abreviatura' },
                { field: 'ValorUno', width: '8%', displayName: 'Valor1' },
                { field: 'ValorDos', width: '8%', displayName: 'Valor2' },
                { field: 'TextoUno', width: '8%', displayName: 'Texto1' },
                { field: 'TextoDos', width: '8%', displayName: 'Texto2' },
                { field: 'EstadoConcepto', displayName: 'Estado' },
                {
                    name: 'Acciones',
                    cellTemplate: context.acciones
                }
                //{ field: 'Empresa.DireccionEmpresa',displayName:'Direción Empresa' }
            ]
        };
        //Eventos
        //Metodos
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
                else if (concepto.TipoConcepto == TipoOperacion)
                    context.listTipoOperacion = respuesta;
            });
        }
        //Carga
    }
})();
