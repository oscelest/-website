using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG.Abstract
{
    public abstract class VolumeItem : Volume
    {
        [Required]
        public required TemplateItem TemplateItem { get; set; }
    }
}
