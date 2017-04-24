using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EiAtende
{

    [Table("PortalUsuario")]
    public partial class PortalUsuario
    {
        [Key]
        public int UsrID { get; set; }

        public int UsrEmpID { get; set; }

        [Required]
        [StringLength(200)]
        public string UsrNome { get; set; }

        [Required]
        [StringLength(200)]
        public string UsrEmail { get; set; }

        [Required]
        [StringLength(200)]
        public string UsrSenha { get; set; }

        public bool UsrAdmin { get; set; }

        public bool UsrAdminSistema { get; set; }

        public bool UsrAtivo { get; set; }

        public DateTime UsrInclusao { get; set; }

        public int UsrInclusaoUsuario { get; set; }

        public DateTime? UsrAlteracao { get; set; }

        public int? UsrAlteracaoUsuario { get; set; }

        public DateTime? UsrExclusao { get; set; }

        public int? UsrExclusaoUsuario { get; set; }

        public int UsrGrpID { get; set; }

        public virtual PortalUsuarioGrupo PortalUsuarioGrupo { get; set; }

    }
}
