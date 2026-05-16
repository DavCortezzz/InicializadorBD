namespace Escuela.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "CodigoVerificacion", c => c.String(nullable: true));
            AddColumn("dbo.Usuario", "FechaExpiracionCodigo", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "FechaExpiracionCodigo");
            DropColumn("dbo.Usuario", "CodigoVerificacion");
        }
    }
}
