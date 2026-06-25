using System.Reflection;

using BibliotecaMusicalBack.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using Shouldly;

namespace BibliotecaMusicalBack.Tests.Controllers;

public class EndpointContractTests
{
    public static TheoryData<Type> CrudControllers =>
        new()
        {
            typeof(PaisesController),
            typeof(GenerosController),
            typeof(TiposArtistaController),
            typeof(SellosDiscograficosController),
            typeof(FormatosMusicalesController),
            typeof(UbicacionesFisicasController),
            typeof(RolesController),
            typeof(ArtistasController),
            typeof(ArtistasTiposController),
            typeof(AlbumsController),
            typeof(CancionesController),
            typeof(CancionesArtistasController),
            typeof(CompositoresController),
            typeof(CancionesCompositoresController),
            typeof(ColeccionesController),
            typeof(ColeccionesAlbumsController),
            typeof(BibliotecaItemsController),
            typeof(PlaylistsController),
            typeof(PlaylistsCancionesController),
            typeof(UsuariosRolesController),
            typeof(AuditoriaController)
        };

    [Theory]
    [MemberData(nameof(CrudControllers))]
    public void CadaControladorDebeExponerLosCincoEndpointsCrud(Type controllerType)
    {
        controllerType.GetCustomAttribute<ApiControllerAttribute>().ShouldNotBeNull();
        controllerType.GetCustomAttribute<RouteAttribute>()?.Template.ShouldNotBeNullOrWhiteSpace();

        Type baseType = controllerType.BaseType!;
        AssertEndpoint<HttpGetAttribute>(baseType, "GetAll", null);
        AssertEndpoint<HttpGetAttribute>(baseType, "GetById", "{id:int}");
        AssertEndpoint<HttpPostAttribute>(baseType, "Create", null);
        AssertEndpoint<HttpPutAttribute>(baseType, "Update", "{id:int}");
        AssertEndpoint<HttpDeleteAttribute>(baseType, "Delete", "{id:int}");
    }

    private static void AssertEndpoint<TAttribute>(
        Type controllerType,
        string methodName,
        string? expectedTemplate)
        where TAttribute : HttpMethodAttribute
    {
        MethodInfo method = controllerType.GetMethod(methodName)
            ?? throw new InvalidOperationException($"No existe la acción {methodName}.");
        TAttribute attribute = method.GetCustomAttribute<TAttribute>()
            ?? throw new InvalidOperationException($"{methodName} no posee {typeof(TAttribute).Name}.");

        attribute.Template.ShouldBe(expectedTemplate);
    }
}
