using Gdoc.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Negocio
{
    public class NUsuario : IDisposable
    {
        private DUsuario dUsuario = new DUsuario();
        public void Dispose()
        {
            dUsuario = null;
        }

        public EUsuario ValidarLogin(EUsuario usuario)
        {
            try
            {
                return dUsuario.ValidarLogin(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public EUsuario CantidadAlerta(EUsuario usuario)
        {
            try
            {
                return dUsuario.CantidadAlerta(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EUsuario CantidadDocumentosRecibidos(EUsuario usuario)
        {
            try
            {
                return dUsuario.CantidadDocumentosRecibidos(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public EUsuario CantidadMesaVirtual(EUsuario usuario)
        {
            try
            {
                return dUsuario.CantidadMesaVirtual(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<EUsuario> ListarUsuario()
        {
            try
            {
                return dUsuario.ListarUsuario();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Usuario GrabarUsuario(Usuario usuario)
        {
            try
            {
                return dUsuario.GrabarUsuario(usuario);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Usuario EditarUsuario(Usuario usuario)
        {
            try
            {
                return dUsuario.EditarUsuario(usuario);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Usuario EliminarUsuario(Usuario usuario)
        {
            try
            {
                return dUsuario.EliminarUsuario(usuario);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
