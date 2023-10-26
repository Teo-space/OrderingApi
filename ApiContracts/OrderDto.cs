/// <summary>
/// Заказ
/// </summary>
/// <param name="OrderId">Идентификатор заказа</param>
/// <param name="CustomerId">Идентификатор покупателя</param>
/// <param name="CreatedAt">Дата создания заказа</param>
public record OrderDto(IdType OrderId, IdType CustomerId, DateTime CreatedAt);





