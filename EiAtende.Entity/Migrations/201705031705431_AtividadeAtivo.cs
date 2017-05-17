namespace EiAtende.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtividadeAtivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortalAtividadeChamados", "Ativo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PortalAtividadeChamados", "Ativo");
        }
    }
}
