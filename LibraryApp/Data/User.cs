using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Data;

public class User
{
    [Key] public int UserId { get; set; }
    public int RoleId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Patronymic { get; set; }
    public int AmountOfOverdue { get; set; }
    
    [NotMapped] public string FullName => Surname + ' ' + Name + ' ' + Patronymic;
    [ForeignKey("RoleId")] public Role Role { get; set; }
}