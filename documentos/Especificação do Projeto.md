# INICIAÇÃO CIENTÍFICA


## Requisitos Funcionais

|ID    | Descrição do Requisito  | Prioridade |
|------|-----------------------------------------|----|
|RF-001| Ativar controle de permissões no sistema baseado no tipo de usuário identificado. | ALTA |
|RF-002| Implementar funcionalidade no sistema de transcrição de áudio por meio de inteligência artificial. | ALTA | 
|RF-003| Implementar um agente de inteligência artificial integrado no sistema que permita ao usuário efetuar prompts e obter respostas. | ALTA |
|RF-004| Permitir cadastro completo de pacientes incluindo dados pessoais e dados clínicos complementares  | ALTA |
|RF-005| Permitir agendamento de consulta/retorno de paciente vinculado à um profissional da clínica  | ALTA |
|RF-006| Permitir visualização de consulta/retorno de paciente vinculado à um profissional da clínica; (Existe a biblioteca full calendar para React) | ALTA |
|RF-007| Permitir o cadastro completo de evolução do paciente com data, tipo profissional e detalhamento  | ALTA |
|RF-008| Permitir a visualização do histórico de evoluções dados do paciente (prescrições, atestados, receitas, dietas, procedimentos etc)  | ALTA |
|RF-009| Permitir a edição e atualização de dados das evoluções e cadastrais dos pacientes   | ALTA |
|RF-010| Permitir que o profissional gerencie o estoque de medicamentos e materiais com alertas de baixo estoque e vencimento de produtos | ALTA |
|RF-011| Permitir gestão de caixa, controle de pagamentos, faturas e relatórios financeiros gerais | ALTA |
|RF-012| Permitir que o usuário com perfil de Administrador da clínica, possua tenha acesso à um relatório financeiro   | ALTA |


## Diagramas de Relacionamento de Entidades

### Diagrama Conceitual
```mermaid
erDiagram
    %% AGENDAMENTO é o centro do sistema
    PACIENTE ||--o{ AGENDAMENTO : "participa"
    PROFISSIONAL ||--o{ AGENDAMENTO : "conduz"
    
    AGENDAMENTO ||--o| EVOLUÇÃO : "resulta em"
    AGENDAMENTO ||--o| TRANSCRICAO : "gera"
    AGENDAMENTO ||--o| ANAMNESE : "inclui"
    
    %% Relacionamentos secundários (limpos)
    CLINICA ||--o{ PACIENTE : "atende"
    CLINICA ||--o{ PROFISSIONAL : "emprega"
    PACIENTE ||--o{ ALERGIA_PACIENTE : "tem"
    PROFISSIONAL ||--|| USUARIO : "acessa via"
    ANAMNESE ||--o{ ANAMNESE_ITEM : "contém"

    CLINICA {
        int Id PK
    }
    
    PACIENTE {
        int Id PK
    }
    
    PROFISSIONAL {
        int Id PK
    }
    
    AGENDAMENTO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
    }
    
    EVOLUÇÃO {
        int Id PK
        int AgendamentoId FK
    }
    
    TRANSCRICAO {
        int Id PK
        int AgendamentoId FK
        string TextoTranscrito
    }
    
    ANAMNESE {
        int Id PK
        int AgendamentoId FK
    }
    
    ANAMNESE_ITEM {
        int Id PK
        int AnamneseId FK
    }
    
    ALERGIA_PACIENTE {
        int Id PK
        int PacienteId FK
    }
    
    USUARIO {
        int Id PK
        int ProfissionalId FK
    }

```

