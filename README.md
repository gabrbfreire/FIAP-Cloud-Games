# FIAP-Cloud-Games

Tech Challenge (FIAP Cloud Games)

### Autor
**Gabriel Barros Freire** - gabrbfreire@gmail.com

## Resumo:

A aplicação **FIAP Cloud Games** é uma API desenvolvida em **.NET 8** com autenticação e autorização baseada em **ASP.NET Core Identity**, persistência de dados em **PostgreSQL** e implantação automatizada no **Microsoft Azure**.

### Principais Recursos

- **Autenticação e Autorização:** Implementadas com `AspNetCore Identity` e JWT.  
- **Banco de Dados:**  
  - PostgreSQL hospedado no **Azure Database for PostgreSQL – Flexible Server** (`fiapcloudgames-db-2`).  
  - Acesso via `Entity Framework Core` e `Dapper`.  
- **Documentação:**  
  - Gerada automaticamente com **Swagger**.  
- **Testes Unitários:**  
  - Feitos com **xUnit** e **Moq**.  
- **Containerização:**  
  - Utilização de **Docker** e **docker-compose** para ambiente local.  
- **Monitoramento:**  
  - Implementado via **Azure Application Insights** para telemetria, logs e métricas.  
- **Deploy na Nuvem:**  
  - Publicado no **Azure App Service** (`fiapcloudgamesappfg456`), sob o plano de serviço **`fiapcloudgames-plan`**.  
- **Container Registry:**  
  - As imagens Docker são armazenadas e versionadas no **Azure Container Registry** (`fcgregistrygabrielfg`).  

## Estrutura do projeto:

	FiapCloudGames.sln
	├── docker-compose.yml
	├── FiapCloudGames.API/
	│   ├── Configuration/
	│   ├── Controllers/
	│   ├── DTOs/
	│   ├── Logs/
	│   └── Middlewares/
	│
	├── FiapCloudGames.Core/
	│   ├── Entities/
	│   ├── Enums/
	│   ├── Interfaces/
	│   └── Services/
	│
	├── FiapCloudGames.Infra/
	│   ├── Data/
	│   ├── Migrations/
	│   └── Repositories/
	│
	└── FiapCloudGames.Test/
	    └── Services/

## Como Executar o Projeto
### Pré-requisitos:
	
	Git
	.NET SDK 8.0 ou superior
	Docker e docker-compose
	
### Comandos:
- Faça o pull do projeto:
```bash
git pull https://github.com/gabrbfreire/FIAP-Cloud-Games
```
- Navegue para a raiz do projeto e rode:
```bash
docker-compose up -d
```
- Navegue para a pasta src\FiapCloudGames.API	
```bash
dotnet run
```
Apos inicializado o projeto ira gerar as tabelas através de migrations e criar o usuário admin inicial


### Dados usuário admin:
	"email": "admin@gmail.com",
	"password": "Admin@123"

## Implantação no Azure

A aplicação está hospedada e monitorada integralmente na plataforma **Azure**, utilizando os seguintes recursos:

| Recurso | Tipo | Região | Descrição |
|----------|------|--------|------------|
| `fcgregistrygabrielfg` | **Azure Container Registry** | Central US | Armazena as imagens Docker do projeto |
| `fiapcloudgames-db-2` | **Azure Database for PostgreSQL – Flexible Server** | Central US | Banco de dados relacional da aplicação |
| `fiapcloudgames-plan` | **App Service Plan** | Central US | Plano de hospedagem que define capacidade e custo do App Service |
| `fiapcloudgamesappfg456` | **Azure App Service** | Central US | Hospeda a API .NET 8 containerizada |
| `fiapcloudgamesappfg456 (Application Insights)` | **Application Insights** | Central US | Monitora logs, exceções e métricas da aplicação |

---
