using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Antecedentes
{
    [Key]
    public int Id { get; set; }
    [JsonPropertyName("rutCon")]
    public string RutCon { get; set; }
    [JsonPropertyName("nombres")]
    public string Nombres { get; set; }
    [JsonPropertyName("apellidoMaterno")]
    public string ApellidoMaterno { get; set; }
    [JsonPropertyName("apellidoPaterno")]
    public string ApellidoPaterno { get; set; }
    [JsonPropertyName("fechaNacimiento")]
    public DateTime FechaNacimiento { get; set; }
    [JsonPropertyName("alergias")]
    public string? Alergias { get; set; }
    [JsonPropertyName("medicamentos")]
    public string? Medicamentos { get; set; }
    [JsonPropertyName("enfermedades")]
    public string? Enfermedades { get; set; }
    [JsonPropertyName("mutualidad")]
    public string? Mutualidad { get; set; }
    [JsonPropertyName("grupoSanguineo")]
    public string? GrupoSanguineo { get; set; }
    [JsonPropertyName("factorRH")]
    public string? FactorRH { get; set; }
    [JsonPropertyName("obs")]
    public string? Obs { get; set; }
    [JsonPropertyName("nombreCont")]
    public string? NombreCont { get; set; }
    [JsonPropertyName("telefono")]
    public string? Telefono { get; set; }
    [JsonPropertyName("direccionCont")]
    public string? DireccionCont { get; set; }
    [JsonPropertyName("nombreCont2")]
    public string? NombreCont2 { get; set; }
    [JsonPropertyName("telefono2")]
    public string? TelefonoCont2 { get; set; }
    [JsonPropertyName("direccionCont2")]
    public string? DireccionCont2 { get; set; }
    [JsonPropertyName("nombreCont3")]
    public string? NombreCont3 { get; set; }
    [JsonPropertyName("telefono3")]
    public string? TelefonoCont3 { get; set; }
    [JsonPropertyName("direccionCont3")]
    public string? DireccionCont3 { get; set; }
}