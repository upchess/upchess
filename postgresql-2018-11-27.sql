CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "Mesas" (
    "MesaId" integer NOT NULL,
    "Nome" text NULL,
    "Inicio" timestamp without time zone NOT NULL,
    "UltimoLance" timestamp without time zone NOT NULL,
    "Historico" text NULL,
    "Configuracao" text NULL,
    "Estado" text NULL,
    CONSTRAINT "PK_Mesas" PRIMARY KEY ("MesaId")
);

CREATE TABLE "Usuario" (
    "UsuarioId" integer NOT NULL,
    "Nome" varchar(30) NOT NULL,
    "Senha" text NULL,
    "Email" text NULL,
    "DataNascimento" timestamp without time zone NOT NULL,
    "Cpf" text NULL,
    "Cep" text NULL,
    "Logradouro" text NULL,
    "Numero" text NULL,
    "Complemento" text NULL,
    "Bairro" text NULL,
    "Cidade" text NULL,
    "Uf" text NULL,
    "Tipo" integer NOT NULL,
    CONSTRAINT "PK_Usuario" PRIMARY KEY ("UsuarioId")
);

CREATE TABLE "MesasUsuarios" (
    "MesaId" integer NOT NULL,
    "UsuarioId" integer NOT NULL,
    CONSTRAINT "PK_MesasUsuarios" PRIMARY KEY ("MesaId", "UsuarioId"),
    CONSTRAINT "FK_MesasUsuarios_Mesas_MesaId" FOREIGN KEY ("MesaId") REFERENCES "Mesas" ("MesaId") ON DELETE CASCADE,
    CONSTRAINT "FK_MesasUsuarios_Usuario_UsuarioId" FOREIGN KEY ("UsuarioId") REFERENCES "Usuario" ("UsuarioId") ON DELETE CASCADE
);

CREATE INDEX "IX_MesasUsuarios_UsuarioId" ON "MesasUsuarios" ("UsuarioId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20181127193639_Initial', '2.1.4-rtm-31024');

