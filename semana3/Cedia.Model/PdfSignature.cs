using System;
using Newtonsoft.Json;

namespace Cedia.Models
{
    public class PdfCertificate
    {
        [JsonProperty(Order = 1, PropertyName = "Razon")]
        public string Reason { get; set; } = "";

        [JsonProperty(Order = 2, PropertyName = "Locacion")]
        public string Location { get; set; } = "";

        [JsonProperty(Order = 3, PropertyName = "Nombre")]
        public string Name { get; set; } = "";

        [JsonProperty(Order = 4, PropertyName = "FechaFirma")]
        public DateTime SignedDate { get; set; }

        [JsonProperty(Order = 5, PropertyName = "ValidoDesde")]
        public DateTime ValidFrom { get; set; }

        [JsonProperty(Order = 6, PropertyName = "ValidoHasta")]
        public DateTime ValidTo { get; set; }

        [JsonProperty(Order = 7, PropertyName = "Asunto")]
        public string Subject { get; set; } = "";

        [JsonProperty(Order = 8, PropertyName = "Emisor")]
        public string Issuer { get; set; } = "";

        [JsonProperty(Order = 9, PropertyName = "OrigenFirma")]
        public string SignatureOrigin { get; set; } = "";
    }
}
