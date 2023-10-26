/// <summary>
/// Товар
/// </summary>
/// <param name="ProductId">Идентификатор товара</param>
/// <param name="ProductTypeId">Идентификатор типа товара</param>
/// <param name="Name">Наименование</param>
/// <param name="Price">Стоимость</param>
/// <param name="QuanityInStock">Количество на складе</param>
public record ProductDto(
    IdType ProductId, 
    IdType ProductTypeId, 
    string Name, 
    double Price, 
    double QuanityInStock
);

