using System.Globalization;

namespace PaginasShell;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public decimal Precio { get; set; }
    public string Imagen { get; set; } = "";

    public string PrecioTexto => GestorTienda.FormatoPrecio(Precio);

    public static Producto? DesdeLinea(string linea)
    {
        string[] campos = linea.Split('#');

        if (campos.Length != 5)
            return null;

        if (!int.TryParse(campos[0].Trim(), out int id))
            return null;

        if (!decimal.TryParse(campos[3].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precio))
            return null;

        return new Producto
        {
            Id = id,
            Nombre = campos[1].Trim(),
            Descripcion = campos[2].Trim(),
            Precio = precio,
            Imagen = campos[4].Trim()
        };
    }
}