### Módulo 1 - Gestão Clínica
```mermaid
erDiagram
    %% ========== NÍVEL 1: CLÍNICA (Raiz) ==========
    CLÍNICA ||--o{ PACIENTE : contem
    CLÍNICA ||--o{ PROFISSIONAL : contem

    %% ========== NÍVEL 2: PROFISSIONAL ==========
    PROFISSIONAL ||--|| USUARIO : login

    CLÍNICA {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string CNPJ
    }
    USUARIO{
        int Id PK
        int ProfissionalId FK
        string Email UK
        string SenhaHash
        string Role
        DateTime UltimoAcesso
        bool Ativo
    }
    PROFISSIONAL{
        int Id PK
        int ChatId FK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string Cargo
        string Foto
        string Permissao
        string CRM
    }
    PACIENTE {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string status
        DateTime DataNascimento
        string Sexo
        string EstadoCivil
        string NomeDaMae
        string NomeDoPai
        string CPF
        string RG
        string OrgaoExpedidor
        string Estado 
        string DataEmissao
        string Telefone
        string Celular
        string Email 
        string CEP
        string Cidade
        string EstadoResidencia
        string Bairro
        string Numero
        string Complemento
        string Profissao
        string NecessidadeEspecial
        string Observacoes

    }

```


### Módulo 2 - Atendimento Clínico
```mermaid
erDiagram
    %% ========== NÍVEL 2: PROFISSIONAL ==========
    PROFISSIONAL ||--o{ AGENDAMENTO : realiza
    PROFISSIONAL ||--o{ ANAMNESE : elabora
    PROFISSIONAL ||--o{ EVOLUÇÃO : preenche
    %% ========== NÍVEL 2: PACIENTE ==========
    PACIENTE ||--o{ AGENDAMENTO : contem    
    PACIENTE ||--o{ EVOLUÇÃO : contem
    PACIENTE ||--o{ ALERGIA : contem
    PACIENTE ||--o{ ANAMNESE : contem
    %% ========== NÍVEL 3: AGENDAMENTO (Centro) ==========
    AGENDAMENTO ||--o| EVOLUÇÃO : gera

    %% ========== NÍVEL 4: ANAMNESE ==========
    ANAMNESE ||--o{ ANAMNESE_ITEM : contem

    PROFISSIONAL{
        int Id PK
        int ChatId FK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string Cargo
        string Foto
        string Permissao
        string CRM
    }
   PACIENTE {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string status
        DateTime DataNascimento
        string Sexo
        string EstadoCivil
        string NomeDaMae
        string NomeDoPai
        string CPF
        string RG
        string OrgaoExpedidor
        string Estado 
        string DataEmissao
        string Telefone
        string Celular
        string Email 
        string CEP
        string Cidade
        string EstadoResidencia
        string Bairro
        string Numero
        string Complemento
        string Profissao
        string NecessidadeEspecial
        string Observacoes

    }
    AGENDAMENTO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Status
        string Tipo
        string Procedimento
        string Profissional
        DateTime HoraInicial
        DateTime HoraFinal
        string Sala
        bool AgendarComoConfirmado
        string Descricao
    }
    EVOLUÇÃO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Conteudo
    }

    ALERGIA {
        int Id PK
        int PacienteId FK
        DateTime CreatedAt
        bool IsDeleted
        string NomeAlergia
        string Gravidade
        DateTime DataIdentificacao
        string Observacoes
        
    }
    ANAMNESE{
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string NomeDaAnamnese
    }
    ANAMNESE_ITEM{
        int Id PK
        int AnamneseId FK
        string Pergunta
        string Resposta
        string Tipo
        DateTime CreatedAt
        bool IsDeleted
    }

```


### Módulo 3 - Funcionalidades de IA
```mermaid
erDiagram
    %% ========== NÍVEL 2: PROFISSIONAL ==========
    PROFISSIONAL ||--o{ CHAT : envia

    %% ========== NÍVEL 2: PACIENTE ==========
    PACIENTE ||--o{ TRANSCRIÇÃO : contem
    PACIENTE ||--o{ AGENDAMENTO : contem   
    %% ========== NÍVEL 3: AGENDAMENTO (Centro) ==========
    AGENDAMENTO ||--o| TRANSCRIÇÃO : gera



    PROFISSIONAL{
        int Id PK
        int ChatId FK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string Cargo
        string Foto
        string Permissao
        string CRM
    }
    PACIENTE {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string status
        DateTime DataNascimento
        string Sexo
        string EstadoCivil
        string NomeDaMae
        string NomeDoPai
        string CPF
        string RG
        string OrgaoExpedidor
        string Estado 
        string DataEmissao
        string Telefone
        string Celular
        string Email 
        string CEP
        string Cidade
        string EstadoResidencia
        string Bairro
        string Numero
        string Complemento
        string Profissao
        string NecessidadeEspecial
        string Observacoes

    }

    AGENDAMENTO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Status
        string Tipo
        string Procedimento
        string Profissional
        DateTime HoraInicial
        DateTime HoraFinal
        string Sala
        bool AgendarComoConfirmado
        string Descricao
    }
    CHAT{
        int ChatId PK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Pergunta
        string Resposta
    }

    TRANSCRIÇÃO {
        int Id PK
        int AgendamentoId FK
        int PacienteId FK
        DateTime CreatedAt
        bool IsDeleted
        string TextoTranscrito
    }

```

