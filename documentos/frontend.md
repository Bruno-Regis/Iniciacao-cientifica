# Frontend Guidelines

## 1. Escopo

Este documento define a fundação atual para o desenvolvimento do frontend e da experiência de uso do MVP do DoctorPilot. Ele orienta:

- estrutura visual;
- padrões de navegação;
- uso do MudBlazor;
- organização de páginas e componentes;
- princípios de UX;
- responsividade;
- estados da interface;
- direção visual provisória.

Estas diretrizes não formam uma especificação pixel-perfect. A identidade visual ainda está em definição, e paleta, branding, tipografia, sombras, radius e detalhes estéticos podem evoluir.

A arquitetura de informação, a clareza dos fluxos e os padrões de interação devem ser mais estáveis que a aparência atual. Mudanças futuras de tema não devem exigir reescrever as páginas ou alterar a lógica de interação dos componentes.

Quando este documento descrever uma direção desejada, isso não significa que ela já esteja implementada. O estado atual do código e as orientações para evolução são distinguidos ao longo do texto.

## 2. Stack de Frontend

O frontend está no projeto `DoctorPilot.Web.Migration`, uma aplicação ASP.NET Core Razor Components em .NET 10 com Interactive Server render mode.

Stack confirmada:

- ASP.NET Core Razor Components;
- Interactive Server;
- MudBlazor 9.8.0;
- componentes e páginas `.razor`;
- CSS global em `wwwroot/app.css`;
- CSS isolation em arquivos `.razor.css` de layout;
- Bootstrap distribuído em `wwwroot/lib` e carregado por `App.razor`;
- `DoctorPilot.Contracts` como única referência de projeto do Web;
- `DoctorPilotApiClient` e `HttpClient` nomeado para comunicação com a API.

O registro atual inclui `AddRazorComponents().AddInteractiveServerComponents()` e `AddMudServices()`. `App.razor` carrega os assets do Bootstrap, do projeto e do MudBlazor. O render mode é Interactive Server quando a rota aceita navegação interativa.

O Web não acessa Entity Framework Core nem o banco. O fluxo de dados deve continuar seguindo:

```text
Razor Component -> Web Service / HttpClient -> DoctorPilot.Api
```

`DoctorPilotApiClient` concentra atualmente operações de pacientes e alergias. `TokenHandlerService` recupera o JWT da sessão autenticada e o adiciona às chamadas da API como Bearer token.

A autenticação do navegador utiliza cookie. O login chama a API, valida o JWT retornado e o armazena no ticket do cookie. `AuthorizeRouteView`, `[Authorize]`, `RedirectToLogin` e `AuthorizeView` participam da proteção e apresentação das rotas. A página de login usa um POST HTML completo e é excluída de interactive routing.

O projeto ainda contém páginas do template inicial, como `Counter` e `Weather`, além de `Home`, login e páginas de pacientes. A presença dessas páginas não define a arquitetura de informação final do produto.

## 3. MudBlazor como biblioteca principal

MudBlazor deve ser a biblioteca principal para componentes de interface. Antes de criar um componente visual manualmente, deve-se verificar se MudBlazor já fornece uma solução apropriada.

Conforme o caso concreto, priorizar componentes como:

- `MudButton` e `MudIconButton` para ações;
- `MudTabs` para conteúdo relacionado dentro de um mesmo contexto;
- `MudTable` ou `MudDataGrid` para dados tabulares;
- `MudDialog` para fluxos modais;
- `MudTextField`, `MudSelect` e outros inputs em formulários;
- `MudPaper` e `MudCard` para surfaces e agrupamentos;
- `MudSnackbar` e `MudAlert` para feedback;
- `MudProgressCircular` ou skeletons para carregamento;
- `MudChip` para estados compactos quando semanticamente adequado.

Essa lista não obriga o uso de um componente específico quando outro componente MudBlazor ou uma solução mais simples for mais adequada.

HTML e CSS customizados são permitidos quando necessários para semântica, fluxos específicos ou identidade visual. O login atual, por exemplo, usa inputs HTML dentro de um POST tradicional. Customizações devem preservar comportamento, acessibilidade, estados, integração e manutenção dos componentes MudBlazor.

Evitar recriar manualmente dialogs, tabs, feedback, grids, menus, seletores e controles que a biblioteca já resolve satisfatoriamente.

## 4. Direção visual

A direção visual do MVP é:

> Modern Health-Tech UI with subtle Soft UI influences

A interface deve ser:

- clean;
- profissional;
- moderna;
- confiável;
- confortável para uso clínico prolongado;
- tecnologicamente atual, incluindo presença associada à IA quando pertinente;
- altamente legível;
- capaz de apresentar boa densidade de informação;
- organizada por uma hierarquia visual clara.

