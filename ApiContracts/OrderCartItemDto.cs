/// <summary>
/// Товар в корзине заказа
/// </summary>
/// <param name="OrderCartItemId">Идентификатор товара в корзине</param>
/// <param name="CustomerId">Идентификатор покупателя</param>
/// <param name="ProductId">Идентификатор товара</param>
/// <param name="Quanity">Количество</param>
public record OrderCartItemDto
(
    IdType OrderCartItemId,
    IdType CustomerId,
    IdType ProductId,
    double Quanity
);





