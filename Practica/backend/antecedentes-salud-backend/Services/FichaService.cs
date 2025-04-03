using System.Text;
using System.Security.Cryptography;
using antecedentes_salud_backend.Models;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Seguridad;

public class FichaService
{
    private readonly AppDbContext _context;

    public FichaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ValidateEmailAsync(string email, string password)
    {
        // Validar el correo en la base de datos
        var userExistsInDb = await _context.Users.AnyAsync(u => u.Correo == email);
        if (!userExistsInDb)
        {
            return false;
        }

        // Validar el correo en LDAP si está habilitado
        if (LdapConfig.UseLoginLdap)
        {
            return LdapUtils.ValidarLogin(email, password);
        }

        // Si LDAP no está habilitado, solo validar en la base de datos
        return true;
    }

    public async Task<List<Antecedentes>> GetFichasAsync()
    {
        try
        {
            return await _context.Fichas.ToListAsync();
        }
        catch (Exception ex)
        {
            // Registrar el error
            Console.WriteLine($"Error al obtener las fichas: {ex.Message}");
            throw;
        }
    }

    public async Task GuardarQR(QR qr)
    {
        var existingQR = await _context.QRs.FirstOrDefaultAsync(q => q.Rut == qr.Rut && q.estado == "activo");
        if (existingQR != null)
        {
            throw new Exception("Ya existe un QR activo para este RUT.");
        }

        qr.fechaCreacion = DateTime.Now;
        qr.fechaEliminacion = DateTime.MinValue; // Valor predeterminado
        _context.QRs.Add(qr);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Obtener detalles de la excepción interna
            var innerException = ex.InnerException?.Message;
            throw new Exception($"Error al guardar el QR: {innerException}", ex);
        }
    }

  

    public async Task ActualizarQR(QR qr)
    {
        _context.QRs.Update(qr);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteQRPorRut(string rut)
    {
        return await _context.QRs.AnyAsync(q => q.Rut == rut);
    }

    public async Task<Antecedentes> ObtenerFichaPorQR(string hash)
    {
        var qr = await _context.QRs.FirstOrDefaultAsync(q => q.Hash == hash);
        if (qr == null)
        {
            return null;
        }

        return await _context.Fichas.FirstOrDefaultAsync(f => f.RutCon == qr.Rut);
    }



    public async Task<string> ObtenerRutPorHash(string hash)
    {
        var qr = await _context.QRs.FirstOrDefaultAsync(q => q.Hash == hash);
        return qr?.Rut;
    }
    public async Task<List<QR>> GetQRAsync()
    {
        try
        {
            return await _context.QRs.ToListAsync();
        }
        catch (Exception ex)
        {
            // Registrar el error
            Console.WriteLine($"Error al obtener los QR: {ex.Message}");
            throw new ApplicationException("Error al obtener los QR", ex);
        }
    }

    public async Task<QR> ObtenerQRPorHash(string hash)
    {
        return await _context.QRs.FirstOrDefaultAsync(q => q.Hash == hash);
    }

    public async Task<Antecedentes> ObtenerFichaPorRut(string rut)
    {
        return await _context.Fichas.FirstOrDefaultAsync(f => f.RutCon == rut);
    }

    public async Task DesactivarQR(string rut)
    {
        var qr = await _context.QRs.FirstOrDefaultAsync(q => q.Rut == rut && q.estado == "activo");
        if (qr == null)
        {
            throw new Exception("QR no encontrado o ya está inactivo");
        }
        qr.estado = "inactivo"; // Actualizar el campo estado
        qr.fechaEliminacion = DateTime.UtcNow; // Establecer la fecha de eliminación
        await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos
    }

    public async Task CrearNuevoQR(string rut)
    {
        var nuevoQR = new QR
        {
            Rut = rut,
            estado = "activo",
            fechaCreacion = DateTime.UtcNow,
            fechaEliminacion = DateTime.MinValue,
            Hash = GenerateHash(rut + DateTime.UtcNow.ToString())
        };
        _context.QRs.Add(nuevoQR);
        await _context.SaveChangesAsync();
    }

    private string GenerateHash(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByRutAsync(string rut)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.rut == rut);
    }

public async Task GuardarUsuario(User user)
{
    try
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
        // Registrar el error interno
        var innerException = ex.InnerException?.Message;
        Console.WriteLine($"Error al guardar el usuario: {innerException}");
        throw new Exception($"Error al guardar el usuario: {innerException}", ex);
    }
}

}