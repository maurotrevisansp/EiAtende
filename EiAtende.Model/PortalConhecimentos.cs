using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EiAtende
{
    public class PortalConhecimentos
    {
        public int CnhID { get; set; }
        public string CnhTitulo { get; set; }
        public string CnhDescricao { get; set; }
        public string CnhTagsBusca { get; set; }
        public Nullable<System.DateTime> CnhDataCriacao { get; set; }
        public string CnhFonte { get; set; }
        public Nullable<System.DateTime> CnhDataAlteracao { get; set; }
        public string CnhUrlParcial { get; set; }
        public string CnhTexto { get; set; }

    }
}
