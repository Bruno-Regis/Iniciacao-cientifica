namespace DoctorPilot.Core.Entities
{
    public class Chat : BaseEntity
    {
        public Chat(Guid usuarioId, string titulo)
        {
            UsuarioId = usuarioId;
            Titulo = titulo;
            Mensagens = [];
        }

        public Guid UsuarioId { get; set; }
        public string Titulo { get; set; }
        public List<ChatMensagem> Mensagens { get; set; }
    }
}
