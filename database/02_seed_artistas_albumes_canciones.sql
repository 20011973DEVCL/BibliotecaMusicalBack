-- ============================================================
-- 02_seed_artistas_albumes_canciones_corregido.sql
-- Proyecto: Biblioteca Musical
-- Modelo corregido: artistas, albums, canciones y relaciones
-- Motor: PostgreSQL / Neon
-- ============================================================

BEGIN;

INSERT INTO artistas (pais_codigo, art_nombre, art_nombre_real, art_fecha_nacimiento, art_biografia, art_activo)
SELECT p.pais_codigo, 'Enigma', 'Michael Cretu', '1957-05-18',
       'Proyecto musical creado por Michael Cretu, asociado a sonidos new age, electrónicos y atmosféricos.', TRUE
FROM paises p WHERE p.pais_nombre = 'Alemania'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_codigo, art_nombre, art_nombre_real, art_fecha_nacimiento, art_biografia, art_activo)
SELECT p.pais_codigo, 'Sarah Brightman', 'Sarah Brightman', '1960-08-14',
       'Cantante británica reconocida por su mezcla de música clásica, pop y crossover.', TRUE
FROM paises p WHERE p.pais_nombre = 'Reino Unido'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_codigo, art_nombre, art_nombre_real, art_fecha_nacimiento, art_biografia, art_activo)
SELECT p.pais_codigo, 'Mylène Farmer', 'Mylène Jeanne Gautier', '1961-09-12',
       'Cantante franco-canadiense de gran influencia en el pop francés.', TRUE
FROM paises p WHERE p.pais_nombre = 'Francia'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_codigo, art_nombre, art_nombre_real, art_biografia, art_activo)
SELECT p.pais_codigo, 'ABBA', NULL,
       'Grupo sueco de pop, uno de los más influyentes de la música popular.', TRUE
FROM paises p WHERE p.pais_nombre = 'Suecia'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_codigo, art_nombre, art_nombre_real, art_biografia, art_activo)
SELECT p.pais_codigo, 'Bee Gees', NULL,
       'Grupo británico-australiano asociado al pop, disco y baladas.', TRUE
FROM paises p WHERE p.pais_nombre = 'Reino Unido'
ON CONFLICT DO NOTHING;

INSERT INTO artistas (pais_codigo, art_nombre, art_nombre_real, art_biografia, art_activo)
SELECT p.pais_codigo, 'Air Supply', NULL,
       'Dúo australiano conocido por sus baladas románticas.', TRUE
FROM paises p WHERE p.pais_nombre = 'Australia'
ON CONFLICT DO NOTHING;

INSERT INTO artistas_tipos (art_codigo, tart_codigo)
SELECT a.art_codigo, t.tart_codigo
FROM artistas a
JOIN tipos_artista t ON t.tart_nombre = 'Grupo'
WHERE a.art_nombre IN ('Enigma', 'ABBA', 'Bee Gees')
ON CONFLICT (art_codigo, tart_codigo) DO NOTHING;

INSERT INTO artistas_tipos (art_codigo, tart_codigo)
SELECT a.art_codigo, t.tart_codigo
FROM artistas a
JOIN tipos_artista t ON t.tart_nombre = 'Solista'
WHERE a.art_nombre IN ('Sarah Brightman', 'Mylène Farmer')
ON CONFLICT (art_codigo, tart_codigo) DO NOTHING;

INSERT INTO artistas_tipos (art_codigo, tart_codigo)
SELECT a.art_codigo, t.tart_codigo
FROM artistas a
JOIN tipos_artista t ON t.tart_nombre = 'Dúo'
WHERE a.art_nombre = 'Air Supply'
ON CONFLICT (art_codigo, tart_codigo) DO NOTHING;

INSERT INTO albums (art_codigo, gen_codigo, sello_codigo, fmt_codigo, alb_titulo, alb_anio_lanzamiento, alb_fecha_lanzamiento, alb_numero_discos, alb_observacion, alb_activo)
SELECT a.art_codigo, g.gen_codigo, s.sello_codigo, f.fmt_codigo, 'MCMXC a.D.', 1990, '1990-12-03', 1,
       'Álbum emblemático de Enigma.', TRUE
