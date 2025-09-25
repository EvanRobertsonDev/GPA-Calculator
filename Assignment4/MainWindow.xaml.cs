using Assignment4.Models;
using Assignment4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Student selectedStudent;
        private Student[] allStudents;

        private List<Course> courses;

        private Stack<TextBox> courseNameStack = new Stack<TextBox>();
        private Stack<TextBox> courseGradeStack = new Stack<TextBox>();
        private Stack<TextBox> courseCreditStack = new Stack<TextBox>();

        public MainWindow()
        {
            InitializeComponent();

            //Populate Combobox with students from file
            UpdateListBox();

            //Add existing fields to stack
            courseNameStack.Push(cName1);
            courseGradeStack.Push(lGrade1);
            courseCreditStack.Push(credits1);

            //Initialize courses;
            courses = new List<Course>();

        }

        private void Button_CreateStudent_Click(object sender, RoutedEventArgs e)
        {
            //Create new student
            string studentName = TextBox_StudentName.Text;
            TextBox_StudentName.Text = "";
            Student student = new Student(studentName);

            //Infrom user of succeful creation
            Popup_CreatedStudent.Visibility = Visibility.Visible;

            //Create student in files
            StudentService.Create(student);

            //Update Combobox to add student
            UpdateListBox();
        }

        private void UpdateListBox()
        {
            //Empty Combobox
            StudentList.Items.Clear();

            //Retrieve all students
            allStudents = StudentService.Get();

            //Populate Combobox with all students
            foreach (Student student in allStudents)
            {
                StudentList.Items.Add(student.Name);
            }
        }

        private void Button_CalculateGPA_Click(object sender, RoutedEventArgs e)
        {
            //Get all courses from stack panels
            for (int i = 0; i < cNamePanel.Children.Count; i++)
            if (((TextBox)cNamePanel.Children[i]).Text != "" && ((TextBox)creditsPanel.Children[i]).Text != "")
            {
                try
                {
                    //Get all data from textfields
                    string courseName = ((TextBox)cNamePanel.Children[i]).Text;
                    LetterGrade grade = new LetterGrade(((TextBox)lGradePanel.Children[i]).Text);
                    int creditCount = Convert.ToInt32(((TextBox)creditsPanel.Children[i]).Text);

                    //Add course to courses list
                    courses.Add(new Course(courseName, grade, creditCount, selectedStudent));

                    //Create new course in files
                    CourseService.Create(courses[i]);
                }
                catch (FormatException) { MessageBox.Show("Please enter an integer for course credits"); return; }
            }
            else { MessageBox.Show("Please fill all fields in"); return; }

            //Update selected student's courses
            selectedStudent.Courses = courses;

            //Write GPA to screen
            GPAResult.Content = "Your GPA is: " + selectedStudent.CalculateGPA();

            //Populate Datagrid with imported data from files
            DataGrid_StudentMarks.ItemsSource = CourseService.Get(selectedStudent.StudentID);

            //Reset course list
            courses = new List<Course>();
        }

        private void Button_AddCourse_Click(object sender, RoutedEventArgs e)
        {
            //Adds empty course fields
            AddCourse();
        }

        private void Button_RemoveCourse_Click(object sender, RoutedEventArgs e)
        {
            //Remove courses from GPA calculator
            if (courseNameStack.Count > 0 && courseGradeStack.Count > 0 && courseCreditStack.Count > 0)
            {
                //Update respective stackpanels and stacks
                cNamePanel.Children.Remove(courseNameStack.Pop());
                lGradePanel.Children.Remove(courseGradeStack.Pop());
                creditsPanel.Children.Remove(courseCreditStack.Pop());
            }
        }

        private void StudentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check if Combobox selection is empty
            if (StudentList.SelectedIndex != -1)
            {
                selectedStudent = allStudents[StudentList.SelectedIndex];
                DataGrid_StudentMarks.ItemsSource = CourseService.Get(selectedStudent.StudentID);
                GpaGrid.IsEnabled = true;
                Button_DeleteStudent.IsEnabled = true;
                DisabledMessage.Visibility = Visibility.Hidden;
            }
            else
            {
                //Safeguards to prevent user from using functions without having a student selected
                GpaGrid.IsEnabled = false;
                Button_DeleteStudent.IsEnabled = false;
                DataGrid_StudentMarks.ClearValue(ItemsControl.ItemsSourceProperty);
                DisabledMessage.Visibility = Visibility.Visible;
            }
            
        }

        private void Button_SendAll_Click(object sender, RoutedEventArgs e)
        {
            //If student isn't selected, fallback
            if (selectedStudent == null) return;

            //Get all courses of selected student
            courses = CourseService.Get(selectedStudent.StudentID).ToList<Course>();

            //Add each course to GPA calculator
            for (int i = 0; i < courses.Count; i++)
            {
                AddCourse(courses[i].CourseName, courses[i].CourseGradeLetter.ToString(), courses[i].CourseCredits.ToString());
            }

            //Reset course list
            courses = new List<Course>();
        }

        private void AddCourse(string courseName = "", string letterGrade = "", string numCredits = "")
        {
            //Create course name textfield
            TextBox cname = new TextBox();
            cname.Name = $"cname{courseNameStack.Count + 1}";
            cname.VerticalAlignment = (VerticalAlignment)AlignmentY.Center;
            cname.Margin = new Thickness(5);
            cname.Text = courseName;

            //Create letter grade textfield
            TextBox lGrade = new TextBox();
            lGrade.Name = $"lGrade{courseGradeStack.Count + 1}";
            lGrade.VerticalAlignment = (VerticalAlignment)AlignmentY.Center;
            lGrade.Margin = new Thickness(5);
            lGrade.Text = letterGrade;

            //Create credit textfield
            TextBox credits = new TextBox();
            credits.Name = $"credits{courseGradeStack.Count + 1}";
            credits.VerticalAlignment = (VerticalAlignment)AlignmentY.Center;
            credits.Margin = new Thickness(5);
            credits.Text = numCredits;

            //Add all fields to respective stackpanel
            cNamePanel.Children.Add(cname);
            lGradePanel.Children.Add(lGrade);
            creditsPanel.Children.Add(credits);

            //Update Textbox Stacks
            courseNameStack.Push(cname);
            courseGradeStack.Push(lGrade);
            courseCreditStack.Push(credits);
        }

        private void DataGrid_StudentMarks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Determine if user can Send/Delete individual courses
            if(DataGrid_StudentMarks.SelectedItem != null)
            {
                Button_DeleteCourse.IsEnabled = true;
                Button_Send.IsEnabled = true;
            }
            else
            {
                Button_DeleteCourse.IsEnabled = false;
                Button_Send.IsEnabled = false;
            }
        }

        private void Button_DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            //Delete selected course in datagrid from files
            CourseService.Delete((Course)DataGrid_StudentMarks.SelectedItem);

            //Remove course from Datagrid
            DataGrid_StudentMarks.ItemsSource = CourseService.Get(selectedStudent.StudentID);
        }

        private void Button_DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            //If student isn't selected, fallback
            if (selectedStudent == null) return;

            //Delete selected student from files
            StudentService.Delete(selectedStudent);

            //Update Combobox
            UpdateListBox();
            StudentList.SelectedIndex = -1;
            StudentList.Items.Remove(StudentList.SelectedItem);
        }

        private void TextBox_StudentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check if textbox is empty
            if (TextBox_StudentName.Text != "")
            {
                //Allow user to create new student
                Button_CreateStudent.IsEnabled = true;
                //Hide popup
                Popup_CreatedStudent.Visibility = Visibility.Hidden;
            }
            else Button_CreateStudent.IsEnabled = false;
        }

        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            //If student isn't selected, fallback
            if (selectedStudent == null) return;

            //Populate course list
            courses = CourseService.Get(selectedStudent.StudentID).ToList<Course>();

            //Add the selected DataGrid course to the GPA Calculator
            AddCourse(courses[DataGrid_StudentMarks.SelectedIndex].CourseName, 
                courses[DataGrid_StudentMarks.SelectedIndex].CourseGradeLetter.ToString(), 
                courses[DataGrid_StudentMarks.SelectedIndex].CourseCredits.ToString());

            //Reset course list
            courses = new List<Course>();
        }
    }
}
