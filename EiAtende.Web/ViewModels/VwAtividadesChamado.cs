using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EiAtende.ViewModels
{
    public class VwAtividadesChamado
    {
        public ICollection<PortalAtividadeChamados> PortalAtividadeChamados { get; set; }
        public ICollection<PortalEmpresa> PortalEmpresa { get; set; }
    }
}