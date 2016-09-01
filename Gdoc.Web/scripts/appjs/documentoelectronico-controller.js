(function () {
    'use strict';

    angular.module('app').controller('documentoelectronico_controller', documentoelectronico_controller);
    documentoelectronico_controller.$inject = ['$location', 'app_factory', 'appService'];

    function documentoelectronico_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        LlenarConcepto("012");
        LlenarConcepto("005");
        LlenarConcepto("002");

        //appService.listarConcepto(concepto).success(function (respuesta) {
        //    context.listTipoDocumento = respuesta;

        //});
        context.operacion = {};

        function LlenarConcepto(tipoConcepto) {
            var concepto = { TipoConcepto: tipoConcepto };
            appService.listarConcepto(concepto).success(function (respuesta) {
                if (concepto.TipoConcepto == "012")
                    context.listTipoDocumento = respuesta;
                else if(concepto.TipoConcepto == "005")
                    context.listPrioridadAtencion = respuesta;
                else if (concepto.TipoConcepto == "002")
                    context.listTipoAcceso = respuesta;
            });
        }
        
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
        //context.grabar = function () {
        //    console.log(context.concepto);
        //    dataProvider.postData("Concepto/GrabarConcepto", context.concepto).success(function (respuesta) {
        //        console.log(respuesta);
        //    }).error(function (error) {
        //        //MostrarError();
        //    });
        //}

        ////Metodos
        //function listarConcepto() {
        //    dataProvider.getData("Concepto/ListarConcepto").success(function (respuesta) {
        //        context.gridOptions.data = respuesta;
        //    }).error(function (error) {
        //        //MostrarError();
        //    });
        //}
        ////Carga
        //listarConcepto();
        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;

        context.usuarioRemitentes = [];
        context.usuarioDestinatarios = [];

        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario= {};
        appService.listarUsuario(usuario).success(function (respuesta) {
            //context.listaUsuario = respuesta;
            context.listaUsuario = respuesta;
        });
        function querySearch(criteria) {
            cachedQuery = cachedQuery || criteria;
            return cachedQuery ? context.listaUsuario.filter(createFilterFor(cachedQuery)) : [];
        }
        function createFilterFor(query) {
            var uppercaseQuery = angular.uppercase(query);
            return function filterFn(usuario) {
                return (usuario.NombreUsuario.indexOf(uppercaseQuery) != -1);;
            };
        }
    }
})();
