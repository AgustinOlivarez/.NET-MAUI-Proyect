using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Repositories;
using MauiApp1.Views;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels
{
    public partial class PlayersViewModel : ObservableObject
    {
        private readonly IPlayerRepository _playerRepository;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _statusMessage;

        // 1. NUEVO: Propiedad para el texto del buscador
        [ObservableProperty]
        private string _searchText = string.Empty;

        // 2. NUEVO: Lista de respaldo para guardar a todos los jugadores en memoria
        private readonly List<Player> _allPlayers = new();

        public ObservableCollection<Player> Players { get; } = new ObservableCollection<Player>();

        public PlayersViewModel(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        // 3. NUEVO: Este método mágico se dispara automáticamente cuando "SearchText" cambia
        partial void OnSearchTextChanged(string value)
        {
            FilterPlayers(value);
        }

        // 4. NUEVO: Lógica que limpia y llena la lista visible según el filtro
        private void FilterPlayers(string searchTerm)
        {
            Players.Clear();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                foreach (var p in _allPlayers)
                    Players.Add(p);
            }
            else
            {
                var filtrados = _allPlayers.Where(p =>
                    (p.Nombre != null && p.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (p.Posicion != null && p.Posicion.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                foreach (var p in filtrados)
                    Players.Add(p);
            }
        }

        [RelayCommand]
        private async Task LoadPlayersAsync()
        {
            if (_isBusy) return;

            IsBusy = true;
            StatusMessage = "Cargando jugadores...";

            try
            {
                var localPlayers = await _playerRepository.GetAllAsync();

                // Actualizamos nuestro backup en memoria
                _allPlayers.Clear();
                if (localPlayers != null)
                {
                    _allPlayers.AddRange(localPlayers);
                }

                // Aplicamos el filtro actual (si no hay nada escrito, mostrará todos)
                FilterPlayers(SearchText);

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

        // 5. NUEVO: Comando para borrar jugadores (Swipe to Delete)
        [RelayCommand]
        private async Task DeletePlayerAsync(Player player)
        {
            if (player == null) return;

            // Preguntamos antes de borrar para evitar accidentes
            bool confirm = await Shell.Current.DisplayAlert("Eliminar", $"¿Dar de baja a {player.Nombre}?", "Sí", "Cancelar");
            if (!confirm) return;

            // Borramos de SQLite
            await _playerRepository.DeleteAsync(player);

            // Borramos de la memoria y actualizamos la UI
            _allPlayers.Remove(player);
            FilterPlayers(SearchText);
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