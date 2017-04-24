namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("PortalModulo")]
    public partial class PortalModulo
    {
        [Key]
        public int ModID { get; set; }

        [Required]
        [StringLength(50)]
        public string ModNome { get; set; }

        [Required]
        [StringLength(100)]
        public string ModDescricao { get; set; }

        public bool ModAtivo { get; set; }
    }
}
