using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[EnableCors]
[ApiController]
[Route("api/[controller]")]
public class FichaMedicaController : ControllerBase
{
    private readonly DatabaseHelper _databaseHelper;

    public FichaMedicaController(DatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            List<Antecedentes> fichaMedicaData = _databaseHelper.GetFichaMedicaData();
            return Ok(fichaMedicaData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error interno del servidor: " + ex.Message);
        }
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        string result = _databaseHelper.PingDatabase();
        if (result.StartsWith("Error"))
        {
            return StatusCode(500, result);
        }
        return Ok(result);
    }
}