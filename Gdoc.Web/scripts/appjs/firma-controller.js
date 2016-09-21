//Leer Archivos de de fisico a binario
var archivosSelecionados = [];
function ReadFileToBinary(control) {
    archivosSelecionados = [];
    for (var i = 0, f; f = control.files[i]; i++) {
        let files = f;
        var reader = new FileReader();
        reader.onloadend = function (e) {
            //console.log(files);
            archivosSelecionados.push({
                NombreArchivo: files.name,
                TamanoArchivo: files.size,
                TipoArchivo: files.type,
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
            if (archivosSelecionados == undefined || archivosSelecionados == "" || archivosSelecionados == null) {
                return appService.mostrarAlerta("Advertencia", "Debe adjuntar una firma", "warning");
            }
            if (usuario.FirmaElectronicaNueva == undefined || usuario.FirmaElectronicaNueva == "") {
                return appService.mostrarAlerta("Advertencia", "Ingrese caracteres", "warning");
            }
            else {

                let listoFirmas = [];

                for (var index in archivosSelecionados) {
                    listoFirmas.push({
                        RutaFisica: archivosSelecionados[index].RutaBinaria,
                        NombreOriginal: archivosSelecionados[index].NombreArchivo,
                        TamanoDocto: archivosSelecionados[index].TamanoArchivo,
                        TipoArchivo: archivosSelecionados[index].TipoArchivo,
                    });
                    console.log(listoFirmas);
                }

                context.usuario.FirmaElectronica = usuario.FirmaElectronicaNueva

                function enviarFomularioOK() {
                    console.log(usuario);
                    dataProvider.postData("GrabarUsuario", usuario).success(function (respuesta) {
                        console.log(respuesta);
                    }).error(function (error) {
                        //MostrarError();
                    });
                    console.log(listoFirmas);
                    dataProvider.postData("MoverFirma", listoFirmas).success(function (respuesta) {
                        console.log(respuesta);
                    }).error(function (error) {
                        //MostrarError();
                    });
                    context.usuario = {};
                    usuario = {};
                    document.getElementById("input_file").value = "";
                    listarUsuario();
                    appService.mostrarAlerta("Información", "se grabo correctamente", "success");
                    location.href = "CambiarFirmaElectronica";
                }
                appService.confirmarEnvio("¿Seguro que deseas continuar?", "No podrás deshacer este paso...", "warning", enviarFomularioOK);

                
                
                
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
