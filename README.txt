PROJETO - REDE NEURONAL
=======================

Requisitos
----------

- .NET SDK instalado (versão utilizada no desenvolvimento: [8.0])
- Windows / Linux / macOS


Estrutura do projeto
--------------------

A solução contém dois projetos:

- "Rede neuronal" - contém a implementação da rede neuronal.
- "testes" - contém o programa utilizado para testar/executar a rede neuronal.


Execução através do terminal
----------------------------

1. Abrir um terminal na pasta raiz do projeto, onde se encontra o
   ficheiro "Rede neuronal.sln".

2. Restaurar as dependências:

   dotnet restore

3. Executar o projeto de testes:

   dotnet run --project ".\testes"


Execução através do Visual Studio
---------------------------------

Também é possível executar o projeto através do Visual Studio.

O projeto "testes" deve estar definido como projeto de inicialização.

Depois, executar através do botão "Start" ou pressionando F5.


Execução através do Visual Studio Code
--------------------------------------

Abrir a pasta raiz do projeto no Visual Studio Code.

No terminal integrado, executar:

   dotnet restore

e depois:

   dotnet run --project ".\testes"


Ficheiros gerados (.csv)
------------------------

Os ficheiros .csv guardados quando o programa corre ficam na pasta de
saída do projeto "testes":

   "Rede-neuronal-main\Rede-neuronal-main\testes\bin\Debug\net8.0"

(Se o programa for executado em modo Release, a pasta será
 ...\testes\bin\Release\net8.0.)

Notas
-----

O projeto "testes" é o projeto que deve ser executado para testar
o funcionamento da rede neuronal.

Não é necessário executar diretamente o projeto "Rede neuronal".