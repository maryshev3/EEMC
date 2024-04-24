using System;

namespace EEMC.Models
{
    public class Version
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Description { get; set; }
    }
}