### Módulo 4 - Relacionamento Completo
```mermaid
erDiagram
    %% ========== NÍVEL 1: CLÍNICA (Raiz) ==========
    CLÍNICA ||--o{ PACIENTE : contem
    CLÍNICA ||--o{ PROFISSIONAL : contem

    %% ========== NÍVEL 2: PROFISSIONAL ==========
    PROFISSIONAL ||--|| USUARIO : login
    PROFISSIONAL ||--o{ AGENDAMENTO : realiza
    PROFISSIONAL ||--o{ ANAMNESE : elabora
    PROFISSIONAL ||--o{ EVOLUÇÃO : preenche
    PROFISSIONAL ||--o{ CHAT : envia

    %% ========== NÍVEL 2: PACIENTE ==========
    PACIENTE ||--o{ AGENDAMENTO : contem    
    PACIENTE ||--o{ EVOLUÇÃO : contem
    PACIENTE ||--o{ TRANSCRIÇÃO : contem
    PACIENTE ||--o{ ALERGIA : contem
    PACIENTE ||--o{ ANAMNESE : contem

    %% ========== NÍVEL 3: AGENDAMENTO (Centro) ==========
    AGENDAMENTO ||--o| TRANSCRIÇÃO : gera
    AGENDAMENTO ||--o| EVOLUÇÃO : gera

    %% ========== NÍVEL 4: ANAMNESE ==========
    ANAMNESE ||--o{ ANAMNESE_ITEM : contem



    CLÍNICA {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string CNPJ
    }
    USUARIO{
        int Id PK
        int ProfissionalId FK
        string Email UK
        string SenhaHash
        string Role
        DateTime UltimoAcesso
        bool Ativo
    }
    PROFISSIONAL{
        int Id PK
        int ChatId FK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string Cargo
        string Foto
        string Permissao
        string CRM
    }
    CHAT{
        int ChatId PK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Pergunta
        string Resposta
    }
    PACIENTE {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string status
        DateTime DataNascimento
        string Sexo
        string EstadoCivil
        string NomeDaMae
        string NomeDoPai
        string CPF
        string RG
        string OrgaoExpedidor
        string Estado 
        string DataEmissao
        string Telefone
        string Celular
        string Email 
        string CEP
        string Cidade
        string EstadoResidencia
        string Bairro
        string Numero
        string Complemento
        string Profissao
        string NecessidadeEspecial
        string Observacoes

    }
    AGENDAMENTO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Status
        string Tipo
        string Procedimento
        string Profissional
        DateTime HoraInicial
        DateTime HoraFinal
        string Sala
        bool AgendarComoConfirmado
        string Descricao
    }
    EVOLUÇÃO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Conteudo
    }
    TRANSCRIÇÃO {
        int Id PK
        int AgendamentoId FK
        int PacienteId FK
        DateTime CreatedAt
        bool IsDeleted
        string TextoTranscrito
    }
    ALERGIA {
        int Id PK
        int PacienteId FK
        DateTime CreatedAt
        bool IsDeleted
        string NomeAlergia
        string Gravidade
        DateTime DataIdentificacao
        string Observacoes
        
    }
    ANAMNESE{
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Observacoes
    }
    ANAMNESE_ITEM{
        int Id PK
        int AnamneseId FK
        string Pergunta
        string Resposta
        DateTime CreatedAt
        bool IsDeleted
    }


```

# INICIAÇÃO CIENTÍFICA


## Requisitos Funcionais

