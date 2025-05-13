namespace EmailProvider.Models;

public class EmailResult
{
    public bool Succeeded { get; set; } 

    public string Message { get; set; } = null!;
}