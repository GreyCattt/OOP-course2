using System.IO;

namespace LabProject
{
    public class FileService
    {
        private readonly string _filePath;

        public FileService(string filePath)
        {
            _filePath = filePath;
        }
        public void SaveData(DatabaseService db)
        {
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                foreach (var student in db.GetAllStudents())
                {
                    writer.WriteLine("Student " + student.LastName);
                    writer.WriteLine("{");
                    writer.WriteLine($"  \"firstname\": \"{student.FirstName}\",");
                    writer.WriteLine($"  \"lastname\": \"{student.LastName}\",");
                    writer.WriteLine($"  \"course\": \"{student.Course}\",");
                    writer.WriteLine($"  \"studentId\": \"{student.StudentCard}\",");
                    writer.WriteLine($"  \"passport\": \"{student.PassportSeriesNumber}\",");
                    writer.WriteLine($"  \"city\": \"{student.ArrivalCity}\",");
                    writer.WriteLine($"  \"canDance\": \"{student.CanDance}\"");
                    writer.WriteLine("};");
                }
                
                foreach (var driver in db.GetAllTaxiDrivers())
                {
                    writer.WriteLine("TaxiDriver " + driver.LastName);
                    writer.WriteLine("{");
                    writer.WriteLine($"  \"firstname\": \"{driver.FirstName}\",");
                    writer.WriteLine($"  \"lastname\": \"{driver.LastName}\",");
                    writer.WriteLine($"  \"license\": \"{driver.LicenseNumber}\",");
                    writer.WriteLine($"  \"canDance\": \"{driver.CanDance}\"");
                    writer.WriteLine("};");
                }

                foreach (var acrobat in db.GetAllAcrobats())
                {
                    writer.WriteLine("Acrobat " + acrobat.LastName);
                    writer.WriteLine("{");
                    writer.WriteLine($"  \"firstname\": \"{acrobat.FirstName}\",");
                    writer.WriteLine($"  \"lastname\": \"{acrobat.LastName}\",");
                    writer.WriteLine($"  \"performerId\": \"{acrobat.PerformerId}\",");
                    writer.WriteLine($"  \"canDance\": \"{acrobat.CanDance}\"");
                    writer.WriteLine("};");
                }
            }
        }
        public void LoadData(DatabaseService db)
        {
            db.ClearAll();
            if (!File.Exists(_filePath)) return;

            using (StreamReader reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("Student"))
                    {
                        var student = ParseStudent(reader);
                        if (student != null) db.AddStudent(student);
                    }
                    else if (line.StartsWith("TaxiDriver"))
                    {
                         var driver = ParseTaxiDriver(reader);
                         if (driver != null) db.AddTaxiDriver(driver);
                    }
                    else if (line.StartsWith("Acrobat"))
                    {
                         var acrobat = ParseAcrobat(reader);
                         if (acrobat != null) db.AddAcrobat(acrobat);
                    }
                }
            }
        }

        private string ParseValue(string line)
        {
            try
            {
                string[] parts = line.Split(':');
                string valuePart = parts[1].Trim();
                valuePart = valuePart.Substring(1, valuePart.Length - 2);
                if (valuePart.EndsWith(","))
                    valuePart = valuePart.Substring(0, valuePart.Length - 1);
                return valuePart;
            }
            catch { return ""; }
        }

        private Student ParseStudent(StreamReader reader)
        {
            string line;
            string fn = "", ln = "", course = "", id = "", passport = "", city = "";
            bool canDance = false;
            while ((line = reader.ReadLine()) != null && !line.Trim().StartsWith("};"))
            {
                if (line.Contains("\"firstname\"")) fn = ParseValue(line);
                else if (line.Contains("\"lastname\"")) ln = ParseValue(line);
                else if (line.Contains("\"course\"")) course = ParseValue(line);
                else if (line.Contains("\"studentId\"")) id = ParseValue(line);
                else if (line.Contains("\"passport\"")) passport = ParseValue(line);
                else if (line.Contains("\"city\"")) city = ParseValue(line);
                else if (line.Contains("\"canDance\"")) canDance = bool.Parse(ParseValue(line));
            }
            if (fn != "") 
            {
                var student = new Student(fn, ln, course, id, passport, city);
                student.CanDance = canDance;
                return student;
            }
            return null;
        }
        private TaxiDriver ParseTaxiDriver(StreamReader reader)
        {
            string line;
            string fn = "", ln = "", license = "";
            bool canDance = false;
            while ((line = reader.ReadLine()) != null && !line.Trim().StartsWith("};"))
            {
                if (line.Contains("\"firstname\"")) fn = ParseValue(line);
                else if (line.Contains("\"lastname\"")) ln = ParseValue(line);
                else if (line.Contains("\"license\"")) license = ParseValue(line);
                else if (line.Contains("\"canDance\"")) canDance = bool.Parse(ParseValue(line));
            }
            if (fn != "") 
            {
                var driver = new TaxiDriver(fn, ln, license);
                driver.CanDance = canDance;
                return driver;
            }
            return null;
        }

        private Acrobat ParseAcrobat(StreamReader reader)
        {
            string line;
            string fn = "", ln = "", perfId = "";
            bool canDance = false;
            while ((line = reader.ReadLine()) != null && !line.Trim().StartsWith("};"))
            {
                if (line.Contains("\"firstname\"")) fn = ParseValue(line);
                else if (line.Contains("\"lastname\"")) ln = ParseValue(line);
                else if (line.Contains("\"performerId\"")) perfId = ParseValue(line);
                else if (line.Contains("\"canDance\"")) canDance = bool.Parse(ParseValue(line));
            }
            if (fn != "") 
            {
                var acrobat = new Acrobat(fn, ln, perfId);
                acrobat.CanDance = canDance;
                return acrobat;
            }
            return null;
        }
    }
}