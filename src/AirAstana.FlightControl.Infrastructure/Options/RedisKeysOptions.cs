using System.Collections.Generic;
using AirAstana.FlightControl.Domain.Entities;

namespace AirAstana.FlightControl.Infrastructure.Options;

public class RedisKeysOptions
{
    public const string SectionName = nameof(RedisKeysOptions);
    public string FlightsCacheKey { get; set; } = null!;
}