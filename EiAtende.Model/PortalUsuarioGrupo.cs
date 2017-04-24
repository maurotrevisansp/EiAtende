using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EiAtende
{
    [Table("PortalUsuarioGrupo")]
    public partial class PortalUsuarioGrupo
    {
        [Key]
        public int UsrGrpID { get; set; }

        [Required]
        [StringLength(100)]
        public string UsrGrpNome { get; set; }

        public bool UsrGrpAtivo { get; set; }

        [Required]
        public bool UsrGrpAbrirChamado { get; set; }
        [Required]
        public bool UsrGrpVerChamado { get; set; }
        [Required]
        public bool UsrGrpTratarChamado { get; set; }
        [Required]
        public bool UsrGrpAdiarChamado { get; set; }
        [Required]
        public bool UsrGrpFecharChamado { get; set; }
        [Required]
        public bool UsrGrpTotalAcessoChamado { get; set; }



    }
}
