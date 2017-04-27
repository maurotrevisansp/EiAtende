using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EiAtende.ViewModels
{
    public class VwChamados
    {
        public ICollection<PortalChamados> PortalChamados { get; set; }
        public PortalChamados PortalChamado { get; set; }
        public PortalChamadosHistorico PortalChamadoHistorico { get; set; }
        public ICollection<PortalChamadosHistorico> PortalChamadosHistorico { get; set; }
        public ICollection<PortalTipoChamados> PortalTipoChamados { get; set; }
        public ICollection<PortalAtividadeChamados> PortalAtividadeChamados { get; set; }
        public ICollection<PortalEmpresa> PortalEmpresa { get; set; }

        public ICollection<PortalUsuario> DeUsuario { get; set; }
        public ICollection<PortalUsuario> ParaUsuario { get; set; }
        public ICollection<PortalEmpresa> AtenderEmpresa { get; set; }

    }
}