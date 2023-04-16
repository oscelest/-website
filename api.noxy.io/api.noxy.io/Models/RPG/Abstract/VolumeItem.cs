using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public abstract class VolumeItem : Volume
    {
        [Required]
        public required TemplateItem TemplateItem { get; set; }
    }
}
