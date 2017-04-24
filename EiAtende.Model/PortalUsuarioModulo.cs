namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("PortalUsuarioModulo")]
    public partial class PortalUsuarioModulo
    {
        [Key]
        public int XUserID { get; set; }

        public int XUserUserID { get; set; }

        public int XUserModID { get; set; }

        public bool XUserAtivo { get; set; }

        public DateTime XUserInclusao { get; set; }

        public int XUserInclusaoUsuario { get; set; }

        public DateTime? XUserAlteracao { get; set; }

        public int? XUserAlteracaoUsuario { get; set; }

        public DateTime? XUserExclusao { get; set; }

        public int? XUserExclusaoUsuario { get; set; }
    }
}
