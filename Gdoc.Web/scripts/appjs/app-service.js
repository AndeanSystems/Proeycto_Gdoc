(function () {
    'use strict';
    angular.module('app').service('appService', service);
    service.$inject = ['app_factory'];

    function service(dataProvider) {
        //CONCEPTO
        this.listarConcepto = function (concepto) {
            return dataProvider.postData("Concepto/ListarConcepto", concepto);
        },
        //this.listarConceptoEditable = function (concepto) {
        //    return dataProvider.postData("Concepto/ListarConceptoEditables", concepto);
        //},
        //USUARIO
        this.listarUsuario = function (usuario) {
            return dataProvider.postData("Usuario/ListarUsuario", usuario);
        },
        this.listarUsuarioGrupo = function (usuario) {
            return dataProvider.postData("Usuario/ListarUsuarioGrupo", usuario);
        },
        //Autocomplete Usuario Grupo
        this.buscarUsuarioGrupoAutoComplete = function (eUsuarioGrupo) {
            return dataProvider.postData("ComboUsuarioGrupo/ObtenerUsuarioGrupo", eUsuarioGrupo);
        },
        //UBIGEO
        this.listarDepartamento = function (ubigeo) {
            return dataProvider.getData("Ubigeo/ListarDepartamento");
        }
        this.listarProvincias = function (ubigeo) {
            return dataProvider.postData("Usuario/ListarProvincias", ubigeo);
        },
        this.listarDistritos = function (ubigeo) {
            return dataProvider.postData("Usuario/ListarDistritos", ubigeo);
        },
        //Utilitario
        this.setFormatDate = function (date) {
            return  new Date(parseInt(date.substr(6)));
        },
        this.mostrarAlerta = function (title, text, type) {
            swal({
                title: title,
                text: text,
                type: type,
                //confirmButtonColor: "#DD6B55",
                closeOnConfirm: false,
            });
            return;
        },
        this.confirmarEnvio = function (title, text, type,metodoOK,metodoCancel) {
            swal({
                title: title,
                text: text,
                type: type,
                showCancelButton: true,
                cancelButtonText: "Cancelar",
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Aceptar",
                closeOnConfirm: true
            },
            function () {
                metodoOK();
            });
        },
        this.obtenerUsuarioId = function () {
            return parseInt(document.getElementById("IDUsuario").value);
        }
        //EMPRESA
    }


})();
(function () {
    'use strict';
    angular.module('app').filter('toDateTime', filter);
    function filter() {
        return function (jsonDate) {
            var parser = jsonDate.replace(/\/Date\((-?\d+)\)\//, '$1');
            return new Date(parseInt(parser)).toJSON();
        };
    }
})();

(function () {
    'use strict';

    angular.module('app').controller('header_controller', header_controller);
    header_controller.$inject = ['app_factory', 'appService'];

    function header_controller(dataProvider, appService) {
        document.querySelector("#fotoAvatar").addEventListener("change", function (e) {
            for (var i = 0, f; f = e.target.files[i]; i++) {
                var reader = new FileReader();
                reader.onloadend = function (e) {
                    var eUsuario = {
                        IDUsuario : appService.obtenerUsuarioId(),
                        RutaAvatar : e.target.result
                    };
                    document.getElementById("AvatarUser").src = e.target.result;
                    dataProvider.postData("Usuario/GrabarUsuarioAvatar", eUsuario).success(function (respuesta) {
                        if (respuesta.Exitoso) {
                            appService.mostrarAlerta("Advertencia", respuesta.Mensaje, "success");
                        } else {
                            appService.mostrarAlerta("Advertencia", respuesta.Mensaje, "warning");
                        }
                    }).error(function (error) {
                        //MostrarError();
                    });
                }
                reader.readAsBinaryString(f);
            }
        });
    }
})();