> [!IMPORTANT]
> Este documento descreve a visão funcional e requisitos do projeto.
>
> A seção de modelagem de entidades pode não refletir integralmente a
> implementação atual.
>
> Para decisões técnicas, arquitetura, entidades, relacionamentos e fluxos,
> consulte primeiro o código-fonte e a documentação técnica em `documentos/`.
>
> Em caso de divergência entre este documento e a implementação atual,
> o código-fonte é a fonte de verdade.


# INICIAÇÃO CIENTÍFICA


## Requisitos Funcionais

|ID    | Descrição do Requisito  | Prioridade |
|------|-----------------------------------------|----|
|RF-001| Ativar controle de permissões no sistema baseado no tipo de usuário identificado. | ALTA |
|RF-002| Implementar funcionalidade no sistema de transcrição de áudio por meio de inteligência artificial. | ALTA | 
|RF-003| Implementar um agente de inteligência artificial integrado no sistema que permita ao usuário efetuar prompts e obter respostas. | ALTA |
|RF-004| Permitir cadastro completo de pacientes incluindo dados pessoais e dados clínicos complementares  | ALTA |
|RF-005| Permitir agendamento de consulta/retorno de paciente vinculado à um profissional da clínica  | ALTA |
|RF-006| Permitir visualização de consulta/retorno de paciente vinculado à um profissional da clínica| ALTA |
|RF-007| Permitir o cadastro completo de evolução do paciente com data, tipo profissional e detalhamento  | ALTA |
|RF-008| Permitir a visualização do histórico de evoluções dados do paciente (prescrições, atestados, receitas, dietas, procedimentos etc)  | ALTA |
|RF-009| Permitir a edição e atualização de dados das evoluções e cadastrais dos pacientes   | ALTA |
|RF-010| Permitir que o profissional gerencie o estoque de medicamentos e materiais com alertas de baixo estoque e vencimento de produtos | ALTA |
|RF-011| Permitir gestão de caixa, controle de pagamentos, faturas e relatórios financeiros gerais | ALTA |
|RF-012| Permitir que o usuário com perfil de Administrador da clínica, possua tenha acesso à um relatório financeiro   | ALTA |


## Diagramas de Relacionamento de Entidades

```mermaid

erDiagram
  CLINICA ||--o{ PACIENTE : "atende"
  CLINICA ||--o{ PROFISSIONAL : "emprega"
  CLINICA ||--o{ ANAMNESE_TEMPLATE : "possui"
  CLINICA ||--o{ AGENDAMENTO : "possui"

  PROFISSIONAL ||--|| USUARIO : "login"
  PROFISSIONAL ||--o{ AGENDAMENTO : "realiza"
  PROFISSIONAL ||--o{ ANAMNESE : "elabora"
  PROFISSIONAL ||--o{ EVOLUCAO : "preenche"

  USUARIO ||--o{ CHAT : "possui"

  PACIENTE ||--o{ AGENDAMENTO : "tem"
  PACIENTE ||--o{ ALERGIA : "tem"
  PACIENTE ||--o{ ANAMNESE : "tem"

  AGENDAMENTO ||--o| EVOLUCAO : "gera"
  AGENDAMENTO ||--o| TRANSCRICAO : "gera"
  

  ANAMNESE ||--o{ ANAMNESE_SESSAO : "contem"
  ANAMNESE_TEMPLATE ||--o{ ANAMNESE_TEMPLATE_SESSAO : "contem"
  CHAT ||--o{ CHAT_MENSAGEM : "contem"

  CLINICA {
    Guid Id PK
    string Nome
    string Email
    string CNPJ
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  USUARIO {
    Guid Id PK
    Guid ClinicaId FK
    string Email
    string Senha
    string Role
    DateTime UltimoAcesso
    bool IsActive
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  PROFISSIONAL {
    Guid Id PK
    Guid ClinicaId FK
    Guid UsuarioId FK
    string Nome
    string Profissao
    string FotoUrl
    string CRM
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  PACIENTE {
    Guid Id PK
    Guid ClinicaId FK
    string Status
    string Nome
    DateTime DataNascimento
    string Genero
    string EstadoCivil
    string NomeDaMae
    string NomeDoPai
    string CPF
    string RG
    string OrgaoExpedidor
    string Telefone
    string Celular
    string Email
    string CEP
    string Cidade
    string Estado
    string Bairro
    string Numero
    string Complemento
    string Profissao
    string NecessidadeEspecial
    string Observacoes
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  AGENDAMENTO {
    Guid Id PK
    Guid PacienteId FK
    Guid ProfissionalId FK
    Guid ClinicaId FK
    string Status
    string Tipo
    string Procedimento
    DateTime DataHoraInicial
    DateTime DataHoraFinal
    string Sala
    bool Confirmacao
    string Descricao
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  EVOLUCAO {
    Guid Id PK
    Guid AgendamentoId FK
    string Conteudo
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  TRANSCRICAO {
    Guid Id PK
    Guid AgendamentoId FK
    string TextoTranscrito
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  ALERGIA {
    Guid Id PK
    Guid PacienteId FK
    string NomeAlergia
    string Gravidade
    DateTime DataIdentificacao
    string Observacoes
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  ANAMNESE {
    Guid Id PK
    Guid PacienteId FK
    Guid ProfissionalId FK
    string NomeDaAnamnese
    string Observacoes
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  ANAMNESE_SESSAO {
    Guid Id PK
    Guid AnamneseId FK
    string Pergunta
    string Resposta
    string Tipo
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  ANAMNESE_TEMPLATE {
    Guid Id PK
    Guid ClinicaId FK
    string Nome
    string Descricao
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  ANAMNESE_TEMPLATE_SESSAO {
    Guid Id PK
    Guid AnamneseTemplateId FK
    string Pergunta
    string Tipo
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  CHAT {
    Guid Id PK
    Guid UsuarioId FK
    string Titulo
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }

  CHAT_MENSAGEM {
    Guid Id PK
    Guid ChatId FK
    string Conteudo
    string TipoMensagem
    DateTime CreatedAt
    DateTime UpdatedAt
    bool IsDeleted
  }
```