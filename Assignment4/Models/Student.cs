using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4.Models
{
    public class Student
    {
        //Using init to make the property read-only after initialization
        public string StudentID { get; init; }

        public string Name { get; init; }

        public List<Course> Courses { get; set; }

        public Student()
        {
            //Default values
            this.StudentID = Guid.NewGuid().ToString();
            this.Name = "---";
            Courses = new List<Course>();
        }

        public Student(string name)
        {
            //Students get unique IDs
            this.StudentID = Guid.NewGuid().ToString();
            this.Name = name;
            Courses = new List<Course>();
        }

        public double CalculateGPA()
        {
            double totalGradePoints = 0;
            int totalCredits = 0;

            //Calculate grade points for all courses
            foreach (Course course in Courses)
            {
                totalGradePoints += course.CalculateGradePoints();
                totalCredits += course.CourseCredits;
            }

            //Return average of gradepoints over credits
            return Math.Round(totalGradePoints / totalCredits, 2);
        }
    }
}
