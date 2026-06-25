-- ============================================================
-- 03_seed_biblioteca_playlists_usuario_corregido.sql
-- Proyecto: Biblioteca Musical
-- Modelo corregido: items físicos, colecciones, playlists y usuario inicial
-- Motor: PostgreSQL / Neon
-- ============================================================

BEGIN;

INSERT INTO biblioteca_items (alb_codigo, ubi_codigo, item_codigo_interno, item_codigo_barra, item_es_fisico, item_estado, item_fecha_compra, item_precio_compra, item_observacion, item_activo)
SELECT al.alb_codigo, u.ubi_codigo, 'BM-CD-0001', NULL, TRUE, 'DISPONIBLE', CURRENT_DATE, 0, 'Carga inicial de ejemplo.', TRUE
FROM albums al
JOIN ubicaciones_fisicas u ON u.ubi_nombre = 'Estante Principal'
WHERE al.alb_titulo = 'MCMXC a.D.'
ON CONFLICT (item_codigo_interno) DO NOTHING;

INSERT INTO biblioteca_items (alb_codigo, ubi_codigo, item_codigo_interno, item_codigo_barra, item_es_fisico, item_estado, item_fecha_compra, item_precio_compra, item_observacion, item_activo)
SELECT al.alb_codigo, u.ubi_codigo, 'BM-CD-0002', NULL, TRUE, 'DISPONIBLE', CURRENT_DATE, 0, 'Carga inicial de ejemplo.', TRUE
FROM albums al
JOIN ubicaciones_fisicas u ON u.ubi_nombre = 'Estante Principal'
WHERE al.alb_titulo = 'Dive'
ON CONFLICT (item_codigo_interno) DO NOTHING;

INSERT INTO biblioteca_items (alb_codigo, ubi_codigo, item_codigo_interno, item_codigo_barra, item_es_fisico, item_estado, item_fecha_compra, item_precio_compra, item_observacion, item_activo)
SELECT al.alb_codigo, u.ubi_codigo, 'BM-VIN-0001', NULL, TRUE, 'DISPONIBLE', CURRENT_DATE, 0, 'Carga inicial de ejemplo.', TRUE
FROM albums al
JOIN ubicaciones_fisicas u ON u.ubi_nombre = 'Rack Vintage Pioneer'
WHERE al.alb_titulo = 'Arrival'
ON CONFLICT (item_codigo_interno) DO NOTHING;

INSERT INTO coleccion_albums (col_codigo, alb_codigo)
SELECT co.col_codigo, al.alb_codigo
FROM colecciones co
JOIN albums al ON al.alb_titulo IN ('MCMXC a.D.', 'Dive', 'Ainsi soit je...', 'Arrival', 'Saturday Night Fever', 'Lost in Love')
WHERE co.col_nombre = 'Colección Principal'
ON CONFLICT (col_codigo, alb_codigo) DO NOTHING;

INSERT INTO coleccion_albums (col_codigo, alb_codigo)
SELECT co.col_codigo, al.alb_codigo
FROM colecciones co
JOIN albums al ON al.alb_titulo IN ('Arrival', 'Saturday Night Fever', 'Lost in Love')
WHERE co.col_nombre = 'Clásicos 70s y 80s'
ON CONFLICT (col_codigo, alb_codigo) DO NOTHING;

INSERT INTO coleccion_albums (col_codigo, alb_codigo)
SELECT co.col_codigo, al.alb_codigo
FROM colecciones co
JOIN albums al ON al.alb_titulo IN ('MCMXC a.D.')
WHERE co.col_nombre = 'New Age y Electrónica'
ON CONFLICT (col_codigo, alb_codigo) DO NOTHING;

INSERT INTO playlist_canciones (play_codigo, can_codigo, play_can_orden)
SELECT p.play_codigo, c.can_codigo, 1
FROM playlists p
JOIN canciones c ON c.can_titulo = 'Sadeness'
WHERE p.play_nombre = 'Noche tranquila'
ON CONFLICT (play_codigo, can_codigo) DO NOTHING;

INSERT INTO playlist_canciones (play_codigo, can_codigo, play_can_orden)
SELECT p.play_codigo, c.can_codigo, 2
FROM playlists p
JOIN canciones c ON c.can_titulo = 'Captain Nemo'
WHERE p.play_nombre = 'Noche tranquila'
ON CONFLICT (play_codigo, can_codigo) DO NOTHING;

INSERT INTO playlist_canciones (play_codigo, can_codigo, play_can_orden)
SELECT p.play_codigo, c.can_codigo, 1
FROM playlists p
JOIN canciones c ON c.can_titulo = 'Dancing Queen'
WHERE p.play_nombre = 'Clásicos favoritos'
ON CONFLICT (play_codigo, can_codigo) DO NOTHING;

INSERT INTO playlist_canciones (play_codigo, can_codigo, play_can_orden)
SELECT p.play_codigo, c.can_codigo, 2
FROM playlists p
JOIN canciones c ON c.can_titulo = 'Stayin Alive'
WHERE p.play_nombre = 'Clásicos favoritos'
ON CONFLICT (play_codigo, can_codigo) DO NOTHING;

INSERT INTO usuarios (usu_nombres, usu_apellidos, usu_email, usu_password_hash, usu_activo)
VALUES ('Danilo', 'Administrador', 'admin@bibliotecamusical.local', 'CAMBIAR_PASSWORD_HASH', TRUE)
ON CONFLICT (usu_email) DO NOTHING;

INSERT INTO usuarios_roles (usu_codigo, rol_codigo)
SELECT u.usu_codigo, r.rol_codigo
FROM usuarios u
JOIN roles r ON r.rol_nombre = 'Administrador'
WHERE u.usu_email = 'admin@bibliotecamusical.local'
ON CONFLICT (usu_codigo, rol_codigo) DO NOTHING;

COMMIT;
