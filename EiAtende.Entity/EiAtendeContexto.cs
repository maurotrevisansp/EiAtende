using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EiAtende.Entity
{
    public class EiAtendeContexto : DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            var mapPortalUsuario = modelBuilder.Entity<PortalUsuario>();
            mapPortalUsuario.Property(x => x.UsrID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mapPortalUsuario.HasKey(x => x.UsrID);
            mapPortalUsuario.ToTable("PortalUsuario");

            var mapPortalUsuarioGrupo = modelBuilder.Entity<PortalUsuarioGrupo>();
            mapPortalUsuarioGrupo.Property(x => x.UsrGrpID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mapPortalUsuarioGrupo.HasKey(x => x.UsrGrpID);
            mapPortalUsuarioGrupo.ToTable("PortalUsuarioGrupo");

            var mapPortalEmpresa = modelBuilder.Entity<PortalEmpresa>();
            mapPortalEmpresa.HasKey(x => x.EmpID);
            mapPortalEmpresa.ToTable("PortalEmpresa");

            var mapPortalConhecimentos = modelBuilder.Entity<PortalConhecimentos>();
            mapPortalConhecimentos.Property(x => x.CnhID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mapPortalConhecimentos.HasKey(x => x.CnhID);
            mapPortalConhecimentos.ToTable("PortalConhecimentos");

            var mapPortalChamados = modelBuilder.Entity<PortalChamados>();
            mapPortalChamados.Property(x => x.ChamadoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mapPortalChamados.HasKey(x => x.ChamadoID);
            mapPortalChamados.ToTable("PortalChamados");

            var mapPortalChamadosHistorico = modelBuilder.Entity<PortalChamadosHistorico>();
            mapPortalChamadosHistorico.Property(x => x.ChamadoHistID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mapPortalChamadosHistorico.HasKey(x => x.ChamadoHistID);
            mapPortalChamadosHistorico.ToTable("PortalChamadosHistorico");

            var mapChamadoAnexos = modelBuilder.Entity<ChamadoAnexos>();
            mapChamadoAnexos.Property(x => x.Id_ChamadoAnexo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mapChamadoAnexos.HasKey(x => x.Id_ChamadoAnexo);
            mapChamadoAnexos.ToTable("ChamadoAnexos");

            var mapPortalTipoChamados = modelBuilder.Entity<PortalTipoChamados>();
            mapPortalTipoChamados.Property(x => x.TipoChamadoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mapPortalTipoChamados.HasKey(x => x.TipoChamadoID);
            mapPortalTipoChamados.ToTable("PortalTipoChamados");

            var mapPortalAtividadeChamados = modelBuilder.Entity<PortalAtividadeChamados>();
            mapPortalAtividadeChamados.Property(x => x.AtividadeChamadoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mapPortalAtividadeChamados.HasKey(x => x.AtividadeChamadoID);
            mapPortalAtividadeChamados.ToTable("PortalAtividadeChamados");

        }

        public DbSet<PortalUsuario> PortalUsuario { get; set; }
        public DbSet<PortalUsuarioGrupo> PortalUsuarioGrupo { get; set; }
        public DbSet<PortalEmpresa> PortalEmpresa { get; set; }
        public DbSet<PortalConhecimentos> PortalConhecimentos { get; set; }
        public DbSet<PortalChamados> PortalChamados { get; set; }
        public DbSet<PortalChamadosHistorico> PortalChamadosHistorico { get; set; }
        public DbSet<ChamadoAnexos> ChamadoAnexos { get; set; }
        public DbSet<PortalTipoChamados> PortalTipoChamados { get; set; }
        public DbSet<PortalAtividadeChamados> PortalAtividadeChamados { get; set; }

    }

}
