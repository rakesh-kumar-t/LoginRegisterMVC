namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relatin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.usertests", "UserId", "dbo.Users");
            DropIndex("dbo.usertests", new[] { "UserId" });
            DropTable("dbo.usertests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.usertests",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        desc = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateIndex("dbo.usertests", "UserId");
            AddForeignKey("dbo.usertests", "UserId", "dbo.Users", "UserId");
        }
    }
}
