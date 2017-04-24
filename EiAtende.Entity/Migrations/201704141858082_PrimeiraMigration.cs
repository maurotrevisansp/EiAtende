namespace EiAtende.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimeiraMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChamadoAnexos",
                c => new
                    {
                        Id_ChamadoAnexo = c.Int(nullable: false, identity: true),
                        ChamadoID = c.Int(nullable: false),
                        Ds_Anexo = c.String(),
                        Patch_Anexo = c.String(),
                        Nome_Arquivo_Anexo = c.String(),
                        Dt_Incl_Anexo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_ChamadoAnexo)
                .ForeignKey("dbo.PortalChamados", t => t.ChamadoID, cascadeDelete: false)
                .Index(t => t.ChamadoID);
            
            CreateTable(
                "dbo.PortalChamados",
                c => new
                    {
                        ChamadoID = c.Int(nullable: false, identity: true),
                        ChamadoTitulo = c.String(nullable: false, maxLength: 200),
                        ChamadoHistorico = c.String(),
                        DeUsrID = c.Int(nullable: false),
                        ParaUsrID = c.Int(nullable: false),
                        AtenderEmpID = c.Int(nullable: false),
                        ChamadoDtAbertura = c.DateTime(nullable: false),
                        ChamadoDtPrevista = c.DateTime(nullable: false),
                        ChamadoDtAdiada = c.DateTime(),
                        ChamadoDtTermino = c.DateTime(),
                        TipoChamadoID = c.Int(nullable: false),
                        AtividadeChamadoID = c.Int(nullable: false),
                        ChamadoConhecimento = c.String(),
                        Status = c.String(),
                        Avaliacao = c.String(),
                    })
                .PrimaryKey(t => t.ChamadoID)
                .ForeignKey("dbo.PortalAtividadeChamados", t => t.AtividadeChamadoID, cascadeDelete: false)
                .ForeignKey("dbo.PortalTipoChamados", t => t.TipoChamadoID, cascadeDelete: false)
                .Index(t => t.TipoChamadoID)
                .Index(t => t.AtividadeChamadoID);
            
            CreateTable(
                "dbo.PortalAtividadeChamados",
                c => new
                    {
                        AtividadeChamadoID = c.Int(nullable: false, identity: true),
                        AtividadeChamadoNome = c.String(nullable: false, maxLength: 200),
                        EmpID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AtividadeChamadoID)
                .ForeignKey("dbo.PortalEmpresa", t => t.EmpID, cascadeDelete: false)
                .Index(t => t.EmpID);
            
            CreateTable(
                "dbo.PortalEmpresa",
                c => new
                    {
                        EmpID = c.Int(nullable: false, identity: true),
                        EmpEmpID = c.Int(nullable: false),
                        EmpCNPJ = c.String(nullable: false, maxLength: 18),
                        EmpRazao = c.String(nullable: false, maxLength: 200),
                        EmpNomeFantasia = c.String(nullable: false, maxLength: 200),
                        EmpEndereco = c.String(nullable: false, maxLength: 150),
                        EmpEnderecoNumero = c.Int(nullable: false),
                        EmpEnderecoComplemento = c.String(maxLength: 100),
                        EmpEnderecoBairro = c.String(nullable: false, maxLength: 30),
                        EmpEnderecoCep = c.String(nullable: false, maxLength: 10),
                        EmpEnderecoCidade = c.String(nullable: false, maxLength: 30),
                        EmpEnderecoUF = c.String(nullable: false, maxLength: 2),
                        EmpEnderecoPais = c.String(nullable: false, maxLength: 20),
                        EmpTelefone = c.String(nullable: false, maxLength: 50),
                        EmpEmail = c.String(nullable: false, maxLength: 100),
                        EmpNaturezaJuridica = c.String(maxLength: 200),
                        EmpCnae = c.String(maxLength: 200),
                        EmpSituacao = c.Boolean(),
                        EmpLogo = c.String(maxLength: 100),
                        EmpPlanoID = c.Int(nullable: false),
                        EmpTeste = c.Boolean(nullable: false),
                        EmpTesteData = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        EmpNegativadoData = c.DateTime(storeType: "smalldatetime"),
                        EmpAtivo = c.Boolean(nullable: false),
                        EmpInclusao = c.DateTime(nullable: false),
                        EmpInclusaoUsuario = c.Int(nullable: false),
                        EmpAlteracao = c.DateTime(),
                        EmpAlteracaoUsuario = c.Int(),
                        EmpExclusao = c.DateTime(),
                        EmpExclusaoUsuario = c.Int(),
                    })
                .PrimaryKey(t => t.EmpID);
            
            CreateTable(
                "dbo.PortalChamadosHistorico",
                c => new
                    {
                        ChamadoHistID = c.Int(nullable: false, identity: true),
                        ChamadoID = c.Int(nullable: false),
                        Descricao = c.String(),
                        DtIncl = c.DateTime(nullable: false),
                        DeStatus = c.String(),
                        ParaStatus = c.String(),
                        DtAdiar = c.DateTime(),
                    })
                .PrimaryKey(t => t.ChamadoHistID)
                .ForeignKey("dbo.PortalChamados", t => t.ChamadoID, cascadeDelete: false)
                .Index(t => t.ChamadoID);
            
            CreateTable(
                "dbo.PortalTipoChamados",
                c => new
                    {
                        TipoChamadoID = c.Int(nullable: false, identity: true),
                        TipoChamadoNome = c.String(nullable: false, maxLength: 200),
                        EmpID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TipoChamadoID)
                .ForeignKey("dbo.PortalEmpresa", t => t.EmpID, cascadeDelete: false)
                .Index(t => t.EmpID);
            
            CreateTable(
                "dbo.PortalConhecimentos",
                c => new
                    {
                        CnhID = c.Int(nullable: false, identity: true),
                        CnhTitulo = c.String(),
                        CnhDescricao = c.String(),
                        CnhTagsBusca = c.String(),
                        CnhDataCriacao = c.DateTime(),
                        CnhFonte = c.String(),
                        CnhDataAlteracao = c.DateTime(),
                        CnhUrlParcial = c.String(),
                        CnhTexto = c.String(),
                    })
                .PrimaryKey(t => t.CnhID);
            
            CreateTable(
                "dbo.PortalUsuario",
                c => new
                    {
                        UsrID = c.Int(nullable: false, identity: true),
                        UsrEmpID = c.Int(nullable: false),
                        UsrNome = c.String(nullable: false, maxLength: 200),
                        UsrEmail = c.String(nullable: false, maxLength: 200),
                        UsrSenha = c.String(nullable: false, maxLength: 200),
                        UsrAdmin = c.Boolean(nullable: false),
                        UsrAdminSistema = c.Boolean(nullable: false),
                        UsrAtivo = c.Boolean(nullable: false),
                        UsrInclusao = c.DateTime(nullable: false),
                        UsrInclusaoUsuario = c.Int(nullable: false),
                        UsrAlteracao = c.DateTime(),
                        UsrAlteracaoUsuario = c.Int(),
                        UsrExclusao = c.DateTime(),
                        UsrExclusaoUsuario = c.Int(),
                        UsrGrpID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UsrID)
                .ForeignKey("dbo.PortalUsuarioGrupo", t => t.UsrGrpID, cascadeDelete: false)
                .Index(t => t.UsrGrpID);
            
            CreateTable(
                "dbo.PortalUsuarioGrupo",
                c => new
                    {
                        UsrGrpID = c.Int(nullable: false, identity: true),
                        UsrGrpNome = c.String(nullable: false, maxLength: 100),
                        UsrGrpAtivo = c.Boolean(nullable: false),
                        UsrGrpAbrirChamado = c.Boolean(nullable: false),
                        UsrGrpVerChamado = c.Boolean(nullable: false),
                        UsrGrpTratarChamado = c.Boolean(nullable: false),
                        UsrGrpAdiarChamado = c.Boolean(nullable: false),
                        UsrGrpFecharChamado = c.Boolean(nullable: false),
                        UsrGrpTotalAcessoChamado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UsrGrpID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PortalUsuario", "UsrGrpID", "dbo.PortalUsuarioGrupo");
            DropForeignKey("dbo.PortalChamados", "TipoChamadoID", "dbo.PortalTipoChamados");
            DropForeignKey("dbo.PortalTipoChamados", "EmpID", "dbo.PortalEmpresa");
            DropForeignKey("dbo.PortalChamadosHistorico", "ChamadoID", "dbo.PortalChamados");
            DropForeignKey("dbo.PortalChamados", "AtividadeChamadoID", "dbo.PortalAtividadeChamados");
            DropForeignKey("dbo.PortalAtividadeChamados", "EmpID", "dbo.PortalEmpresa");
            DropForeignKey("dbo.ChamadoAnexos", "ChamadoID", "dbo.PortalChamados");
            DropIndex("dbo.PortalUsuario", new[] { "UsrGrpID" });
            DropIndex("dbo.PortalTipoChamados", new[] { "EmpID" });
            DropIndex("dbo.PortalChamadosHistorico", new[] { "ChamadoID" });
            DropIndex("dbo.PortalAtividadeChamados", new[] { "EmpID" });
            DropIndex("dbo.PortalChamados", new[] { "AtividadeChamadoID" });
            DropIndex("dbo.PortalChamados", new[] { "TipoChamadoID" });
            DropIndex("dbo.ChamadoAnexos", new[] { "ChamadoID" });
            DropTable("dbo.PortalUsuarioGrupo");
            DropTable("dbo.PortalUsuario");
            DropTable("dbo.PortalConhecimentos");
            DropTable("dbo.PortalTipoChamados");
            DropTable("dbo.PortalChamadosHistorico");
            DropTable("dbo.PortalEmpresa");
            DropTable("dbo.PortalAtividadeChamados");
            DropTable("dbo.PortalChamados");
            DropTable("dbo.ChamadoAnexos");
        }
    }
}
