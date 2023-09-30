namespace UseCases.OrderCart;


/// <summary>
/// Увеличить количество товара в корзине
/// </summary>
/// <param name="CustomerId">Идентификатор клиента</param>
/// <param name="ProductId">Идентификатор товара</param>
public record CommandOrderCartItemDecrement(IdType CustomerId, IdType ProductId)
{
    public class Validator : AbstractValidator<CommandOrderCartItemDecrement>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
            RuleFor(x => x.ProductId).NotNull().NotEmpty();
        }
    }


}
