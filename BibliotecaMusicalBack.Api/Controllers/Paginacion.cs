namespace BibliotecaMusicalBack.Api.Controllers;

internal static class Paginacion
{
    public static (int Pagina,int Tamano) Normalizar(int pagina,int tamano)=>(Math.Max(1,pagina),Math.Clamp(tamano,1,100));
}
