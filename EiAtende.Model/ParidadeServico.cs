namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ParidadeServico")]
    public partial class ParidadeServico
    {
        [Key]
        public int SvcID { get; set; }

        [Required]
        [StringLength(250)]
        public string SvcNome { get; set; }

        public int SvcEmpID { get; set; }

        [StringLength(20)]
        public string SvcClasse { get; set; }

        public bool SvcAtivo { get; set; }

        public DateTime SvcInclusao { get; set; }

        public int SvcInclusaoUsuario { get; set; }

        public DateTime? SvcAlteracao { get; set; }

        public int? SvcAlteracaoUsuario { get; set; }

        public DateTime? SvcExclusao { get; set; }

        public int? SvcExclusaoUsuario { get; set; }
    }
}
