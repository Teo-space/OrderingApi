namespace UseCases.Customers;


/// <summary>
/// Создание нового типа товаров
/// </summary>
/// <param name="Name">Наименование MaximumLength(100)</param>
public record CommandProductTypeCreate(string Name)
{
    public class Validator : AbstractValidator<CommandProductTypeCreate>
    {
        public Validator() 
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100);
        }
    }


}
