
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EiAtende
{
    [Table("PortalUsuarioGrupo")]
    public class PortalChamados
    {
        [Key]
        public int ChamadoID { get; set; }

        [Required]
        [StringLength(200)]
        public string ChamadoTitulo { get; set; }
        public string ChamadoHistorico { get; set; }

        [Required]
        public int DeUsrID { get; set; }
        [Required]
        public int ParaUsrID { get; set; }
        [Required]
        public int AtenderEmpID { get; set; }
        [Required]
        public DateTime ChamadoDtAbertura { get; set; }
        [Required]
        public DateTime ChamadoDtPrevista { get; set; }

        public Nullable<DateTime> ChamadoDtAdiada { get; set; }
        public Nullable<DateTime> ChamadoDtTermino { get; set; }

        [Required]
        public int TipoChamadoID { get; set; }
        [Required]
        public int AtividadeChamadoID { get; set; }

        public string ChamadoConhecimento { get; set; }

        public string Status { get; set; }
        public string paraStatus { get; set; }
        public string Avaliacao { get; set; }
        
        public virtual PortalTipoChamados PortalTipoChamados { get; set; }
        public virtual PortalAtividadeChamados PortalAtividadeChamados { get; set; }
        public virtual ICollection<ChamadoAnexos> ChamadoAnexos { get; set; }
        public virtual ICollection<PortalChamadosHistorico> PortalChamadosHistorico { get; set; }

    }
}
