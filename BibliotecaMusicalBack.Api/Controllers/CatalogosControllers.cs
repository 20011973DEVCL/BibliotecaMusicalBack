using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController, Route("api/paises")]
public class PaisesController(ICrudService<PaisDto> service) : CrudControllerBase<PaisDto>(service)
{
}

[ApiController, Route("api/generos")]
public class GenerosController(ICrudService<GeneroDto> service) : CrudControllerBase<GeneroDto>(service)
{
}

[ApiController, Route("api/tipos-artista")]
public class TiposArtistaController(ICrudService<TipoArtistaDto> service)
    : CrudControllerBase<TipoArtistaDto>(service)
{
}

[ApiController, Route("api/sellos-discograficos")]
public class SellosDiscograficosController(ICrudService<SelloDiscograficoDto> service)
    : CrudControllerBase<SelloDiscograficoDto>(service)
{
}

[ApiController, Route("api/formatos-musicales")]
public class FormatosMusicalesController(ICrudService<FormatoMusicalDto> service)
    : CrudControllerBase<FormatoMusicalDto>(service)
{
}

[ApiController, Route("api/ubicaciones-fisicas")]
public class UbicacionesFisicasController(ICrudService<UbicacionFisicaDto> service)
    : CrudControllerBase<UbicacionFisicaDto>(service)
{
}

[ApiController, Route("api/roles")]
public class RolesController(ICrudService<RolDto> service) : CrudControllerBase<RolDto>(service)
{
}
