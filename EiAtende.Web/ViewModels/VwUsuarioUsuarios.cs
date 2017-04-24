using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EiAtende.ViewModels
{
    public class VwUsuarioUsuarios
    {
        public PortalUsuario _Usuario { get; set; }
        public PortalEmpresa _PortalEmpresa { get; set; }
        public ICollection<PortalUsuario> _Usuarios { get; set; }
        public ICollection<PortalUsuarioGrupo> _UsuarioGrupo { get; set; }
    }
}