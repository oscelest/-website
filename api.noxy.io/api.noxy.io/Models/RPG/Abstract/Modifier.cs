using Database.Exceptions;
using Database.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Database.Models.RPG.Abstract
{
    public abstract class Modifier
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required float Value { get; set; }

        [Required]
        [Column(TypeName = "varchar(32)")]
        public required ArithmeticalTagType ArithmeticalTag { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        public static AritmaticalSet GetArithmaticalSet<T>(List<T> list) where T : Modifier
        {
            AritmaticalSet set = new();

            foreach (var item in list)
            {
                if (item.ArithmeticalTag == ArithmeticalTagType.Additive)
                {
                    set.Additive += item.Value;
                }
                else if (item.ArithmeticalTag == ArithmeticalTagType.Multiplicative)
                {
                    set.Multiplicative += item.Value;
                }
                else if (item.ArithmeticalTag == ArithmeticalTagType.Exponential)
                {
                    set.Exponential += item.Value;
                }
                else
                {
                    throw new EntityConditionException<Modifier>(item);
                }
            }

            return set;
        }

        public class AritmaticalSet
        {
            public float Additive { get; set; }
            public float Multiplicative { get; set; }
            public float Exponential { get; set; }

            public int GetTotalInt()
            {
                return GetTotalInt(0f);
            }

            public int GetTotalInt(float flat)
            {
                return (int)((flat + Additive) * Multiplicative * Exponential);
            }
        }
    }
}
