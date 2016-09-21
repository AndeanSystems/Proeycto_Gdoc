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
                return appService.mostrarAlerta("Informacion", "Ingrese Clave", "warning");
            }
            else if (usuario.ClaveUsuarioAntigua != usuario.ClaveUsuario) {
                return appService.mostrarAlerta("Informacion", "Clave Actual incorrecta", "error");
            }
            else if (usuario.ClaveUsuarioNueva == undefined || usuario.ClaveUsuarioNueva == "") {
                return appService.mostrarAlerta("Informacion", "Ingrese la clave nueva", "warning");
            }
            else if (usuario.ClaveUsuarioNueva == context.usuario.ClaveUsuario) {
                return appService.mostrarAlerta("Informacion", "La nueva Clave es igual a la Anterior", "warning");
            }
            else if (usuario.ClaveUsuarioNueva2 == undefined || usuario.ClaveUsuarioNueva2 == "") {
                return appService.mostrarAlerta("Informacion", "Confirme la clave nueva", "warning");
            }
            else if (usuario.ClaveUsuarioNueva != usuario.ClaveUsuarioNueva2) {
                return appService.mostrarAlerta("Informacion", "Las claves no coinciden", "error");
            }
            else {


                function enviarFomularioOK() {
                    context.usuario.ClaveUsuario = usuario.ClaveUsuarioNueva2
                    dataProvider.postData("GrabarUsuario", usuario).success(function (respuesta) {
                        if (respuesta.Exitoso)
                            TipoMensaje = "success";
                        appService.mostrarAlerta("Información", respuesta.Mensaje, TipoMensaje);
                    }).error(function (error) {
                        //MostrarError();
                    });
                    context.usuario = {};
                    usuario = {};
                    listarUsuario();
                }
                appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);
                
                
                //location.href = "CambiarContraseña";
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
