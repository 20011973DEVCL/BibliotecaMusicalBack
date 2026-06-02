-- ============================================================
-- 04_verificar_datos_corregido.sql
-- Proyecto: Biblioteca Musical
-- Verificación rápida de datos cargados
-- ============================================================

SELECT 'paises' AS tabla, COUNT(*) AS total FROM paises
UNION ALL SELECT 'generos', COUNT(*) FROM generos
UNION ALL SELECT 'tipos_artista', COUNT(*) FROM tipos_artista
UNION ALL SELECT 'artistas', COUNT(*) FROM artistas
UNION ALL SELECT 'artistas_tipos', COUNT(*) FROM artistas_tipos
UNION ALL SELECT 'sellos_discograficos', COUNT(*) FROM sellos_discograficos
UNION ALL SELECT 'formatos_musicales', COUNT(*) FROM formatos_musicales
UNION ALL SELECT 'albums', COUNT(*) FROM albums
UNION ALL SELECT 'canciones', COUNT(*) FROM canciones
UNION ALL SELECT 'canciones_artistas', COUNT(*) FROM canciones_artistas
UNION ALL SELECT 'colecciones', COUNT(*) FROM colecciones
UNION ALL SELECT 'coleccion_albums', COUNT(*) FROM coleccion_albums
UNION ALL SELECT 'ubicaciones_fisicas', COUNT(*) FROM ubicaciones_fisicas
UNION ALL SELECT 'biblioteca_items', COUNT(*) FROM biblioteca_items
UNION ALL SELECT 'playlists', COUNT(*) FROM playlists
UNION ALL SELECT 'playlist_canciones', COUNT(*) FROM playlist_canciones
UNION ALL SELECT 'usuarios', COUNT(*) FROM usuarios
UNION ALL SELECT 'roles', COUNT(*) FROM roles
UNION ALL SELECT 'usuarios_roles', COUNT(*) FROM usuarios_roles
ORDER BY tabla;

SELECT
    al.alb_codigo,
    ar.art_nombre AS artista,
    al.alb_titulo AS album,
    g.gen_nombre AS genero,
    al.alb_anio_lanzamiento,
    f.fmt_nombre AS formato
FROM albums al
JOIN artistas ar ON ar.art_codigo = al.art_codigo
LEFT JOIN generos g ON g.gen_codigo = al.gen_codigo
LEFT JOIN formatos_musicales f ON f.fmt_codigo = al.fmt_codigo
ORDER BY ar.art_nombre, al.alb_anio_lanzamiento;

SELECT
    c.can_codigo,
    ar.art_nombre AS artista,
    al.alb_titulo AS album,
    c.can_titulo AS cancion,
    c.can_numero_pista,
    c.can_duracion_segundos
FROM canciones c
LEFT JOIN albums al ON al.alb_codigo = c.alb_codigo
LEFT JOIN artistas ar ON ar.art_codigo = al.art_codigo
ORDER BY ar.art_nombre, al.alb_titulo, c.can_numero_pista;
