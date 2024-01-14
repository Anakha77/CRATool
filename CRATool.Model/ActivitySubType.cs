namespace CRATool.Model
{
    public class ActivitySubType
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public bool IsProject { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}
