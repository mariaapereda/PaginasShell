namespace PaginasShell;

public partial class ProductosPage : ContentPage
{
    public ProductosPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await GestorTienda.CargarProductos();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error al cargar", "No se ha podido leer productos.txt.\n" + ex.Message, "Aceptar");
        }

        indCarga.IsRunning = false;
        indCarga.IsVisible = false;

        MostrarProductos();
    }

    private void MostrarProductos()
    {
        string texto = sbBuscar.Text?.Trim() ?? "";

        List<Producto> lista = GestorTienda.Productos
            .Where(p => p.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase))
            .ToList();

        cvProductos.ItemsSource = lista;
        lblTotal.Text = lista.Count == 1 ? "1 PRODUCTO" : $"{lista.Count} PRODUCTOS";
    }

    private void sbBuscar_TextChanged(object sender, TextChangedEventArgs e)
    {
        MostrarProductos();
    }

    private async void cvProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Producto producto)
            return;

        GestorTienda.ProductoSeleccionado = producto;
        cvProductos.SelectedItem = null;

        await Shell.Current.GoToAsync("//detalles");
    }

    private async void btnAyuda_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Ayuda",
            "Pulsa un producto para ver todos sus detalles en la pestaña Detalles.\n\n" +
            "Desde allí puedes elegir la cantidad y añadirlo a la cesta.\n\n" +
            "Usa el buscador para encontrar un producto por su nombre.",
            "Entendido");
    }
}
