﻿using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Entity.Extension
{
    [NotMapped]
    public class EOperacion:Operacion
    {
        public DocumentoDigitalOperacion DocumentoDigitalOperacion { get; set; }
        public DocumentoElectronicoOperacion DocumentoElectronicoOperacion { get; set; }
        public Usuario Usuario { get; set; }
        public UsuarioParticipante UsuarioParticipante { get; set; }
        public Concepto TipoOpe { get; set; }
        public Concepto TipoDoc { get; set; }
        public Concepto Estado { get; set; }
        public Concepto Acceso { get; set; }
        public Concepto Prioridad { get; set; }
        public string OrganizadorMV { get; set; }
        public string Remitente { get; set; }
        public string Prioridoc { get; set; }
        public string Emisor { get; set; }
    }
}
