//Leer Archivos de de fisico a binario
var archivosSelecionados = [];
function ReadFileToBinary(control) {
    for (var i = 0, f; f = control.files[i]; i++) {
        let Name = f.name; Size = f.size; Type = f.type;
        var reader = new FileReader();
        reader.onloadend = function (e) {
            archivosSelecionados.push({
                NombreArchivo: Name,
                TamanoArchivo: Size,
                TipoArchivo: Type,
                RutaBinaria: e.target.result
            });
        }
        reader.readAsBinaryString(f);
    }
}
//Angular JS
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
        context.pasar = function () {
            dataProvider.postData("GrabarUsuario", usuario).success(function (respuesta) {
                console.log(respuesta);
                context.usuario = {};
            }).error(function (error) {
                //MostrarError();
            });
        }

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
                dataProvider.getData("GrabarUsuario", usuario).success(function (respuesta) {
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
