# Authentication and Authorization

## 1. Visão geral

Este documento descreve a implementação de autenticação e autorização observada no código atual. Limitações conhecidas e regras arquiteturais desejadas são identificadas separadamente; elas não devem ser confundidas com funcionalidades já implementadas.

A solução usa dois mecanismos relacionados:

- `DoctorPilot.Api` autentica requests com JWT Bearer.
- `DoctorPilot.Web.Migration` mantém a sessão do navegador com cookie authentication.

O Web não envia seu cookie diretamente à API. Depois do login, ele armazena o JWT emitido pela API dentro do ticket do cookie. Nas chamadas posteriores, o Web recupera esse token e o envia à API no header `Authorization: Bearer`.

## 2. Fluxo completo de login

### 2.1 Página `/login`

A página `Components/Pages/Auth/Login.razor` é pública e usa `ExcludeFromInteractiveRouting`. Ela renderiza um formulário HTML que:

- envia `POST /account/login`;
- inclui um `AntiforgeryToken`;
- usa `data-enhance="false"`, causando navegação completa;
- envia `Email`, `Senha` e um `ReturnUrl` local validado.

### 2.2 Endpoint Web `POST /account/login`

O endpoint é mapeado em `DoctorPilot.Web.Migration/Program.cs` e usa `AllowAnonymous`. Seu fluxo é:

1. Normalizar o `returnUrl`, aceitando somente caminhos locais.
2. Rejeitar e-mail ou senha vazios.
3. Chamar `POST /api/auth/login` por meio do `HttpClient` nomeado `DoctorPilotApi`.
4. Interpretar a resposta como `ResultViewModel<LoginResponseDto>`.
5. Extrair o JWT de `Data.Token`.
6. Validar o JWT localmente.
7. Criar uma identidade com as claims validadas.
8. Criar o ticket e o cookie de autenticação do Web.
9. Redirecionar para o `returnUrl` local.

Falhas de credenciais, respostas sem token ou tokens inválidos retornam o usuário para `/login` com um código de erro na query string. Falhas de conexão com a API usam o erro `unavailable`.

### 2.3 Endpoint API `POST /api/auth/login`

`AuthController.Login` é o único endpoint de controller explicitamente marcado com `AllowAnonymous`. O controller recebe diretamente um `LoginCommand` e o envia com `IMediator.Send`.

### 2.4 `LoginCommand` e `LoginHandler`

`LoginCommand` implementa `ICommand<ResultViewModel<LoginResponseDto>>` e contém `Email` e `Senha`.

O `LoginHandler`:

1. Busca o usuário por e-mail com `IUsuarioRepository.GetByEmailAsync`.
2. Rejeita usuário inexistente.
3. Rejeita usuário inativo.
4. Verifica a senha com `IPasswordHasher.Verify`.
5. Gera o JWT com `ITokenGenerator.GenerateToken`.
6. Atualiza o último acesso do usuário.
7. Persiste a atualização com o repositório e `IUnitOfWork.CommitAsync`.
8. Retorna token, expiração, ID do usuário, `ClinicaId`, e-mail e role em `LoginResponseDto`.

### 2.5 BCrypt

`BCryptPasswordHasher` implementa `IPasswordHasher` usando `BCrypt.Net.BCrypt`:

- `HashPassword` para gerar hashes na criação de usuários;
- `Verify` para comparar a senha apresentada com o hash persistido.

### 2.6 Criação da sessão Web

Depois de validar o JWT, o Web cria uma nova `ClaimsIdentity` usando o esquema padrão de cookie. As claims da identidade são copiadas do principal produzido pela validação do JWT.

O Web cria `AuthenticationProperties` não persistentes, define `ExpiresUtc` com a expiração retornada pela API e armazena o JWT como um token chamado `access_token`. Por fim, chama `SignInAsync`, o que emite o cookie `DoctorPilot.Auth`.

## 3. JWT

### 3.1 Emissão

`JwtTokenGenerator` gera tokens com:

