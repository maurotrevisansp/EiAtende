namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("PortalBalancete")]
    public partial class PortalBalancete
    {
        [Key]
        public int BlcID { get; set; }

        [Required]
        [StringLength(18)]
        public string BlcEmpID { get; set; }

        [Required]
        [StringLength(20)]
        public string BlcPeriodo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime BlcFechamento { get; set; }

        public int BlcAcesso { get; set; }

        [Required]
        [StringLength(15)]
        public string BlcClassificador { get; set; }

        [Required]
        [StringLength(150)]
        public string BlcNomeConta { get; set; }

        [StringLength(50)]
        public string BlcCentroCusto { get; set; }

        [Column(TypeName = "money")]
        public decimal BlcSaldoInicial { get; set; }

        [Column(TypeName = "money")]
        public decimal BlcMovDebito { get; set; }

        [Column(TypeName = "money")]
        public decimal BlcMovCredito { get; set; }

        [Column(TypeName = "money")]
        public decimal BlcSaldoFinal { get; set; }
    }
}
