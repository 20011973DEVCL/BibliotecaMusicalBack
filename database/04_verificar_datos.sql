-- ============================================================
-- 04_verificar_datos.sql
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

-- Vista resumen de albums con artista y genero
SELECT
    al.album_id,
    ar.nombre AS artista,
    al.titulo AS album,
    g.nombre AS genero,
    al.anio_lanzamiento,
    f.nombre AS formato
FROM albums al
JOIN artistas ar ON ar.artista_id = al.artista_id
LEFT JOIN generos g ON g.genero_id = al.genero_id
LEFT JOIN formatos_musicales f ON f.formato_id = al.formato_id
ORDER BY ar.nombre, al.anio_lanzamiento;

-- Vista resumen de canciones
SELECT
    c.cancion_id,
    ar.nombre AS artista,
    al.titulo AS album,
    c.titulo AS cancion,
    c.numero_pista,
    c.duracion_segundos
FROM canciones c
LEFT JOIN albums al ON al.album_id = c.album_id
LEFT JOIN artistas ar ON ar.artista_id = al.artista_id
ORDER BY ar.nombre, al.titulo, c.numero_pista;
