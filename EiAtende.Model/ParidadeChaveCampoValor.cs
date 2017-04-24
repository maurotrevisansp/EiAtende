namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ParidadeChaveCampoValor")]
    public partial class ParidadeChaveCampoValor
    {
        [Key]
        public int ChvCmpVlrID { get; set; }

        [Required]
        [StringLength(500)]
        public string ChvCmpVlrValor { get; set; }

        public int ChvCmpVlrChvCmpID { get; set; }

        public int ChvCmpVlrPrddCmpVlrID { get; set; }

        [Required]
        [StringLength(50)]
        public string ChvCmpVlrSortID { get; set; }

        public int ChvCmpVlrEmpID { get; set; }

        public int ChvCmpVlrSvcID { get; set; }

        public int? ChvCmpVlrPrddID { get; set; }

        public bool ChvCmpVlrKey { get; set; }

        public bool ChvCmpVlrAtivo { get; set; }
    }
}
