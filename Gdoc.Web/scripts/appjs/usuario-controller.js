(function () {
    'use strict';

    angular.module('app').controller('usuario_controller', usuario_controller);
    usuario_controller.$inject = ['$location', 'app_factory', 'appService'];

    function usuario_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        
        var context = this;
        //LLENAR CONCEPTOS
        LlenarConcepto("010");
        LlenarConcepto("007");
        LlenarConcepto("013");
        LlenarConcepto("021");
        LlenarConcepto("009");
        LlenarConcepto("015");

        context.personal = {};
        context.usuario = {};
        context.listUsuario = [];
        context.listDepartamento = [];
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

        //Eventos
        context.grabar = function (numeroboton) {
            console.log(context.personal);

            var personal = context.personal;
            var usuario = context.usuario;

            var departamento, provincia, distrito;

            departamento = (context.codigodepartamento < 10) ? "0" + context.codigodepartamento : context.codigodepartamento.toString();
            provincia = (context.codigoprovincia < 10) ? "0" + context.codigoprovincia : context.codigoprovincia.toString();
            distrito = (context.codigodistrito < 10) ? "0" + context.codigodistrito : context.codigodistrito.toString();
            //GRABAR PERSONAL
            if (numeroboton == 1)
                personal.EstadoPersonal = 0
            else if (numeroboton == 2)
                personal.EstadoPersonal = 1
            personal.CodigoUbigeo = (departamento + provincia + distrito);

            dataProvider.postData("Personal/GrabarPersonal", personal).success(function (respuesta) {
                console.log(respuesta);
                //listarUsuario();
                //$("#modal_contenido").modal("hide");
            }).error(function (error) {
                //MostrarError();
            });
            //GRABAR USUARIO
            usuario.NombreUsuario = personal.NombrePers.substr(0,1) + personal.ApellidoPersonal;
            usuario.ClaveUsuario = 123;

            //usuario.IDPersonal = personal.IDPersonal;

            if (numeroboton == 1)
                usuario.EstadoUsuario = 0
            else if (numeroboton == 2)
                usuario.EstadoUsuario = 1
            

            dataProvider.postData("Usuario/GrabarUsuario", usuario).success(function (respuesta) {
                console.log(respuesta);
                listarUsuario();
                context.personal = {};
                context.usuario = {};
                context.listDepartamento = [];
                context.listPronvincia = [];
                context.listDistrito = [];
                $("#modal_contenido").modal("hide");
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.listPronvincia = function (CodigoDepartamento) {
            dataProvider.postData("Ubigeo/ListarProvincias", { CodigoDepartamento: CodigoDepartamento }).success(function (respuesta) {
                console.log(respuesta);
                context.listPronvincia = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.listDistrito = function (CodigoDepartamento, CodigoProvincia) {
            dataProvider.postData("Ubigeo/ListarDistritos", { CodigoDepartamento: CodigoDepartamento, CodigoProvincia: CodigoProvincia }).success(function (respuesta) {
                console.log(respuesta);
                context.listDistrito = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        //Metodos
        function listarUsuario() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                context.listUsuario = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarDepartamento() {
            dataProvider.getData("Ubigeo/ListarDepartamento").success(function (respuesta) {
                context.listDepartamento = respuesta;
            }).error(function (error) {

            });
        }

        function LlenarConcepto(tipoConcepto) {
            var concepto = { TipoConcepto: tipoConcepto };
            appService.listarConcepto(concepto).success(function (respuesta) {
                if (concepto.TipoConcepto == "010")
                    context.listTipoUsuario = respuesta;
                else if (concepto.TipoConcepto == "007")
                    context.listTipoCargo = respuesta;
                else if (concepto.TipoConcepto == "013")
                    context.listArea = respuesta;
                else if (concepto.TipoConcepto == "021")
                    context.listClaseUsuario = respuesta;
                else if (concepto.TipoConcepto == "009")
                    context.listTipoRol = respuesta;
                else if (concepto.TipoConcepto == "015")
                    context.listTipoPersonal = respuesta;
            });
        }
        //Carga
        listarUsuario();
        listarDepartamento();
    }
})();
