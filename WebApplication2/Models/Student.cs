using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Student
    {
        int id;
        string name;
        List<string> courses;
        private string course;

        public Student(string name,int id)
        {
            this.id = id;
            this.name=name;
            courses = new List<string>();
        
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Course")]
        public string Course
        {
            get
            {
                return course;
            }
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name
        {
            get
            {
                return name;
            }
        }
        [Required]
        [Display(Name = "Student ID")]
        public int ID
        {
            get
            {
                return id;
            }
        }
        public void add(string course)
        {
            courses.Add(course);
        }
        public List<string> Courses
        {
            set
            {
                courses = value;
            }
            get
            {
                return courses;
            }
        }
    }
}