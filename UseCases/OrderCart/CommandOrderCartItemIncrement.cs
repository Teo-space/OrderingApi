namespace UseCases.OrderCart;


/// <summary>
/// Уменьшить количество товара в корзине
/// </summary>
/// <param name="CustomerId">Идентификатор клиента</param>
/// <param name="ProductId">Идентификатор товара</param>
public record CommandOrderCartItemIncrement(IdType CustomerId, IdType ProductId)
{
    public class Validator : AbstractValidator<CommandOrderCartItemIncrement>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
            RuleFor(x => x.ProductId).NotNull().NotEmpty();
        }
    }


}
