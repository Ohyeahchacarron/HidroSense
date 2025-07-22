using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string ApellidoPaterno { get; set; }

    public string ApellidoMaterno { get; set; }

    public int Edad { get; set; }

    public string Pais { get; set; }

    [Required]
    public string Correo { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Telefono { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    [Required]
    [RegularExpression("[1-3]")]
    public string Nivel { get; set; }

    public void EstablecerPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        PasswordHash = Convert.ToBase64String(sha256.ComputeHash(bytes));
    }

    public bool VerificarPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        return PasswordHash == Convert.ToBase64String(sha256.ComputeHash(bytes));
    }
}
