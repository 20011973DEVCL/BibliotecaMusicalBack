using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController, Route("api/artistas")]
public class ArtistasController(ICrudService<ArtistaDto> service) : CrudControllerBase<ArtistaDto>(service)
{
}

[ApiController, Route("api/artistas-tipos")]
public class ArtistasTiposController(ICrudService<ArtistaTipoDto> service)
    : CrudControllerBase<ArtistaTipoDto>(service)
{
}

[ApiController, Route("api/albums")]
public class AlbumsController(ICrudService<AlbumDto> service) : CrudControllerBase<AlbumDto>(service)
{
}

[ApiController, Route("api/canciones")]
public class CancionesController(ICrudService<CancionDto> service) : CrudControllerBase<CancionDto>(service)
{
}

[ApiController, Route("api/canciones-artistas")]
public class CancionesArtistasController(ICrudService<CancionArtistaDto> service)
    : CrudControllerBase<CancionArtistaDto>(service)
{
}

[ApiController, Route("api/compositores")]
public class CompositoresController(ICrudService<CompositorDto> service)
    : CrudControllerBase<CompositorDto>(service)
{
}

[ApiController, Route("api/canciones-compositores")]
public class CancionesCompositoresController(ICrudService<CancionCompositorDto> service)
    : CrudControllerBase<CancionCompositorDto>(service)
{
}
