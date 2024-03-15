namespace FinanceFolio.Models.DTO.AccountDTOs;

public class GetAccountsDto
{
    public string accountType { get; set; }
    public int balance { get; set; }
    public int userId { get; set; }   
}