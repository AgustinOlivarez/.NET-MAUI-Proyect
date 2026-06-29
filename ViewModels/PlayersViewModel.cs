using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models; // Asegúrate de que apunte al Core si el Player está allí
using MauiApp1.Repositories;
using MauiApp1.Services;
using MauiApp1.Views;
using System.Collections.ObjectModel;
// using MauiApp1.Core.Repositories; // Descomenta si tu IPlayerRepository está en otro namespace

namespace MauiApp1.ViewModels
{
    public partial class PlayersViewModel : ObservableObject
    {
        private readonly IPlayerRepository _playerRepository; // Inyectamos el repositorio local

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _statusMessage;

        public ObservableCollection<Player> Players { get; } = new ObservableCollection<Player>();

        // Agregamos el IPlayerRepository al constructor
        public PlayersViewModel(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [RelayCommand]
        private async Task LoadPlayersAsync()
        {
            if (_isBusy) return;

            IsBusy = true;
            StatusMessage = "Cargando jugadores...";
            Players.Clear();

            try
            {
                // 2. Cargar los jugadores "Scouteados" desde SQLite (Local)
                // Asumimos que tu repositorio tiene un método GetAllAsync()
                var localPlayers = await _playerRepository.GetAllAsync();
                if (localPlayers != null)
                {
                    foreach (var player in localPlayers)
                    {
                        Players.Add(player);
                    }
                }

                StatusMessage = string.Empty;
            }
            catch (HttpRequestException httpEx)
            {
                if (httpEx.StatusCode.HasValue)
                {
                    int statusCode = (int)httpEx.StatusCode.Value;
                    if (statusCode >= 400 && statusCode < 500)
                        StatusMessage = $"Error del cliente ({statusCode}): Verifique la solicitud.";
                    else if (statusCode >= 500)
                        StatusMessage = $"Error del servidor ({statusCode}): Intente más tarde.";
                    else
                        StatusMessage = $"Error HTTP ({statusCode}): {httpEx.Message}";
                }
                else
                {
                    StatusMessage = "Problema de conexión. Compruebe su internet.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SelectPlayerAsync(Player player)
        {
            if (player == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "SelectedPlayer", player }
            };

            await Shell.Current.GoToAsync(nameof(PlayerDetailPage), navigationParameter);
        }

        [RelayCommand]
        private async Task GoToAddPlayerAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddPlayerPage));
        }
    }
}