O DoctorPilot não deve adotar neumorphism puro. Soft UI/neumorphism deve funcionar apenas como assinatura visual sutil.

Evitar:

- sombras excessivas;
- cards dentro de cards sem necessidade;
- elementos excessivamente arredondados;
- decoração competindo com informação clínica;
- páginas internas com aparência de landing page;
- aparência excessivamente futurista;
- grandes áreas vazias que reduzam indevidamente a densidade útil;
- sacrificar contraste, legibilidade ou previsibilidade por estética.

## 5. Tema e Design Tokens

O projeto deverá evoluir para um `MudTheme` centralizado. Atualmente existe `MudThemeProvider`, mas nenhum tema customizado central é fornecido a ele; a UI usa principalmente defaults do MudBlazor, classes utilitárias e CSS herdado do template.

Cores, tipografia, radius, espaçamento, elevação e outros tokens compartilhados não devem ser repetidos como valores hardcoded nos componentes. O tema e classes CSS reutilizáveis devem permitir mudanças de identidade sem reescrever as páginas.

A paleta atual é provisória. A direção cromática a explorar inclui:

- deep navy para navegação e estrutura;
- indigo/cobalt como cor primária;
- cyan, teal ou mint como accent secundário;
- backgrounds claros e frios;
- surfaces claras;
- texto em slate ou navy escuro.

Este documento não fixa valores hexadecimais nem uma paleta definitiva.

Cores semânticas devem permanecer independentes da identidade da marca:

- vermelho para erro, perigo e risco alto;
- âmbar para alerta e atenção;
- verde para sucesso e estado positivo;
- estados clínicos adicionais devem priorizar significado consistente, contraste e acessibilidade.

O CSS atual contém cores hardcoded herdadas do template, inclusive o gradiente da sidebar. Esses valores representam o estado provisório e não devem ser tratados como tokens definitivos.

## 6. Soft UI / Neumorphism

Soft UI pode ser utilizado de forma controlada em:

- CTA principal;
- cards especiais;
- cards de métricas;
- elementos de destaque;
- estados selecionados específicos;
- elementos relacionados à experiência de IA, quando fizer sentido.

Evitar sua aplicação uniforme em:

- todas as tabs;
- todas as tabelas;
- todos os inputs;
- cada card;
- filtros;
- ações pequenas;
- grandes quantidades de informação clínica.

Botões principais podem receber sombra suave, leve elevação no hover, feedback de pressão no active, radius moderado e transições curtas. `MudButton` deve continuar sendo a base quando adequado, complementado por classes CSS reutilizáveis em vez de implementações paralelas.

Elevações devem ajudar a comunicar hierarquia ou interação, não funcionar apenas como decoração.

## 7. Layout principal

O layout conceitual do produto deve oferecer:

- sidebar persistente em desktop/notebook;
- área principal clara;
- navegação principal consistente;
- identificação do usuário autenticado;
- acesso previsível a logout e configurações;
- separação visual entre funções administrativas/clínicas e funções de inteligência quando apropriado.

O `MainLayout` atual já usa uma sidebar sticky de 250 px em viewports maiores e muda para navegação recolhível abaixo de 641 px. Porém, ele ainda é próximo do template padrão: possui um link “About”, usa CSS/branding provisórios e não exibe identidade do usuário.

O `NavMenu` atual contém apenas Home, Counter, Pacientes e logout autenticado. Conceitualmente, a sidebar futura poderá organizar:

- Visão geral;
- Agenda;
- Pacientes;
- outros módulos clínicos conforme forem implementados;
- seção de Inteligência;
- Assistente IA;
- Transcrições;
- Configurações;
- usuário autenticado e logout.

Itens dessa lista que ainda não possuem rota ou funcionalidade não devem ser apresentados como implementados. A navegação deve crescer junto com módulos reais do MVP, evitando links vazios ou telas fictícias.

## 8. Padrão de páginas

### Listagens

Listagens devem conter, conforme necessário:

1. título e contexto da página;
2. ação principal claramente posicionada;
3. busca e filtros relevantes;
4. tabela ou lista adequada ao conteúdo;
5. paginação;
6. estado de loading;
7. empty state;
8. estado de erro.

`Pacientes.razor` já inicia esse padrão com título, busca, ação de cadastro, `MudDataGrid`, status com `MudChip` e ação de visualização. Loading existe; paginação, empty state e error state ainda não estão completos na tela atual.

### Detalhes

Páginas de detalhe devem oferecer:

- breadcrumb ou ação clara de retorno;
- cabeçalho persistente da entidade;
- identificação e informações resumidas;
- ações contextuais;
- conteúdo organizado em seções ou tabs quando necessário.

O usuário não deve perder o contexto da entidade ao alternar entre subconjuntos de informação.

### Formulários

