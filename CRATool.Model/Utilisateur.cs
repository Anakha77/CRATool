namespace CRATool.Model
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Trigram { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
