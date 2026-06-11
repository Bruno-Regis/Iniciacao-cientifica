using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Core.Entities
{
    public class ChatMensagem : BaseEntity
    {
        public ChatMensagem(Guid chatId, string conteudo, TipoMensagemEnum tipoMensagem)
        {
            ChatId = chatId;
            Conteudo = conteudo;
            TipoMensagem = tipoMensagem;
        }

        public Guid ChatId { get; set; }
        public string Conteudo { get; set; }
        public TipoMensagemEnum TipoMensagem { get; set; }
    }

    public enum TipoMensagemEnum
    {
        Usuario = 0,
        ia = 1,
    }
}
