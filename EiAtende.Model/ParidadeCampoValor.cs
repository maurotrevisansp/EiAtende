namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ParidadeCampoValor")]
    public partial class ParidadeCampoValor
    {
        [Key]
        public int PrddCmpVlrID { get; set; }

        [Required]
        [StringLength(250)]
        public string PrddCmpVlrValor { get; set; }

        public int PrddCmpVlrPrddCmpID { get; set; }

        [Required]
        [StringLength(50)]
        public string PrddCmpVlrSortID { get; set; }

        public int? PrddCmpPrddID { get; set; }

        public bool PrddCmpVlrAtivo { get; set; }
    }
}
