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

    [NotMapped] private bool _isOverdue;
    [NotMapped] public bool IsOverdue
    {
        get => _isOverdue;
        set
        {
            _isOverdue = value;
            _isOverdue = DateOfDeadline.CompareTo(DateTime.Now) < 0;
        }
    }

    [ForeignKey("UserId")] public User User { get; set; }
    [ForeignKey("BookId")] public Book Book { get; set; }
}