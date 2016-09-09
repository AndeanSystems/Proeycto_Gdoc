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
        LlenarConcepto("024");

        context.personal = {};
        context.usuario = {};
        context.listDepartamento = [];

        context.usuario.ExpiraFirma=0;

        //var NombreCompleto = "Personal.NombrePers".concat(" Personal.ApellidoPersonal");

        context.editarUsuario = function (rowIndex) {
            context.usuario = context.gridOptions.data[rowIndex];
            console.log(context.usuario);
            console.log(context.usuario.Personal.NombrePers);
            $("#modal_contenido").modal("show");
        };

        context.buscarUsuarioPersonal = function (user,documento,idetificacion) {
            
            dataProvider.postData("Usuario/BuscarUsuarioPersonal", { NombreUsuario: user }, { NumeroIdentificacion: documento }, { TipoIdentificacion: idetificacion }).success(function (respuesta) {
                console.log(respuesta);
                context.usuario = respuesta[0];
                console.log(context.usuario)
            }).error(function (error) {
                //MostrarError();
            });
        };

        context.eliminarUsuario = function (rowIndex) {
            var usuario = context.gridOptions.data[rowIndex];
            dataProvider.postData("Usuario/EliminarUsuario", usuario).success(function (respuesta) {
                console.log(usuario);
                listarUsuario();
            }).error(function (error) {
                //MostrarError();
            });
        };

        context.abrirAcceso = function (rowIndex) {
            location.href = "/Acceso/Index";
            //var usuario = context.gridOptions.data[rowIndex];
            //dataProvider.postData("Acceso/ListarAccesoSistema", usuario).success(function (respuesta) {
            //    console.log(usuario);
            //    context.accesosistema = respuesta[0];
            //    context.gridAccesos.data = respuesta;
            //}).error(function (error) {
            //    //MostrarError();
            //});
            //@Url.Action("Index", "Acceso")
        };

        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NombreUsuario', displayName: 'ID Usuario' },
                { field: 'NombreCompleto', displayName: 'Apellidos y Nombres' },
                { field: 'TipoUsuario.DescripcionConcepto', displayName: 'Tipo Usuario' },
                { field: 'Cargo.DescripcionConcepto', displayName: 'Cargo' },
                { field: 'Area.DescripcionConcepto', displayName: 'Área' },
                { field: 'Personal.EmailTrabrajo', displayName: 'Email Trabajo' },
                { field: 'Estado.DescripcionConcepto', displayName: 'Estado' },
                //{ field: 'Personal.TelefonoPersonal', displayName: 'Telefono Personal' },
                //{ field: 'ClaseUsu.DescripcionConcepto', displayName: 'Clase Usuario' },
                {
                    name: 'Acciones', cellTemplate: '<i ng-click="grid.appScope.editarUsuario(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-pencil-square-o" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Editar"></i>' +
                                                  '<i ng-click="grid.appScope.eliminarUsuario(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="fa fa-times" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Desactivar"></i> ' +
                                                  '<i ng-click="grid.appScope.abrirAcceso(grid.renderContainers.body.visibleRowCache.indexOf(row))" class="glyphicon glyphicon-list-alt" style="padding: 4px;font-size: 1.4em;" data-placement="top" data-toggle="tooltip" title="Accesos"></i> '
                }

            ],
        };

        //Eventos
        context.grabar = function (numeroboton) {
            console.log(context.personal);

            var personal = context.personal;
            var usuario = context.usuario;

            //GRABAR PERSONAL
            if (numeroboton == 1)
                personal.EstadoPersonal = 0
            else if (numeroboton == 2)
                personal.EstadoPersonal = 1
            //personal.CodigoUbigeo = (departamento + provincia + distrito);

            personal.IDEmpresa=1001 //POR TERMINAR

            //dataProvider.postData("Personal/GrabarPersonal", personal).success(function (respuesta) {
            //    console.log(respuesta);
            //    //listarUsuario();
            //    //$("#modal_contenido").modal("hide");
            //}).error(function (error) {
            //    //MostrarError();
            //});
            //GRABAR USUARIO
            //usuario.NombreUsuario = personal.NombrePers.substr(0,1) + personal.ApellidoPersonal;
            //usuario.ClaveUsuario = 123;

            //usuario.IDPersonal = personal.IDPersonal;

            if (numeroboton == 1)
                usuario.EstadoUsuario = 0
            else if (numeroboton == 2)
                usuario.EstadoUsuario = 1
            
            //Aqui se llena la entidad usuario, y tambien personal. Asignandole la propiedad de usuario.personal con la (Entidad) Personal
            //usuario.Personal = personal;
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

        context.buscarUsuario = function (usuario) {
            //usuario.Personal.NombrePers = usuario
            dataProvider.postData("Usuario/BuscarUsuarioNombre", usuario).success(function (respuesta) {
                //context.usuario = respuesta[0];
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        //Metodos
        function listarUsuario() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                //context.usuario = respuesta[0];
                console.log(context.usuario);
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
                else if (concepto.TipoConcepto == "024")
                    context.listTipoIdentificacion = respuesta;
            });
        }
        //Carga
        listarUsuario();
        listarDepartamento();
    }
})();
