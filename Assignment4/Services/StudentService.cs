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
    class StudentService
    {
        private static string s_directory;
        private static XmlSerializer s_serializer;


        static StudentService()
        {
            s_directory = @"..\..\..\AppData\Students\";
            s_serializer = new XmlSerializer(typeof(Student));
        }


        public static void Create(Student student)
        {
            string fileName = $"{student.StudentID}.xml";

            //Update if student already exists
            if (File.Exists(s_directory + fileName))
            {
                Update(student);
            }
            else
            {
                //Serialize data to XML and store in file
                using (Stream stream = new FileStream(s_directory + fileName, FileMode.Create))
                {
                    s_serializer.Serialize(stream, student);
                }
            }
        }

        public static Student Get(string studentID)
        {
            string fileName = $"{studentID}.xml";

            //Check if file exists
            if (File.Exists(s_directory + fileName))
            {
                using (Stream stream = new FileStream(s_directory + fileName, FileMode.Open))
                {
                    //Deserialize from XML and return specfic student
                    return (Student)s_serializer.Deserialize(stream);
                }
            }
            else return null;
        }

        public static Student[] Get()
        {
            string[] files = Directory.GetFiles(s_directory, "*.xml");
            Student[] students = new Student[files.Length];

            //Deserialize and return all students in students folder
            for (int i = 0; i < students.Length; i++)
            {
                if (File.Exists(files[i]))
                {
                    using (Stream stream = new FileStream(files[i], FileMode.Open))
                    {
                        students[i] = (Student)s_serializer.Deserialize(stream);
                    }
                }
            }

            return students;
        }

        public static void Update(Student student)
        {
            //Update by removing and adding back
            Delete(student);
            Create(student);
        }

        public static void Delete(Student student)
        {
            string fileName = $"{student.StudentID}.xml";

            //Check if file exists
            if (File.Exists(s_directory + fileName))
            {
                File.Delete(s_directory + fileName);
            }
                
            //Remove directory as well
            if (Directory.Exists(@"..\..\..\AppData\" + student.StudentID))
            {
                //Remove all files in directory
                Directory.Delete(@"..\..\..\AppData\" + student.StudentID, true);
            }
        }
    }
}
