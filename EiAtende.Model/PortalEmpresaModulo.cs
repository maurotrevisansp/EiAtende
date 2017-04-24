namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("PortalEmpresaModulo")]
    public partial class PortalEmpresaModulo
    {
        [Key]
        public int XEmpID { get; set; }

        public int XEmpEmpID { get; set; }

        public int XEmpModID { get; set; }

        public bool XEmpAtivo { get; set; }

        public DateTime XEmpInclusao { get; set; }

        public int XEmpInclusaoUsuario { get; set; }

        public DateTime? XEmpAlteracao { get; set; }

        public int? XEmpAlteracaoUsuario { get; set; }

        public DateTime? XEmpExclusao { get; set; }

        public int? XEmpExclusaoUsuario { get; set; }
    }
}
