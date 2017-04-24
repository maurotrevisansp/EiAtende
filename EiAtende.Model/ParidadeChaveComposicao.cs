namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ParidadeChaveComposicao")]
    public partial class ParidadeChaveComposicao
    {
        [Key]
        public int CompID { get; set; }

        public int CompChvID { get; set; }

        public int CompChvCmpID { get; set; }

        public int CompEmpID { get; set; }

        public int CompSvcID { get; set; }

        public int? CompPrddID { get; set; }

        public bool CompAtivo { get; set; }
    }
}
