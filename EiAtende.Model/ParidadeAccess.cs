namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ParidadeAccess")]
    public partial class ParidadeAccess
    {
        [Key]
        public int AccID { get; set; }

        [Required]
        [StringLength(250)]
        public string AccPath { get; set; }

        [Required]
        [StringLength(250)]
        public string AccNomeTabela { get; set; }

        [Required]
        [StringLength(8000)]
        public string AccComandoDrop { get; set; }

        [Required]
        [StringLength(8000)]
        public string AccComandoCreate { get; set; }

        [Required]
        [StringLength(8000)]
        public string AccComandoSelect { get; set; }

        public int AccEmpID { get; set; }

        public int AccSvcID { get; set; }

        public int? AccPrddID { get; set; }

        [Required]
        [StringLength(250)]
        public string AccPathMdb { get; set; }

        [StringLength(250)]
        public string AccTemplate { get; set; }

        public bool AccAtivo { get; set; }
    }
}
