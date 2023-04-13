using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public class VolumeItem : Volume
    {
        [Required]
        public required Item Item { get; set; }
    }
}
