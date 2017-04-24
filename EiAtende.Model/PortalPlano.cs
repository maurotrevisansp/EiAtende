namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("PortalPlano")]
    public partial class PortalPlano
    {
        [Key]
        public int PlanoID { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanoNome { get; set; }

        [Required]
        [StringLength(100)]
        public string PlanoDescricao { get; set; }

        public bool PlanoAtivo { get; set; }

        public DateTime PlanoInclusao { get; set; }

        public int PlanoInclusaoUsuario { get; set; }

        public DateTime? PlanoAlteracao { get; set; }

        public int? PlanoAlteracaoUsuario { get; set; }

        public DateTime? PlanoExclusao { get; set; }

        public int? PlanoExclusaoUsuario { get; set; }
    }
}