Formulários devem utilizar:

- labels claras;
- agrupamento lógico;
- validação próxima ao campo;
- indicação de obrigatoriedade quando relevante;
- ações salvar e cancelar previsíveis;
- feedback de sucesso e erro;
- densidade compatível com o fluxo.

Evitar formulários extensos apresentados como uma massa única de inputs. O formulário atual de paciente já agrupa dados em informações básicas, documentação, localização e informações adicionais.

## 9. Patient Details como padrão inicial

`PacienteDetails.razor` será a primeira tela de referência visual do MVP para validar esta direção.

Estrutura conceitual:

1. retorno para a lista de pacientes;
2. patient header;
3. identificação do paciente;
4. informações resumidas relevantes;
5. ações principais:
   - Iniciar Atendimento;
   - Agendar Consulta;
6. navegação por `MudTabs`;
7. conteúdo da tab selecionada.

Tabs planejadas:

- Informações;
- Alergias;
- Evoluções;
- Anamnese;
- Histórico;
- Transcrições.

No estado atual, `PacienteDetails` já possui retorno, nome, celular, dois botões de ação e seis `MudTabPanel`s. Informações e Alergias possuem componentes reais. Anamnese, Evoluções, Transcrições e Histórico ainda apresentam conteúdo provisório. A ordem atual também difere da lista conceitual acima.

A presença da tab no planejamento não significa que sua funcionalidade já exista no Web ou na API. Cada tab deve ser habilitada e finalizada conforme as capacidades reais do projeto.

## 10. Patient Header

O patient header deve permanecer visível e consistente entre as tabs. Ele pode conter:

- avatar ou iniciais;
- nome;
- informações resumidas;
- status;
- telefone quando relevante;
- ações principais.

Essas informações não devem ser repetidas dentro de cada tab. O header deve preservar contexto sem ocupar espaço desproporcional nem competir com informações clínicas.

No código atual ainda não existe um componente `PatientHeader`; nome e celular são renderizados diretamente em `PacienteDetails`. A extração futura deve ocorrer somente se trouxer responsabilidade clara, reutilização ou redução real de complexidade.

## 11. Visualização versus edição

Dados em modo somente leitura não devem parecer campos editáveis.

Preferir a apresentação:

```text
Label
Valor
```

em vez de representar todas as informações com `MudTextField` desabilitado ou read-only.

Ao entrar em modo de edição, utilizar `MudTextField`, `MudSelect`, `MudDatePicker` e outros componentes de formulário apropriados. A mudança de modo deve ser perceptível e as ações salvar/cancelar devem ser claras.

`InformacoesTab.razor` atualmente apresenta os dados do paciente em vários `MudTextField ReadOnly="true"`. Esse é o estado atual, não o padrão visual desejado para a evolução da tela. A separação entre leitura e edição deve reduzir ruído e comunicar claramente o estado da interface.

## 12. Tabs

`MudTabs` é a solução preferencial para informações relacionadas dentro do contexto do paciente.

Regras:

- o contexto do paciente permanece visível;
- trocar de tab não deve parecer navegar para outro paciente;
- o estado selecionado deve ser evidente;
- evitar efeitos visuais exagerados em todas as tabs;
- preservar navegação por teclado e comportamento acessível padrão do MudBlazor;
- avaliar scroll, quebra ou outra estratégia responsiva quando as tabs não couberem horizontalmente;
- loading, empty e error state pertencem ao conteúdo de cada tab quando os dados são carregados separadamente.

O `MudTabs` atual usa elevação, arredondamento e efeitos no container. Esses parâmetros são provisórios e devem ser reavaliados junto com a fundação visual.

## 13. Componentização

Não transformar cada bloco pequeno em um componente. Criar componentes reutilizáveis quando houver pelo menos uma justificativa concreta:

- responsabilidade própria;
- comportamento próprio;
- reutilização;
- complexidade relevante;
- necessidade de isolamento de estado ou carregamento.

O código atual já separa:

- `PacienteForm`;
- `InformacoesTab`;
- `AlergiasTab`;
- `AlergiaFormDialog`.

`PacienteDetails` poderá futuramente utilizar componentes como:

- `PatientHeader`;
- `PatientInformation`;
- `PatientAllergies`;
- `PatientEvolutions`;
- `PatientAnamnesis`;
- `PatientTranscriptions`.

Esses nomes são exemplos conceituais, não uma estrutura obrigatória. Se uma tab tiver apenas markup simples e nenhuma responsabilidade própria, mantê-la na página pode ser mais claro. Evitar abstração prematura e wrappers que apenas repassam parâmetros.

## 14. Estados da interface

Toda funcionalidade assíncrona relevante deve considerar:

- loading;
- success;
- empty;
- validation error;
- API/server error;
- unauthorized/forbidden quando aplicável.

