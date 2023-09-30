namespace UseCases.Customers;


/// <summary>
/// Запрос на получение пользователя по номеру телефона
/// </summary>
/// <param name="PhoneNumber">Номер телефона в формате +79871234567</param>
public record QueryCustomerGet(string PhoneNumber)
{
    public class Validator : AbstractValidator<QueryCustomerGet>
    {
        public Validator() 
        {
            RuleFor(x => x.PhoneNumber)
                .Length(12)
                .Matches(@"^[+][7][0-9]{10}$");
        }
    }
}

