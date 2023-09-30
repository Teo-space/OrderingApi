namespace UseCases.OrderCart;


/// <summary>
/// Добавить предмет в корзину
/// </summary>
/// <param name="CustomerId">Идентификатор клиента</param>
/// <param name="ProductId">Идентификатор продукта</param>
/// <param name="Quanity">Количество продукта</param>
public record CommandOrderCartItemAdd(IdType CustomerId, IdType ProductId, double Quanity)
{
    public class Validator : AbstractValidator<CommandOrderCartItemAdd>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
            RuleFor(x => x.ProductId).NotNull().NotEmpty();

            //тут по идее должны быть проверки коррелирующие с единицами измерения товара
            RuleFor(x => x.Quanity).NotNull().NotEmpty().Must(x => x > 0);
        }
    }
}
