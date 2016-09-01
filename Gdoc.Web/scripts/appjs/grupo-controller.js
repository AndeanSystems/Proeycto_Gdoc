(function () {
    'use strict';

    angular.module('app').controller('grupo_controller', grupo_controller);
    grupo_controller.$inject = ['$location', 'app_factory'];

    function grupo_controller($location, dataProvider) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.listGrupo = [];
        context.listUsuario = [];
        context.grupo = {};
        context.listaUsuariosGrupo = [];

        //context.listausuariosgrupo = [];
        //context.Usuario = {};
        //Eventos

        context.agregar = function (IDUsuario) {
            if (context.grupo.CodigoGrupo == undefined)
                alert("vacio");
            else {
                context.listaUsuariosGrupo.push(context.grupo);
                context.grupo = {};
            }
            //dataProvider.postData("Usuario/ListarUsuarioGrupo", { IDUsuario: IDUsuario }).success(function (respuesta) {
            //    console.log(respuesta);
            //    context.listaUsuariosGrupo.push(respuesta);
            //}).error(function (error) {
            //    //MostrarError();
            //});
        }
        context.grabar = function () {
            console.log(context.grupo);
            dataProvider.postData("Grupo/GrabarGrupoUsuarios", context.grupo).success(function (respuesta) {
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }

        //Metodos
        function listarGrupo() {
            dataProvider.getData("Grupo/ListarGrupo").success(function (respuesta) {
                context.listGrupo = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarUsuario() {
            dataProvider.getData("Usuario/ListarUsuario").success(function (respuesta) {
                context.listUsuario = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        
        //Carga
        listarGrupo();
        listarUsuario();
    }
})();
