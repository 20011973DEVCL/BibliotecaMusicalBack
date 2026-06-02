-- ============================================================
-- 02_seed_artistas_albumes_canciones.sql
-- Proyecto: Biblioteca Musical
-- Modelo real: artistas, albums, canciones y relaciones
-- Motor: PostgreSQL / Neon
-- ============================================================

BEGIN;

-- =========================
-- ARTISTAS
-- =========================
INSERT INTO artistas (pais_id, nombre, nombre_real, fecha_nacimiento, biografia, activo)
SELECT p.pais_id, 'Enigma', 'Michael Cretu', '1957-05-18',
       'Proyecto musical creado por Michael Cretu, asociado a sonidos new age, electrónicos y atmosféricos.',
       TRUE
FROM paises p WHERE p.nombre = 'Alemania'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_id, nombre, nombre_real, fecha_nacimiento, biografia, activo)
SELECT p.pais_id, 'Sarah Brightman', 'Sarah Brightman', '1960-08-14',
       'Cantante británica reconocida por su mezcla de música clásica, pop y crossover.',
       TRUE
FROM paises p WHERE p.nombre = 'Reino Unido'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_id, nombre, nombre_real, fecha_nacimiento, biografia, activo)
SELECT p.pais_id, 'Mylène Farmer', 'Mylène Jeanne Gautier', '1961-09-12',
       'Cantante franco-canadiense de gran influencia en el pop francés.',
       TRUE
FROM paises p WHERE p.nombre = 'Francia'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_id, nombre, nombre_real, biografia, activo)
SELECT p.pais_id, 'ABBA', NULL,
       'Grupo sueco de pop, uno de los más influyentes de la música popular.',
       TRUE
FROM paises p WHERE p.nombre = 'Suecia'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_id, nombre, nombre_real, biografia, activo)
SELECT p.pais_id, 'Bee Gees', NULL,
       'Grupo británico-australiano asociado al pop, disco y baladas.',
       TRUE
FROM paises p WHERE p.nombre = 'Reino Unido'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_id, nombre, nombre_real, biografia, activo)
SELECT p.pais_id, 'Air Supply', NULL,
       'Dúo australiano conocido por sus baladas románticas.',
       TRUE
FROM paises p WHERE p.nombre = 'Australia'
ON CONFLICT DO NOTHING;

-- =========================
-- ARTISTAS_TIPOS
-- =========================
INSERT INTO artistas_tipos (artista_id, tipo_artista_id)
SELECT a.artista_id, t.tipo_artista_id
FROM artistas a
JOIN tipos_artista t ON t.nombre = 'Grupo'
WHERE a.nombre IN ('Enigma', 'ABBA', 'Bee Gees')
ON CONFLICT (artista_id, tipo_artista_id) DO NOTHING;

INSERT INTO artistas_tipos (artista_id, tipo_artista_id)
SELECT a.artista_id, t.tipo_artista_id
FROM artistas a
JOIN tipos_artista t ON t.nombre = 'Solista'
WHERE a.nombre IN ('Sarah Brightman', 'Mylène Farmer')
ON CONFLICT (artista_id, tipo_artista_id) DO NOTHING;

INSERT INTO artistas_tipos (artista_id, tipo_artista_id)
SELECT a.artista_id, t.tipo_artista_id
FROM artistas a
JOIN tipos_artista t ON t.nombre = 'Dúo'
WHERE a.nombre = 'Air Supply'
ON CONFLICT (artista_id, tipo_artista_id) DO NOTHING;

-- =========================
-- ALBUMS
-- =========================
INSERT INTO albums (artista_id, genero_id, sello_id, formato_id, titulo, anio_lanzamiento, fecha_lanzamiento, numero_discos, observacion, activo)
SELECT a.artista_id, g.genero_id, s.sello_id, f.formato_id, 'MCMXC a.D.', 1990, '1990-12-03', 1,
       'Álbum emblemático de Enigma.', TRUE
FROM artistas a
JOIN generos g ON g.nombre = 'New Age'
JOIN sellos_discograficos s ON s.nombre = 'Virgin Records'
JOIN formatos_musicales f ON f.nombre = 'CD'
WHERE a.nombre = 'Enigma'
ON CONFLICT DO NOTHING;

INSERT INTO albums (artista_id, genero_id, sello_id, formato_id, titulo, anio_lanzamiento, fecha_lanzamiento, numero_discos, observacion, activo)
SELECT a.artista_id, g.genero_id, s.sello_id, f.formato_id, 'Dive', 1993, '1993-04-26', 1,
       'Álbum de Sarah Brightman con sonido pop y atmosférico.', TRUE
FROM artistas a
JOIN generos g ON g.nombre = 'Pop'
JOIN sellos_discograficos s ON s.nombre = 'Universal Music'
JOIN formatos_musicales f ON f.nombre = 'CD'
WHERE a.nombre = 'Sarah Brightman'
ON CONFLICT DO NOTHING;

INSERT INTO albums (artista_id, genero_id, sello_id, formato_id, titulo, anio_lanzamiento, fecha_lanzamiento, numero_discos, observacion, activo)
SELECT a.artista_id, g.genero_id, s.sello_id, f.formato_id, 'Ainsi soit je...', 1988, '1988-04-04', 1,
       'Álbum importante dentro del pop francés.', TRUE
