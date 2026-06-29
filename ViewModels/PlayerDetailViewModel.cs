using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Repositories;

namespace MauiApp1.ViewModels
{
    [QueryProperty(nameof(Player), "SelectedPlayer")]
    public partial class PlayerDetailViewModel : ObservableObject
    {
        // Inyectamos el repositorio
        private readonly IPlayerRepository _playerRepository;

        [ObservableProperty]
        private Player _player;

        // Constructor para recibir el repositorio (y evitar el error que vimos hace un rato en los tests)
        public PlayerDetailViewModel(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        // Comando para actualizar los datos en SQLite
        [RelayCommand]
        private async Task SaveChangesAsync()
        {
            if (Player == null) return;

            // Validaciones rápidas para que no guarden un jugador en blanco
            if (string.IsNullOrWhiteSpace(Player.Nombre) || string.IsNullOrWhiteSpace(Player.Posicion))
            {
                await Shell.Current.DisplayAlert("Error", "El nombre y la posición son obligatorios.", "OK");
                return;
            }

            // Actualizamos en la base de datos
            await _playerRepository.UpdateAsync(Player);

            // Avisamos al usuario y regresamos a la lista
            await Shell.Current.DisplayAlert("Scouting", "Perfil del jugador actualizado exitosamente.", "OK");

            // GoBack: regresa a la página anterior
            await Shell.Current.GoToAsync("..");
        }
    }
}