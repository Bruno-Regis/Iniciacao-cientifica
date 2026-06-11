using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Core.Entities
{
    public class Chat : BaseEntity
    {
        public Chat(Guid usuarioId, string titulo)
        {
            UsuarioId = usuarioId;
            Titulo = titulo;
        }

        public Guid UsuarioId { get; set; }
        public string Titulo { get; set; }
    }
}
