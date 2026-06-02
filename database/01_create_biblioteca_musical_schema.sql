DROP TABLE IF EXISTS auditoria CASCADE;
DROP TABLE IF EXISTS usuarios_roles CASCADE;
DROP TABLE IF EXISTS roles CASCADE;
DROP TABLE IF EXISTS usuarios CASCADE;
DROP TABLE IF EXISTS playlist_canciones CASCADE;
DROP TABLE IF EXISTS playlists CASCADE;
DROP TABLE IF EXISTS biblioteca_items CASCADE;
DROP TABLE IF EXISTS ubicaciones_fisicas CASCADE;
DROP TABLE IF EXISTS coleccion_albums CASCADE;
DROP TABLE IF EXISTS colecciones CASCADE;
DROP TABLE IF EXISTS canciones_compositores CASCADE;
DROP TABLE IF EXISTS compositores CASCADE;
DROP TABLE IF EXISTS canciones_artistas CASCADE;
DROP TABLE IF EXISTS canciones CASCADE;
DROP TABLE IF EXISTS albums CASCADE;
DROP TABLE IF EXISTS formatos_musicales CASCADE;
DROP TABLE IF EXISTS sellos_discograficos CASCADE;
DROP TABLE IF EXISTS artistas_tipos CASCADE;
DROP TABLE IF EXISTS tipos_artista CASCADE;
DROP TABLE IF EXISTS artistas CASCADE;
DROP TABLE IF EXISTS generos CASCADE;
DROP TABLE IF EXISTS paises CASCADE;

DROP FUNCTION IF EXISTS sp_obtener_generos();

