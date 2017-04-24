namespace EiAtende.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtividadesPrevisaoDias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortalAtividadeChamados", "PrevisaoDias", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PortalAtividadeChamados", "PrevisaoDias");
        }
    }
}
