using System;
using System.Threading.Tasks;
using MauiApp1.Models;
using MauiApp1.Repositories;
using MauiApp1.Utils;

namespace MauiApp1.Services.Core
{
    public class AddPlayerService : IAddPlayerService
    {
        private readonly IPlayerRepository _repository;

        public AddPlayerService(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool Success, string Message)> SavePlayerAsync(Player player)
        {
            if (player == null) return (false, "Jugador nulo.");

            if (!PlayerValidator.ValidateAll(player.Nombre, player.Posicion, player.Equipo, out var error))
                return (false, error);

            try
            {
                await _repository.InitializeAsync();
                var res = await _repository.AddAsync(player);
                if (res > 0) return (true, "Jugador guardado correctamente.");
                return (false, "No se pudo insertar el jugador en la base de datos.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar: {ex.Message}");
            }
        }
    }
}
