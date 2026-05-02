namespace Escuela.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Restricciones : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Alumno", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Alumno", "CURP", c => c.String(nullable: false, maxLength: 18));
            CreateIndex("dbo.Alumno", "CURP", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Alumno", new[] { "CURP" });
            AlterColumn("dbo.Alumno", "CURP", c => c.String());
            AlterColumn("dbo.Alumno", "Nombre", c => c.String());
        }
    }
}