- algoritmo de assinatura `HmacSha256`;
- chave simétrica lida de `Jwt:SecretKey`;
- issuer lido de `Jwt:Issuer`;
- audience lida de `Jwt:Audience`;
- expiração de um dia a partir de `DateTime.UtcNow`.

No ambiente de desenvolvimento observado, issuer e audience são, respectivamente, `DoctorPilot.Api` e `DoctorPilot.Web`.

### 3.2 Claims emitidas

O JWT contém:

| Claim | Origem | Uso atual |
|---|---|---|
| `ClaimTypes.NameIdentifier` | `Usuario.Id` | Identificação do usuário atual |
| `ClaimTypes.Email` | `Usuario.Email` | E-mail da identidade |
| `ClaimTypes.Role` | `Usuario.Role.ToString()` | Autorização por role |
| `ClinicaId` | `Usuario.ClinicaId` | Escopo da clínica |

`ClinicaId` e role são dados assinados no JWT. O código de autorização deve usar essas claims, e não valores equivalentes enviados pelo cliente.

### 3.3 Validação pela API

A API configura JWT Bearer em `InfrastructureModule`. Ela valida:

- issuer;
- audience;
- lifetime;
- assinatura;
- chave de assinatura.

O `TokenValidationParameters` da API não define explicitamente `ClockSkew`; portanto, usa o padrão da biblioteca.

### 3.4 Validação pelo Web

Antes de criar o cookie, o Web valida os mesmos elementos com `JwtSecurityTokenHandler.ValidateToken`. O Web define `ClockSkew` de um minuto.

Essa validação local impede que o Web crie uma sessão a partir de um token expirado, mal assinado ou destinado a issuer/audience diferentes.

### 3.5 Configuração de secrets

Em ambiente de desenvolvimento, valores sensíveis não são armazenados nos arquivos `appsettings`.

Os seguintes valores são fornecidos por .NET User Secrets:

- `DoctorPilot.Api`
  - `Jwt:SecretKey`
  - `ConnectionStrings:DoctorPilotCs`

- `DoctorPilot.Web.Migration`
  - `Jwt:SecretKey`

API e Web devem utilizar o mesmo `Jwt:SecretKey`.

Configurações não sensíveis, como `Jwt:Issuer`, `Jwt:Audience`, `Cors:AllowedOrigins` e URLs locais, permanecem nos arquivos `appsettings`.

## 4. Cookie do Web

O esquema padrão do Web é `CookieAuthenticationDefaults.AuthenticationScheme`. Suas opções atuais são:

| Opção | Valor observado |
|---|---|
| Nome | `DoctorPilot.Auth` |
| `HttpOnly` | `true` |
| `SecurePolicy` | `Always` |
| `SameSite` | `Lax` |
| `IsEssential` | `true` |
| `SlidingExpiration` | `false` |
| `LoginPath` | `/login` |
| `AccessDeniedPath` | `/login` |

A sessão criada no login usa `IsPersistent = false`. O cookie é, portanto, um cookie de sessão, com expiração do ticket alinhada ao `ExpiresAt` retornado junto com o JWT.

O JWT não é armazenado em `localStorage` nem em `sessionStorage`. Ele é salvo como `access_token` nas `AuthenticationProperties` do ticket protegido pelo cookie.

## 5. Requests autenticados

`DoctorPilotApiClient` encapsula as operações HTTP atualmente usadas pelos componentes do Web. Ele recebe o `HttpClient` configurado para a API.

O cliente nomeado `DoctorPilotApi` registra `TokenHandlerService` como `DelegatingHandler`. Antes de enviar cada request, o handler:

1. Obtém o `HttpContext` por `IHttpContextAccessor`.
2. Confirma que a identidade está autenticada.
3. Recupera `access_token` com `HttpContext.GetTokenAsync("access_token")`.
4. Adiciona `Authorization: Bearer <token>` quando o token está disponível.
5. Envia o request ao próximo handler.

A API não confia na sessão do Web. Ela valida novamente o JWT recebido antes de autorizar o request.

