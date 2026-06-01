CREATE TABLE IF NOT EXISTS paises (
    pais_id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    codigo_iso VARCHAR(3),
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS generos (
    genero_id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(300),
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS artistas (
    artista_id SERIAL PRIMARY KEY,
    pais_id INTEGER REFERENCES paises(pais_id),
    nombre VARCHAR(150) NOT NULL,
    nombre_real VARCHAR(150),
    fecha_nacimiento DATE,
    fecha_fallecimiento DATE,
    biografia TEXT,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS tipos_artista (
    tipo_artista_id SERIAL PRIMARY KEY,
    nombre VARCHAR(80) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS artistas_tipos (
    artista_tipo_id SERIAL PRIMARY KEY,
    artista_id INTEGER NOT NULL REFERENCES artistas(artista_id),
    tipo_artista_id INTEGER NOT NULL REFERENCES tipos_artista(tipo_artista_id),
    UNIQUE (artista_id, tipo_artista_id)
);

CREATE TABLE IF NOT EXISTS sellos_discograficos (
    sello_id SERIAL PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL UNIQUE,
    pais_id INTEGER REFERENCES paises(pais_id),
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS formatos_musicales (
    formato_id SERIAL PRIMARY KEY,
    nombre VARCHAR(80) NOT NULL UNIQUE,
    descripcion VARCHAR(200),
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS albums (
    album_id SERIAL PRIMARY KEY,
    artista_id INTEGER NOT NULL REFERENCES artistas(artista_id),
    genero_id INTEGER REFERENCES generos(genero_id),
    sello_id INTEGER REFERENCES sellos_discograficos(sello_id),
    formato_id INTEGER REFERENCES formatos_musicales(formato_id),
    titulo VARCHAR(200) NOT NULL,
    anio_lanzamiento INTEGER,
    fecha_lanzamiento DATE,
    numero_discos INTEGER NOT NULL DEFAULT 1,
    portada_url TEXT,
    observacion TEXT,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS canciones (
    cancion_id SERIAL PRIMARY KEY,
    album_id INTEGER REFERENCES albums(album_id),
    genero_id INTEGER REFERENCES generos(genero_id),
    titulo VARCHAR(200) NOT NULL,
    duracion_segundos INTEGER,
    numero_pista INTEGER,
    letra TEXT,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS canciones_artistas (
    cancion_artista_id SERIAL PRIMARY KEY,
    cancion_id INTEGER NOT NULL REFERENCES canciones(cancion_id),
    artista_id INTEGER NOT NULL REFERENCES artistas(artista_id),
    es_principal BOOLEAN NOT NULL DEFAULT FALSE,
    UNIQUE (cancion_id, artista_id)
);

CREATE TABLE IF NOT EXISTS compositores (
    compositor_id SERIAL PRIMARY KEY,
    nombres VARCHAR(150) NOT NULL,
    apellidos VARCHAR(150),
    pais_id INTEGER REFERENCES paises(pais_id),
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS canciones_compositores (
    cancion_compositor_id SERIAL PRIMARY KEY,
    cancion_id INTEGER NOT NULL REFERENCES canciones(cancion_id),
    compositor_id INTEGER NOT NULL REFERENCES compositores(compositor_id),
    UNIQUE (cancion_id, compositor_id)
);

CREATE TABLE IF NOT EXISTS colecciones (
    coleccion_id SERIAL PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL UNIQUE,
    descripcion VARCHAR(300),
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS coleccion_albums (
    coleccion_album_id SERIAL PRIMARY KEY,
    coleccion_id INTEGER NOT NULL REFERENCES colecciones(coleccion_id),
    album_id INTEGER NOT NULL REFERENCES albums(album_id),
    fecha_agregado TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (coleccion_id, album_id)
);

CREATE TABLE IF NOT EXISTS ubicaciones_fisicas (
    ubicacion_id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(300),
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS biblioteca_items (
    item_id SERIAL PRIMARY KEY,
    album_id INTEGER NOT NULL REFERENCES albums(album_id),
    ubicacion_id INTEGER REFERENCES ubicaciones_fisicas(ubicacion_id),
    codigo_interno VARCHAR(50) UNIQUE,
    codigo_barra VARCHAR(80),
    es_fisico BOOLEAN NOT NULL DEFAULT TRUE,
    estado VARCHAR(50) NOT NULL DEFAULT 'DISPONIBLE',
    fecha_compra DATE,
    precio_compra NUMERIC(12,2),
    observacion TEXT,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS playlists (
    playlist_id SERIAL PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    descripcion VARCHAR(300),
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS playlist_canciones (
    playlist_cancion_id SERIAL PRIMARY KEY,
    playlist_id INTEGER NOT NULL REFERENCES playlists(playlist_id),
    cancion_id INTEGER NOT NULL REFERENCES canciones(cancion_id),
    orden INTEGER,
    fecha_agregado TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (playlist_id, cancion_id)
);

CREATE TABLE IF NOT EXISTS usuarios (
    usuario_id SERIAL PRIMARY KEY,
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS roles (
    rol_id SERIAL PRIMARY KEY,
    nombre VARCHAR(80) NOT NULL UNIQUE,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS usuarios_roles (
    usuario_rol_id SERIAL PRIMARY KEY,
    usuario_id INTEGER NOT NULL REFERENCES usuarios(usuario_id),
    rol_id INTEGER NOT NULL REFERENCES roles(rol_id),
    UNIQUE (usuario_id, rol_id)
);

CREATE TABLE IF NOT EXISTS auditoria (
    auditoria_id SERIAL PRIMARY KEY,
    usuario_id INTEGER REFERENCES usuarios(usuario_id),
    tabla VARCHAR(100) NOT NULL,
    accion VARCHAR(30) NOT NULL,
    registro_id INTEGER,
    detalle TEXT,
    fecha_evento TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);