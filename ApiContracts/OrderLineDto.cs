/// <summary>
/// Спецификация к заказу (позиция в заказе)
/// </summary>
/// <param name="OrderLineId">Идентификатор Спецификации к заказу</param>
/// <param name="OrderId">Идентификатор заказа</param>
/// <param name="ProductId">Идентификатор продукта</param>
/// <param name="Quanity">количество</param>
public record OrderLine(IdType OrderLineId, IdType OrderId, IdType ProductId, double Quanity);




