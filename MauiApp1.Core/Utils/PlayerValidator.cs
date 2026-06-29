using System;

namespace MauiApp1.Utils
{
    public static class PlayerValidator
    {
        public static bool ValidateNombre(string nombre, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                error = "El nombre no puede estar vacío.";
                return false;
            }
            return true;
        }

        public static bool ValidatePosicion(string posicion, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(posicion))
            {
                error = "La posición no puede estar vacía.";
                return false;
            }
            return true;
        }

        public static bool ValidateEquipo(string equipo, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(equipo))
            {
                error = "El equipo no puede estar vacío.";
                return false;
            }
            return true;
        }

        public static bool ValidateAll(string nombre, string posicion, string equipo, out string error)
        {
            if (!ValidateNombre(nombre, out error)) return false;
            if (!ValidatePosicion(posicion, out error)) return false;
            if (!ValidateEquipo(equipo, out error)) return false;
            error = string.Empty;
            return true;
        }
    }
}
