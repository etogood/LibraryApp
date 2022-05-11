using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Data;

public class Genre
{
    [Key] public int GenreId { get; set; }
    public string GenreName { get; set; }
}