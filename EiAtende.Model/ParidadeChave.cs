namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ParidadeChave")]
    public partial class ParidadeChave
    {
        [Key]
        public int ChvID { get; set; }

        [Required]
        [StringLength(250)]
        public string ChvNome { get; set; }

        public int ChvEmpID { get; set; }

        public int ChvSvcID { get; set; }

        public int? ChvPrddID { get; set; }

        public bool ChvAtivo { get; set; }
    }
}
