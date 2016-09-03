(function () {
    'use strict';

    angular.module('app').controller('alerta_controller', alerta_controller);
    alerta_controller.$inject = ['$location', 'app_factory', 'appService'];

    function alerta_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.alerta = {};

        //Eventos
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'FechaAlerta', displayName: 'Fecha Emision', type: 'date', cellFilter: 'toDateTime | date:"mediumDate"' },
                { field: 'TipoOperacion.DescripcionConcepto', displayName: 'Tipo Operacion' },
                { field: 'Operacion.NumeroOperacion', displayName: 'Numero Operacion' },
                { field: 'TipoDocumento.DescripcionConcepto', displayName: 'Tipo Documento' },
                { field: 'Usuario.NombreUsuario', displayName: 'Remitente' },
                {
                    name: 'Acciones',
                    cellTemplate: '<i class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Editar"></i>' 
                }

            ],

        };

        //Metodos
        function listarMensajeAlerta() {
            dataProvider.getData("Alertas/ListarMensajeAlerta").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                //context.listEmpresa = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        ////Carga
        listarMensajeAlerta();
    }
})();
