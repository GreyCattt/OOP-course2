namespace StudentApp.DAL.Entities
{
    [Serializable]
    public class Acrobat : ITalent
    {
        public string Nickname { get; set; }
        public int ExperienceYears { get; set; }

        public string Dance()
        {
            return "Акробат виконує сальто у танці.";
        }
    }
}