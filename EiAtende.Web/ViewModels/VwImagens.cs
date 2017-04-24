using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EiAtende.ViewModels
{
    public class VwImagens
    {
        public string[] Imagens { get; set; }
        public string[] NomeArquivo { get; set; }
        public string[] DataUpload { get; set; }
        public int ChamadoID { get; set; }
        public string ChamadoTitulo { get; set; }

    }
}