## 6. Autorização da API

`DoctorPilot.Api/Program.cs` adiciona globalmente um `AuthorizeFilter` cuja política exige usuário autenticado. Assim, todos os actions dos controllers exigem autenticação, exceto quando marcados com `AllowAnonymous`.

### 6.1 Exceção anônima

Somente `POST /api/auth/login` está marcado com `AllowAnonymous`.

### 6.2 Autorização por role

A autorização declarativa usa os nomes do enum por meio de `nameof`, compatíveis com o valor emitido em `ClaimTypes.Role`.

- `POST /api/usuarios` exige `TenantAdmnin`.
- `POST /api/clinicas` exige `SuperAdmin`.

Na criação de usuários, o handler também valida a role solicitada. Na implementação observada, um usuário não-SuperAdmin pode solicitar somente `Medico` ou `Admnistrativo`; `SuperAdmin`, `TenantAdmnin` e valores fora do enum são rejeitados pelo handler. Como o controller exige `TenantAdmnin`, esse é o comportamento efetivo atual do endpoint.

O controller de clínicas contém atualmente somente a operação de criação, restrita a `SuperAdmin`.

## 7. Proteção do Web

`Components/Routes.razor` usa `AuthorizeRouteView`. Quando a rota possui metadados de autorização e o usuário não está autorizado, o roteador renderiza `RedirectToLogin`.

`RedirectToLogin`:

- obtém o caminho relativo atual, incluindo sua query;
- monta um `returnUrl` iniciado por `/`;
- redireciona para `/login?returnUrl=...`;
- usa `forceLoad: true` para realizar navegação completa.

As páginas protegidas usam `[Authorize]`. Atualmente estão protegidas:

- `/`;
- `/counter`;
- `/weather`;
- `/pacientes`;
- `/pacientes/cadastrar`;
- `/pacientes/{id}`;
- `/pacientes/{id}/editar`.

As páginas públicas atuais são:

- `/login`;
- `/Error`;
- `/not-found`.

