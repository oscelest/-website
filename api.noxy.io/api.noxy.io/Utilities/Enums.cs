namespace Database.Utilities
{
    public enum GuildStateType
    {
        Home = 0,
        Combat = 1,
    }

    public enum ArithmeticalTagType
    {
        Additive,
        Multiplicative,
        Exponential,
    }

    public enum GuildModifierEntityType
    {
        Unit = 0,
        Role = 1
    }

    public enum ModifierGuildRoleTagType
    {
        Experience = 0,
        Count = 1,
    }

    public enum ModifierGuildUnitTagType
    {
        RefreshTime = 0,
        Experience = 1,
        Count = 2,
        Limit = 3,
    }

    public enum ModifierGuildMissionTagType
    {
        RefreshTime = 0,
        Count = 1,
        Limit = 2,
    }

    public enum StatisticTagType
    {
        TotalUnitCount = 0,
        TotalCurrencyCount = 1,
        TotalFeatCount = 2,
        TotalMissionCount = 3,
    }

    public enum EventTagType
    {
        UnitRefresh = 0,
        MissionRefresh = 1,
    }

    public enum ItemAttributeTagType
    {
        Health,
        AttackPower,
        SpellPower,
        Armor,
        Ward,
    }

    public enum UnitAttributeTagType
    {
        Health,
        AttackPower,
        SpellPower,
        Armor,
        Ward,
    }

    public enum SkillAttributeTagType
    {
        Damage,
        Heal,

        ChargeSkillMax,

        ComboPointChange,
        ComboPointRetain,
        ComboPointMax,

        EffectDuration,
        HitCount,

        UnitAttributeHealth,
        UnitAttributeAttackPower,
        UnitAttributeSpellPower,
        UnitAttributeArmor,
        UnitAttributeWard
    }

    public enum VolumeItemRecipeTagType
    {
        Input = 0,
        Output = 1,
    }
}
