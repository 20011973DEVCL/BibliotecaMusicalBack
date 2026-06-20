# Biblioteca Musical — Backend

API REST para administrar y consultar un catálogo musical personal: artistas, álbumes, canciones, géneros, colecciones, playlists e inventario físico/digital.

## Estado actual

El proyecto partía con el esquema PostgreSQL completo y un único endpoint de lectura de géneros. La API ahora incluye:

- consultas paginadas y filtrables de artistas, álbumes y canciones;
- consulta individual de los recursos principales;
- CRUD de géneros con borrado lógico;
- creación y borrado lógico de playlists, además de agregar, reordenar y quitar canciones;
- colecciones con sus álbumes;
- inventario de biblioteca con filtros por estado y soporte físico/digital;
- catálogos auxiliares (países, formatos, sellos, tipos de artista y ubicaciones);
- respuestas `404`, `400` y `ProblemDetails` consistentes;
- `CancellationToken` desde HTTP hasta PostgreSQL;
- pool de conexiones mediante `NpgsqlDataSource`;
- endpoint de salud y Swagger en desarrollo;
- restricciones e índices para mantener integridad y acelerar las consultas;
- pruebas unitarias reales para el servicio de géneros.

La autenticación, autorización, administración de usuarios/roles, auditoría y escrituras completas de artistas, álbumes, canciones, colecciones e inventario no forman parte todavía de esta versión. Las tablas existen, pero no se exponen endpoints inseguros antes de implementar JWT, hash de contraseñas y permisos.

## Tecnologías

- .NET 10 / ASP.NET Core
- PostgreSQL (compatible con Neon)
- Npgsql
- Swagger / OpenAPI
- xUnit y Shouldly
- Arquitectura por capas: `Api`, `Application`, `Domain`, `Infrastructure`, `Shared` y `Tests`

## Estructura

```text
BibliotecaMusicalBack.Api/             Controladores, pipeline HTTP y configuración
BibliotecaMusicalBack.Application/     Casos de uso y DTO de escritura
BibliotecaMusicalBack.Domain/          Entidades y contratos de repositorio
BibliotecaMusicalBack.Infrastructure/  Acceso parametrizado a PostgreSQL
BibliotecaMusicalBack.Shared/          Código transversal futuro
BibliotecaMusicalBack.Tests/           Pruebas automatizadas
database/                              Esquema, datos iniciales y verificaciones SQL
```

## Configuración local

Requisitos: SDK de .NET 10 y PostgreSQL 15 o superior.

1. Crea la base de datos y ejecuta los scripts en orden:

```bash
psql -d biblioteca_musical -f database/01_create_biblioteca_musical_schema.sql
psql -d biblioteca_musical -f database/01_seed_catalogos_base.sql
psql -d biblioteca_musical -f database/02_seed_artistas_albumes_canciones.sql
psql -d biblioteca_musical -f database/03_seed_biblioteca_playlists_usuario.sql
psql -d biblioteca_musical -f database/04_verificar_datos.sql
```

`01_create_biblioteca_musical_schema.sql` es destructivo: elimina y vuelve a crear las tablas. No debe ejecutarse sobre datos que necesites conservar sin respaldo.

2. Configura la conexión sin subir credenciales al repositorio. La opción recomendada es User Secrets:

```bash
dotnet user-secrets init --project BibliotecaMusicalBack.Api
dotnet user-secrets set \
  --project BibliotecaMusicalBack.Api \
  "ConnectionStrings:BibliotecaMusical" \
  "Host=localhost;Port=5432;Database=biblioteca_musical;Username=postgres;Password=TU_PASSWORD;SSL Mode=Prefer"
```

También puedes usar la variable de entorno equivalente:

```bash
export ConnectionStrings__BibliotecaMusical="Host=localhost;Port=5432;Database=biblioteca_musical;Username=postgres;Password=TU_PASSWORD;SSL Mode=Prefer"
```

El archivo `BibliotecaMusicalBack.Api/appsettings.Development.json` está ignorado por Git. Usa `appsettings.Development.example.json` como plantilla y nunca confirmes secretos reales.

3. Restaura, compila y ejecuta:

```bash
dotnet restore BibliotecaMusicalBack.slnx
dotnet build BibliotecaMusicalBack.slnx
dotnet run --project BibliotecaMusicalBack.Api
```

La API queda por defecto en `http://localhost:5171`. Swagger está disponible en `http://localhost:5171/swagger` cuando `ASPNETCORE_ENVIRONMENT=Development`.

## Endpoints

