using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace MauiApp1.Tests
{
    public class PlayerRepositoryIntegrationTests
    {
        [Fact]
        public async Task PlayerRepositorySQLite_TempDB_CRUD_Works()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"test_players_{Guid.NewGuid()}.db3");
            var repo = new MauiApp1.Repositories.PlayerRepositorySQLite(dbPath);
            await repo.InitializeAsync();

            var player = new MauiApp1.Models.Player
            {
                Nombre = "Test Player",
                Posicion = "Mediocampista",
                Equipo = "Test FC"
            };

            var insertResult = await repo.AddAsync(player);
            var all = await repo.GetAllAsync();

            Assert.True(insertResult > 0);
            Assert.Contains(all, p => p.Nombre == "Test Player" && p.Equipo == "Test FC");

            try { File.Delete(dbPath); } catch { }
        }
    }
}
