using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EiAtende
{
    public class ChamadoAnexos
    {
        public int Id_ChamadoAnexo { get; set; }
        public int ChamadoID { get; set; }
        public string Ds_Anexo { get; set; }
        public string Patch_Anexo { get; set; }
        public string Nome_Arquivo_Anexo { get; set; }
        public DateTime Dt_Incl_Anexo { get; set; }

        public virtual PortalChamados PortalChamados { get; set; }

    }
}
