# Biblioteca Musical Back

API REST para administrar el catálogo, la colección física/digital, playlists y usuarios de una biblioteca musical. La solución usa .NET 10, ASP.NET Core, PostgreSQL/Neon, Npgsql, Swagger y xUnit.

## Estado actual

La implementación inicial sólo consultaba géneros mediante `sp_obtener_generos()`. La solución ahora cubre las 22 tablas definidas en `database/01_create_biblioteca_musical_schema.sql` y mantiene un flujo común entre las capas:

```text
HTTP Controller -> Application Service -> Domain Repository Contract
                -> Infrastructure/Npgsql -> PostgreSQL
```

- `BibliotecaMusicalBack.Api`: controladores, rutas, Swagger, manejo de errores e inyección de dependencias.
- `BibliotecaMusicalBack.Application`: DTOs, servicios CRUD y tratamiento seguro de contraseñas.
- `BibliotecaMusicalBack.Domain`: entidades, metadatos de columnas y contratos de repositorio.
- `BibliotecaMusicalBack.Infrastructure`: repositorio CRUD genérico y acceso parametrizado con Npgsql.
- `BibliotecaMusicalBack.Tests`: pruebas unitarias de respuestas HTTP, contratos de rutas y seguridad de usuarios.
- `database`: creación del esquema, datos iniciales y consultas de verificación.

El repositorio genérico usa `[Table]`, `[Column]`, `[Key]` y `[DatabaseGenerated]` para construir SQL parametrizado. Las columnas identity o calculadas no se envían en `INSERT`/`UPDATE`, pero sí se devuelven en las consultas.

## Configuración

No se debe guardar la contraseña de PostgreSQL en Git. Configure la conexión con User Secrets:

```powershell
cd BibliotecaMusicalBack.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:BibliotecaMusical" "Host=HOST;Port=5432;Database=DATABASE;Username=USER;Password=PASSWORD;SSL Mode=Require"
```

También puede usar la variable de entorno:

```powershell
$env:ConnectionStrings__BibliotecaMusical = "Host=HOST;Port=5432;Database=DATABASE;Username=USER;Password=PASSWORD;SSL Mode=Require"
```

Para crear y poblar la base de datos, ejecute en orden:

1. `database/01_create_biblioteca_musical_schema.sql`
2. `database/01_seed_catalogos_base.sql`
3. `database/02_seed_artistas_albumes_canciones.sql`
4. `database/03_seed_biblioteca_playlists_usuario.sql`
5. `database/04_verificar_datos.sql`

## Ejecución

```powershell
dotnet restore BibliotecaMusicalBack.slnx --configfile NuGet.Config
dotnet run --project BibliotecaMusicalBack.Api
```

En desarrollo, Swagger queda disponible en `http://localhost:5171/swagger`. El archivo `BibliotecaMusicalBack.Api/BibliotecaMusicalBack.Api.http` contiene solicitudes de ejemplo.

## Convención de endpoints

Todos los recursos exponen el mismo contrato:

| Método | Ruta | Resultado |
|---|---|---|
| `GET` | `/api/{recurso}` | Lista completa (`200`) |
| `GET` | `/api/{recurso}/{id}` | Recurso (`200`) o inexistente (`404`) |
| `POST` | `/api/{recurso}` | Recurso creado y cabecera `Location` (`201`) |
| `PUT` | `/api/{recurso}/{id}` | Actualizado (`204`) o inexistente (`404`) |
| `DELETE` | `/api/{recurso}/{id}` | Eliminado (`204`) o inexistente (`404`) |

Recursos disponibles:

- Catálogos: `paises`, `generos`, `tipos-artista`, `sellos-discograficos`, `formatos-musicales`, `ubicaciones-fisicas`, `roles`.
- Música: `artistas`, `artistas-tipos`, `albums`, `canciones`, `canciones-artistas`, `compositores`, `canciones-compositores`.
- Biblioteca: `colecciones`, `colecciones-albums`, `biblioteca-items`, `playlists`, `playlists-canciones`.
- Seguridad: `usuarios`, `usuarios-roles`, `auditoria`.

Las eliminaciones son físicas. PostgreSQL rechazará una eliminación cuando existan relaciones que la impidan; primero deben eliminarse o reasignarse los registros dependientes.

## Usuarios y contraseñas

`POST /api/usuarios` exige `password`. `PUT /api/usuarios/{id}` conserva la contraseña actual cuando `password` viene vacío o nulo. La aplicación almacena un hash PBKDF2-SHA256 con salt aleatorio y nunca incluye `usuPasswordHash` en las respuestas.

Ejemplo:

```json
{
  "usuNombres": "Ana",
  "usuApellidos": "Pérez",
  "usuEmail": "ana@example.com",
  "password": "una-clave-segura",
  "usuActivo": true
}
```

La autenticación JWT y la autorización por roles no forman parte de esta entrega; antes de publicar la API se deben proteger especialmente los endpoints de `usuarios`, `roles`, `usuarios-roles` y `auditoria`.

## Pruebas y compilación

```powershell
dotnet build BibliotecaMusicalBack.slnx --no-restore
dotnet test BibliotecaMusicalBack.slnx --no-build --no-restore
```

Las pruebas verifican:

- respuestas `200`, `201`, `204` y `404`;
- presencia de los cinco endpoints CRUD en cada controlador;
- rutas y atributos HTTP de los 22 recursos;
- creación de usuarios con PBKDF2 y ausencia del hash en el DTO de salida.

## Dependencias

Se retiraron referencias que no tenían uso (`FluentValidation.AspNetCore`, `FluentValidation`, `JwtBearer` y la referencia directa a `Microsoft.AspNetCore.OpenApi`). Se mantienen únicamente los paquetes necesarios para Swagger, Npgsql y pruebas.
