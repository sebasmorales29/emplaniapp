-- Script para crear tablas financieras necesarias para EmplaniApp
USE EmplaniappBD;

-- Verificar si existe tabla TipoMoneda
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoMoneda]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[TipoMoneda](
        [idTipoMoneda] [int] IDENTITY(1,1) NOT NULL,
        [nombreMoneda] [varchar](50) NOT NULL,
        CONSTRAINT [PK_TipoMoneda] PRIMARY KEY CLUSTERED ([idTipoMoneda] ASC)
    )
    PRINT 'Tabla TipoMoneda creada';
END

-- Insertar tipos de moneda si no existen
IF NOT EXISTS (SELECT 1 FROM TipoMoneda WHERE idTipoMoneda = 1)
    INSERT INTO TipoMoneda (nombreMoneda) VALUES ('Colones');

IF NOT EXISTS (SELECT 1 FROM TipoMoneda WHERE idTipoMoneda = 2)
    INSERT INTO TipoMoneda (nombreMoneda) VALUES ('Dólares');

-- Verificar si existe tabla Bancos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bancos]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Bancos](
        [idBanco] [int] IDENTITY(1,1) NOT NULL,
        [nombreBanco] [varchar](100) NOT NULL,
        CONSTRAINT [PK_Bancos] PRIMARY KEY CLUSTERED ([idBanco] ASC)
    )
    PRINT 'Tabla Bancos creada';
END

-- Insertar bancos si no existen
IF NOT EXISTS (SELECT 1 FROM Bancos WHERE idBanco = 1)
    INSERT INTO Bancos (nombreBanco) VALUES ('Banco Nacional');

IF NOT EXISTS (SELECT 1 FROM Bancos WHERE idBanco = 2)
    INSERT INTO Bancos (nombreBanco) VALUES ('Banco de Costa Rica');

IF NOT EXISTS (SELECT 1 FROM Bancos WHERE idBanco = 3)
    INSERT INTO Bancos (nombreBanco) VALUES ('BAC San José');

-- Verificar resultados
SELECT 'TipoMoneda' as Tabla, * FROM TipoMoneda;
SELECT 'Bancos' as Tabla, * FROM Bancos;

-- Verificar foreign keys en tabla Empleado
SELECT 
    OBJECT_NAME(f.parent_object_id) as TablaOrigen,
    COL_NAME(fc.parent_object_id,fc.parent_column_id) as ColumnaOrigen,
    OBJECT_NAME (f.referenced_object_id) as TablaDestino,
    COL_NAME(fc.referenced_object_id,fc.referenced_column_id) as ColumnaDestino
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Empleado'
   AND COL_NAME(fc.parent_object_id,fc.parent_column_id) IN ('idTipoMoneda', 'idBanco'); 