using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController, Route("api/colecciones")]
public class ColeccionesController(ICrudService<ColeccionDto> service)
    : CrudControllerBase<ColeccionDto>(service)
{
}

[ApiController, Route("api/colecciones-albums")]
public class ColeccionesAlbumsController(ICrudService<ColeccionAlbumDto> service)
    : CrudControllerBase<ColeccionAlbumDto>(service)
{
}

[ApiController, Route("api/biblioteca-items")]
public class BibliotecaItemsController(ICrudService<BibliotecaItemDto> service)
    : CrudControllerBase<BibliotecaItemDto>(service)
{
}

[ApiController, Route("api/playlists")]
public class PlaylistsController(ICrudService<PlaylistDto> service)
    : CrudControllerBase<PlaylistDto>(service)
{
}

[ApiController, Route("api/playlists-canciones")]
public class PlaylistsCancionesController(ICrudService<PlaylistCancionDto> service)
    : CrudControllerBase<PlaylistCancionDto>(service)
{
}
