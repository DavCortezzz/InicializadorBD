namespace Escuela.Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Escuela.Models.EscuelaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Escuela.Models.EscuelaContext context)
        {
            context.Usuarios.AddOrUpdate(u => u.Correo,
                new Usuario { UsuarioId = 1, Nombre = "Admin Dirección", Correo = "admin@escuela.com", Contrasena = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", TipoUsuario = "Direccion" },
                new Usuario { UsuarioId = 2, Nombre = "Example", Correo = "ray@example.com", Contrasena = "d07164a628596323ebcf8796dee0e5c164620e0922b52483bc805f54416ee73c", TipoUsuario = "Direccion" },
                new Usuario { UsuarioId = 3, Nombre = "Prof. Newton", Correo = "newton@escuela.com", Contrasena = "790fad4744a9da8d301e23c83b386f1baabd229611dbe5265df62a97b5357393", TipoUsuario = "Profesor" }
            );

            context.Grupos.AddOrUpdate(g => g.GrupoId,
                new Grupo { GrupoId = 1, Grado = "1roA", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 2, Grado = "1roB", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 3, Grado = "2doA", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 4, Grado = "2doB", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 5, Grado = "3roA", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 6, Grado = "3roB", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 7, Grado = "4toA", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 8, Grado = "4toB", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 9, Grado = "5toA", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 10, Grado = "5toB", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 11, Grado = "6toA", Turno = "Matutino", CicloEscolar = "2025-2026" },
                new Grupo { GrupoId = 12, Grado = "6toB", Turno = "Matutino", CicloEscolar = "2025-2026" }
            );

            context.SaveChanges();

            context.Asignaturas.AddOrUpdate(a => a.Nombre,
                new Asignatura { AsignaturaId = 1, Nombre = "Física I", UsuarioId = 2, GrupoId = 1 },
                new Asignatura { AsignaturaId = 2, Nombre = "Matemáticas II", UsuarioId = 2, GrupoId = 2 }
            );
            context.SaveChanges();
            context.Alumnos.AddOrUpdate(a => a.idCURP,


        new Alumno { idCURP = "GOMA190101HDFRRN01", Nombre = "Marcos", PrimerApellido = "Gomez", SegundoApellido = "Alba", FechaNacimiento = new DateTime(2019, 1, 1), Tutor = "Elena", PrimerApellidoTutor = "Alba", SegundoApellidoTutor = "Castro", ParentescoTutor = "Madre", Direccion = "Calle 1 #123", TelefonoTutor = "6671000001", GrupoId = 1 },
        new Alumno { idCURP = "LOPS190201MDFRRN02", Nombre = "Sofia", PrimerApellido = "Lopez", SegundoApellido = "Sanz", FechaNacimiento = new DateTime(2019, 2, 1), Tutor = "Pedro", PrimerApellidoTutor = "Lopez", SegundoApellidoTutor = "Mendoza", ParentescoTutor = "Padre", Direccion = "Calle 2 #456", TelefonoTutor = "6671000002", GrupoId = 1 },

        new Alumno { idCURP = "RUIZ190301HDFRRN03", Nombre = "Raul", PrimerApellido = "Ruiz", SegundoApellido = "Diaz", FechaNacimiento = new DateTime(2019, 3, 1), Tutor = "Ana", PrimerApellidoTutor = "Diaz", SegundoApellidoTutor = "Reyes", ParentescoTutor = "Madre", Direccion = "Avenida Central 10", TelefonoTutor = "6671000003", GrupoId = 2 },
        new Alumno { idCURP = "MORL190401MDFRRN04", Nombre = "Lucia", PrimerApellido = "Morales", SegundoApellido = "Luna", FechaNacimiento = new DateTime(2019, 4, 1), Tutor = "Ivan", PrimerApellidoTutor = "Morales", SegundoApellidoTutor = "Villalba", ParentescoTutor = "Padre", Direccion = "Privada Cedros 5", TelefonoTutor = "6671000004", GrupoId = 2 },

        new Alumno { idCURP = "HERJ180501HDFRRN05", Nombre = "Jorge", PrimerApellido = "Hernandez", SegundoApellido = "Jimenez", FechaNacimiento = new DateTime(2018, 5, 1), Tutor = "Rosa", PrimerApellidoTutor = "Jimenez", SegundoApellidoTutor = "Solis", ParentescoTutor = "Madre", Direccion = "Calle Morelos 88", TelefonoTutor = "6671000005", GrupoId = 3 },
        new Alumno { idCURP = "CAST180601MDFRRN06", Nombre = "Tania", PrimerApellido = "Castro", SegundoApellido = "Solis", FechaNacimiento = new DateTime(2018, 6, 1), Tutor = "Luis", PrimerApellidoTutor = "Castro", SegundoApellidoTutor = "Perez", ParentescoTutor = "Padre", Direccion = "Blvd Libertad 200", TelefonoTutor = "6671000006", GrupoId = 3 },

        new Alumno { idCURP = "VALA180701HDFRRN07", Nombre = "Arturo", PrimerApellido = "Valdez", SegundoApellido = "Arce", FechaNacimiento = new DateTime(2018, 7, 1), Tutor = "Sonia", PrimerApellidoTutor = "Arce", SegundoApellidoTutor = "Meza", ParentescoTutor = "Madre", Direccion = "Calle Rio 12", TelefonoTutor = "6671000007", GrupoId = 4 },
        new Alumno { idCURP = "FERM180801MDFRRN08", Nombre = "Marta", PrimerApellido = "Fernandez", SegundoApellido = "Meza", FechaNacimiento = new DateTime(2018, 8, 1), Tutor = "Hugo", PrimerApellidoTutor = "Fernandez", SegundoApellidoTutor = "Gala", ParentescoTutor = "Padre", Direccion = "Calle Pino 44", TelefonoTutor = "6671000008", GrupoId = 4 },

        new Alumno { idCURP = "OSOR170901HDFRRN09", Nombre = "Roberto", PrimerApellido = "Osorio", SegundoApellido = "Rojas", FechaNacimiento = new DateTime(2017, 9, 1), Tutor = "Carmen", PrimerApellidoTutor = "Rojas", SegundoApellidoTutor = "Ibarra", ParentescoTutor = "Madre", Direccion = "Colonia Maya 1", TelefonoTutor = "6671000009", GrupoId = 5 },
        new Alumno { idCURP = "MENA171001MDFRRN10", Nombre = "Alicia", PrimerApellido = "Mendez", SegundoApellido = "Aguilar", FechaNacimiento = new DateTime(2017, 10, 1), Tutor = "Saul", PrimerApellidoTutor = "Mendez", SegundoApellidoTutor = "Sosa", ParentescoTutor = "Padre", Direccion = "Colonia Maya 2", TelefonoTutor = "6671000010", GrupoId = 5 },

        new Alumno { idCURP = "RAMG171101HDFRRN11", Nombre = "Gustavo", PrimerApellido = "Ramirez", SegundoApellido = "Gila", FechaNacimiento = new DateTime(2017, 11, 1), Tutor = "Silvia", PrimerApellidoTutor = "Gila", SegundoApellidoTutor = "Torres", ParentescoTutor = "Madre", Direccion = "Calle Norte 5", TelefonoTutor = "6671000011", GrupoId = 6 },
        new Alumno { idCURP = "SANT171201MDFRRN12", Nombre = "Brenda", PrimerApellido = "Sanchez", SegundoApellido = "Torres", FechaNacimiento = new DateTime(2017, 12, 1), Tutor = "Felipe", PrimerApellidoTutor = "Sanchez", SegundoApellidoTutor = "Luna", ParentescoTutor = "Padre", Direccion = "Calle Sur 9", TelefonoTutor = "6671000012", GrupoId = 6 },

        new Alumno { idCURP = "VILP160101HDFRRN13", Nombre = "Pablo", PrimerApellido = "Villalba", SegundoApellido = "Pena", FechaNacimiento = new DateTime(2016, 1, 1), Tutor = "Laura", PrimerApellidoTutor = "Pena", SegundoApellidoTutor = "Vigo", ParentescoTutor = "Madre", Direccion = "Sector 7G", TelefonoTutor = "6671000013", GrupoId = 7 },
        new Alumno { idCURP = "NAVY160201MDFRRN14", Nombre = "Yolanda", PrimerApellido = "Navarro", SegundoApellido = "Vigo", FechaNacimiento = new DateTime(2016, 2, 1), Tutor = "Oscar", PrimerApellidoTutor = "Navarro", SegundoApellidoTutor = "Ramos", ParentescoTutor = "Padre", Direccion = "Sector 8H", TelefonoTutor = "6671000014", GrupoId = 7 },

        new Alumno { idCURP = "GUEM160301HDFRRN15", Nombre = "Mario", PrimerApellido = "Guerrero", SegundoApellido = "Mota", FechaNacimiento = new DateTime(2016, 3, 1), Tutor = "Rosa", PrimerApellidoTutor = "Mota", SegundoApellidoTutor = "Domenech", ParentescoTutor = "Madre", Direccion = "Av. Las Torres", TelefonoTutor = "6671000015", GrupoId = 8 },
        new Alumno { idCURP = "DOMK160401MDFRRN16", Nombre = "Karla", PrimerApellido = "Dominguez", SegundoApellido = "Kuri", FechaNacimiento = new DateTime(2016, 4, 1), Tutor = "Jose", PrimerApellidoTutor = "Dominguez", SegundoApellidoTutor = "Kuri", ParentescoTutor = "Padre", Direccion = "Av. Los Sauces", TelefonoTutor = "6671000016", GrupoId = 8 },

        new Alumno { idCURP = "AGUF150501HDFRRN17", Nombre = "Fernando", PrimerApellido = "Aguirre", SegundoApellido = "Frias", FechaNacimiento = new DateTime(2015, 5, 1), Tutor = "Maura", PrimerApellidoTutor = "Frias", SegundoApellidoTutor = "Sosa", ParentescoTutor = "Madre", Direccion = "Calle Hidalgo 1", TelefonoTutor = "6671000017", GrupoId = 9 },
        new Alumno { idCURP = "IBAS150601MDFRRN18", Nombre = "Sara", PrimerApellido = "Ibarra", SegundoApellido = "Sosa", FechaNacimiento = new DateTime(2015, 6, 1), Tutor = "Tomas", PrimerApellidoTutor = "Ibarra", SegundoApellidoTutor = "Soria", ParentescoTutor = "Padre", Direccion = "Calle Juarez 2", TelefonoTutor = "6671000018", GrupoId = 9 },

        new Alumno { idCURP = "PARL150701HDFRRN19", Nombre = "Luis", PrimerApellido = "Parra", SegundoApellido = "Lara", FechaNacimiento = new DateTime(2015, 7, 1), Tutor = "Ines", PrimerApellidoTutor = "Lara", SegundoApellidoTutor = "Reyes", ParentescoTutor = "Madre", Direccion = "Residencial A", TelefonoTutor = "6671000019", GrupoId = 10 },
        new Alumno { idCURP = "REYE150801MDFRRN20", Nombre = "Elena", PrimerApellido = "Reyes", SegundoApellido = "Espinosa", FechaNacimiento = new DateTime(2015, 8, 1), Tutor = "Luis", PrimerApellidoTutor = "Reyes", SegundoApellidoTutor = "Duarte", ParentescoTutor = "Padre", Direccion = "Residencial B", TelefonoTutor = "6671000020", GrupoId = 10 },

        new Alumno { idCURP = "FLOH140901HDFRRN21", Nombre = "Hugo", PrimerApellido = "Flores", SegundoApellido = "Haro", FechaNacimiento = new DateTime(2014, 9, 1), Tutor = "Paola", PrimerApellidoTutor = "Haro", SegundoApellidoTutor = "Zazueta", ParentescoTutor = "Madre", Direccion = "Barrio San Juan", TelefonoTutor = "6671000021", GrupoId = 11 },
        new Alumno { idCURP = "MARI141001MDFRRN22", Nombre = "Irene", PrimerApellido = "Martinez", SegundoApellido = "Islas", FechaNacimiento = new DateTime(2014, 10, 1), Tutor = "Juan", PrimerApellidoTutor = "Martinez", SegundoApellidoTutor = "Esparza", ParentescoTutor = "Padre", Direccion = "Barrio San Pedro", TelefonoTutor = "6671000022", GrupoId = 11 },

        new Alumno { idCURP = "URIV141101HDFRRN23", Nombre = "Victor", PrimerApellido = "Uribe", SegundoApellido = "Vaca", FechaNacimiento = new DateTime(2014, 11, 1), Tutor = "Lola", PrimerApellidoTutor = "Vaca", SegundoApellidoTutor = "Galindo", ParentescoTutor = "Madre", Direccion = "Final de Calle 99", TelefonoTutor = "6671000023", GrupoId = 12 },
        new Alumno { idCURP = "ZAMX141201MDFRRN24", Nombre = "Ximena", PrimerApellido = "Zamora", SegundoApellido = "Mora", FechaNacimiento = new DateTime(2014, 12, 1), Tutor = "Oscar", PrimerApellidoTutor = "Zamora", SegundoApellidoTutor = "Tiznado", ParentescoTutor = "Padre", Direccion = "Final de Calle 100", TelefonoTutor = "6671000024", GrupoId = 12 }
        );
            context.SaveChanges();
        }
    }
}
