using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APIMe.Models;

namespace APIMe.Entities.Models
{
    public partial class DeviceCode
    {
        [Key]
        public string UserCode { get; set; } = null!;
        public string DeviceCode1 { get; set; } = null!;
        public string? SubjectId { get; set; }
        public string? SessionId { get; set; }
        public string ClientId { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime Expiration { get; set; }
        public string Data { get; set; } = null!;
    }
}
