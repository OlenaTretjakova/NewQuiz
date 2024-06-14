namespace MyQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTwoColumInResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "NumGetingBalls", c => c.Int(nullable: false));
            AddColumn("dbo.Results", "NumMaxBalls", c => c.Int(nullable: false));
            AddColumn("dbo.Results", "LastDatePassing", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "LastDatePassing");
            DropColumn("dbo.Results", "NumMaxBalls");
            DropColumn("dbo.Results", "NumGetingBalls");
        }
    }
}
