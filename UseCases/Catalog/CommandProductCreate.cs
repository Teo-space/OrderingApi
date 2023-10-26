namespace UseCases.Customers;


/// <summary>
/// Создание нового товара
/// </summary>
/// <param name="ProductTypeId">Идентификатор типа товара</param>
/// <param name="Name">Наименование товара. MaximumLength(100)</param>
/// <param name="Price">Цена за единицу</param>
/// <param name="QuanityInStock">Количество на складе</param>
public record CommandProductCreate(IdType ProductTypeId, string Name, double Price, double QuanityInStock)
{
    public class Validator : AbstractValidator<CommandProductCreate>
    {
        public Validator() 
        {
            RuleFor(x => x.ProductTypeId).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).NotNull().Must(x => x >= 0);
            RuleFor(x => x.QuanityInStock).NotNull().Must(x => x >= 0);
        }
    }


}
