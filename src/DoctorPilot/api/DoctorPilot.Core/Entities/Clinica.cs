namespace DoctorPilot.Core.Entities
{
    public class Clinica : BaseEntity
    {
        public Clinica(string nome, string email, string cnpj)
        {
            Nome = nome;
            Email = email;
            Cnpj = cnpj;
        }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }
        
    }
}
