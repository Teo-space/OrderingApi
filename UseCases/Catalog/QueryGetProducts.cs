namespace UseCases.Catalog;


/// <summary>
/// Запрос получения списка товаров,
/// с возможностью фильтрации
/// по типу товара и/или
/// по наличию на складе и
/// сортировки по цене (возрастанию и убыванию).
/// </summary>
/// <param name="ProductTypeId">[Опционально] Идентификатор типа продукта</param>
/// <param name="Instock">[Опционально] Только товары в наличии</param>
/// <param name="OrderByDescending">[Опционально] Сортировка по убыванию</param>
public record QueryGetProducts(IdType ProductTypeId, bool Instock = true, bool OrderByDescending = false)
{
    /// <summary>
    /// Валидатор который ничего не проверяет)))
    /// </summary>
    public class Validator : AbstractValidator<QueryGetProducts>
    {
        public Validator()
        {
        }
    }

}
