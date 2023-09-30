namespace UseCases.Ordering;


/// <summary>
/// Запрос получения списка заказов по конкретному клиенту за выбранный временной период, отсортированный по дате создания.
/// </summary>
/// <param name="CustomerId">Идентификатор клиента</param>
/// <param name="StartDate">Начальная дата</param>
/// <param name="EndDate">Конечная дата</param>
public record QueryGetCustomerOrders(IdType CustomerId, DateTime StartDate, DateTime EndDate)
{

    public class Validator : AbstractValidator<QueryGetCustomerOrders>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();

            RuleFor(x => x.StartDate).NotNull().NotEmpty();//.Must((Model, StartDate) => StartDate > Model.EndDate);

            RuleFor(x => x.EndDate).NotNull().NotEmpty();//.Must((Model, EndDate) => EndDate < Model.StartDate);
        }
    }
}
