PROJETO - REDE NEURONAL
=======================

Requisitos
----------

- .NET SDK instalado (versão utilizada no desenvolvimento: [8.0])
- Windows / Linux / macOS


Estrutura do projeto
--------------------

A solução contém dois projetos:

- "SARSA" - contém a implementação do SARSA.
- "Games" - contém o programa utilizado para testar/executar.


Execução através do terminal
----------------------------

# 1. Abrir um terminal na pasta raiz do projeto (onde está o ficheiro .sln)

# 2. Restaurar as dependências
dotnet restore

# 3. Entrar na pasta do projeto Games
cd Games

# 4. Executar o projeto
dotnet run


Execução através do Visual Studio
---------------------------------

Também é possível executar o projeto através do Visual Studio.

O projeto "testes" deve estar definido como projeto de inicialização.

Depois, executar através do botão "Start" ou pressionando F5.


Execução através do Visual Studio Code
--------------------------------------

Abrir a pasta raiz do projeto no Visual Studio Code.

# 1. Abrir um terminal na pasta raiz do projeto (onde está o ficheiro .sln)

# 2. Restaurar as dependências
dotnet restore

# 3. Entrar na pasta do projeto Games
cd Games

# 4. Executar o projeto
dotnet run