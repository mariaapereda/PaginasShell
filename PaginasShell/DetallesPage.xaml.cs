namespace PaginasShell;

public partial class DetallesPage : ContentPage
{
    private Producto? producto;

    public DetallesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (producto != GestorTienda.ProductoSeleccionado)
            stpCantidad.Value = 1;

        producto = GestorTienda.ProductoSeleccionado;

        if (producto == null)
        {
            vslSinProducto.IsVisible = true;
            vslProducto.IsVisible = false;
            return;
        }

        vslSinProducto.IsVisible = false;
        vslProducto.IsVisible = true;

        imgProducto.Source = producto.Imagen;
        lblNombre.Text = producto.Nombre;
        lblPrecio.Text = producto.PrecioTexto;
        lblDescripcion.Text = producto.Descripcion;
        MostrarCantidad();
    }

    private void MostrarCantidad()
    {
        if (producto == null)
            return;

        int cantidad = (int)stpCantidad.Value;
        lblCantidad.Text = cantidad == 1 ? "1 unidad" : $"{cantidad} unidades";
        lblSubtotal.Text = "Subtotal: " + GestorTienda.FormatoPrecio(producto.Precio * cantidad);
    }

    private void stpCantidad_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        MostrarCantidad();
    }

    private async void btnAnadir_Clicked(object sender, EventArgs e)
    {
        if (producto == null)
            return;

        int cantidad = (int)stpCantidad.Value;
        GestorTienda.AnadirACesta(producto, cantidad);

        bool verCesta = await DisplayAlert("Añadido a la cesta",
            $"Has añadido {cantidad} x {producto.Nombre}.\nAhora tienes {GestorTienda.UnidadesEnCesta()} artículos en la cesta.",
            "Ver cesta", "Seguir comprando");

        stpCantidad.Value = 1;

        if (verCesta)
            await Shell.Current.GoToAsync("//cesta");
        else
            await Shell.Current.GoToAsync("//productos");
    }

    private async void btnIrProductos_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//productos");
    }
}
