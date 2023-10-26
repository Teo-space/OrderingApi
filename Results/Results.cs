using FluentValidation.Results;

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
public static class ResultTypedExtensions
{
    public static Result<T> Ok<T>(this T result) where T : class => Result.Ok(result);
}


public partial class Result
{
    private static readonly IReadOnlyCollection<string> EmptyErrors = new List<string>().AsReadOnly();

    public static Result<T> Ok<T>(T Value)
        => new Result<T>(Value: Value, Success: true, Type: "OkResult", "OkResult", EmptyErrors);

    public static Result<T> Problem<T>(string Type, string Detail)
        => new Result<T>(Value: default, Success: false, Type, Detail, EmptyErrors);

    public static Result<T> Exception<T>(string Type, string Detail) => Problem<T>("Exception", Detail);

    public static Result<T> Errors<T>(string Type, string Detail, IReadOnlyCollection<string> Errors)
        => new Result<T>(Value: default, Success: false, Type, Detail, Errors);

    public static Result<T> InputValidationError<T>(string Detail)
        => Errors<T>("InputValidationError", Detail, EmptyErrors);

    public static Result<T> InputValidationErrors<T>(ValidationResult result)
    {
        var errors = result.Errors
                .Select(error => $"Property : {error.PropertyName}, With error : {error.ErrorMessage}, InputValue: {error.AttemptedValue}")
                .ToList().AsReadOnly();
        return Errors<T>("InputValidationErrors", $"One or more validation errors during execution", errors);
    }

    public static Result<T> UnAuthorizedResult<T>(string Detail) => Problem<T>("UnAuthorized", Detail);
    public static Result<T> NotEnoughPermissions<T>(string Detail) => Problem<T>("NotEnoughPermissions", Detail);


    public static Result<T> NotFound<T>(string Detail) => Problem<T>("NotFound", Detail);
    public static Result<T> Deleted<T>(string Detail) => Problem<T>("Deleted", Detail);
    public static Result<T> ParentNotFound<T>(string Detail) => Problem<T>("ParentNotFound", Detail);
    public static Result<T> Conflict<T>(string Detail) => Problem<T>("Conflict", Detail);
    public static Result<T> InvalidOperation<T>(string Detail) => Problem<T>("InvalidOperation", Detail);



}



