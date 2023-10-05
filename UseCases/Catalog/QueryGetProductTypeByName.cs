namespace UseCases.Customers;


/// <summary>
/// Получение типа товара по имени
/// </summary>
/// <param name="Name"></param>
public record QueryGetProductTypeByName(string Name)
{
    public class Validator : AbstractValidator<QueryGetProductTypeByName>
    {
        public Validator() 
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100);
        }
    }
}

