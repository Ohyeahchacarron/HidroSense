namespace HidroSense.DTOs
{
    public class MedicionDTO
    {
        public string NombreSistema { get; set; }
        public string NombreFuente { get; set; }
        public float? Ph { get; set; }
        public float?  Turbidez { get; set; }
        public float? Temperatura { get; set; }
    }
}