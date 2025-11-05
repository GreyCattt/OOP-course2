namespace LabProject
{
    public class TaxiDriver : Person
    {
        public string LicenseNumber { get; set; }

        public TaxiDriver(string firstName, string lastName, string licenseNumber)
            : base(firstName, lastName)
        {
            LicenseNumber = licenseNumber;
        }


        public override string GetInfo()
        {
            string danceStatus = CanDance ? "вміє танцювати" : "не вміє танцювати";
            return $"[ТАКСИСТ] {base.GetInfo()}, Посвідчення: {LicenseNumber}, {danceStatus}";
        }
    }
}