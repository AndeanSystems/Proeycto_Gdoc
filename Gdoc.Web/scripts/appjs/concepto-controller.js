(function () {
    'use strict';

    angular.module('app').controller('concepto_controller', concepto_controller);
    concepto_controller.$inject = ['$location', 'app_factory', 'appService'];

    function concepto_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;

        LlenarConcepto("999");

        var concepto = { TipoConcepto: "012" };
        appService.listarConcepto(concepto).success(function (respuesta) {
            console.log(respuesta);
        });
        context.concepto = {};
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            multiSelect: false,
            enableSelectAll: false,
            modifierKeysToMultiSelect: false,
            noUnselect: true,
            enableRowHeaderSelection: false, 
            enableRowSelection: true, 
            data: [],
            columnDefs: [
                { field: 'TipoConcepto', displayName: 'Tipo Concepto' },
                { field: 'CodiConcepto', displayName: 'Codigo' },
                { field: 'DescripcionConcepto', width: '20%', displayName: 'Descripcion' },
                { field: 'DescripcionCorta', displayName: 'Abreviatura' },
                { field: 'ValorUno', width: '8%', displayName: 'Valor1' },
                { field: 'ValorDos', width: '8%', displayName: 'Valor2' },
                { field: 'TextoUno', width: '8%', displayName: 'Texto1' },
                { field: 'TextoDos', width: '8%', displayName: 'Texto2' },
                { field: 'EstadoConcepto', displayName: 'Estado' }
                //{ field: 'Empresa.DireccionEmpresa',displayName:'Direción Empresa' }
            ]
        };
        //Eventos
        context.grabar = function (numeroboton) {

            var concepto = context.concepto;

            if (numeroboton == 1)
                concepto.EstadoConcepto = 0
            else if (numeroboton == 2)
                concepto.EstadoConcepto = 1

            console.log(context.concepto);

            dataProvider.postData("Concepto/GrabarConcepto", context.concepto).success(function (respuesta) {
                console.log(respuesta);
                listarConcepto();
                context.concepto = {};
                $("#modal_contenido").modal("hide");
            }).error(function (error) {
                //MostrarError();
            });
        }

        //Metodos
        function listarConcepto() {
            dataProvider.getData("Concepto/ListarConcepto").success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        function LlenarConcepto(tipoConcepto) {
            var concepto = { TipoConcepto: tipoConcepto };
            appService.listarConcepto(concepto).success(function (respuesta) {
                if (concepto.TipoConcepto == "999")
                    context.listTipoConceptoTitulo = respuesta;
            });
        }
        //Carga
        listarConcepto();
    }
})();
