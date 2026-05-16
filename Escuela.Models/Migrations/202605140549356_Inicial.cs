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
                        idCURP = c.String(nullable: false, maxLength: 18),
                        Nombre = c.String(nullable: false),
                        PrimerApellido = c.String(),
                        SegundoApellido = c.String(),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Tutor = c.String(),
                        PrimerApellidoTutor = c.String(),
                        SegundoApellidoTutor = c.String(),
                        Direccion = c.String(),
                        ParentescoTutor = c.String(),
                        TelefonoTutor = c.String(),
                        GrupoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idCURP)
                .ForeignKey("dbo.Grupo", t => t.GrupoId, cascadeDelete: true)
                .Index(t => t.idCURP, unique: true)
                .Index(t => t.GrupoId);
            
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
                "dbo.Asignatura",
                c => new
                    {
                        AsignaturaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        UsuarioId = c.Int(nullable: false),
                        GrupoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AsignaturaId)
                .ForeignKey("dbo.Grupo", t => t.GrupoId, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.GrupoId);
            
            CreateTable(
                "dbo.Calificacion",
                c => new
                    {
                        CalificacionId = c.Int(nullable: false, identity: true),
                        IdCURP = c.String(maxLength: 18),
                        AsignaturaId = c.Int(nullable: false),
                        PrimerTrimestre = c.Int(nullable: false),
                        SegundoTrimestre = c.Int(nullable: false),
                        TercerTrimestre = c.Int(nullable: false),
                        CalificacionFinal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CalificacionId)
                .ForeignKey("dbo.Alumno", t => t.IdCURP,cascadeDelete:true)
                .ForeignKey("dbo.Asignatura", t => t.AsignaturaId, cascadeDelete: true)
                .Index(t => t.IdCURP)
                .Index(t => t.AsignaturaId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        UsuarioId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Correo = c.String(),
                        Contrasena = c.String(),
                        TipoUsuario = c.String(),
                    })
                .PrimaryKey(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Asistencia",
                c => new
                    {
                        AsistenciaId = c.Int(nullable: false, identity: true),
                        IdCURP = c.String(maxLength: 18),
                        Fecha = c.DateTime(nullable: false),
                        Estado = c.String(),
                    })
                .PrimaryKey(t => t.AsistenciaId)
                .ForeignKey("dbo.Alumno", t => t.IdCURP,cascadeDelete: true)
                .Index(t => t.IdCURP);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Asistencia", "IdCURP", "dbo.Alumno");
            DropForeignKey("dbo.Asignatura", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Asignatura", "GrupoId", "dbo.Grupo");
            DropForeignKey("dbo.Calificacion", "AsignaturaId", "dbo.Asignatura");
            DropForeignKey("dbo.Calificacion", "IdCURP", "dbo.Alumno");
            DropForeignKey("dbo.Alumno", "GrupoId", "dbo.Grupo");
            DropIndex("dbo.Asistencia", new[] { "IdCURP" });
            DropIndex("dbo.Calificacion", new[] { "AsignaturaId" });
            DropIndex("dbo.Calificacion", new[] { "IdCURP" });
            DropIndex("dbo.Asignatura", new[] { "GrupoId" });
            DropIndex("dbo.Asignatura", new[] { "UsuarioId" });
            DropIndex("dbo.Alumno", new[] { "GrupoId" });
            DropIndex("dbo.Alumno", new[] { "idCURP" });
            DropTable("dbo.Asistencia");
            DropTable("dbo.Usuario");
            DropTable("dbo.Calificacion");
            DropTable("dbo.Asignatura");
            DropTable("dbo.Grupo");
            DropTable("dbo.Alumno");
        }
    }
}
