namespace UseCases.OrderCart;


/// <summary>
/// Получить список товаров в корзине пользователя
/// </summary>
/// <param name="CustomerId">Идентификатор клиента</param>
public record QueryOrderCartItemsGet(IdType CustomerId)
{
    public class Validator : AbstractValidator<QueryOrderCartItemsGet>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
        }
    }
}