Todos los endpoints usan el prefijo `/api`, salvo salud. Los identificadores son enteros.

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/health` | Comprueba que el proceso HTTP está activo |
| `GET` | `/api/generos?soloActivos=true` | Lista géneros |
| `GET` | `/api/generos/{codigo}` | Obtiene un género |
| `POST` | `/api/generos` | Crea un género |
| `PUT` | `/api/generos/{codigo}` | Reemplaza los datos de un género |
| `DELETE` | `/api/generos/{codigo}` | Desactiva un género |
| `GET` | `/api/artistas` | Lista artistas con filtros y paginación |
| `GET` | `/api/artistas/{codigo}` | Obtiene un artista |
| `GET` | `/api/albums` | Lista álbumes con filtros y paginación |
| `GET` | `/api/albums/{codigo}` | Obtiene un álbum |
| `GET` | `/api/canciones` | Lista canciones con filtros y paginación |
| `GET` | `/api/canciones/{codigo}` | Obtiene una canción |
| `GET` | `/api/playlists` | Lista playlists |
| `GET` | `/api/playlists/{codigo}` | Obtiene una playlist y sus canciones |
| `POST` | `/api/playlists` | Crea una playlist |
| `PUT` | `/api/playlists/{codigo}/canciones` | Agrega una canción o actualiza su orden |
| `DELETE` | `/api/playlists/{codigo}/canciones/{cancionCodigo}` | Quita una canción |
| `DELETE` | `/api/playlists/{codigo}` | Desactiva una playlist |
| `GET` | `/api/colecciones` | Lista colecciones |
| `GET` | `/api/colecciones/{codigo}` | Obtiene una colección y sus álbumes |
| `GET` | `/api/biblioteca-items` | Lista ejemplares de la biblioteca |
| `GET` | `/api/catalogos/{catalogo}` | Lista un catálogo auxiliar |

Valores válidos de `{catalogo}`: `paises`, `formatos`, `sellos`, `tipos-artista` y `ubicaciones`.

### Filtros y paginación

- Artistas: `buscar`, `paisCodigo`, `soloActivos`, `pagina`, `tamanoPagina`.
- Álbumes: `buscar`, `artistaCodigo`, `generoCodigo`, `anio`, `soloActivos`, `pagina`, `tamanoPagina`.
- Canciones: `buscar`, `albumCodigo`, `artistaCodigo`, `generoCodigo`, `soloActivos`, `pagina`, `tamanoPagina`.
- Inventario: `estado`, `esFisico`, `soloActivos`.
- `tamanoPagina` acepta de 1 a 100 elementos; valores fuera del rango se normalizan.

Las listas paginadas retornan:

```json
{
  "items": [],
  "paginaActual": 1,
  "tamanoPagina": 20,
  "total": 0,
  "totalPaginas": 0
}
```

### Ejemplos

Crear un género:

```http
POST /api/generos
Content-Type: application/json

{
  "nombre": "Ambient",
  "descripcion": "Música atmosférica",
  "activo": true
}
```

Crear una playlist y agregar una canción:

```http
POST /api/playlists
Content-Type: application/json

{
  "nombre": "Para programar",
  "descripcion": "Concentración sin sobresaltos"
}
```

```http
PUT /api/playlists/1/canciones
Content-Type: application/json

{
  "cancionCodigo": 3,
  "orden": 1
}
```

Más ejemplos ejecutables están en `BibliotecaMusicalBack.Api/BibliotecaMusicalBack.Api.http`.

## Pruebas

```bash
dotnet test BibliotecaMusicalBack.slnx
```

Las pruebas actuales cubren mapeo, normalización de datos y recursos inexistentes del servicio de géneros. El siguiente incremento recomendable es añadir pruebas de integración con PostgreSQL efímero para validar consultas, restricciones y códigos HTTP de punta a punta.

## Decisiones y mejoras aplicadas

- Todas las consultas usan parámetros; no interpolan datos proporcionados por el cliente.
- Los endpoints de colección filtran registros activos de forma predeterminada, pero permiten incluir inactivos.
- Los borrados de géneros y playlists son lógicos para preservar relaciones e historial.
- El esquema agrega claves únicas para que los seeds sean repetibles y restricciones para fechas, duraciones, pistas, años y cantidad de discos.
- Se agregaron índices sobre claves foráneas y columnas usadas por las relaciones principales.
- La conexión se crea una vez como `NpgsqlDataSource`, que administra pooling y evita abrir infraestructura nueva por solicitud.
- Se retiraron dependencias de autenticación y validación que estaban instaladas pero no configuradas; deben volver únicamente junto con su implementación completa.

## Próximos pasos recomendados

1. Implementar registro/inicio de sesión con hash Argon2id o bcrypt, JWT de corta duración y refresh tokens.
2. Aplicar autorización por roles y proteger todas las operaciones de escritura.
3. Completar comandos CRUD de artistas, álbumes, canciones, colecciones e inventario con transacciones.
4. Incorporar migraciones versionadas (por ejemplo, DbUp o FluentMigrator) en lugar de recrear el esquema.
5. Añadir pruebas de integración, observabilidad y un health check de disponibilidad de PostgreSQL.
6. Versionar la API (`/api/v1`) antes de publicar clientes estables.

## Nota de seguridad

El seed de usuario contiene `CAMBIAR_PASSWORD_HASH`; no es una contraseña válida ni debe usarse en producción. Antes de habilitar autenticación hay que generar hashes reales desde la aplicación. Si alguna credencial real fue compartida o confirmada por error, debe rotarse en el proveedor de base de datos.
