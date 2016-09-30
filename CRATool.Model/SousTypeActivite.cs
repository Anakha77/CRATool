namespace CRATool.Model
{
    public class SousTypeActivite
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public bool IsProject { get; set; }
        public int? IdTypeActivite { get; set; }
    }
}
