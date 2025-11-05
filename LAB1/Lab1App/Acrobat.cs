namespace LabProject
{
    public class Acrobat : Person
    {
        public string PerformerId { get; set; }

        public Acrobat(string firstName, string lastName, string performerId)
            : base(firstName, lastName)
        {
            PerformerId = performerId;
        }

        public override string GetInfo()
        {
            string danceStatus = CanDance ? "вміє танцювати" : "не вміє танцювати";
            return $"[АКРОБАТ] {base.GetInfo()}, ID Перформера: {PerformerId}, {danceStatus}";
        }
    }
}