namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ParidadeChaveCampo")]
    public partial class ParidadeChaveCampo
    {
        [Key]
        public int ChvCmpID { get; set; }

        [Required]
        [StringLength(250)]
        public string ChvCmpNome { get; set; }

        public int ChvCmpPosicao { get; set; }

        public int ChvCmpEmpID { get; set; }

        public int ChvCmpSvcID { get; set; }

        public int? ChvCmpPrddID { get; set; }

        public bool ChvCmpKey { get; set; }

        public bool ChvCmpAtivo { get; set; }
    }
}
