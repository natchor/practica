using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using antecedentes_salud_backend.Models;
using Microsoft.AspNetCore.Mvc;
using ZXing;
using ZXing.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace antecedentes_salud_backend.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class FichaController : ControllerBase
    {
        private readonly FichaService _fichaService;
        private readonly ExternalAPI _externalAPI;

        public FichaController(FichaService fichaService, ExternalAPI externalAPIService)
        {
            _fichaService = fichaService;
            _externalAPI = externalAPIService;
        }

        [HttpGet("{rutCon}")]
        public async Task<IActionResult> Get(string rutCon)
        {
            try
            {
                var fichasMedicas = await _externalAPI.GetFichasMedicasAsync();
                var fichaMedica = fichasMedicas.FirstOrDefault(f => f.RutCon == rutCon);

                if (fichaMedica == null)
                {
                    return NotFound(new { message = "Ficha médica no encontrada" });
                }

                return Ok(fichaMedica);
            }
            catch (HttpRequestException ex)
            {
                return NotFound(new { message = "Ficha médica no encontrada", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            var isConnected = await _externalAPI.PingAsync();
            if (isConnected)
            {
                return Ok(new { message = "Conexión exitosa a la API" });
            }
            else
            {
                return StatusCode(500, new { message = "Error al conectar con la API" });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFichas()
        {
            try
            {
                var fichasMedicas = await _externalAPI.GetFichasMedicasAsync();
                return Ok(fichasMedicas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("qr")]
        public async Task<IActionResult> CreateQR([FromBody] QRRequest request)
        {
            try
            {
                var uuid = Guid.NewGuid();

                var qrData = new QR
                {
                    Rut = request.Rut,
                    Nombres = request.Nombres,
                    ApellidoMaterno = request.ApellidoMaterno,
                    ApellidoPaterno = request.ApellidoPaterno,
                    estado = "activo",
                    fechaCreacion = DateTime.UtcNow,
                    fechaEliminacion = DateTime.MinValue,
                    Hash = GenerateHash(request.Rut + DateTime.UtcNow.ToString())
                };

                await _fichaService.GuardarQR(qrData);

                var cleanUrl = GenerateCleanURL(qrData.Hash);
                var qrCodeImage = GenerateQRCode(cleanUrl);

                return Ok(new { qrData, cleanUrl, qrCodeImage });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private string GenerateCleanURL(string hash)
        {
            return $"http://localhost:3000/funcionario/FichaPage?hash={hash}";
        }

        [HttpPost("qr/deactivate/{rut}")]
        public async Task<IActionResult> DesactivarQR(string rut)
        {
            try
            {
                await _fichaService.DesactivarQR(rut);
                return Ok(new { message = "QR desactivado" });
            }
            catch (Exception ex)
            {
                if (ex.Message == "QR no encontrado o ya está inactivo")
                {
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Error al desactivar el QR" });
            }
        }

        [HttpPost("qr/create/{rut}")]
        public async Task<IActionResult> CrearNuevoQR(string rut)
        {
            try
            {
                // Verificar si ya existe un QR activo para el RUT
                var existeQR = await _fichaService.ExisteQRPorRut(rut);
                if (existeQR)
                {
                    return BadRequest(new { message = "Ya existe un QR activo para este RUT." });
                }

                // Crear un nuevo QR
                var nuevoQR = new QR
                {
                    Rut = rut,
                    estado = "activo",
                    fechaCreacion = DateTime.UtcNow,
                    fechaEliminacion = DateTime.MinValue,
                    Hash = GenerateHash(rut + DateTime.UtcNow.ToString())
                };

                await _fichaService.GuardarQR(nuevoQR);

                // Generar la imagen del código QR
                var qrCodeImage = GenerateQRCode($"http://localhost:3000/funcionario/FichaPage?hash={nuevoQR.Hash}");

                return Ok(new { message = "Nuevo QR creado", qrData = nuevoQR, qrCodeImage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el nuevo QR", details = ex.Message });
            }
        }

        private string GenerateHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        private string GenerateQRCode(string url)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 200,
                    Width = 200
                }
            };

            var pixelData = writer.Write(url);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                        pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        [HttpGet("qr/ficha/{hash}")]
        public async Task<IActionResult> ObtenerFichaPorQR(string hash)
        {
            try
            {
                // Obtener el RUT asociado al hash del QR
                var rut = await _fichaService.ObtenerRutPorHash(hash);
                if (rut == null)
                {
                    // Si no se encuentra el RUT, devolver un error 404
                    return NotFound("QR not found.");
                }

                // Obtener todas las fichas médicas desde la API externa
                var fichasMedicas = await _externalAPI.GetFichasMedicasAsync();

                // Buscar la ficha médica específica asociada al RUT
                var fichaMedica = fichasMedicas.FirstOrDefault(f => f.RutCon == rut);
                if (fichaMedica == null)
                {
                    // Si no se encuentra la ficha médica, devolver un error 404
                    return NotFound("Ficha no encontrada.");
                }

                // Devolver la ficha médica encontrada
                return Ok(fichaMedica);
            }
            catch (Exception ex)
            {
                // En caso de error, registrar el mensaje y devolver un error 500
                Console.WriteLine($"Error al obtener la ficha por QR: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("qr")]
        public async Task<ActionResult<IEnumerable<object>>> GetQRs()
        {
            try
            {
                var qrs = await _fichaService.GetQRAsync();
                var qrDataList = qrs.Select(qr => new
                {
                    qr.Rut,
                    qr.Nombres,
                    qr.ApellidoMaterno,
                    qr.ApellidoPaterno,
                    qr.estado,
                    qr.fechaCreacion,
                    qr.fechaEliminacion,
                    qr.Hash, // Usar Hash en lugar de ShortId
                    qrImage = GenerateQRCode($"http://localhost:3000/funcionario/FichaPage?hash={qr.Hash}") // Usar Hash en la URL
                }).ToList();

                return Ok(qrDataList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los QR: {ex.Message}");
                return StatusCode(500, $"Error al obtener los QR: {ex.Message}");
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _fichaService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                await _fichaService.GuardarUsuario(user);
                return Ok(new { message = "Usuario creado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpGet("user/{rut}")]
        public async Task<IActionResult> GetUserByRut(string rut)
        {
            try
            {
                var user = await _fichaService.GetUserByRutAsync(rut);
                if (user == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

       
        public class LoginRequest
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }

        public class QRRequest
        {
            public required string Rut { get; set; }
            public required string Nombres { get; set; }
            public required string ApellidoMaterno { get; set; }
            public required string ApellidoPaterno { get; set; }
        }

        public class QRUpdateRequest
        {
            public required string Estado { get; set; }
        }
    }
}