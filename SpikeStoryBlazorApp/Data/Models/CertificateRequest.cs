using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SpikeStoryBlazorApp.Data.Models
{
    public class CertificateRequest
    {
        [JsonPropertyName("alt_names")]
        public string AltNames { get; set; }

        [JsonPropertyName("common_name")]
        public string JsonCommonName { get; set; }
        [JsonIgnore]
        [Required]
        public string CommonName
        {
            get
            {
                return JsonCommonName?[0..^19];
            }
            set 
            {
                JsonCommonName = $"{value}.kubetest.dotgov.md";
            }
        }

        [JsonPropertyName("exclude_cn_from_sans")]
        public bool ExcludeCnFromSans { get; set; } = true;
        public string Format { get; set; } = "pem";

        [JsonPropertyName("ip_sans")]
        public List<string> IpSans { get; set; }

        [JsonPropertyName("other_sans")]
        public List<string> OtherSans { get; set; }

        [JsonPropertyName("private_key_format")]
        public string PrivateKeyFormat { get; set; } = "pfx";

        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }
        public string Ttl { get; set; } = "63072000";

        [JsonPropertyName("uri_sans")]
        public List<string> UriSans { get; set; }
        public string Pin { get; set; }
    }
}
