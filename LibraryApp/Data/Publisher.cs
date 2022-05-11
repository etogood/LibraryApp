using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Data;

public class Publisher
{
    [Key] public int PublisherId { get; set; }
    public string PublisherName { get; set; }
}