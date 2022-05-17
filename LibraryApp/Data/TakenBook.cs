using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Data;

public class TakenBook
{
    public int TakenBookId { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime DateOfIssue { get; set; }
    public DateTime DateOfDeadline { get; set; }
    public int Code { get; set; }

    [NotMapped] private string _isOverdue;
    [NotMapped] public string IsOverdue
    {
        get => DateOfDeadline.CompareTo(DateTime.Now) < 0 ? "Просрочено" : "";
        set => _isOverdue = value;
    }

    [ForeignKey("UserId")] public User User { get; set; }
    [ForeignKey("BookId")] public Book Book { get; set; }
}