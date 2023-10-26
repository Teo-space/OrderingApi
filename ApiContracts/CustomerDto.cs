/// <summary>
/// Покупатель
/// </summary>
/// <param name="CustomerId">Идентификатор</param>
/// <param name="PhoneNumber">Номер телефона</param>
/// <param name="UserName">Имя пользователя</param>
public record CustomerDto(IdType CustomerId, string PhoneNumber, string UserName);


