/* ============================================================
   Lab 13 - API en capas con DTOs y AutoMapper
   Tema: Clínica Veterinaria (Propietario 1 : N Mascota)
   Motor: SQL Server (LocalDB, Express, Developer o Docker)
   Ejecutar completo: se puede re-ejecutar para reiniciar los datos.
   ============================================================ */
USE master;
GO
IF DB_ID(N'Veterinaria') IS NULL
    CREATE DATABASE Veterinaria;
GO
USE Veterinaria;
GO

IF OBJECT_ID(N'dbo.Mascota', N'U') IS NOT NULL DROP TABLE dbo.Mascota;
IF OBJECT_ID(N'dbo.Propietario', N'U') IS NOT NULL DROP TABLE dbo.Propietario;
GO

CREATE TABLE dbo.Propietario (
    PropietarioId  INT IDENTITY(1,1) NOT NULL,
    Dni            NVARCHAR(8)   NOT NULL,
    Nombres        NVARCHAR(80)  NOT NULL,
    Apellidos      NVARCHAR(80)  NOT NULL,
    Telefono       NVARCHAR(15)  NULL,
    Email          NVARCHAR(100) NULL,
    FechaRegistro  DATETIME2     NOT NULL CONSTRAINT DF_Propietario_FechaRegistro DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_Propietario PRIMARY KEY (PropietarioId),
    CONSTRAINT UQ_Propietario_Dni UNIQUE (Dni)
);
GO

CREATE TABLE dbo.Mascota (
    MascotaId        INT IDENTITY(1,1) NOT NULL,
    PropietarioId    INT           NOT NULL,
    Nombre           NVARCHAR(60)  NOT NULL,
    Especie          NVARCHAR(30)  NOT NULL,
    Raza             NVARCHAR(60)  NULL,
    FechaNacimiento  DATE          NOT NULL,
    PesoKg           DECIMAL(6,2)  NOT NULL,
    Microchip        NVARCHAR(15)  NULL,
    CONSTRAINT PK_Mascota PRIMARY KEY (MascotaId),
    CONSTRAINT FK_Mascota_Propietario FOREIGN KEY (PropietarioId) REFERENCES dbo.Propietario (PropietarioId)
);
GO

-- Microchip único solo cuando tiene valor (varias mascotas pueden no tenerlo)
CREATE UNIQUE INDEX UQ_Mascota_Microchip ON dbo.Mascota (Microchip) WHERE Microchip IS NOT NULL;
GO

/* ---------------- Datos de ejemplo ---------------- */
INSERT INTO dbo.Propietario (Dni, Nombres, Apellidos, Telefono, Email) VALUES
 (N'45879632', N'Carlos', N'Ramírez Soto',     N'987654321', N'carlos.ramirez@email.com'),  -- Id 1 (5 mascotas: límite)
 (N'70231458', N'María',  N'Torres Vega',      N'912345678', N'maria.torres@email.com'),    -- Id 2 (2 mascotas)
 (N'41236587', N'Luis',   N'Mendoza Paredes',  N'956781234', N'luis.mendoza@email.com'),    -- Id 3 (1 mascota)
 (N'72589631', N'Ana',    N'Quispe Huamán',    N'934567812', N'ana.quispe@email.com');      -- Id 4 (sin mascotas)
GO

INSERT INTO dbo.Mascota (PropietarioId, Nombre, Especie, Raza, FechaNacimiento, PesoKg, Microchip) VALUES
 (1, N'Firulais', N'Perro',  N'Labrador', '2020-03-15', 28.50, N'985112345678901'),
 (1, N'Rocky',    N'Perro',  N'Bulldog',  '2019-07-22', 22.00, N'985112345678902'),
 (1, N'Michi',    N'Gato',   N'Siamés',   '2021-01-10',  4.20, NULL),
 (1, N'Luna',     N'Gato',   N'Persa',    '2022-05-30',  3.80, NULL),
 (1, N'Pelusa',   N'Conejo', N'Enano',    '2023-02-14',  1.50, NULL),
 (2, N'Toby',     N'Perro',  N'Beagle',   '2018-11-05', 12.30, N'985112345678903'),
 (2, N'Nala',     N'Gato',   N'Mestizo',  '2020-09-18',  4.00, NULL),
 (3, N'Coco',     N'Ave',    N'Loro',     '2017-06-01',  0.45, NULL);
GO

SELECT * FROM dbo.Propietario;
SELECT * FROM dbo.Mascota;
GO
