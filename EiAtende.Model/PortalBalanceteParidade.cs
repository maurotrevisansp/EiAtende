namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("PortalBalanceteParidade")]
    public partial class PortalBalanceteParidade
    {
        [Key]
        public int BlcPrrdID { get; set; }

        public int BlcPrrdEmpID { get; set; }

        [Required]
        [StringLength(100)]
        public string BlcPrrdNome { get; set; }

        [StringLength(2000)]
        public string BlcPrrdChave { get; set; }
    }
}
