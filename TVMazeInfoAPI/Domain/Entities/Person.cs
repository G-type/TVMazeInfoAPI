﻿

using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Birthday { get; set; }
        public string? Gender { get; set; }
        public Country? Country { get; set; }
        public Image? Image { get; set; }
    }
}