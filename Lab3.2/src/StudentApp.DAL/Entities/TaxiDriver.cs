namespace StudentApp.DAL.Entities
{
    [Serializable]
    public class TaxiDriver : ITalent
    {
        public string Name { get; set; }
        public string CarModel { get; set; }
        
        public string Dance()
        {
            return " ";
        }
    }
}