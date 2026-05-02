namespace Escuela.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alumno",
                c => new
                    {
                        AlumnoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        CURP = c.String(),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Tutor = c.String(),
                        TelefonoTutor = c.String(),
                        GrupoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AlumnoId)
                .ForeignKey("dbo.Grupo", t => t.GrupoId, cascadeDelete: true)
                .Index(t => t.GrupoId);
            
            CreateTable(
                "dbo.Asistencia",
                c => new
                    {
                        AsistenciaId = c.Int(nullable: false, identity: true),
                        AlumnoId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AsistenciaId)
                .ForeignKey("dbo.Alumno", t => t.AlumnoId, cascadeDelete: true)
                .Index(t => t.AlumnoId);
            
            CreateTable(
                "dbo.Calificacion",
                c => new
                    {
                        CalificacionId = c.Int(nullable: false, identity: true),
                        AlumnoId = c.Int(nullable: false),
                        AsignaturaId = c.Int(nullable: false),
                        PrimerTrimestre = c.Int(nullable: false),
                        SegundoTrimestre = c.Int(nullable: false),
                        TercerTrimestre = c.Int(nullable: false),
                        CalificacionFinal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CalificacionId)
                .ForeignKey("dbo.Alumno", t => t.AlumnoId, cascadeDelete: true)
                .ForeignKey("dbo.Asignatura", t => t.AsignaturaId, cascadeDelete: true)
                .Index(t => t.AlumnoId)
                .Index(t => t.AsignaturaId);
            
            CreateTable(
                "dbo.Asignatura",
                c => new
                    {
                        AsignaturaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        ProfesorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AsignaturaId)
                .ForeignKey("dbo.Profesor", t => t.ProfesorId, cascadeDelete: true)
                .Index(t => t.ProfesorId);
            
            CreateTable(
                "dbo.Profesor",
                c => new
                    {
                        ProfesorId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProfesorId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        UsuarioId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Correo = c.String(),
                        Contrasena = c.String(),
                    })
                .PrimaryKey(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Grupo",
                c => new
                    {
                        GrupoId = c.Int(nullable: false, identity: true),
                        Grado = c.String(),
                        Turno = c.String(),
                        CicloEscolar = c.String(),
                    })
                .PrimaryKey(t => t.GrupoId);
            
            CreateTable(
                "dbo.Direccion",
                c => new
                    {
                        DireccionId = c.Int(nullable: false, identity: true),
                        Calle = c.String(),
                        Ciudad = c.String(),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DireccionId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.UsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Direccion", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Alumno", "GrupoId", "dbo.Grupo");
            DropForeignKey("dbo.Profesor", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Asignatura", "ProfesorId", "dbo.Profesor");
            DropForeignKey("dbo.Calificacion", "AsignaturaId", "dbo.Asignatura");
            DropForeignKey("dbo.Calificacion", "AlumnoId", "dbo.Alumno");
            DropForeignKey("dbo.Asistencia", "AlumnoId", "dbo.Alumno");
            DropIndex("dbo.Direccion", new[] { "UsuarioId" });
            DropIndex("dbo.Profesor", new[] { "UsuarioId" });
            DropIndex("dbo.Asignatura", new[] { "ProfesorId" });
            DropIndex("dbo.Calificacion", new[] { "AsignaturaId" });
            DropIndex("dbo.Calificacion", new[] { "AlumnoId" });
            DropIndex("dbo.Asistencia", new[] { "AlumnoId" });
            DropIndex("dbo.Alumno", new[] { "GrupoId" });
            DropTable("dbo.Direccion");
            DropTable("dbo.Grupo");
            DropTable("dbo.Usuario");
            DropTable("dbo.Profesor");
            DropTable("dbo.Asignatura");
            DropTable("dbo.Calificacion");
            DropTable("dbo.Asistencia");
            DropTable("dbo.Alumno");
        }
    }
}
