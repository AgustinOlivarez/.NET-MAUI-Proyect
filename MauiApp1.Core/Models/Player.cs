using System.Text.Json.Serialization;
using SQLite;

namespace MauiApp1.Models
{
    [Table("Players")]
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("posicion")]
        public string Posicion { get; set; } = string.Empty;

        [JsonPropertyName("equipo")]
        public string Equipo { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        // Los dejamos como double normal (no nulos)
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}