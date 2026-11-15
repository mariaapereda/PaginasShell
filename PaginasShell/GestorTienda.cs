using System.Globalization;

namespace PaginasShell;

public static class GestorTienda
{
    private static readonly CultureInfo cultura = new CultureInfo("es-ES");
    private static bool cargado = false;

    public static List<Producto> Productos { get; } = new List<Producto>();
    public static List<LineaCesta> Cesta { get; } = new List<LineaCesta>();

    public static Producto? ProductoSeleccionado { get; set; }

    public static async Task CargarProductos()
    {
        if (cargado)
            return;

        using Stream flujo = await FileSystem.OpenAppPackageFileAsync("productos.txt");
        using StreamReader lector = new StreamReader(flujo);

        string? linea;
        while ((linea = await lector.ReadLineAsync()) != null)
        {
            Producto? producto = Producto.DesdeLinea(linea);

            if (producto != null)
                Productos.Add(producto);
        }

        cargado = true;
    }

    public static void AnadirACesta(Producto producto, int cantidad)
    {
        // si el producto ya estaba en la cesta solo sumo la cantidad
        LineaCesta? linea = Cesta.FirstOrDefault(l => l.Producto.Id == producto.Id);

        if (linea != null)
            linea.Cantidad += cantidad;
        else
            Cesta.Add(new LineaCesta { Producto = producto, Cantidad = cantidad });
    }

    public static int UnidadesEnCesta()
    {
        return Cesta.Sum(l => l.Cantidad);
    }

    public static decimal TotalCesta()
    {
        return Cesta.Sum(l => l.Subtotal);
    }

    public static string FormatoPrecio(decimal precio)
    {
        return precio.ToString("C", cultura);
    }
}
