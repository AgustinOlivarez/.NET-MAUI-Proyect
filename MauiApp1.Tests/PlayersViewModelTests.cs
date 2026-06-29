using MauiApp1.Models;
using MauiApp1.Repositories;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
// using MauiApp1.Core.Repositories; // Descomenta si tu IPlayerRepository está acá

namespace MauiApp1.Tests
{
    public class PlayersViewModelTests
    {
        [Fact]
        public async Task LoadPlayersCommand_PopulatesPlayersList_And_TurnsOffIsBusy()
        {
            // 1. ARRANGE (Preparar el escenario)

            // Creamos una lista falsa (Mock) de jugadores que simulará venir de SQLite
            var mockPlayersFromDb = new List<Player>
            {
                new Player { Nombre = "Lionel Messi", Posicion = "Delantero", Equipo = "Inter Miami" },
                new Player { Nombre = "Dibu Martínez", Posicion = "Arquero", Equipo = "Aston Villa" }
            };

            // Creamos el "Mock" (doble de riesgo) del Repositorio
            var mockRepo = new Mock<IPlayerRepository>();

            // Le decimos al Mock: "Cuando alguien llame a GetAllAsync(), devuelve nuestra lista falsa"
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockPlayersFromDb);

            // Nota: Si tienes una interfaz IPlayerService para la API simulada, deberías mockearla también.
            // Si pasas null, asegúrate de que el ViewModel maneje el null sin explotar, 
            // o usa un Mock de tu servicio si implementaste la interfaz.
            var viewModel = new PlayersViewModel(mockRepo.Object);

            // 2. ACT (Ejecutar la acción)
            // Ejecutamos el comando de carga (los comandos en MVVM Toolkit generan métodos con "Command")
            await viewModel.LoadPlayersCommand.ExecuteAsync(null);

            // 3. ASSERT (Verificar los resultados)
            Assert.False(viewModel.IsBusy); // Debe haber terminado de cargar
            Assert.Equal(string.Empty, viewModel.StatusMessage); // No debe haber errores
            Assert.True(viewModel.Players.Count >= 2); // Debe tener al menos los 2 jugadores que mockeamos
        }
    }
}