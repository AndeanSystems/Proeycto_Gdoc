(function () {
    'use strict';

    angular.module('app').controller('usuario_controller', usuario_controller);
    usuario_controller.$inject = ['$location', 'app_factory'];

    function usuario_controller($location, dataProvider) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.listUsuario = [];
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],

            columnDefs: [
                { field: 'IDUsuario', displayName: 'IDUsuario' },
                { field: 'Personal.NombrePers', displayName: 'Nombres' },
                { field: 'Personal.ApellidoPersonal', displayName: 'Apellidos' },
                { field: 'Personal.IDEmpresa', displayName: 'IDEmpresa' },
                { field: 'TipoUsuario.DescripcionConcepto', displayName: 'Tipo Usuario' },
                { field: 'Cargo.DescripcionConcepto', displayName: 'Cargo' },
                { field: 'Area.DescripcionConcepto', displayName: 'Area' },
                { field: 'Personal.EmailTrabrajo', displayName: 'Email Trabrajo' },
                { field: 'Personal.TelefonoPersonal', displayName: 'Telefono Personal' },
                { field: 'ClaseUsu.DescripcionConcepto', displayName: 'Clase Usuario' },
                { name: 'Acciones', cellTemplate: '<i class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="modal" data-target="#modal_contenido" title="Editar"></i><i class="fa fa-times" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Borrar"></i> ' }

            ],
             multiSelect : false,
             modifierKeysToMultiSelect: false,
             //onRegisterApi : function( gridApi ) {
             //    context.gridApi = gridApi;
             //    gridApi.selection.on.rowSelectionChanged(context, function (row) {
             //        var msg = 'row selected ' + row.isSelected;
             //        console.log(msg);
             //    });
             //}
        };

       
        //context.Usuario = {};
        //Eventos

        //context.grabar = function () {
        //    console.log(context.Usuario);
        //    dataProvider.postData("Concepto/GrabarConcepto", context.Usuario).success(function (respuesta) {
        //        console.log(respuesta);
        //    }).error(function (error) {
        //        //MostrarError();
        //    });
        //}

        //Metodos
        function listarUsuario() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                context.listUsuario = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
        listarUsuario();
    }
})();
