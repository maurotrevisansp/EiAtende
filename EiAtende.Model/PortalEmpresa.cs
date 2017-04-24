namespace EiAtende
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("PortalEmpresa")]
    public partial class PortalEmpresa
    {
        [Key]
        public int EmpID { get; set; }

        public int EmpEmpID { get; set; }

        [Required]
        [StringLength(18)]
        public string EmpCNPJ { get; set; }

        [Required]
        [StringLength(200)]
        public string EmpRazao { get; set; }

        [Required]
        [StringLength(200)]
        public string EmpNomeFantasia { get; set; }

        [Required]
        [StringLength(150)]
        public string EmpEndereco { get; set; }

        public int EmpEnderecoNumero { get; set; }

        [StringLength(100)]
        public string EmpEnderecoComplemento { get; set; }

        [Required]
        [StringLength(30)]
        public string EmpEnderecoBairro { get; set; }

        [Required]
        [StringLength(10)]
        public string EmpEnderecoCep { get; set; }

        [Required]
        [StringLength(30)]
        public string EmpEnderecoCidade { get; set; }

        [Required]
        [StringLength(2)]
        public string EmpEnderecoUF { get; set; }

        [Required]
        [StringLength(20)]
        public string EmpEnderecoPais { get; set; }

        [Required]
        [StringLength(50)]
        public string EmpTelefone { get; set; }

        [Required]
        [StringLength(100)]
        public string EmpEmail { get; set; }

        [StringLength(200)]
        public string EmpNaturezaJuridica { get; set; }

        [StringLength(200)]
        public string EmpCnae { get; set; }

        public bool? EmpSituacao { get; set; }

        [StringLength(100)]
        public string EmpLogo { get; set; }

        public int EmpPlanoID { get; set; }

        public bool EmpTeste { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime EmpTesteData { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? EmpNegativadoData { get; set; }

        public bool EmpAtivo { get; set; }

        public DateTime EmpInclusao { get; set; }

        public int EmpInclusaoUsuario { get; set; }

        public DateTime? EmpAlteracao { get; set; }

        public int? EmpAlteracaoUsuario { get; set; }

        public DateTime? EmpExclusao { get; set; }

        public int? EmpExclusaoUsuario { get; set; }
    }
}
