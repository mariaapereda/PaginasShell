namespace PaginasShell;

public partial class CestaPage : ContentPage
{
    public CestaPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ActualizarCesta();
    }

    private void ActualizarCesta()
    {
        cvCesta.ItemsSource = GestorTienda.Cesta.ToList();

        int unidades = GestorTienda.UnidadesEnCesta();
        lblArticulos.Text = unidades == 1 ? "1 ARTÍCULO" : $"{unidades} ARTÍCULOS";
        lblTotal.Text = GestorTienda.FormatoPrecio(GestorTienda.TotalCesta());

        grdBotones.IsVisible = GestorTienda.Cesta.Count > 0;
    }

    private async void btnQuitar_Clicked(object sender, EventArgs e)
    {
        var boton = (Button)sender;

        if (boton.BindingContext is LineaCesta linea)
        {
            bool quitar = await DisplayAlert("Quitar producto",
                $"¿Quieres quitar {linea.Producto.Nombre} de la cesta?", "Sí, quitar", "Cancelar");

            if (!quitar)
                return;

            GestorTienda.Cesta.Remove(linea);
            ActualizarCesta();
        }
    }

    private async void btnVaciar_Clicked(object sender, EventArgs e)
    {
        bool vaciar = await DisplayAlert("Vaciar cesta",
            "Se quitarán todos los productos de la cesta. ¿Seguro?", "Sí, vaciar", "Cancelar");

        if (!vaciar)
            return;

        GestorTienda.Cesta.Clear();
        ActualizarCesta();
    }

    private async void btnPedido_Clicked(object sender, EventArgs e)
    {
        string total = GestorTienda.FormatoPrecio(GestorTienda.TotalCesta());

        bool confirmar = await DisplayAlert("Confirmar pedido",
            $"Vas a hacer un pedido de {GestorTienda.UnidadesEnCesta()} artículos por {total}. ¿Continuar?",
            "Confirmar", "Cancelar");

        if (!confirmar)
            return;

        GestorTienda.Cesta.Clear();
        ActualizarCesta();

        await DisplayAlert("Pedido realizado", $"Tu pedido de {total} se ha registrado correctamente. ¡Gracias por tu compra!", "Aceptar");
    }
}
