# FIAP-Cloud-Games

Tech Challenge (FIAP Cloud Games)

### Autor
**Gabriel Barros Freire** - gabrbfreire@gmail.com

## Resumo:

- Autenticação e Autorização: Controle de autenticação utilizando AspNetCore Identity.
- Bando de Dados: Utilizando PosgreSQL e migrations para criação das tabelas. Acesso ao banco de dados através do EntityFramework.
- Testes unitários: Utilizando Moq e xUnit
- Documentação: Utilizando Swagger
- Containerização: Utilizando docker-compose para inicialização do banco de dados
- Testes unitários: Utilizando Moq e xUnit

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
