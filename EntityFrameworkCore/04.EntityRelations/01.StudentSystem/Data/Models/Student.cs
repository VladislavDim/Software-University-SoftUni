﻿namespace _01.StudentSystem.Data.Models;

using Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class Student
{
    public Student()
    {
        StudentCourses = new HashSet<StudentCourse>();
        Homeworks = new HashSet<Homework>();
    }
    [Key]
    public int StudentId { get; set; }

    [Required]
    [Unicode]
    [MaxLength(ValidationConstants.StudentNameMaxLength)]
    public string Name { get; set; }

    [MaxLength(ValidationConstants.StudentPhoneNumberLength)]
    public string? PhoneNumber { get; set; }

    public DateTime RegisteredOn { get; set; }

    public DateTime? Birthday { get; set; }

    public ICollection<StudentCourse> StudentCourses { get; set; }

    public ICollection<Homework> Homeworks { get; set; }
}

