using api.noxy.io.Models.Game.Feat;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Models.Game.Requirement;
using api.noxy.io.Models.RPG.Junction;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    public abstract class Template
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        // Implementations

        [Table("TemplateFeat")]
        public class Feat : Template
        {
            public List<Requirement> RequirementList { get; set; } = new();

            public List<Modifier.Guild> GuildModifierList { get; set; } = new();
        }

        public abstract class Item
        {

            [Table("TemplateItemAugmentation")]
            public class Augmentation : Item
            {
                [Required]
                public required Slot Slot { get; set; }

                public required List<Guid> ModifierList { get; set; }
            }

            [Table("TemplateItemEquipment")]
            public class Equipment : Item
            {
                [Required]
                public required Slot Slot { get; set; }

                public required List<Slot> TemplateSlotList { get; set; }
            }

            [Table("TemplateItemMap")]
            public class Map : Item
            {
                public required List<Slot> TemplateSlotList { get; set; }
            }
        }

        [Table("TemplateMission")]
        public class Mission : Template
        {

        }

        [Table("TemplateRecipe")]
        public class Recipe : Template
        {
            [Comment("A list of items required for performing the recipe.")]
            public required List<ItemVolume> NeededItemVolumeList { get; set; }

            [Comment("A list of items rewarded for performing the recipe.")]
            public required List<ItemVolume> ResultItemVolumeList { get; set; }
        }

        [Table("TemplateRequirement")]
        public class Requirement : Template
        {
            // Mappings

            public List<FeatEntity> FeatList { get; set; } = new();
            public List<GuildRoleEntity> RoleList { get; set; } = new();
        }

        [Table("TemplateRole")]
        public class Role : Template
        {

        }

        [Table("TemplateSlot")]
        public class Slot : Template
        {

        }

        [Table("TemplateUnit")]
        public class Unit : Template
        {

        }
    }
}
