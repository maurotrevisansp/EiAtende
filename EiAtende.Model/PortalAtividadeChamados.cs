using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EiAtende
{
    public class PortalAtividadeChamados
    {
        [Key]
        public int AtividadeChamadoID { get; set; }

        [Required]
        [StringLength(200)]
        public string AtividadeChamadoNome { get; set; }

        [Required]
        public int EmpID { get; set; }

        [Required]
        public int PrevisaoDias { get; set; }

        [Required]
        public int PrevisaoHoras { get; set; }

        public virtual PortalEmpresa PortalEmpresa { get; set; }

    }
}
