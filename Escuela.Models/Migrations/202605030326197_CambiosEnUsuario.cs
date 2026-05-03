namespace Escuela.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosEnUsuario : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Asignatura", "ProfesorId", "dbo.Profesor");
            DropForeignKey("dbo.Profesor", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Direccion", "UsuarioId", "dbo.Usuario");
            DropIndex("dbo.Asignatura", new[] { "ProfesorId" });
            DropIndex("dbo.Profesor", new[] { "UsuarioId" });
            DropIndex("dbo.Direccion", new[] { "UsuarioId" });
            AddColumn("dbo.Asignatura", "Usuario_UsuarioId", c => c.Int());
            AddColumn("dbo.Usuario", "TipoUsuario", c => c.String());
            CreateIndex("dbo.Asignatura", "Usuario_UsuarioId");
            AddForeignKey("dbo.Asignatura", "Usuario_UsuarioId", "dbo.Usuario", "UsuarioId");
            DropTable("dbo.Profesor");
            DropTable("dbo.Direccion");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Direccion",
                c => new
                    {
                        DireccionId = c.Int(nullable: false, identity: true),
                        Calle = c.String(),
                        Ciudad = c.String(),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DireccionId);
            
            CreateTable(
                "dbo.Profesor",
                c => new
                    {
                        ProfesorId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProfesorId);
            
            DropForeignKey("dbo.Asignatura", "Usuario_UsuarioId", "dbo.Usuario");
            DropIndex("dbo.Asignatura", new[] { "Usuario_UsuarioId" });
            DropColumn("dbo.Usuario", "TipoUsuario");
            DropColumn("dbo.Asignatura", "Usuario_UsuarioId");
            CreateIndex("dbo.Direccion", "UsuarioId");
            CreateIndex("dbo.Profesor", "UsuarioId");
            CreateIndex("dbo.Asignatura", "ProfesorId");
            AddForeignKey("dbo.Direccion", "UsuarioId", "dbo.Usuario", "UsuarioId", cascadeDelete: true);
            AddForeignKey("dbo.Profesor", "UsuarioId", "dbo.Usuario", "UsuarioId", cascadeDelete: true);
            AddForeignKey("dbo.Asignatura", "ProfesorId", "dbo.Profesor", "ProfesorId", cascadeDelete: true);
        }
    }
}