Tanto a página de login quanto `GetLocalReturnUrl` validam que o retorno seja local. Valores vazios, sem `/` inicial, iniciados por `//` ou por `/\` são substituídos por `/`. O redirecionamento após login usa `Results.LocalRedirect`.

## 8. Roles

Os valores atuais de `RoleEnum`, incluindo suas grafias exatas, são:

- `SuperAdmin`;
- `TenantAdmnin`;
- `Medico`;
- `Admnistrativo`.

`TenantAdmnin` e `Admnistrativo` contêm erros ortográficos, mas seus nomes são usados em dados, serialização, claims JWT e verificações de autorização. Essas grafias devem ser preservadas até que exista uma migração planejada que trate dados persistidos, contratos e sessões/tokens existentes.

## 9. Isolamento por clínica

`ICurrentUserService` expõe:

- `UsuarioId`;
- `ClinicaId`;
- `Role`;
- `IsSuperAdmin`.

`CurrentUserService` obtém esses valores das claims do principal autenticado. `IsSuperAdmin` compara a claim de role com `nameof(RoleEnum.SuperAdmin)`.

Para usuários comuns, `ICurrentUserService.ClinicaId` é a fonte de verdade. Handlers de listagem usam a clínica autenticada, e operações por ID verificam a clínica do recurso ou de uma entidade relacionada antes de retornar ou modificar dados.

Um `ClinicaId` recebido em query, command, rota ou body não deve ser usado como autorização para usuários comuns. Quando esse dado existe no request, o handler deve substituí-lo pela claim ou validar o recurso contra a claim.

`SuperAdmin` pode ignorar verificações de clínica em handlers que testam explicitamente `IsSuperAdmin`. Esse comportamento ainda não é uniforme: algumas listagens permitem selecionar outra clínica, enquanto outras sempre usam o `ClinicaId` do principal, inclusive para SuperAdmin.

## 10. Logout

O menu autenticado apresenta um formulário para `POST /account/logout`. O formulário:

- aparece dentro de `AuthorizeView`;
- inclui `AntiforgeryToken`;
- usa `data-enhance="false"` para forçar navegação completa.

O endpoint de logout:

1. Exige autenticação com `RequireAuthorization`.
2. Valida antiforgery explicitamente com `IAntiforgery.ValidateRequestAsync`.
3. Executa `SignOutAsync` no esquema de cookie atual.
4. Retorna `LocalRedirect("/login")`.

O sign-out remove o ticket e expira o cookie `DoctorPilot.Auth`. Como `access_token` está armazenado dentro do ticket, ele deixa de estar disponível para `GetTokenAsync`. Requests posteriores originados da nova sessão anônima não recebem `Authorization: Bearer` de `TokenHandlerService`.

O logout é local ao Web. O JWT é stateless e continua criptograficamente válido até expirar; não há chamada de revogação à API.

## 11. Security considerations / known limitations

As condições abaixo descrevem limitações atuais. Elas não representam requisitos de implementação imediata definidos por este documento:

- Em desenvolvimento local, `Jwt:SecretKey` é fornecido por .NET User Secrets nos projetos API e Web. A connection string local da API também é fornecida por User Secrets. API e Web devem receber o mesmo valor de `Jwt:SecretKey`.
- O endpoint de login não possui rate limiting, lockout ou proteção equivalente contra tentativas repetidas.
- Não existe revogação ou blacklist de JWT. Um token copiado para fora da sessão Web permanece válido até expirar.
- Não existe refresh token.
- O comportamento de SuperAdmin entre clínicas não é uniforme em todas as listagens.
- Ambientes com múltiplas instâncias do Web precisarão configurar compartilhamento e persistência adequada das chaves do ASP.NET Core Data Protection usadas para proteger o cookie.
- API e Web duplicam a configuração de validação JWT e usam valores diferentes de `ClockSkew`.
- A API responde especificamente quando o usuário está inativo, embora o Web apresente as falhas de autenticação como credenciais inválidas.

## 12. Architectural rules

As regras desta seção orientam alterações futuras. Elas são regras arquiteturais, não uma descrição adicional de funcionalidades já implementadas:

- A API deve continuar sendo a autoridade que valida credenciais e emite JWTs.
- O Web não deve considerar o conteúdo de um JWT confiável antes de validar assinatura, issuer, audience e expiração.
- O cookie do Web não deve expor o JWT a JavaScript; `HttpOnly` deve ser preservado.
- JWTs e cookies devem ser transmitidos somente por HTTPS.
- Endpoints anônimos devem ser exceções explícitas e mínimas.
- Autorizações por role devem usar valores existentes em `RoleEnum`, preferencialmente por `nameof`, sem strings mágicas divergentes.
- A role solicitada pelo cliente ao criar ou alterar usuários deve ser validada no backend; autorização do endpoint, isoladamente, não é suficiente.
- Para usuários não-SuperAdmin, `ClinicaId` proveniente das claims deve ser a fonte de verdade para autorização e isolamento de dados.
- Operações por ID devem validar a clínica do recurso antes de retornar, atualizar ou excluir dados.
- Exceções de escopo para SuperAdmin devem ser explícitas no código e não inferidas apenas de parâmetros recebidos do cliente.
- Logout deve continuar sendo uma operação POST protegida contra CSRF e deve remover o ticket que contém o `access_token`.
- Segredos de assinatura e chaves de Data Protection não devem ser incorporados ao código-fonte ou a configurações versionadas de ambientes reais.
- Alterações futuras nas grafias de roles devem incluir uma migração coordenada de dados, claims, contratos e verificações de autorização.

## 13. Related documentation

- [`documentos/architecture.md`](architecture.md): projetos, dependências, fluxo de requests e regras arquiteturais gerais.
- [`documentos/Especificação do Projeto.md`](Especificação%20do%20Projeto.md): requisitos funcionais e comportamento pretendido do produto. Se houver conflito técnico, o código atual é a fonte de verdade para a implementação observada.
