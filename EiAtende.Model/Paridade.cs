using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EiAtende
{

    [Table("Paridade")]
    public partial class Paridade
    {
        [Key]
        public int PrddID { get; set; }

        [Required]
        [StringLength(250)]
        public string PrddNome { get; set; }

        public int PrddEmpID { get; set; }

        public int PrddSvcID { get; set; }

        public bool PrddAtivo { get; set; }
    }
}
