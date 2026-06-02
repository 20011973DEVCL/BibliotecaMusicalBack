-- ============================================================
-- 01_seed_catalogos_base_corregido.sql
-- Proyecto: Biblioteca Musical
-- Modelo corregido: columnas con prefijo por tabla
-- Motor: PostgreSQL / Neon
-- ============================================================

BEGIN;

INSERT INTO paises (pais_nombre, pais_codigo_iso, pais_activo)
VALUES
('Chile', 'CHL', TRUE),
('Argentina', 'ARG', TRUE),
('Brasil', 'BRA', TRUE),
('España', 'ESP', TRUE),
('Francia', 'FRA', TRUE),
('Alemania', 'DEU', TRUE),
('Italia', 'ITA', TRUE),
('Reino Unido', 'GBR', TRUE),
('Estados Unidos', 'USA', TRUE),
('Canadá', 'CAN', TRUE),
('Suecia', 'SWE', TRUE),
('Noruega', 'NOR', TRUE),
('Australia', 'AUS', TRUE)
ON CONFLICT (pais_nombre) DO NOTHING;

INSERT INTO generos (gen_nombre, gen_descripcion, gen_activo)
VALUES
('Pop', 'Música popular orientada a melodías accesibles.', TRUE),
('Rock', 'Género derivado del rock and roll con guitarras eléctricas.', TRUE),
('Balada', 'Canciones melódicas de carácter romántico o sentimental.', TRUE),
('Synth Pop', 'Pop basado en sintetizadores y producción electrónica.', TRUE),
('New Age', 'Música atmosférica, espiritual o instrumental contemporánea.', TRUE),
('Disco', 'Música bailable popularizada en los años 70.', TRUE),
('Soundtrack', 'Música compuesta o usada para cine, series o videojuegos.', TRUE),
('Electrónica', 'Música producida principalmente con instrumentos electrónicos.', TRUE),
('Eurodance', 'Música dance europea de los años 90.', TRUE),
('Clásica', 'Música académica de tradición orquestal o de cámara.', TRUE),
('Folk', 'Música tradicional o de raíz cultural.', TRUE),
('Jazz', 'Género basado en improvisación, armonía compleja y swing.', TRUE)
ON CONFLICT (gen_nombre) DO NOTHING;

INSERT INTO tipos_artista (tart_nombre)
VALUES
('Solista'),
('Grupo'),
('Banda'),
('Dúo'),
('Compositor'),
('Productor')
ON CONFLICT (tart_nombre) DO NOTHING;

INSERT INTO formatos_musicales (fmt_nombre, fmt_descripcion, fmt_activo)
VALUES
('CD', 'Disco compacto físico.', TRUE),
('Vinilo', 'Disco fonográfico de larga duración.', TRUE),
('Cassette', 'Cinta magnética de audio.', TRUE),
('Digital FLAC', 'Archivo digital sin pérdida.', TRUE),
('Digital ALAC', 'Archivo digital sin pérdida compatible con Apple.', TRUE),
('Digital MP3', 'Archivo digital comprimido.', TRUE),
('Streaming', 'Contenido reproducido desde plataforma en línea.', TRUE)
ON CONFLICT (fmt_nombre) DO NOTHING;

INSERT INTO sellos_discograficos (sello_nombre, pais_codigo, sello_activo)
SELECT 'Virgin Records', p.pais_codigo, TRUE FROM paises p WHERE p.pais_nombre = 'Reino Unido'
ON CONFLICT (sello_nombre) DO NOTHING;

INSERT INTO sellos_discograficos (sello_nombre, pais_codigo, sello_activo)
SELECT 'Polydor Records', p.pais_codigo, TRUE FROM paises p WHERE p.pais_nombre = 'Reino Unido'
ON CONFLICT (sello_nombre) DO NOTHING;

INSERT INTO sellos_discograficos (sello_nombre, pais_codigo, sello_activo)
SELECT 'Universal Music', p.pais_codigo, TRUE FROM paises p WHERE p.pais_nombre = 'Estados Unidos'
ON CONFLICT (sello_nombre) DO NOTHING;

INSERT INTO sellos_discograficos (sello_nombre, pais_codigo, sello_activo)
SELECT 'Polar Music', p.pais_codigo, TRUE FROM paises p WHERE p.pais_nombre = 'Suecia'
ON CONFLICT (sello_nombre) DO NOTHING;

INSERT INTO sellos_discograficos (sello_nombre, pais_codigo, sello_activo)
SELECT 'Arista Records', p.pais_codigo, TRUE FROM paises p WHERE p.pais_nombre = 'Estados Unidos'
ON CONFLICT (sello_nombre) DO NOTHING;

INSERT INTO ubicaciones_fisicas (ubi_nombre, ubi_descripcion, ubi_activo)
VALUES
('Estante Principal', 'Estante principal de la colección musical.', TRUE),
('Rack Vintage Pioneer', 'Zona asociada al equipo de sonido principal.', TRUE),
('Caja Respaldo', 'Caja de respaldo para discos o formatos físicos.', TRUE),
('Biblioteca Digital', 'Ubicación lógica para música digital.', TRUE)
ON CONFLICT (ubi_nombre) DO NOTHING;

INSERT INTO colecciones (col_nombre, col_descripcion, col_activo)
VALUES
('Colección Principal', 'Colección general de álbumes favoritos.', TRUE),
('Clásicos 70s y 80s', 'Álbumes destacados de los años 70 y 80.', TRUE),
('New Age y Electrónica', 'Música atmosférica, electrónica y espiritual.', TRUE),
('Baladas Favoritas', 'Selección de baladas y canciones melódicas.', TRUE)
ON CONFLICT (col_nombre) DO NOTHING;

INSERT INTO playlists (play_nombre, play_descripcion, play_activo)
VALUES
('Noche tranquila', 'Canciones suaves para escuchar de noche.', TRUE),
('Viaje por carretera', 'Música para acompañar viajes largos.', TRUE),
('Clásicos favoritos', 'Selección de canciones clásicas de la biblioteca.', TRUE)
ON CONFLICT DO NOTHING;

INSERT INTO roles (rol_nombre, rol_activo)
VALUES
('Administrador', TRUE),
('Usuario', TRUE),
('Invitado', TRUE)
ON CONFLICT (rol_nombre) DO NOTHING;

COMMIT;
