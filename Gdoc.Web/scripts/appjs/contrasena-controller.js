(function () {
    'use strict';

    angular.module('app').controller('contrasena_controller', contrasena_controller);
    contrasena_controller.$inject = ['$location', 'app_factory', 'appService'];

    function contrasena_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables

        var context = this;

        context.usuario = {};

        //Eventos
        context.grabar = function () {
            console.log(context.usuario);

            var usuario = context.usuario;

            if (usuario.ClaveUsuarioAntigua == undefined || usuario.ClaveUsuarioAntigua == "") {
                alert("Ingrese clave")
                return;
            }
            else if (usuario.ClaveUsuarioAntigua != usuario.ClaveUsuario) {
                alert("Clave Actual incorrecta")
                return;
            }
            else if (usuario.ClaveUsuarioNueva == undefined || usuario.ClaveUsuarioNueva == "") {
                alert("Ingrese la clave nueva");
                return;
            }
            else if (usuario.ClaveUsuarioNueva == context.usuario.ClaveUsuario) {
                alert("La nueva Clave es igual a la Anterior")
                return;
            }
            else if (usuario.ClaveUsuarioNueva2 == undefined || usuario.ClaveUsuarioNueva2 == "") {
                alert("Confirme la clave nueva");
                return;
            }
            else if (usuario.ClaveUsuarioNueva != usuario.ClaveUsuarioNueva2) {
                alert("Las claves no coinciden");
                return;
            }
            else {
                alert("grabo");

                context.usuario.ClaveUsuario = usuario.ClaveUsuarioNueva2
                dataProvider.postData("GrabarUsuario", usuario).success(function (respuesta) {
                    console.log(respuesta);
                    context.usuario = {};
                }).error(function (error) {
                    //MostrarError();
                });

                context.usuario = {};
                usuario = {};
                location.href = "CambiarContraseña";
            }
        }

        //Metodos
        function listarUsuario() {
            dataProvider.getData("BuscarUsuarioNombreClave").success(function (respuesta) {
                context.usuario = respuesta[0];
                console.log(context.usuario);
                context.listUsuario = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
        listarUsuario();
    }
})();
