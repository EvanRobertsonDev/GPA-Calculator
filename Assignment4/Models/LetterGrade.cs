using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4.Models
{
    public class LetterGrade
    {
        private Dictionary<string, double> LetterGrades;

        public string Letter { get; set; }

        public LetterGrade()
        {
            //Default value
            this.Letter = "F";

            SetupDictionary();
        }

        public LetterGrade(string letter)
        {
            this.Letter = letter;

            SetupDictionary();
        }

        public double GetGradeScore()
        {
            try { return LetterGrades[Letter]; }
            //Fallback for entries that do not match dictionary
            catch (KeyNotFoundException) { Letter = "F"; return 0; }
        }
        public override string ToString()
        {
            try { double points = LetterGrades[Letter]; }
            catch (KeyNotFoundException) { Letter = "F"; }
            return Letter;
        }

        private void SetupDictionary()
        {
            //Letter & grade point values in Ontario
            LetterGrades = new Dictionary<string, double>();
            LetterGrades.Add("A+", 4.3);
            LetterGrades.Add("A", 4.0);
            LetterGrades.Add("A-", 3.7);
            LetterGrades.Add("B+", 3.3);
            LetterGrades.Add("B", 3);
            LetterGrades.Add("B-", 2.7);
            LetterGrades.Add("C+", 2.3);
            LetterGrades.Add("C", 2);
            LetterGrades.Add("C-", 1.7);
            LetterGrades.Add("D+", 1.3);
            LetterGrades.Add("D", 1);
            LetterGrades.Add("D-", 0.7);
            LetterGrades.Add("F", 0);
        }
    }
}
