using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Data;

public class Book
{ 
    [Key] public int BookId { get; set; }
    public string BookName { get; set; }
    public int YearOfIssue { get; set; }
    public string ISBN { get; set; }
    public string RedactorFullName { get; set; }
    public int NumberOfPages { get; set; }
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
    public int PublisherId { get; set; }
    [DefaultValue(false)] public bool IsTaken { get; set; }

    [ForeignKey("AuthorId")] public Author Author { get; set; }
    [ForeignKey("GenreId")] public Genre Genre { get; set; }
    [ForeignKey("PublisherId")] public Publisher Publisher { get; set; }
}