FROM artistas a
JOIN generos g ON g.gen_nombre = 'New Age'
JOIN sellos_discograficos s ON s.sello_nombre = 'Virgin Records'
JOIN formatos_musicales f ON f.fmt_nombre = 'CD'
WHERE a.art_nombre = 'Enigma'
ON CONFLICT DO NOTHING;

INSERT INTO albums (art_codigo, gen_codigo, sello_codigo, fmt_codigo, alb_titulo, alb_anio_lanzamiento, alb_fecha_lanzamiento, alb_numero_discos, alb_observacion, alb_activo)
SELECT a.art_codigo, g.gen_codigo, s.sello_codigo, f.fmt_codigo, 'Dive', 1993, '1993-04-26', 1,
       'Álbum de Sarah Brightman con sonido pop y atmosférico.', TRUE
FROM artistas a
JOIN generos g ON g.gen_nombre = 'Pop'
JOIN sellos_discograficos s ON s.sello_nombre = 'Universal Music'
JOIN formatos_musicales f ON f.fmt_nombre = 'CD'
WHERE a.art_nombre = 'Sarah Brightman'
ON CONFLICT DO NOTHING;

INSERT INTO albums (art_codigo, gen_codigo, sello_codigo, fmt_codigo, alb_titulo, alb_anio_lanzamiento, alb_fecha_lanzamiento, alb_numero_discos, alb_observacion, alb_activo)
SELECT a.art_codigo, g.gen_codigo, s.sello_codigo, f.fmt_codigo, 'Ainsi soit je...', 1988, '1988-04-04', 1,
       'Álbum clásico de Mylène Farmer.', TRUE
FROM artistas a
JOIN generos g ON g.gen_nombre = 'Pop'
JOIN sellos_discograficos s ON s.sello_nombre = 'Polydor Records'
JOIN formatos_musicales f ON f.fmt_nombre = 'CD'
WHERE a.art_nombre = 'Mylène Farmer'
ON CONFLICT DO NOTHING;

INSERT INTO albums (art_codigo, gen_codigo, sello_codigo, fmt_codigo, alb_titulo, alb_anio_lanzamiento, alb_fecha_lanzamiento, alb_numero_discos, alb_observacion, alb_activo)
SELECT a.art_codigo, g.gen_codigo, s.sello_codigo, f.fmt_codigo, 'Arrival', 1976, '1976-10-11', 1,
       'Álbum clásico de ABBA.', TRUE
FROM artistas a
JOIN generos g ON g.gen_nombre = 'Pop'
JOIN sellos_discograficos s ON s.sello_nombre = 'Polar Music'
JOIN formatos_musicales f ON f.fmt_nombre = 'Vinilo'
WHERE a.art_nombre = 'ABBA'
ON CONFLICT DO NOTHING;

INSERT INTO albums (art_codigo, gen_codigo, sello_codigo, fmt_codigo, alb_titulo, alb_anio_lanzamiento, alb_fecha_lanzamiento, alb_numero_discos, alb_observacion, alb_activo)
SELECT a.art_codigo, g.gen_codigo, s.sello_codigo, f.fmt_codigo, 'Saturday Night Fever', 1977, '1977-11-15', 1,
       'Banda sonora representativa de la era disco.', TRUE
FROM artistas a
JOIN generos g ON g.gen_nombre = 'Disco'
JOIN sellos_discograficos s ON s.sello_nombre = 'Polydor Records'
JOIN formatos_musicales f ON f.fmt_nombre = 'Vinilo'
WHERE a.art_nombre = 'Bee Gees'
ON CONFLICT DO NOTHING;

