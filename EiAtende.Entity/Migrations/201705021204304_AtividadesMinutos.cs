namespace EiAtende.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtividadesMinutos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortalAtividadeChamados", "PrevisaoMinutos", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PortalAtividadeChamados", "PrevisaoMinutos");
        }
    }
}
