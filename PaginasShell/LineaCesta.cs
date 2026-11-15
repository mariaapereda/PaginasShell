namespace PaginasShell;

public class LineaCesta
{
    public Producto Producto { get; set; } = new Producto();
    public int Cantidad { get; set; }

    public decimal Subtotal => Producto.Precio * Cantidad;

    public string TextoCantidad => $"{Cantidad} x {Producto.PrecioTexto}";

    public string SubtotalTexto => GestorTienda.FormatoPrecio(Subtotal);
}
