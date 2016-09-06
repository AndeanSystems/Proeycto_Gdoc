(function () {
    'use strict';

    angular.module('app').controller('firma_controller', firma_controller);
    firma_controller.$inject = ['$location', 'app_factory', 'appService'];

    function firma_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables

        var context = this;

        context.usuario = {};

        //Eventos
        context.grabar = function () {
            console.log(context.usuario);

            var usuario = context.usuario;

            if (usuario.FirmaElectronicaNueva == undefined || usuario.FirmaElectronicaNueva == "") {
                alert("Ingrese Firma")
                return;
            }
            else {
                alert("grabo");

                context.usuario.FirmaElectronica = usuario.FirmaElectronicaNueva
                dataProvider.postData("GrabarUsuario", usuario).success(function (respuesta) {
                    console.log(respuesta);
                    context.usuario = {};
                }).error(function (error) {
                    //MostrarError();
                });

                context.usuario = {};
                usuario = {};
                location.href = "CambiarFirmaElectronica";
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
