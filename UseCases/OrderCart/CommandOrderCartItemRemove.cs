namespace UseCases.OrderCart;


/// <summary>
/// Удалить товар из корзины
/// </summary>
/// <param name="CustomerId">Идентификатор клиента</param>
/// <param name="ProductId">Идентификатор товара</param>
public record CommandOrderCartItemRemove(IdType CustomerId, IdType ProductId)
{
    public class Validator : AbstractValidator<CommandOrderCartItemRemove>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
            RuleFor(x => x.ProductId).NotNull().NotEmpty();
        }
    }


}
