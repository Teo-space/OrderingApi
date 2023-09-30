namespace UseCases.OrderCart;


/// <summary>
/// Очистка корзины
/// </summary>
/// <param name="CustomerId">Идентификатор клиента</param>
public record CommandOrderCartItemsClear(IdType CustomerId)
{
    public class Validator : AbstractValidator<CommandOrderCartItemsClear>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
        }
    }


}
