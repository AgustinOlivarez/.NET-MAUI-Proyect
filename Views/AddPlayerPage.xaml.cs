using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class AddPlayerPage : ContentPage
{
    private readonly AddPlayerViewModel _vm;
    public AddPlayerPage(AddPlayerViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm?.StartListeningShake();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm?.StopListeningShake();
    }
}
