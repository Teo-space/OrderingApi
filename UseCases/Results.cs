using FluentValidation.Results;


public record Result<T>(T Value, bool Success, string message, IReadOnlyCollection<string> Errors)
{
	public static implicit operator T(Result<T> Result) => Result.Value;
	public static explicit operator Result<T>(T o) => Result.Ok(o);
	public static implicit operator string(Result<T> Result) => Result.Value?.ToString() ?? string.Empty;

}
public static class ResultTypedExtensions
{
	public static Result.OkResult<T> Ok<T>(this T result) where T : class => Result.Ok(result);
}
public partial record Result
{
	private static readonly IReadOnlyCollection<string> EmptyErrors = new List<string>().AsReadOnly();


    public record OkResult<T>(T Value) : Result<T>(Value, true, string.Empty, EmptyErrors);
	public static OkResult<T> Ok<T>(T Value) => new OkResult<T>(Value);


	public record MessageResult<T>(string message) : Result<T>(default(T), false, message, EmptyErrors);
	public static MessageResult<T> Message<T>(string message) => new MessageResult<T>(message);



	public record InputValidationErrorResult<T>(string message) : MessageResult<T>($"[InputValidationError] {message}");
	public static InputValidationErrorResult<T> InputValidationError<T>(string message) => new InputValidationErrorResult<T>(message);



	public record InputValidationErrorsResult<T>(string message, IReadOnlyCollection<string> Errors) 
		: Result<T>(default(T), false, message, Errors);

	public static InputValidationErrorsResult<T> InputValidationErrors<T>(string message, IReadOnlyCollection<string> Errors) 
		=> new InputValidationErrorsResult<T>(message, Errors);

	public static InputValidationErrorsResult<T> InputValidationErrors<T>(string message, ValidationResult result) 
		=> new InputValidationErrorsResult<T>($"One or more validation errors during execution : {message}",
            result.Errors
                .Select(error => $"Property : {error.PropertyName}, With error : {error.ErrorMessage}, InputValue: {error.AttemptedValue}")
                .ToList().AsReadOnly());






	public record UnAuthorizedResult<T>(string message) : MessageResult<T>($"[UnAuthorized] {message}");
	public static UnAuthorizedResult<T> UnAuthorized<T>(string message) => new UnAuthorizedResult<T>(message);
	public static UnAuthorizedResult<T> UnAuthorized<T>() => UnAuthorized<T>("Authenticated user required");
	public static UnAuthorizedResult<T> NotEnoughPermissions<T>() => UnAuthorized<T>("Not Enough Permissions");
	public static UnAuthorizedResult<T> Required<T>(string message) => UnAuthorized<T>($"{message} Required");


	public record NotFoundResult<T>(string message) : MessageResult<T>($"[NotFound] {message}");
	public static NotFoundResult<T> NotFound<T>(string message) => new NotFoundResult<T>(message);

	public record DeletedResult<T>(string message) : MessageResult<T>($"[Deleted] {message}");
	public static DeletedResult<T> Deleted<T>(string message) => new DeletedResult<T>(message);

	public record ParentNotFoundResult<T>(string message) : MessageResult<T>($"[ParentNotFound] {message}");
	public static ParentNotFoundResult<T> ParentNotFound<T>(string message) => new ParentNotFoundResult<T>(message);

	public record ConflictResult<T>(string message) : MessageResult<T>($"[Conflict] {message}");
	public static ConflictResult<T> Conflict<T>(string message) => new ConflictResult<T>(message);


	public record InvalidOperationResult<T>(string message) : MessageResult<T>($"[InvalidOperation] {message}");
	public static InvalidOperationResult<T> InvalidOperation<T>(string message) => new InvalidOperationResult<T>(message);



}

