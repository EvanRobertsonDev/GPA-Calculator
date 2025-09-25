using Assignment4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assignment4.Services
{
    static class CourseService {
        private static string s_directory;
        private static XmlSerializer s_serializer;


        static CourseService()
        {
            s_directory = @"..\..\..\AppData\";
            s_serializer = new XmlSerializer(typeof(Course));
        }


        public static void Create(Course course)
        {
            //Store course data in a folder belonging to the student
            string fileName = $"{course.StudentID}\\{course.CourseName}.xml";

            //If the file already exists update it
            if (File.Exists(s_directory + fileName))
            {
                Update(course);
            }
            else
            {
                //Create Directory if it doesn't exist
                if (!Directory.Exists(s_directory + course.StudentID))
                {
                    Directory.CreateDirectory(s_directory + course.StudentID);
                }
                //Serialize data to XML and store in file
                using (Stream stream = new FileStream(s_directory + fileName, FileMode.Create))
                {
                    s_serializer.Serialize(stream, course);
                }
            }
        }

        public static Course Get(string courseName, string studentID)
        {
            string fileName = $"{studentID}\\{courseName}.xml";

            //Check file exists
            if (File.Exists(s_directory + fileName))
            {
                //Deserialize from XML and return specfic course
                using (Stream stream = new FileStream(s_directory + fileName, FileMode.Open))
                {
                    return (Course)s_serializer.Deserialize(stream);
                }
            }
            else return null;
        }

        public static Course[] Get(string studentID)
        {
            //Check if Directory exists
            if(!Directory.Exists(s_directory + studentID))
            {
                Directory.CreateDirectory(s_directory + studentID);
            }

            //Get all files in student's folder
            string[] files = Directory.GetFiles(s_directory, $"{studentID}\\*.xml");
            Course[] courses = new Course[files.Length];

            //Deserialize and return all files in student's folder
            for (int i = 0; i < courses.Length; i++)
            {
                if (File.Exists(files[i]))
                {
                    using (Stream stream = new FileStream(files[i], FileMode.Open))
                    {
                        courses[i] = (Course)s_serializer.Deserialize(stream);
                    }
                }
            }

            return courses;
        }

        public static void Update(Course course)
        {
            //Update by removing and adding back
            Delete(course);
            Create(course);
        }

        public static void Delete(Course course)
        {
            string fileName = $"{course.StudentID}\\{course.CourseName}.xml";

            //Check if file exists
            if (File.Exists(s_directory + fileName))
            {
                File.Delete(s_directory + fileName);
            }
                
        }
    }
}
