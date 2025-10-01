create table clientes (
    id uniqueidentifier primary key,
    nome nvarchar(200) not null,
    data_nascimento date not null,
    cpf varchar(11) not null unique,
    email nvarchar(200) not null,
    rendimento_anual decimal(14,2) not null,
    logradouro nvarchar(200) null,
    numero nvarchar(20) null,
    complemento nvarchar(100) null,
    bairro nvarchar(100) null,
    cidade nvarchar(100) null,
    estado char(2) not null,
    cep char(8) null,
    telefone_ddd char(2) not null,
    telefone_numero varchar(9) not null
);

create index ix_clientes_nome on clientes (nome);