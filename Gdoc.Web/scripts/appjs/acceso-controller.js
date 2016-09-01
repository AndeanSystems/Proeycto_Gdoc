(function () {
    'use strict';

    angular.module('app').controller('acceso_controller', acceso_controller);
    acceso_controller.$inject = ['$location', 'app_factory', 'appService'];

    function acceso_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables

        var context = this;

        context.usuario = {};
        context.listUsuario = [];
        context.gridUsuarios = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],

            columnDefs: [
                { field: 'IDUsuario', displayName: 'IDUsuario' },
                { field: 'NombreUsuario', displayName: 'Usuario' },
                { field: 'RazoSocial.RazonSocial', displayName: 'Empresa' },
                { field: 'Cargo.DescripcionConcepto', displayName: 'Cargo' },
                { field: 'Area.DescripcionConcepto', displayName: 'Area' },
                { name: 'Acciones', cellTemplate: '<i class="fa fa-pencil-square-o  " style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Permisos"></i>' }

            ],
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            //onRegisterApi : function( gridApi ) {
            //    context.gridApi = gridApi;
            //    gridApi.selection.on.rowSelectionChanged(context, function (row) {
            //        var msg = 'row selected ' + row.isSelected;
            //        console.log(msg);
            //    });
            //}
        };

        //Eventos
        //context.grabar = function (numeroboton) {
        //    console.log(context.personal);

        //    var personal = context.personal;
        //    var usuario = context.usuario;

        //    var departamento, provincia, distrito;

        //    departamento = (context.codigodepartamento < 10) ? "0" + context.codigodepartamento : context.codigodepartamento.toString();
        //    provincia = (context.codigoprovincia < 10) ? "0" + context.codigoprovincia : context.codigoprovincia.toString();
        //    distrito = (context.codigodistrito < 10) ? "0" + context.codigodistrito : context.codigodistrito.toString();
        //    //GRABAR PERSONAL
        //    if (numeroboton == 1)
        //        personal.EstadoPersonal = 0
        //    else if (numeroboton == 2)
        //        personal.EstadoPersonal = 1
        //    personal.CodigoUbigeo = (departamento + provincia + distrito);

        //    dataProvider.postData("Personal/GrabarPersonal", personal).success(function (respuesta) {
        //        console.log(respuesta);
        //        //listarUsuario();
        //        //$("#modal_contenido").modal("hide");
        //    }).error(function (error) {
        //        //MostrarError();
        //    });
        //    //GRABAR USUARIO
        //    usuario.NombreUsuario = personal.NombrePers.substr(0, 1) + personal.ApellidoPersonal;
        //    usuario.ClaveUsuario = 123;
        //    usuario.IDPersonal = personal.IDPersonal;
        //    if (numeroboton == 1)
        //        usuario.EstadoUsuario = 0
        //    else if (numeroboton == 2)
        //        usuario.EstadoUsuario = 1


        //    dataProvider.postData("Usuario/GrabarUsuario", usuario).success(function (respuesta) {
        //        console.log(respuesta);
        //        listarUsuario();
        //        $("#modal_contenido").modal("hide");
        //    }).error(function (error) {
        //        //MostrarError();
        //    });
        //}


        //Metodos
        function listarUsuario() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.gridUsuarios.data = respuesta;
                context.listUsuario = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }


        
        //Carga
        listarUsuario();
    }
})();