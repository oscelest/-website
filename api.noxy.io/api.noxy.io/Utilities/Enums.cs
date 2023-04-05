namespace api.noxy.io.Utilities
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

    public enum GuildRoleModifierTagType
    {
        Experience = 0,
        Count = 1,
    }

    public enum GuildUnitModifierTagType
    {
        RefreshTime = 0,
        Experience = 1,
        Count = 2,
    }

    public enum GuildMissionModifierTagType
    {
        RefreshTime = 0,
        CompletionTime = 1,
        Count = 2,
    }

    public enum StatisticTagType
    {
        TotalUnitCount = 0,
        TotalCurrencyCount = 1,
        TotalFeatCount = 2,
        TotalMissionCount = 3,
    }
}
