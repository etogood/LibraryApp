﻿using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Data;

public class Role
{
    [Key] public int RoleId { get; set; }
    public string RoleName { get; set; }
}