namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ParidadeCampo")]
    public partial class ParidadeCampo
    {
        [Key]
        public int PrddCmpID { get; set; }

        [Required]
        [StringLength(250)]
        public string PrddCmpNome { get; set; }

        public int PrddCmpPrddID { get; set; }

        public int PrddCmpEmpID { get; set; }

        public int PrddCmpSvcID { get; set; }

        public int? PrddCmpPosicao { get; set; }

        public bool PrddCmpAtivo { get; set; }
    }
}