CREATE TABLE paises (
    pais_codigo SERIAL PRIMARY KEY,
    pais_nombre VARCHAR(100) NOT NULL UNIQUE,
    pais_codigo_iso VARCHAR(3),
    pais_activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE generos (
    gen_codigo SERIAL PRIMARY KEY,
    gen_nombre VARCHAR(100) NOT NULL UNIQUE,
    gen_descripcion VARCHAR(300),
    gen_activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE artistas (
    art_codigo SERIAL PRIMARY KEY,
    pais_codigo INTEGER REFERENCES paises(pais_codigo),
    art_nombre VARCHAR(150) NOT NULL,
    art_nombre_real VARCHAR(150),
    art_fecha_nacimiento DATE,
    art_fecha_fallecimiento DATE,
    art_biografia TEXT,
    art_activo BOOLEAN NOT NULL DEFAULT TRUE,
    art_fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE tipos_artista (
    tart_codigo SERIAL PRIMARY KEY,
    tart_nombre VARCHAR(80) NOT NULL UNIQUE
);

CREATE TABLE artistas_tipos (
    art_tipo_codigo SERIAL PRIMARY KEY,
    art_codigo INTEGER NOT NULL REFERENCES artistas(art_codigo),
    tart_codigo INTEGER NOT NULL REFERENCES tipos_artista(tart_codigo),
    UNIQUE (art_codigo, tart_codigo)
);

CREATE TABLE sellos_discograficos (
    sello_codigo SERIAL PRIMARY KEY,
    sello_nombre VARCHAR(150) NOT NULL UNIQUE,
    pais_codigo INTEGER REFERENCES paises(pais_codigo),
    sello_activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE formatos_musicales (
    fmt_codigo SERIAL PRIMARY KEY,
    fmt_nombre VARCHAR(80) NOT NULL UNIQUE,
    fmt_descripcion VARCHAR(200),
    fmt_activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE albums (
    alb_codigo SERIAL PRIMARY KEY,
    art_codigo INTEGER NOT NULL REFERENCES artistas(art_codigo),
    gen_codigo INTEGER REFERENCES generos(gen_codigo),
    sello_codigo INTEGER REFERENCES sellos_discograficos(sello_codigo),
    fmt_codigo INTEGER REFERENCES formatos_musicales(fmt_codigo),
    alb_titulo VARCHAR(200) NOT NULL,
    alb_anio_lanzamiento INTEGER,
    alb_fecha_lanzamiento DATE,
    alb_numero_discos INTEGER NOT NULL DEFAULT 1,
    alb_portada_url TEXT,
    alb_observacion TEXT,
    alb_activo BOOLEAN NOT NULL DEFAULT TRUE,
    alb_fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE canciones (
    can_codigo SERIAL PRIMARY KEY,
    alb_codigo INTEGER REFERENCES albums(alb_codigo),
    gen_codigo INTEGER REFERENCES generos(gen_codigo),
    can_titulo VARCHAR(200) NOT NULL,
    can_duracion_segundos INTEGER,
    can_numero_pista INTEGER,
    can_letra TEXT,
    can_activo BOOLEAN NOT NULL DEFAULT TRUE,
    can_fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE canciones_artistas (
    can_art_codigo SERIAL PRIMARY KEY,
    can_codigo INTEGER NOT NULL REFERENCES canciones(can_codigo),
    art_codigo INTEGER NOT NULL REFERENCES artistas(art_codigo),
    can_art_es_principal BOOLEAN NOT NULL DEFAULT FALSE,
    UNIQUE (can_codigo, art_codigo)
);

CREATE TABLE compositores (
    comp_codigo SERIAL PRIMARY KEY,
    comp_nombres VARCHAR(150) NOT NULL,
    comp_apellidos VARCHAR(150),
    pais_codigo INTEGER REFERENCES paises(pais_codigo),
    comp_activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE canciones_compositores (
    can_comp_codigo SERIAL PRIMARY KEY,
    can_codigo INTEGER NOT NULL REFERENCES canciones(can_codigo),
    comp_codigo INTEGER NOT NULL REFERENCES compositores(comp_codigo),
    UNIQUE (can_codigo, comp_codigo)
);

CREATE TABLE colecciones (
    col_codigo SERIAL PRIMARY KEY,
    col_nombre VARCHAR(150) NOT NULL UNIQUE,
    col_descripcion VARCHAR(300),
    col_activo BOOLEAN NOT NULL DEFAULT TRUE,
    col_fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE coleccion_albums (
    col_alb_codigo SERIAL PRIMARY KEY,
    col_codigo INTEGER NOT NULL REFERENCES colecciones(col_codigo),
    alb_codigo INTEGER NOT NULL REFERENCES albums(alb_codigo),
    col_alb_fecha_agregado TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (col_codigo, alb_codigo)
);

CREATE TABLE ubicaciones_fisicas (
    ubi_codigo SERIAL PRIMARY KEY,
    ubi_nombre VARCHAR(100) NOT NULL UNIQUE,
    ubi_descripcion VARCHAR(300),
    ubi_activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE biblioteca_items (
    item_codigo SERIAL PRIMARY KEY,
    alb_codigo INTEGER NOT NULL REFERENCES albums(alb_codigo),
    ubi_codigo INTEGER REFERENCES ubicaciones_fisicas(ubi_codigo),
    item_codigo_interno VARCHAR(50) UNIQUE,
    item_codigo_barra VARCHAR(80),
    item_es_fisico BOOLEAN NOT NULL DEFAULT TRUE,
    item_estado VARCHAR(50) NOT NULL DEFAULT 'DISPONIBLE',
    item_fecha_compra DATE,
    item_precio_compra NUMERIC(12,2),
    item_observacion TEXT,
    item_activo BOOLEAN NOT NULL DEFAULT TRUE,
    item_fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE playlists (
    play_codigo SERIAL PRIMARY KEY,
    play_nombre VARCHAR(150) NOT NULL,
    play_descripcion VARCHAR(300),
    play_activo BOOLEAN NOT NULL DEFAULT TRUE,
    play_fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE playlist_canciones (
    play_can_codigo SERIAL PRIMARY KEY,
    play_codigo INTEGER NOT NULL REFERENCES playlists(play_codigo),
    can_codigo INTEGER NOT NULL REFERENCES canciones(can_codigo),
    play_can_orden INTEGER,
    play_can_fecha_agregado TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (play_codigo, can_codigo)
);

CREATE TABLE usuarios (
    usu_codigo SERIAL PRIMARY KEY,
    usu_nombres VARCHAR(100) NOT NULL,
    usu_apellidos VARCHAR(100) NOT NULL,
    usu_email VARCHAR(150) NOT NULL UNIQUE,
    usu_password_hash TEXT NOT NULL,
    usu_activo BOOLEAN NOT NULL DEFAULT TRUE,
    usu_fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE roles (
    rol_codigo SERIAL PRIMARY KEY,
    rol_nombre VARCHAR(80) NOT NULL UNIQUE,
    rol_activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE usuarios_roles (
    usu_rol_codigo SERIAL PRIMARY KEY,
    usu_codigo INTEGER NOT NULL REFERENCES usuarios(usu_codigo),
    rol_codigo INTEGER NOT NULL REFERENCES roles(rol_codigo),
    UNIQUE (usu_codigo, rol_codigo)
);

CREATE TABLE auditoria (
    aud_codigo SERIAL PRIMARY KEY,
    usu_codigo INTEGER REFERENCES usuarios(usu_codigo),
    aud_tabla VARCHAR(100) NOT NULL,
    aud_accion VARCHAR(30) NOT NULL,
    aud_registro_id INTEGER,
    aud_detalle TEXT,
    aud_fecha_evento TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION sp_obtener_generos()
RETURNS TABLE
(
    gen_codigo integer,
    gen_nombre varchar,
    gen_activo boolean
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        g.gen_codigo,
        g.gen_nombre,
        g.gen_activo
    FROM generos g
    ORDER BY g.gen_nombre;
END;
$$;