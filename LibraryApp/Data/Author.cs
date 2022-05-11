using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Data;

public class Author
{
    [Key] public int AuthorId { get; set; }
    public string AuthorFullName { get; set; }
}