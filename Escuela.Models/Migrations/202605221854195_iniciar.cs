namespace Escuela.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iniciar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Asignatura", "UsuarioId", "dbo.Usuario");
            AddForeignKey("dbo.Asignatura", "UsuarioId", "dbo.Usuario", "UsuarioId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Asignatura", "UsuarioId", "dbo.Usuario");
            AddForeignKey("dbo.Asignatura", "UsuarioId", "dbo.Usuario", "UsuarioId", cascadeDelete: true);
        }
    }
}
