# Gestão de Clientes API  
API para cadastro de clientes, desenvolvida como prova técnica.  

## Sumário  
- [Descrição](#descrição)  
- [Arquitetura](#arquitetura)  
- [Tecnologias](#tecnologias)  
- [Como rodar](#como-rodar)  
- [Testes](#testes)  
- [API](#api)  
- [Cálculo de Score](#cálculo-de-score)  
- [Banco de Dados](#banco-de-dados)  

---

## Descrição  
A **Gestão de Clientes API** é uma aplicação backend para realizar **cadastro de clientes** com regras de validação de campos obrigatórios, CPF único e válido, e cálculo de **Score de Confiança**.  

O objetivo é classificar clientes como **Bons, Regulares ou Maus** de acordo com critérios de **idade** e **rendimento anual**.  

---

## Arquitetura  
- **Domain**: Entidade `Cliente` e regras de negócio.  
- **Application**: Casos de uso, validações (`FluentValidation`) e orquestração.  
- **Infrastructure**:  
  - Repositório para persistência em banco de dados (SQL puro, sem ORM).  
  - Scripts SQL para criação de tabelas e índices.  
- **Tests**: Testes de unidade com **xUnit** e **FluentAssertions**.  

---

## Tecnologias  
- .NET 8  
- SQL Server  
- Swagger  
- xUnit + FluentAssertions  

---

## Como rodar  

### Pré-requisitos  
- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- Um banco de dados relacional (SQL Server).  

### Passos  
1. Clone o repositório:  
   ```bash
   git clone https://github.com/usuario/gestao-clientes-api.git
   cd gestao-clientes-api
   ```

2. Configure a connection string no `appsettings.json`.  

3. Crie o banco de dados executando o script em `database/script.sql`.  

4. Rode a aplicação:
