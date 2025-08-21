using System;
using System.Threading.Tasks;

namespace AirAstana.FlightControl.Application.Interfaces.DistributedCache;

/// <summary>
/// Сервис для работы с распределенным кэшом
/// </summary>
public interface IDistributedCacheService
{
    /// <summary>
    /// Сохранение в кэше по времени
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    Task SetDataWithAbsExpTimeAsync<T>(string key, T value, TimeSpan timeSpan);

    /// <summary>
    /// Получение из кэша
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T> GetDataAsync<T>(string key);

    /// <summary>
    /// Удаление из кэша
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task RemoveDataAsync(string key);
}