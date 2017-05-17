namespace EiAtende.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrevisaoHoras : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortalAtividadeChamados", "PrevisaoHoras", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PortalAtividadeChamados", "PrevisaoHoras");
        }
    }
}
