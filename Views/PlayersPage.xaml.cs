using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class PlayersPage : ContentPage
{
    public PlayersPage(PlayersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // NUEVO: Este método se ejecuta CADA VEZ que la página se vuelve visible en pantalla
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Verificamos que el contexto de datos sea nuestro ViewModel
        if (BindingContext is PlayersViewModel viewModel)
        {
            // Ejecutamos el comando de recarga
            viewModel.LoadPlayersCommand.Execute(null);
        }
    }
}