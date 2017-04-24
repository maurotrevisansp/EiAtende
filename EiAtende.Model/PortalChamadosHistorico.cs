using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EiAtende
{
    public class PortalChamadosHistorico
    {
        public int ChamadoHistID { get; set; }
        public int ChamadoID { get; set; }
        public string Descricao { get; set; }
        public DateTime DtIncl { get; set; }
        public string DeStatus { get; set; }
        public string ParaStatus { get; set; }
        public Nullable<DateTime> DtAdiar { get; set; }

        public virtual PortalChamados PortalChamados { get; set; }
    }
}
