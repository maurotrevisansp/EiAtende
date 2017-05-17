namespace EiAtende.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoChamadoAtivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortalTipoChamados", "Ativo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PortalTipoChamados", "Ativo");
        }
    }
}
