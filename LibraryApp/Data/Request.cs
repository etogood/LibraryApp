using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Data;

public class Request
{
    [Key] public int RequestId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }

    [ForeignKey("UserId")] public User User { get; set; }
    [ForeignKey("BookId")] public Book Book { get; set; }
}