Priorizar componentes MudBlazor adequados, como progress indicators, skeletons, alerts, snackbars, dialogs e componentes de empty state compostos com texto e ação.

Evitar páginas ou tabs que simplesmente não renderizam nada durante loading ou falha. Feedback de sucesso deve ser claro, mas não interromper desnecessariamente o fluxo.

O estado atual já possui alguns exemplos:

- `MudProgressCircular` em carregamentos;
- `MudSnackbar` para sucesso, validação e erro em formulários;
- `MudAlert` no login;
- mensagem vazia na lista de alergias;
- redirecionamento de rotas não autorizadas para login;
- modal de reconexão do Interactive Server.

Tratamento de erro e empty state ainda não é uniforme entre páginas e serviços. Alguns métodos do API client retornam coleção vazia ou `null`, o que pode ocultar a diferença entre ausência de dados e falha de comunicação.

## 15. Responsividade

A experiência principal do MVP é desktop/notebook. Ainda assim:

- evitar larguras rígidas desnecessárias;
- utilizar layouts e grids responsivos;
- reorganizar colunas em telas menores;
- manter estratégia de sidebar para viewport reduzido;
- garantir uso das tabs quando não couberem horizontalmente;
- impedir que ações principais quebrem ou transbordem o layout;
- preservar legibilidade e targets de interação.

O layout atual já alterna entre sidebar desktop e menu recolhível em telas menores. `MudGrid` usa combinações como `xs="12"` e `sm="6"` nos formulários e informações. Esses comportamentos são uma base, mas as páginas ainda precisam ser validadas em viewports reais.

Não é necessário neste momento otimizar pixel-perfect para todos os dispositivos móveis.

## 16. Acessibilidade e UX clínica

Priorizar:

- contraste adequado;
- tipografia legível;
- foco visível;
- navegação por teclado;
- labels associadas aos controles;
- estados que não dependam apenas de cor;
- targets de interação adequados;
- hierarquia clara de títulos e ações;
- mensagens compreensíveis e acionáveis.

Informações clínicas importantes têm prioridade sobre decoração. Risco, alerta, gravidade e estados clínicos não devem ser expressos apenas pela cor da marca ou por cor sem texto/ícone complementar.

Componentes MudBlazor devem manter sua semântica e comportamento acessível. CSS customizado não deve remover foco, reduzir contraste ou tornar hit areas menores.

## 17. Figma e referências visuais

As telas atuais do Figma são referência principalmente para:

- fluxo;
- arquitetura de informação;
- conteúdo;
- agrupamento;
- navegação.

Elas não são especificação visual definitiva para:

- cores;
- tipografia;
- sombras;
- radius;
- espaçamento;
- dimensões pixel-perfect.

Referências externas de Soft UI ou neumorphism são inspiração. Não devem ser copiadas integralmente nem substituir avaliação de acessibilidade, densidade clínica e manutenção.

Nenhum arquivo ou link do Figma está versionado na estrutura analisada; estas diretrizes registram o papel esperado dessas referências, não validam uma versão específica de design.

## 18. Evolução do design

Processo recomendado:

1. estabelecer a fundação visual e os tokens iniciais;
2. implementar uma tela de referência;
3. validar UX com os fluxos reais;
4. reutilizar os padrões aprovados;
5. desenvolver o restante do MVP;
6. revisar a identidade visual;
7. consolidar paleta e branding;
8. atualizar o `MudTheme` central;
9. realizar o polimento final.

`PacienteDetails` será a primeira tela usada para validar essa direção. Os padrões aprovados nela devem ser reutilizados com critério, sem forçar a mesma composição em telas com necessidades diferentes.

## 19. Regras para futuras implementações

Antes de criar ou alterar uma tela:

1. verificar os componentes MudBlazor disponíveis;
2. verificar os padrões já existentes no projeto;
3. reutilizar tokens do tema;
4. evitar cores hardcoded nos componentes;
5. evitar CSS duplicado;
6. manter lógica de negócio fora do componente visual quando apropriado;
7. preservar a separação Web -> API;
8. considerar loading, error e empty states;
9. considerar autenticação e autorização;
10. considerar responsividade;
11. manter consistência com telas já aprovadas;
12. evitar afirmar ou exibir módulos ainda não implementados.

Componentes Razor devem concentrar apresentação, interação e estado de UI. Regras de negócio e persistência não devem migrar para o Web. Comunicação com o backend deve continuar por serviços e contratos apropriados, sem acesso direto ao banco ou ao `DbContext`.

Mudanças de identidade visual devem ser absorvidas preferencialmente pelo `MudTheme`, tokens e classes reutilizáveis. Páginas não devem depender de valores visuais repetidos que tornem uma mudança de tema um refactor funcional.
