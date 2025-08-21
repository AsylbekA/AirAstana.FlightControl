using System;
using System.Threading.Tasks;
using AirAstana.FlightControl.Application.Interfaces.DistributedCache;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace AirAstana.FlightControl.Infrastructure.Services.DistributedCache;

public class DistributedCacheService : IDistributedCacheService
{
    private readonly IDistributedCache _distributedCache;

    public DistributedCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task RemoveDataAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }

    public async Task<T> GetDataAsync<T>(string key)
    {
        var value = await _distributedCache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(value))
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        return default;
    }

    public async Task SetDataWithAbsExpTimeAsync<T>(string key, T value, TimeSpan timeSpan)
    {
        await _distributedCache.SetStringAsync(key,
            JsonConvert.SerializeObject(value),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeSpan, });
    }
}