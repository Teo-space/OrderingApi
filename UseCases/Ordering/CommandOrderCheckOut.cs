namespace UseCases.Ordering;

/// <summary>
/// Команда формирования заказа с проверкой наличия требуемого количества товара на складе, 
/// а также уменьшение доступного количества товара на складе в БД в случае успешного создания заказа.
/// </summary>
/// <param name="CustomerId">Идентификатор клиента</param>
public record CommandOrderCheckOut(IdType CustomerId)
{
    public class Validator : AbstractValidator<CommandOrderCheckOut>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
        }
    }
}
