namespace AirAstana.FlightControl.Infrastructure.Options;

public class RedisKeysOptions
{
    public const string SectionName = nameof(RedisKeysOptions);
    public string Otp { get; set; } = null!;
}