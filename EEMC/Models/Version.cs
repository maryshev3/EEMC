using Newtonsoft.Json;
using System;

namespace EEMC.Models
{
    public class Version
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        [JsonIgnore]
        public string CreatedDateToString { get => CreatedDate.ToString("dd/MM/yyyy HH:mm:ss zzz"); }
        public string VersionName { get; set; }
        public string SavedFolder { get; set; }
    }
}
