using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EiAtende.ViewModels
{
    public class VwChamadosHistorico
    {
        public ICollection<PortalChamadosHistorico> PortalChamadosHistoricos { get; set; }
        public PortalChamadosHistorico PortalChamadosHistorico { get; set; }

    }
}