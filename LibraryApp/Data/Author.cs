using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Data;

public class Author
{
    [Key] public int AuthorId { get; set; }
    public string AuthorName { get; set; }
    public string AuthorSurName { get; set; }
    public string AuthorPatronymic { get; set; }

    [NotMapped] public string AuthorFullName => AuthorName[0] + ". " + AuthorPatronymic[0] + ". " + AuthorSurName;
}