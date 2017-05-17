namespace EiAtende.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParaStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortalChamados", "paraStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PortalChamados", "paraStatus");
        }
    }
}
