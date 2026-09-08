using System;
using System.Collections.Generic;
using System.Text;

namespace TiqueTac.Domain.Entities;

public class Usuario
{
    // Propiedades que coinciden con dtUsuarios de PostgreSQL
    public int IdUsuario { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;

    // Constructor vacío para Entity Framework Core
    private Usuario() { }

    // Constructor oficial
    public Usuario(string email, string nombre)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("El formato del correo electrónico no es válido.", nameof(email));

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del usuario no puede estar vacío.", nameof(nombre));

        Email = email.ToLower().Trim();
        Nombre = nombre.Trim(); 
    }
}
