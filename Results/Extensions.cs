public static class ResultTypedExtensions
{
    public static Result<T> Ok<T>(this T result) where T : class => Result.Ok(result);

    public static Result<IReadOnlyCollection<T>> Ok<T>(this List<T> objects) where T : class
        => Result.Ok(objects as IReadOnlyCollection<T>);

}