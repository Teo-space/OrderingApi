
/// <summary>
/// Результат Запроса\Команды
/// </summary>
/// <typeparam name="T">Тип</typeparam>
/// <param name="Value">При успешном выполнении - результат</param>
/// <param name="Success">Успешно\Проблема</param>
/// <param name="Type">Тип проблемы</param>
/// <param name="Detail">Описание проблемы</param>
/// <param name="Errors">Ошибки, если их несколько. Например ошибки валидации.</param>
public record Result<T>(T Value, bool Success, string Type, string Detail, IReadOnlyCollection<string> Errors)
{
    public static implicit operator T(Result<T> Result) => Result.Value;
    public static explicit operator Result<T>(T o) => Result.Ok(o);
    public static implicit operator string(Result<T> Result)
        => $"Result<{typeof(T)}>(Success:{Result.Success}, Type: {Result.Type}) Value: {Result?.Value?.ToString()}";
}