|ID    | Descrição do Requisito  | Prioridade |
|------|-----------------------------------------|----|
|RF-001| Ativar controle de permissões no sistema baseado no tipo de usuário identificado. | ALTA |
|RF-002| Implementar funcionalidade no sistema de transcrição de áudio por meio de inteligência artificial. | ALTA | 
|RF-003| Implementar um agente de inteligência artificial integrado no sistema que permita ao usuário efetuar prompts e obter respostas. | ALTA |
|RF-004| Permitir cadastro completo de pacientes incluindo dados pessoais e dados clínicos complementares  | ALTA |
|RF-005| Permitir agendamento de consulta/retorno de paciente vinculado à um profissional da clínica  | ALTA |
|RF-006| Permitir visualização de consulta/retorno de paciente vinculado à um profissional da clínica; (Existe a biblioteca full calendar para React) | ALTA |
|RF-007| Permitir o cadastro completo de evolução do paciente com data, tipo profissional e detalhamento  | ALTA |
|RF-008| Permitir a visualização do histórico de evoluções dados do paciente (prescrições, atestados, receitas, dietas, procedimentos etc)  | ALTA |
|RF-009| Permitir a edição e atualização de dados das evoluções e cadastrais dos pacientes   | ALTA |
|RF-010| Permitir que o profissional gerencie o estoque de medicamentos e materiais com alertas de baixo estoque e vencimento de produtos | ALTA |
|RF-011| Permitir gestão de caixa, controle de pagamentos, faturas e relatórios financeiros gerais | ALTA |
|RF-012| Permitir que o usuário com perfil de Administrador da clínica, possua tenha acesso à um relatório financeiro   | ALTA |


## Diagramas de Relacionamento de Entidades

### Diagrama Conceitual
```mermaid
erDiagram
    %% AGENDAMENTO é o centro do sistema
    PACIENTE ||--o{ AGENDAMENTO : "participa"
    PROFISSIONAL ||--o{ AGENDAMENTO : "conduz"
    
    AGENDAMENTO ||--o| EVOLUÇÃO : "resulta em"
    AGENDAMENTO ||--o| TRANSCRICAO : "gera"
    AGENDAMENTO ||--o| ANAMNESE : "inclui"
    
    %% Relacionamentos secundários (limpos)
    CLINICA ||--o{ PACIENTE : "atende"
    CLINICA ||--o{ PROFISSIONAL : "emprega"
    PACIENTE ||--o{ ALERGIA_PACIENTE : "tem"
    PROFISSIONAL ||--|| USUARIO : "acessa via"
    ANAMNESE ||--o{ ANAMNESE_ITEM : "contém"

    CLINICA {
        int Id PK
    }
    
    PACIENTE {
        int Id PK
    }
    
    PROFISSIONAL {
        int Id PK
    }
    
    AGENDAMENTO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
    }
    
    EVOLUÇÃO {
        int Id PK
        int AgendamentoId FK
    }
    
    TRANSCRICAO {
        int Id PK
        int AgendamentoId FK
        string TextoTranscrito
    }
    
    ANAMNESE {
        int Id PK
        int AgendamentoId FK
    }
    
    ANAMNESE_ITEM {
        int Id PK
        int AnamneseId FK
    }
    
    ALERGIA_PACIENTE {
        int Id PK
        int PacienteId FK
    }
    
    USUARIO {
        int Id PK
        int ProfissionalId FK
    }

```

### Módulo 1 - Gestão Clínica
```mermaid
erDiagram
    %% ========== NÍVEL 1: CLÍNICA (Raiz) ==========
    CLÍNICA ||--o{ PACIENTE : contem
    CLÍNICA ||--o{ PROFISSIONAL : contem

    %% ========== NÍVEL 2: PROFISSIONAL ==========
    PROFISSIONAL ||--|| USUARIO : login

    CLÍNICA {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string CNPJ
    }
    USUARIO{
        int Id PK
        int ProfissionalId FK
        string Email UK
        string SenhaHash
        string Role
        DateTime UltimoAcesso
        bool Ativo
    }
    PROFISSIONAL{
        int Id PK
        int ChatId FK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string Cargo
        string Foto
        string Permissao
        string CRM
    }
    PACIENTE {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string status
        DateTime DataNascimento
        string Sexo
        string EstadoCivil
        string NomeDaMae
        string NomeDoPai
        string CPF
        string RG
        string OrgaoExpedidor
        string Estado 
        string DataEmissao
        string Telefone
        string Celular
        string Email 
        string CEP
        string Cidade
        string EstadoResidencia
        string Bairro
        string Numero
        string Complemento
        string Profissao
        string NecessidadeEspecial
        string Observacoes

    }

```


