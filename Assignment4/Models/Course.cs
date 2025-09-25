using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assignment4.Models
{
    public class Course
    {
        public string CourseName { get; set; }
        public LetterGrade CourseGradeLetter { get; set; }
        public int CourseCredits { get; set; }
        public string StudentID { get; init; }

        public Course()
        {
            //Default values
            this.CourseName = "---";
            this.CourseGradeLetter = new LetterGrade("F");
            this.CourseCredits = 0;
        }

        public Course(string courseName, LetterGrade courseGrade, int courseCredits, Student student)
        {
            this.CourseName = courseName;
            this.CourseGradeLetter = courseGrade;
            this.CourseCredits = courseCredits;
            this.StudentID = student.StudentID;
        }

        public double CalculateGradePoints()
        {
            //Calcualte grade points for this course
            return CourseCredits * CourseGradeLetter.GetGradeScore();
        }
    }
}