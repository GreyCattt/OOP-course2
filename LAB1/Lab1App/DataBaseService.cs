namespace LabProject
{
    public class DatabaseService
    {
        private const int MaxSize = 100;

        private Student[] _students = new Student[MaxSize];
        private TaxiDriver[] _drivers = new TaxiDriver[MaxSize];
        private Acrobat[] _acrobats = new Acrobat[MaxSize];
        private int _studentCount = 0;
        private int _driverCount = 0;
        private int _acrobatCount = 0;
        private bool IsIdUnique(string id)
        {
            for (int i = 0; i < _studentCount; i++)
            {
                if (_students[i].StudentCard.Equals(id, StringComparison.OrdinalIgnoreCase) ||
                    _students[i].PassportSeriesNumber.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return false;
            }
        
            for (int i = 0; i < _driverCount; i++)
            {
                if (_drivers[i].LicenseNumber.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            
            for (int i = 0; i < _acrobatCount; i++)
            {
                if (_acrobats[i].PerformerId.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            
            return true;
        }

        public bool AddStudent(Student student)
        {
            if (!IsIdUnique(student.StudentCard) || !IsIdUnique(student.PassportSeriesNumber))
            {
                return false;
            }

            if (_studentCount < MaxSize)
            {
                _students[_studentCount] = student;
                _studentCount++;
                return true;
            }
            return false;
        }
        
        public bool AddTaxiDriver(TaxiDriver driver)
        {
            if (!IsIdUnique(driver.LicenseNumber))
            {
                return false;
            }

            if (_driverCount < MaxSize)
            {
                _drivers[_driverCount] = driver;
                _driverCount++;
                return true;
            }
            return false;
        }

        public bool AddAcrobat(Acrobat acrobat)
        {
            if (!IsIdUnique(acrobat.PerformerId))
            {
                return false;
            }

            if (_acrobatCount < MaxSize)
            {
                _acrobats[_acrobatCount] = acrobat;
                _acrobatCount++;
                return true;
            }
            return false;
        }
        public Student[] GetAllStudents()
        {
            Student[] result = new Student[_studentCount];
            for (int i = 0; i < _studentCount; i++)
            {
                result[i] = _students[i];
            }
            return result;
        }

        public TaxiDriver[] GetAllTaxiDrivers()
        {
            TaxiDriver[] result = new TaxiDriver[_driverCount];
            for (int i = 0; i < _driverCount; i++)
            {
                result[i] = _drivers[i];
            }
            return result;
        }

        public Acrobat[] GetAllAcrobats()
        {
            Acrobat[] result = new Acrobat[_acrobatCount];
            for (int i = 0; i < _acrobatCount; i++)
            {
                result[i] = _acrobats[i];
            }
            return result;
        }
        public Person FindByLastName(string lastName)
        {
            for (int i = 0; i < _studentCount; i++)
            {
                if (_students[i].LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                    return _students[i];
            }
            for (int i = 0; i < _driverCount; i++)
            {
                if (_drivers[i].LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                    return _drivers[i];
            }
            for (int i = 0; i < _acrobatCount; i++)
            {
                if (_acrobats[i].LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                    return _acrobats[i];
            }
            return null;
        }

        public Person FindByUniqueId(string id)
        {
            for (int i = 0; i < _studentCount; i++)
            {
                if (_students[i].StudentCard.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return _students[i];
            }
            for (int i = 0; i < _driverCount; i++)
            {
                if (_drivers[i].LicenseNumber.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return _drivers[i];
            }
            for (int i = 0; i < _acrobatCount; i++)
            {
                if (_acrobats[i].PerformerId.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return _acrobats[i];
            }
            return null;
        }

        public bool DeleteByUniqueId(string id)
        {
            for (int i = 0; i < _studentCount; i++)
            {
                if (_students[i].StudentCard.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    for (int j = i; j < _studentCount - 1; j++)
                    {
                        _students[j] = _students[j + 1];
                    }
                    _students[_studentCount - 1] = null;
                    _studentCount--;
                    return true;
                }
            }

            for (int i = 0; i < _driverCount; i++)
            {
                if (_drivers[i].LicenseNumber.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    for (int j = i; j < _driverCount - 1; j++)
                    {
                        _drivers[j] = _drivers[j + 1];
                    }
                    _drivers[_driverCount - 1] = null;
                    _driverCount--;
                    return true;
                }
            }

            for (int i = 0; i < _acrobatCount; i++)
            {
                if (_acrobats[i].PerformerId.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    for (int j = i; j < _acrobatCount - 1; j++)
                    {
                        _acrobats[j] = _acrobats[j + 1];
                    }
                    _acrobats[_acrobatCount - 1] = null;
                    _acrobatCount--;
                    return true;
                }
            }
            
            return false;
        }
        public void ClearAll()
        {
            Array.Clear(_students, 0, _studentCount);
            Array.Clear(_drivers, 0, _driverCount);
            Array.Clear(_acrobats, 0, _acrobatCount);
            _studentCount = 0;
            _driverCount = 0;
            _acrobatCount = 0;
        }
    }
}