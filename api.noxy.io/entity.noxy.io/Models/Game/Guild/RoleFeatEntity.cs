using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("RoleFeat")]
    public class RoleFeatEntity : SingleEntity
    {
        public required RoleEntity Role { get; set; }
        public required Guid RoleID { get; set; }

        public required FeatEntity Feat { get; set; }
        public required Guid FeatID { get; set; }
    }
}