FROM artistas a
JOIN generos g ON g.nombre = 'Pop'
JOIN sellos_discograficos s ON s.nombre = 'Polydor Records'
JOIN formatos_musicales f ON f.nombre = 'CD'
WHERE a.nombre = 'Mylène Farmer'
ON CONFLICT DO NOTHING;

INSERT INTO albums (artista_id, genero_id, sello_id, formato_id, titulo, anio_lanzamiento, fecha_lanzamiento, numero_discos, observacion, activo)
SELECT a.artista_id, g.genero_id, s.sello_id, f.formato_id, 'Arrival', 1976, '1976-10-11', 1,
       'Álbum clásico de ABBA.', TRUE
FROM artistas a
JOIN generos g ON g.nombre = 'Pop'
JOIN sellos_discograficos s ON s.nombre = 'Polar Music'
JOIN formatos_musicales f ON f.nombre = 'Vinilo'
WHERE a.nombre = 'ABBA'
ON CONFLICT DO NOTHING;

INSERT INTO albums (artista_id, genero_id, sello_id, formato_id, titulo, anio_lanzamiento, fecha_lanzamiento, numero_discos, observacion, activo)
SELECT a.artista_id, g.genero_id, s.sello_id, f.formato_id, 'Saturday Night Fever', 1977, '1977-11-15', 1,
       'Banda sonora representativa de la era disco.', TRUE
FROM artistas a
JOIN generos g ON g.nombre = 'Disco'
JOIN sellos_discograficos s ON s.nombre = 'Polydor Records'
JOIN formatos_musicales f ON f.nombre = 'Vinilo'
WHERE a.nombre = 'Bee Gees'
ON CONFLICT DO NOTHING;

INSERT INTO albums (artista_id, genero_id, sello_id, formato_id, titulo, anio_lanzamiento, fecha_lanzamiento, numero_discos, observacion, activo)
SELECT a.artista_id, g.genero_id, s.sello_id, f.formato_id, 'Lost in Love', 1980, '1980-03-01', 1,
       'Álbum clásico de baladas de Air Supply.', TRUE
FROM artistas a
JOIN generos g ON g.nombre = 'Balada'
JOIN sellos_discograficos s ON s.nombre = 'Arista Records'
JOIN formatos_musicales f ON f.nombre = 'CD'
WHERE a.nombre = 'Air Supply'
ON CONFLICT DO NOTHING;

-- =========================
-- CANCIONES
-- =========================
INSERT INTO canciones (album_id, genero_id, titulo, duracion_segundos, numero_pista, activo)
SELECT al.album_id, g.genero_id, 'Sadeness', 256, 1, TRUE
FROM albums al
JOIN generos g ON g.nombre = 'New Age'
WHERE al.titulo = 'MCMXC a.D.'
ON CONFLICT DO NOTHING;

INSERT INTO canciones (album_id, genero_id, titulo, duracion_segundos, numero_pista, activo)
SELECT al.album_id, g.genero_id, 'Mea Culpa', 301, 2, TRUE
FROM albums al
JOIN generos g ON g.nombre = 'New Age'
WHERE al.titulo = 'MCMXC a.D.'
ON CONFLICT DO NOTHING;

INSERT INTO canciones (album_id, genero_id, titulo, duracion_segundos, numero_pista, activo)
SELECT al.album_id, g.genero_id, 'Captain Nemo', 272, 1, TRUE
FROM albums al
JOIN generos g ON g.nombre = 'Pop'
WHERE al.titulo = 'Dive'
ON CONFLICT DO NOTHING;

INSERT INTO canciones (album_id, genero_id, titulo, duracion_segundos, numero_pista, activo)
SELECT al.album_id, g.genero_id, 'Sans contrefaçon', 248, 1, TRUE
FROM albums al
JOIN generos g ON g.nombre = 'Pop'
WHERE al.titulo = 'Ainsi soit je...'
ON CONFLICT DO NOTHING;

INSERT INTO canciones (album_id, genero_id, titulo, duracion_segundos, numero_pista, activo)
SELECT al.album_id, g.genero_id, 'Dancing Queen', 231, 1, TRUE
FROM albums al
JOIN generos g ON g.nombre = 'Pop'
WHERE al.titulo = 'Arrival'
ON CONFLICT DO NOTHING;

INSERT INTO canciones (album_id, genero_id, titulo, duracion_segundos, numero_pista, activo)
SELECT al.album_id, g.genero_id, 'Stayin Alive', 285, 1, TRUE
FROM albums al
JOIN generos g ON g.nombre = 'Disco'
WHERE al.titulo = 'Saturday Night Fever'
ON CONFLICT DO NOTHING;

INSERT INTO canciones (album_id, genero_id, titulo, duracion_segundos, numero_pista, activo)
SELECT al.album_id, g.genero_id, 'Lost in Love', 234, 1, TRUE
FROM albums al
JOIN generos g ON g.nombre = 'Balada'
WHERE al.titulo = 'Lost in Love'
ON CONFLICT DO NOTHING;

-- =========================
-- CANCIONES_ARTISTAS
-- =========================
INSERT INTO canciones_artistas (cancion_id, artista_id, es_principal)
SELECT c.cancion_id, a.artista_id, TRUE
FROM canciones c
JOIN albums al ON al.album_id = c.album_id
JOIN artistas a ON a.artista_id = al.artista_id
ON CONFLICT (cancion_id, artista_id) DO NOTHING;

COMMIT;
