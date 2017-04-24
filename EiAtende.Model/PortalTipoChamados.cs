using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace EiAtende
{
    public class PortalTipoChamados
    {
        [Key]
        public int TipoChamadoID { get; set; }

        [Required]
        [StringLength(200)]
        public string TipoChamadoNome { get; set; }

        [Required]
        public int EmpID { get; set; }

        public virtual PortalEmpresa PortalEmpresa { get; set; }

    }
}