INSERT INTO albums (art_codigo, gen_codigo, sello_codigo, fmt_codigo, alb_titulo, alb_anio_lanzamiento, alb_fecha_lanzamiento, alb_numero_discos, alb_observacion, alb_activo)
SELECT a.art_codigo, g.gen_codigo, s.sello_codigo, f.fmt_codigo, 'Lost in Love', 1980, '1980-03-01', 1,
       'Álbum clásico de baladas de Air Supply.', TRUE
FROM artistas a
JOIN generos g ON g.gen_nombre = 'Balada'
JOIN sellos_discograficos s ON s.sello_nombre = 'Arista Records'
JOIN formatos_musicales f ON f.fmt_nombre = 'CD'
WHERE a.art_nombre = 'Air Supply'
ON CONFLICT DO NOTHING;

INSERT INTO canciones (alb_codigo, gen_codigo, can_titulo, can_duracion_segundos, can_numero_pista, can_activo)
SELECT al.alb_codigo, g.gen_codigo, 'Sadeness', 256, 1, TRUE FROM albums al JOIN generos g ON g.gen_nombre = 'New Age' WHERE al.alb_titulo = 'MCMXC a.D.' ON CONFLICT DO NOTHING;
INSERT INTO canciones (alb_codigo, gen_codigo, can_titulo, can_duracion_segundos, can_numero_pista, can_activo)
SELECT al.alb_codigo, g.gen_codigo, 'Mea Culpa', 301, 2, TRUE FROM albums al JOIN generos g ON g.gen_nombre = 'New Age' WHERE al.alb_titulo = 'MCMXC a.D.' ON CONFLICT DO NOTHING;
INSERT INTO canciones (alb_codigo, gen_codigo, can_titulo, can_duracion_segundos, can_numero_pista, can_activo)
SELECT al.alb_codigo, g.gen_codigo, 'Captain Nemo', 272, 1, TRUE FROM albums al JOIN generos g ON g.gen_nombre = 'Pop' WHERE al.alb_titulo = 'Dive' ON CONFLICT DO NOTHING;
INSERT INTO canciones (alb_codigo, gen_codigo, can_titulo, can_duracion_segundos, can_numero_pista, can_activo)
SELECT al.alb_codigo, g.gen_codigo, 'Sans contrefaçon', 248, 1, TRUE FROM albums al JOIN generos g ON g.gen_nombre = 'Pop' WHERE al.alb_titulo = 'Ainsi soit je...' ON CONFLICT DO NOTHING;
INSERT INTO canciones (alb_codigo, gen_codigo, can_titulo, can_duracion_segundos, can_numero_pista, can_activo)
SELECT al.alb_codigo, g.gen_codigo, 'Dancing Queen', 231, 1, TRUE FROM albums al JOIN generos g ON g.gen_nombre = 'Pop' WHERE al.alb_titulo = 'Arrival' ON CONFLICT DO NOTHING;
INSERT INTO canciones (alb_codigo, gen_codigo, can_titulo, can_duracion_segundos, can_numero_pista, can_activo)
SELECT al.alb_codigo, g.gen_codigo, 'Stayin Alive', 285, 1, TRUE FROM albums al JOIN generos g ON g.gen_nombre = 'Disco' WHERE al.alb_titulo = 'Saturday Night Fever' ON CONFLICT DO NOTHING;
INSERT INTO canciones (alb_codigo, gen_codigo, can_titulo, can_duracion_segundos, can_numero_pista, can_activo)
SELECT al.alb_codigo, g.gen_codigo, 'Lost in Love', 234, 1, TRUE FROM albums al JOIN generos g ON g.gen_nombre = 'Balada' WHERE al.alb_titulo = 'Lost in Love' ON CONFLICT DO NOTHING;

INSERT INTO canciones_artistas (can_codigo, art_codigo, can_art_es_principal)
SELECT c.can_codigo, a.art_codigo, TRUE
FROM canciones c
JOIN albums al ON al.alb_codigo = c.alb_codigo
JOIN artistas a ON a.art_codigo = al.art_codigo
ON CONFLICT (can_codigo, art_codigo) DO NOTHING;

COMMIT;
