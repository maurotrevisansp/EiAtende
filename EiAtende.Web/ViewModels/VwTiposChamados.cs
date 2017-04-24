using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EiAtende.ViewModels
{
    public class VwTiposChamados
    {
        public ICollection<PortalTipoChamados> PortalTipoChamados { get; set; }
        public ICollection<PortalEmpresa> PortalEmpresa { get; set; }
    }
}