namespace UseCases.Customers;


/// <summary>
/// Создание нового товара
/// </summary>
/// <param name="Name"></param>
public record CommandProductCreate(IdType ProductTypeId
    , string Name, double Price, double QuanityInStock/*Должно устанавливатся отдельно от создания*/)
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