### Módulo 2 - Atendimento Clínico
```mermaid
erDiagram
    %% ========== NÍVEL 2: PROFISSIONAL ==========
    PROFISSIONAL ||--o{ AGENDAMENTO : realiza
    PROFISSIONAL ||--o{ ANAMNESE : elabora
    PROFISSIONAL ||--o{ EVOLUÇÃO : preenche
    %% ========== NÍVEL 2: PACIENTE ==========
    PACIENTE ||--o{ AGENDAMENTO : contem    
    PACIENTE ||--o{ EVOLUÇÃO : contem
    PACIENTE ||--o{ ALERGIA : contem
    PACIENTE ||--o{ ANAMNESE : contem
    %% ========== NÍVEL 3: AGENDAMENTO (Centro) ==========
    AGENDAMENTO ||--o| EVOLUÇÃO : gera

    %% ========== NÍVEL 4: ANAMNESE ==========
    ANAMNESE ||--o{ ANAMNESE_ITEM : contem

    PROFISSIONAL{
        int Id PK
        int ChatId FK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string Cargo
        string Foto
        string Permissao
        string CRM
    }
   PACIENTE {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string status
        DateTime DataNascimento
        string Sexo
        string EstadoCivil
        string NomeDaMae
        string NomeDoPai
        string CPF
        string RG
        string OrgaoExpedidor
        string Estado 
        string DataEmissao
        string Telefone
        string Celular
        string Email 
        string CEP
        string Cidade
        string EstadoResidencia
        string Bairro
        string Numero
        string Complemento
        string Profissao
        string NecessidadeEspecial
        string Observacoes

    }
    AGENDAMENTO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Status
        string Tipo
        string Procedimento
        string Profissional
        DateTime HoraInicial
        DateTime HoraFinal
        string Sala
        bool AgendarComoConfirmado
        string Descricao
    }
    EVOLUÇÃO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Conteudo
    }

    ALERGIA {
        int Id PK
        int PacienteId FK
        DateTime CreatedAt
        bool IsDeleted
        string NomeAlergia
        string Gravidade
        DateTime DataIdentificacao
        string Observacoes
        
    }
    ANAMNESE{
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string NomeDaAnamnese
    }
    ANAMNESE_ITEM{
        int Id PK
        int AnamneseId FK
        string Pergunta
        string Resposta
        string Tipo
        DateTime CreatedAt
        bool IsDeleted
    }

```


### Módulo 3 - Funcionalidades de IA
```mermaid
erDiagram
    %% ========== NÍVEL 2: PROFISSIONAL ==========
    PROFISSIONAL ||--o{ CHAT : envia

    %% ========== NÍVEL 2: PACIENTE ==========
    PACIENTE ||--o{ TRANSCRIÇÃO : contem
    PACIENTE ||--o{ AGENDAMENTO : contem   
    %% ========== NÍVEL 3: AGENDAMENTO (Centro) ==========
    AGENDAMENTO ||--o| TRANSCRIÇÃO : gera



    PROFISSIONAL{
        int Id PK
        int ChatId FK
        DateTime CreatedAt
        bool IsDeleted
        string Nome
        string Email
        string Cargo
        string Foto
        string Permissao
        string CRM
    }
    PACIENTE {
        int Id PK
        DateTime CreatedAt
        bool IsDeleted
        string status
        DateTime DataNascimento
        string Sexo
        string EstadoCivil
        string NomeDaMae
        string NomeDoPai
        string CPF
        string RG
        string OrgaoExpedidor
        string Estado 
        string DataEmissao
        string Telefone
        string Celular
        string Email 
        string CEP
        string Cidade
        string EstadoResidencia
        string Bairro
        string Numero
        string Complemento
        string Profissao
        string NecessidadeEspecial
        string Observacoes

    }

    AGENDAMENTO {
        int Id PK
        int PacienteId FK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Status
        string Tipo
        string Procedimento
        string Profissional
        DateTime HoraInicial
        DateTime HoraFinal
        string Sala
        bool AgendarComoConfirmado
        string Descricao
    }
    CHAT{
        int ChatId PK
        int ProfissionalId FK
        DateTime CreatedAt
        bool IsDeleted
        string Pergunta
        string Resposta
    }

    TRANSCRIÇÃO {
        int Id PK
        int AgendamentoId FK
        int PacienteId FK
        DateTime CreatedAt
        bool IsDeleted
        string TextoTranscrito
    }

```

