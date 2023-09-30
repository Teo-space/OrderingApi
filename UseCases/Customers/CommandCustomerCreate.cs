namespace UseCases.Customers;


/// <summary>
/// Команда для создания клиента
/// </summary>
/// <param name="PhoneNumber">Номер телефона в формате +79871234567 (12 символов)</param>
/// <param name="UserName">Фамилия имя отчество</param>
public record CommandCustomerCreate(string PhoneNumber, string UserName)
{
    public class Validator : AbstractValidator<CommandCustomerCreate>
    {
        public Validator() 
        {
            RuleFor(x => x.PhoneNumber)
                .Length(12)
                .Matches(@"^[+][7][0-9]{10}$");

            RuleFor(x => x.UserName).NotNull().NotEmpty().MaximumLength(100);
        }
    }


}
