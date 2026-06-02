-- ============================================================
-- 03_seed_biblioteca_playlists_usuario.sql
-- Proyecto: Biblioteca Musical
-- Items físicos, colecciones, playlists y usuario inicial
-- Motor: PostgreSQL / Neon
-- ============================================================

BEGIN;

-- =========================
-- ITEMS DE BIBLIOTECA
-- =========================
INSERT INTO biblioteca_items (
    album_id,
    ubicacion_id,
    codigo_interno,
    codigo_barra,
    es_fisico,
    estado,
    fecha_compra,
    precio_compra,
    observacion,
    activo
)
SELECT al.album_id, u.ubicacion_id,
       'BM-CD-0001', NULL, TRUE, 'DISPONIBLE',
       CURRENT_DATE, 0,
       'Carga inicial de ejemplo.',
       TRUE
FROM albums al
JOIN ubicaciones_fisicas u ON u.nombre = 'Estante Principal'
WHERE al.titulo = 'MCMXC a.D.'
ON CONFLICT (codigo_interno) DO NOTHING;

INSERT INTO biblioteca_items (
    album_id, ubicacion_id, codigo_interno, codigo_barra, es_fisico,
    estado, fecha_compra, precio_compra, observacion, activo
)
SELECT al.album_id, u.ubicacion_id,
       'BM-CD-0002', NULL, TRUE, 'DISPONIBLE',
       CURRENT_DATE, 0,
       'Carga inicial de ejemplo.',
       TRUE
FROM albums al
JOIN ubicaciones_fisicas u ON u.nombre = 'Estante Principal'
WHERE al.titulo = 'Dive'
ON CONFLICT (codigo_interno) DO NOTHING;

INSERT INTO biblioteca_items (
    album_id, ubicacion_id, codigo_interno, codigo_barra, es_fisico,
    estado, fecha_compra, precio_compra, observacion, activo
)
SELECT al.album_id, u.ubicacion_id,
       'BM-VIN-0001', NULL, TRUE, 'DISPONIBLE',
       CURRENT_DATE, 0,
       'Carga inicial de ejemplo.',
       TRUE
FROM albums al
JOIN ubicaciones_fisicas u ON u.nombre = 'Rack Vintage Pioneer'
WHERE al.titulo = 'Arrival'
ON CONFLICT (codigo_interno) DO NOTHING;

-- =========================
-- COLECCION_ALBUMS
-- =========================
INSERT INTO coleccion_albums (coleccion_id, album_id)
SELECT co.coleccion_id, al.album_id
FROM colecciones co
JOIN albums al ON al.titulo IN ('MCMXC a.D.', 'Dive', 'Ainsi soit je...', 'Arrival', 'Saturday Night Fever', 'Lost in Love')
WHERE co.nombre = 'Colección Principal'
ON CONFLICT (coleccion_id, album_id) DO NOTHING;

INSERT INTO coleccion_albums (coleccion_id, album_id)
SELECT co.coleccion_id, al.album_id
FROM colecciones co
JOIN albums al ON al.titulo IN ('Arrival', 'Saturday Night Fever', 'Lost in Love')
WHERE co.nombre = 'Clásicos 70s y 80s'
ON CONFLICT (coleccion_id, album_id) DO NOTHING;

INSERT INTO coleccion_albums (coleccion_id, album_id)
SELECT co.coleccion_id, al.album_id
FROM colecciones co
JOIN albums al ON al.titulo IN ('MCMXC a.D.')
WHERE co.nombre = 'New Age y Electrónica'
ON CONFLICT (coleccion_id, album_id) DO NOTHING;

-- =========================
-- PLAYLIST_CANCIONES
-- =========================
INSERT INTO playlist_canciones (playlist_id, cancion_id, orden)
SELECT p.playlist_id, c.cancion_id, 1
FROM playlists p
JOIN canciones c ON c.titulo = 'Sadeness'
WHERE p.nombre = 'Noche tranquila'
ON CONFLICT (playlist_id, cancion_id) DO NOTHING;

INSERT INTO playlist_canciones (playlist_id, cancion_id, orden)
SELECT p.playlist_id, c.cancion_id, 2
FROM playlists p
JOIN canciones c ON c.titulo = 'Captain Nemo'
WHERE p.nombre = 'Noche tranquila'
ON CONFLICT (playlist_id, cancion_id) DO NOTHING;

INSERT INTO playlist_canciones (playlist_id, cancion_id, orden)
SELECT p.playlist_id, c.cancion_id, 1
FROM playlists p
JOIN canciones c ON c.titulo = 'Dancing Queen'
WHERE p.nombre = 'Clásicos favoritos'
ON CONFLICT (playlist_id, cancion_id) DO NOTHING;

INSERT INTO playlist_canciones (playlist_id, cancion_id, orden)
SELECT p.playlist_id, c.cancion_id, 2
FROM playlists p
JOIN canciones c ON c.titulo = 'Stayin Alive'
WHERE p.nombre = 'Clásicos favoritos'
ON CONFLICT (playlist_id, cancion_id) DO NOTHING;

-- =========================
-- USUARIO INICIAL
-- Password ficticia solo para desarrollo.
-- Luego la reemplazaremos por hash real desde .NET.
-- =========================
INSERT INTO usuarios (nombres, apellidos, email, password_hash, activo)
VALUES ('Danilo', 'Administrador', 'admin@bibliotecamusical.local', 'CAMBIAR_PASSWORD_HASH', TRUE)
ON CONFLICT (email) DO NOTHING;

INSERT INTO usuarios_roles (usuario_id, rol_id)
SELECT u.usuario_id, r.rol_id
FROM usuarios u
JOIN roles r ON r.nombre = 'Administrador'
WHERE u.email = 'admin@bibliotecamusical.local'
ON CONFLICT (usuario_id, rol_id) DO NOTHING;

COMMIT;