### Módulo 4 - Relacionamento Completo
```mermaid

  
  erDiagram
  CLINICA ||--o{ PACIENTE : "atende"
  CLINICA ||--o{ PROFISSIONAL : "emprega"
 
  PROFISSIONAL ||--|| USUARIO : "login"
  PROFISSIONAL ||--o{ AGENDAMENTO : "realiza"
  PROFISSIONAL ||--o{ ANAMNESE : "elabora"
  PROFISSIONAL ||--o{ EVOLUCAO : "preenche"
  PROFISSIONAL ||--o{ CHAT : "possui"
 
  PACIENTE ||--o{ AGENDAMENTO : "tem"
  PACIENTE ||--o{ ALERGIA : "tem"
  PACIENTE ||--o{ ANAMNESE : "tem"
 
  AGENDAMENTO ||--o| EVOLUCAO : "gera"
  AGENDAMENTO ||--o| TRANSCRICAO : "gera"
 
  ANAMNESE ||--o{ ANAMNESE_ITEM : "contem"
  CHAT ||--o{ CHAT_MENSAGEM : "contem"
 
  CLINICA {
    int Id PK
    string Nome
    string Email
    string CNPJ
    DateTime CreatedAt
    bool IsDeleted
  }
  USUARIO {
    int Id PK
    int ProfissionalId FK
    string Email
    string SenhaHash
    string Role
    DateTime UltimoAcesso
    bool Ativo
  }
  PROFISSIONAL {
    int Id PK
    int ClinicaId FK
    string Nome
    string Email
    string Cargo
    string Foto
    string Permissao
    string CRM
    DateTime CreatedAt
    bool IsDeleted
  }
  PACIENTE {
    int Id PK
    int ClinicaId FK
    string Status
    string Nome
    DateTime DataNascimento
    string Sexo
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
    bool IsDeleted
  }
  AGENDAMENTO {
    int Id PK
    int PacienteId FK
    int ProfissionalId FK
    string Status
    string Tipo
    string Procedimento
    DateTime HoraInicial
    DateTime HoraFinal
    string Sala
    bool Confirmado
    string Descricao
    DateTime CreatedAt
    bool IsDeleted
  }
  EVOLUCAO {
    int Id PK
    int AgendamentoId FK
    string Conteudo
    DateTime CreatedAt
    bool IsDeleted
  }
  TRANSCRICAO {
    int Id PK
    int AgendamentoId FK
    string TextoTranscrito
    DateTime CreatedAt
    bool IsDeleted
  }
  ALERGIA {
    int Id PK
    int PacienteId FK
    string NomeAlergia
    string Gravidade
    DateTime DataIdentificacao
    string Observacoes
    DateTime CreatedAt
    bool IsDeleted
  }
  ANAMNESE {
    int Id PK
    int PacienteId FK
    int ProfissionalId FK
    string NomeDaAnamnese
    string Observacoes
    DateTime CreatedAt
    bool IsDeleted
  }
  ANAMNESE_ITEM {
    int Id PK
    int AnamneseId FK
    string Pergunta
    string Resposta
    string Tipo
    DateTime CreatedAt
    bool IsDeleted
  }
  CHAT {
    int Id PK
    int ProfissionalId FK
    string Titulo
    DateTime CreatedAt
    bool IsDeleted
  }
  CHAT_MENSAGEM {
    int Id PK
    int ChatId FK
    string Papel
    string Conteudo
    DateTime CreatedAt
  }


```
