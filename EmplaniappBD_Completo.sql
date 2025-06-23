-- =====================================================================================
-- SCRIPT FINAL Y LIMPIO PARA BASE DE DATOS EMPLANIAPP
-- Versión: 2.0
-- Descripción: Script que crea la estructura de la BD y los datos maestros.
--              El usuario 'admin' se crea con este script.
-- =====================================================================================

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EmplaniappBD')
BEGIN
    CREATE DATABASE EmplaniappBD;
    PRINT 'Base de datos EmplaniappBD creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La base de datos EmplaniappBD ya existe';
END
GO

USE EmplaniappBD;
GO

PRINT '=== INICIANDO CREACIÓN DE TABLAS ===';

-- =====================================================================================
-- TABLAS GEOGRÁFICAS
-- =====================================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Estado]') AND type in (N'U'))
BEGIN
    CREATE TABLE Estado (
        idEstado INT PRIMARY KEY NOT NULL,
        nombreEstado VARCHAR(100) NOT NULL
    );
    PRINT 'Tabla Estado creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Provincia]') AND type in (N'U'))
BEGIN
    CREATE TABLE Provincia (
        idProvincia INT PRIMARY KEY NOT NULL,
        nombreProvincia VARCHAR(100) NOT NULL
    );
    PRINT 'Tabla Provincia creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Canton]') AND type in (N'U'))
BEGIN
    CREATE TABLE Canton (
        idCanton INT PRIMARY KEY NOT NULL,
        nombreCanton VARCHAR(100) NOT NULL,
        idProvincia INT NOT NULL FOREIGN KEY REFERENCES Provincia(idProvincia)
    );
    PRINT 'Tabla Canton creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Distrito]') AND type in (N'U'))
BEGIN
    CREATE TABLE Distrito (
        idDistrito INT PRIMARY KEY NOT NULL,
        nombreDistrito VARCHAR(100) NOT NULL,
        idCanton INT NOT NULL FOREIGN KEY REFERENCES Canton(idCanton)
    );
    PRINT 'Tabla Distrito creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Calle]') AND type in (N'U'))
BEGIN
    CREATE TABLE Calle (
        idCalle INT PRIMARY KEY NOT NULL,
        nombreCalle VARCHAR(100) NOT NULL,
        idDistrito INT NOT NULL FOREIGN KEY REFERENCES Distrito(idDistrito)
    );
    PRINT 'Tabla Calle creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Direccion]') AND type in (N'U'))
BEGIN
    CREATE TABLE Direccion (
        idDireccion INT PRIMARY KEY NOT NULL,
        idProvincia INT NOT NULL FOREIGN KEY REFERENCES Provincia(idProvincia),
        idCanton INT NOT NULL FOREIGN KEY REFERENCES Canton(idCanton),
        idDistrito INT NOT NULL FOREIGN KEY REFERENCES Distrito(idDistrito),
        idCalle INT NOT NULL FOREIGN KEY REFERENCES Calle(idCalle)
    );
    PRINT 'Tabla Direccion creada';
END

-- =====================================================================================
-- TABLAS DE EMPLEADOS Y CARGOS
-- =====================================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NumeroOcupacion]') AND type in (N'U'))
BEGIN
    CREATE TABLE NumeroOcupacion (
        idNumeroOcupacion INT PRIMARY KEY NOT NULL,
        numeroOcupacion INT NOT NULL
    );
    PRINT 'Tabla NumeroOcupacion creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cargos]') AND type in (N'U'))
BEGIN
    CREATE TABLE Cargos (
        idCargo INT PRIMARY KEY NOT NULL,
        nombreCargo VARCHAR(100) NOT NULL,
        idNumeroOcupacion INT NOT NULL FOREIGN KEY REFERENCES NumeroOcupacion(idNumeroOcupacion)
    );
    PRINT 'Tabla Cargos creada';
END

-- =====================================================================================
-- TABLAS FINANCIERAS
-- =====================================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoMoneda]') AND type in (N'U'))
BEGIN
    CREATE TABLE TipoMoneda (
        idTipoMoneda INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        nombreMoneda VARCHAR(50) NOT NULL
    );
    PRINT 'Tabla TipoMoneda creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bancos]') AND type in (N'U'))
BEGIN
    CREATE TABLE Bancos (
        idBanco INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        nombreBanco VARCHAR(100) NOT NULL
    );
    PRINT 'Tabla Bancos creada';
END

-- =====================================================================================
-- TABLAS ASP.NET IDENTITY (ESTÁNDAR)
-- =====================================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetRoles](
        [Id] [nvarchar](128) NOT NULL PRIMARY KEY,
        [Name] [nvarchar](256) NOT NULL
    );
    PRINT 'Tabla AspNetRoles creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUsers](
        [Id] [nvarchar](128) NOT NULL PRIMARY KEY,
        [Email] [nvarchar](256) NULL,
        [EmailConfirmed] [bit] NOT NULL,
        [PasswordHash] [nvarchar](max) NULL,
        [SecurityStamp] [nvarchar](max) NULL,
        [PhoneNumber] [nvarchar](max) NULL,
        [PhoneNumberConfirmed] [bit] NOT NULL,
        [TwoFactorEnabled] [bit] NOT NULL,
        [LockoutEndDateUtc] [datetime] NULL,
        [LockoutEnabled] [bit] NOT NULL,
        [AccessFailedCount] [int] NOT NULL,
        [UserName] [nvarchar](256) NOT NULL
    );
    PRINT 'Tabla AspNetUsers creada';
END

-- =====================================================================================
-- TABLA EMPLEADO (ESTRUCTURA CORREGIDA Y ALINEADA CON EL PROYECTO)
-- =====================================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Empleado]') AND type in (N'U'))
BEGIN
    CREATE TABLE Empleado (
        idEmpleado INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        nombre VARCHAR(100) NOT NULL,
        primerApellido VARCHAR(100) NOT NULL,
        segundoApellido VARCHAR(100) NOT NULL,
        fechaNacimiento DATE NOT NULL,
        cedula INT NOT NULL UNIQUE CHECK (cedula BETWEEN 100000000 AND 999999999),
        numeroTelefonico VARCHAR(50) NOT NULL,
        correoInstitucional VARCHAR(100) NOT NULL,
        idDireccion INT NOT NULL FOREIGN KEY REFERENCES Direccion(idDireccion),
        idCargo INT NOT NULL FOREIGN KEY REFERENCES Cargos(idCargo),
        fechaContratacion DATE NOT NULL,
        fechaSalida DATE NULL,
        periocidadPago VARCHAR(50) NOT NULL,
        salarioDiario DECIMAL(18,2) NOT NULL,
        salarioAprobado DECIMAL(18,2) NOT NULL,
        salarioPorMinuto DECIMAL(18,2) NOT NULL,
        salarioPoHora DECIMAL(18,2) NOT NULL,
        salarioPorHoraExtra DECIMAL(18,2) NOT NULL,
        idTipoMoneda INT NOT NULL FOREIGN KEY REFERENCES TipoMoneda(idTipoMoneda),
        cuentaIBAN VARCHAR(100) NOT NULL,
        idBanco INT NOT NULL FOREIGN KEY REFERENCES Bancos(idBanco),
        idEstado INT NOT NULL FOREIGN KEY REFERENCES Estado(idEstado),
        -- Columna CRÍTICA añadida para vincular con el usuario de login
        IdNetUser NVARCHAR(128) NULL FOREIGN KEY REFERENCES AspNetUsers(Id)
    );
    PRINT 'Tabla Empleado creada con estructura CORREGIDA';
END

-- =====================================================================================
-- TABLA OBSERVACIONES (ESTRUCTURA COMPLETAMENTE CORREGIDA)
-- =====================================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Observaciones]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[Observaciones];
    PRINT 'Tabla Observaciones obsoleta eliminada.';
END

CREATE TABLE [dbo].[Observaciones] (
    [IdObservacion]  INT            IDENTITY (1, 1) NOT NULL,
    [IdEmpleado]     INT            NOT NULL,
    [Titulo]         NVARCHAR (200) NOT NULL,
    [Descripcion]    NVARCHAR (MAX) NOT NULL,
    [FechaCreacion]  DATETIME       NOT NULL,
    [IdUsuarioCreo]  NVARCHAR (128) NOT NULL,
    [FechaEdicion]   DATETIME       NULL,
    [IdUsuarioEdito] NVARCHAR (128) NULL,
    CONSTRAINT [PK_Observaciones] PRIMARY KEY CLUSTERED ([IdObservacion] ASC),
    CONSTRAINT [FK_Observaciones_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado] ([idEmpleado]),
    CONSTRAINT [FK_Observaciones_UsuarioCreo] FOREIGN KEY ([IdUsuarioCreo]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Observaciones_UsuarioEdito] FOREIGN KEY ([IdUsuarioEdito]) REFERENCES [dbo].[AspNetUsers] ([Id])
);
PRINT 'Tabla Observaciones creada con la estructura CORRECTA para la funcionalidad actual.';
GO

-- =====================================================================================
-- OTRAS TABLAS DEL SISTEMA (SE REALIZA CORRECCIONES)
-- =====================================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoRemuneracion]') AND type in (N'U'))
BEGIN
    CREATE TABLE TipoRemuneracion (
        idTipoRemuneracion INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        nombreTipoRemuneracion VARCHAR(100) NOT NULL,
        porcentajeRemuneracion FLOAT NOT NULL,
        idEstado INT NOT NULL FOREIGN KEY REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla TipoRemuneracion creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Remuneracion]') AND type in (N'U'))
BEGIN
    CREATE TABLE Remuneracion (
        idRemuneracion INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        idEmpleado INT NOT NULL FOREIGN KEY REFERENCES Empleado(idEmpleado),
        idTipoRemuneracion INT NOT NULL FOREIGN KEY REFERENCES TipoRemuneracion(idTipoRemuneracion),
        fechaRemuneracion DATE NOT NULL,
        horasTrabajadas INT NULL,
        horasExtras INT NULL,
        comision DECIMAL(12,2) NULL,
        pagoQuincenal DECIMAL(12,2) NULL,
        horasFeriados DECIMAL(12,2) NULL,
        horasVacaciones DECIMAL(12,2) NULL,
        horasLicencias DECIMAL(12,2) NULL,
        idEstado INT NOT NULL FOREIGN KEY REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla Remuneracion creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoRetenciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE TipoRetenciones (
        idTipoRetencion INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        nombreTipoRetencio VARCHAR(100) NOT NULL,
        porcentajeRetencion FLOAT NOT NULL,
        idEstado INT NOT NULL FOREIGN KEY REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla TipoRetenciones creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Retenciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE Retenciones (
        idRetencion INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        idEmpleado INT NOT NULL FOREIGN KEY REFERENCES Empleado(idEmpleado),
        idTipoRetencion INT NOT NULL FOREIGN KEY REFERENCES TipoRetenciones(idTipoRetencion),
        rebajo DECIMAL(12,2) NOT NULL,
        fechaRetencion DATE NOT NULL,
        idEstado INT NOT NULL FOREIGN KEY REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla Retenciones creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Liquidaciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE Liquidaciones (
        idLiquidacion INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        idEmpleado INT NOT NULL FOREIGN KEY REFERENCES Empleado(idEmpleado),
        costoLiquidacion DECIMAL(12,2) NULL,
        motivoLiquidacion VARCHAR(255) NULL,
        observacionLiquidacion VARCHAR(255) NULL,
        fechaLiquidacion DATE NULL,
        idEstado INT NOT NULL FOREIGN KEY REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla Liquidaciones creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PeriodoPago]') AND type in (N'U'))
BEGIN
    CREATE TABLE PeriodoPago (
        idPeriodoPago INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        PeriodoPago VARCHAR(255) NOT NULL,
        aprobacion BIT NOT NULL,
        fechaAprobado DATE NULL,
        idUsuario [nvarchar](128) NOT NULL FOREIGN KEY REFERENCES AspNetUsers(Id),
        registroPeriodoPago NVARCHAR(MAX) NOT NULL
    );
    PRINT 'Tabla PeriodoPago creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoQuincenal]') AND type in (N'U'))
BEGIN
    CREATE TABLE PagoQuincenal (
        idPagoQuincenal INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        fechaInicio DATE NOT NULL,
        fechaFin DATE NOT NULL,
        idPeriodoPago INT NOT NULL FOREIGN KEY REFERENCES PeriodoPago(idPeriodoPago),
        idEmpleado INT NOT NULL FOREIGN KEY REFERENCES Empleado(idEmpleado),
        idRemuneracion INT NOT NULL FOREIGN KEY REFERENCES Remuneracion(idRemuneracion),
        idRetencion INT NOT NULL FOREIGN KEY REFERENCES Retenciones(idRetencion),
        salarioNeto DECIMAL(12,2) NOT NULL,
        idLiquidacion INT NULL FOREIGN KEY REFERENCES Liquidaciones(idLiquidacion),
        total DECIMAL(12,2) NOT NULL,
        aprobacion BIT NOT NULL,
        idUsuario [nvarchar](128) NULL FOREIGN KEY REFERENCES AspNetUsers(Id)
    );
    PRINT 'Tabla PagoQuincenal creada';
END

-- RESTO DE TABLAS IDENTITY
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserClaims](
        [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [UserId] [nvarchar](128) NOT NULL FOREIGN KEY REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
        [ClaimType] [nvarchar](max) NULL,
        [ClaimValue] [nvarchar](max) NULL
    );
    PRINT 'Tabla AspNetUserClaims creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserLogins](
        [LoginProvider] [nvarchar](128) NOT NULL,
        [ProviderKey] [nvarchar](128) NOT NULL,
        [UserId] [nvarchar](128) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId]),
        CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    PRINT 'Tabla AspNetUserLogins creada';
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserRoles](
        [UserId] [nvarchar](128) NOT NULL,
        [RoleId] [nvarchar](128) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId], [RoleId]),
        CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    PRINT 'Tabla AspNetUserRoles creada';
END

PRINT '=== TODAS LAS TABLAS CREADAS EXITOSAMENTE ===';
GO
-- =====================================================================================
-- INSERCIÓN DE DATOS MAESTROS
-- =====================================================================================

PRINT '=== INICIANDO INSERCIÓN DE DATOS MAESTROS ===';

-- Insertar Estados
IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 1) INSERT INTO Estado (idEstado, nombreEstado) VALUES (1, 'Activo');
IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 2) INSERT INTO Estado (idEstado, nombreEstado) VALUES (2, 'Inactivo');
IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 3) INSERT INTO Estado (idEstado, nombreEstado) VALUES (3, 'En Licencia');
PRINT 'Estados (Activo, Inactivo, En Licencia) insertados';

-- Insertar Cargos y Ocupaciones
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 1) INSERT INTO NumeroOcupacion VALUES (1, 1001);
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 1) INSERT INTO Cargos VALUES (1, 'Administrador', 1);
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 2) INSERT INTO NumeroOcupacion VALUES (2, 2001);
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 2) INSERT INTO Cargos VALUES (2, 'Contador', 2);
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 3) INSERT INTO NumeroOcupacion VALUES (3, 3001);
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 3) INSERT INTO Cargos VALUES (3, 'Gerente', 3);
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 4) INSERT INTO NumeroOcupacion VALUES (4, 4001);
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 4) INSERT INTO Cargos VALUES (4, 'Asistente', 4);
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 5) INSERT INTO NumeroOcupacion VALUES (5, 5001);
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 5) INSERT INTO Cargos VALUES (5, 'Transportista', 5);
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 6) INSERT INTO NumeroOcupacion VALUES (6, 6001);
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 6) INSERT INTO Cargos VALUES (6, 'Analista', 6);
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 7) INSERT INTO NumeroOcupacion VALUES (7, 7001);
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 7) INSERT INTO Cargos VALUES (7, 'Supervisor', 7);
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 8) INSERT INTO NumeroOcupacion VALUES (8, 8001);
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 8) INSERT INTO Cargos VALUES (8, 'Coordinador', 8);
PRINT 'Cargos y Ocupaciones insertados';

-- Insertar datos geográficos básicos
IF NOT EXISTS (SELECT 1 FROM Provincia WHERE idProvincia = 1) INSERT INTO Provincia VALUES (1, 'San José');
IF NOT EXISTS (SELECT 1 FROM Canton WHERE idCanton = 1) INSERT INTO Canton VALUES (1, 'San José', 1);
IF NOT EXISTS (SELECT 1 FROM Distrito WHERE idDistrito = 1) INSERT INTO Distrito VALUES (1, 'Carmen', 1);
IF NOT EXISTS (SELECT 1 FROM Calle WHERE idCalle = 1) INSERT INTO Calle VALUES (1, 'Avenida Central', 1);
IF NOT EXISTS (SELECT 1 FROM Direccion WHERE idDireccion = 1) INSERT INTO Direccion VALUES (1, 1, 1, 1, 1);
PRINT 'Datos geográficos básicos insertados';

-- Insertar tipos de moneda
IF NOT EXISTS (SELECT 1 FROM TipoMoneda WHERE nombreMoneda = 'Colones Costarricenses')
    INSERT INTO TipoMoneda (nombreMoneda) VALUES ('Colones Costarricenses');
IF NOT EXISTS (SELECT 1 FROM TipoMoneda WHERE nombreMoneda = 'Dólares Americanos')
    INSERT INTO TipoMoneda (nombreMoneda) VALUES ('Dólares Americanos');
PRINT 'Tipos de moneda insertados';

-- Insertar bancos
IF NOT EXISTS (SELECT 1 FROM Bancos WHERE nombreBanco = 'Banco Nacional de Costa Rica')
    INSERT INTO Bancos (nombreBanco) VALUES ('Banco Nacional de Costa Rica');
IF NOT EXISTS (SELECT 1 FROM Bancos WHERE nombreBanco = 'Banco de Costa Rica')
    INSERT INTO Bancos (nombreBanco) VALUES ('Banco de Costa Rica');
IF NOT EXISTS (SELECT 1 FROM Bancos WHERE nombreBanco = 'BAC Credomatic')
    INSERT INTO Bancos (nombreBanco) VALUES ('BAC Credomatic');
PRINT 'Bancos insertados';

PRINT '=== DATOS MAESTROS INSERTADOS EXITOSAMENTE ===';
GO

-- =====================================================================================
-- INSERCIÓN DE USUARIO ADMINISTRADOR Y DATOS ASOCIADOS
-- =====================================================================================
PRINT '=== INICIANDO CREACIÓN DE USUARIO Y ROL DE ADMINISTRADOR ===';
GO

-- Suprimir la salida de recuento de filas afectadas para que los PRINT sean más claros
SET NOCOUNT ON;

-- Declarar variables para los IDs para asegurar consistencia
DECLARE @AdminRoleId NVARCHAR(128) = '1';
DECLARE @AdminUserId NVARCHAR(128); -- No lo predefinimos, lo obtendremos de la BD

-- 1. Insertar el rol de Administrador si no existe
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetRoles] WHERE [Name] = 'Administrador')
BEGIN
    -- Usamos un ID predecible para el rol para consistencia
    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name])
    VALUES (@AdminRoleId, 'Administrador');
    PRINT 'Rol "Administrador" creado.';
END
ELSE
BEGIN
    -- Si ya existe, obtenemos su ID
    SELECT @AdminRoleId = Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'Administrador';
    PRINT 'Rol "Administrador" ya existe.';
END

-- 2. Insertar el usuario 'admin' si no existe
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [UserName] = 'admin')
BEGIN
    SET @AdminUserId = NEWID(); -- Generamos un nuevo GUID para el nuevo usuario
    INSERT INTO [dbo].[AspNetUsers] 
        ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName])
    VALUES 
        (@AdminUserId, 
         'admin@emplaniapp.com', 
         1, 
         'AKYg28DrixVhlLzGa4gZfcfNvg+Q+JwMtSwIj/w9REjSKIDRtbV8m62JCVoo7OoXYQ==', -- Hash para 'Password123.' (GENERADO POR LA APP)
         NEWID(), -- Security Stamp
         NULL, 0, 0, NULL, 1, 0, 
         'admin');
    PRINT 'Usuario "admin" creado.';
END
ELSE
BEGIN
    -- Si el usuario ya existe, obtenemos su ID para usarlo después
    SELECT @AdminUserId = [Id] FROM [dbo].[AspNetUsers] WHERE [UserName] = 'admin';
    PRINT 'Usuario "admin" ya existe.';
END

-- 3. Vincular usuario 'admin' con rol 'Administrador'
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUserRoles] WHERE [UserId] = @AdminUserId AND [RoleId] = @AdminRoleId)
BEGIN
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
    VALUES (@AdminUserId, @AdminRoleId);
    PRINT 'Usuario "admin" asignado al rol "Administrador".';
END
ELSE
BEGIN
    PRINT 'Usuario "admin" ya estaba asignado al rol "Administrador".';
END

-- 4. Insertar o ACTUALIZAR el registro de Empleado para el usuario 'admin'
-- Esto asegura que el IdNetUser esté correctamente vinculado, incluso si el script se corre varias veces.
IF NOT EXISTS (SELECT 1 FROM [dbo].[Empleado] WHERE [correoInstitucional] = 'admin@emplaniapp.com')
BEGIN
    -- Se usan valores por defecto para rellenar los campos obligatorios.
    INSERT INTO [dbo].[Empleado] (
        [nombre], [primerApellido], [segundoApellido], [fechaNacimiento], [cedula], 
        [numeroTelefonico], [correoInstitucional], [idDireccion], [idCargo], [fechaContratacion], 
        [fechaSalida], [periocidadPago], [salarioDiario], [salarioAprobado], [salarioPorMinuto], 
        [salarioPoHora], [salarioPorHoraExtra], [idTipoMoneda], [cuentaIBAN], [idBanco], 
        [idEstado], [IdNetUser])
    VALUES (
        'Admin', 'User', '', '1990-01-01', 999999999,
        '00000000', 'admin@emplaniapp.com', 1, 1, GETDATE(),
        NULL, 'Quincenal', 0, 0, 0,
        0, 0, 1, 'CR00000000000000000000', 1,
        1, @AdminUserId
    );
    PRINT 'Registro de Empleado para el usuario "admin" creado y vinculado.';
END
ELSE
BEGIN
    -- Si el empleado ya existe, nos aseguramos de que el IdNetUser sea el correcto.
    UPDATE [dbo].[Empleado]
    SET [IdNetUser] = @AdminUserId
    WHERE [correoInstitucional] = 'admin@emplaniapp.com' AND [IdNetUser] IS NULL;
    PRINT 'El registro de Empleado para "admin" ya existía, se aseguró el vínculo con IdNetUser.';
END

-- Resetear NOCOUNT a su estado original
SET NOCOUNT OFF;

PRINT '=== CONFIGURACIÓN DE USUARIO "admin" COMPLETADA ===';
GO

-- Finalmente, se añade la columna opcional segundoNombre como se especificó
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'segundoNombre' AND Object_ID = Object_ID(N'dbo.Empleado'))
BEGIN
    ALTER TABLE Empleado ADD segundoNombre NVARCHAR(MAX) NULL;
    PRINT 'Columna [segundoNombre] añadida a la tabla Empleado.';
END
GO

PRINT '=== BASE DE DATOS EMPLANIAPPBDPrueba CONFIGURADA EXITOSAMENTE ===';
GO

-- Script para agregar los roles 'Contador' y 'Empleado' a la base de datos.
-- Ejecuta este script en la base de datos de Emplaniapp.

-- Se inserta el rol de Contador
-- La función NEWID() genera un identificador único para el rol.
IF NOT EXISTS (SELECT 1 FROM dbo.AspNetRoles WHERE Name = 'Contador')
BEGIN
    INSERT INTO dbo.AspNetRoles (Id, Name) VALUES (NEWID(), 'Contador');
    PRINT 'Rol "Contador" agregado exitosamente.';
END
ELSE
BEGIN
    PRINT 'El rol "Contador" ya existe.';
END
GO

-- Se inserta el rol de Empleado
IF NOT EXISTS (SELECT 1 FROM dbo.AspNetRoles WHERE Name = 'Empleado')
BEGIN
    INSERT INTO dbo.AspNetRoles (Id, Name) VALUES (NEWID(), 'Empleado');
    PRINT 'Rol "Empleado" agregado exitosamente.';
END
ELSE
BEGIN
    PRINT 'El rol "Empleado" ya existe.';
END
GO



-- Script para agregar los valores de Tipos de Remuneraciones y Retenciones

-- Tipos de remuneraciones
INSERT INTO [dbo].[TipoRemuneracion] ([nombreTipoRemuneracion], [porcentajeRemuneracion],[idEstado])
     VALUES('Horas Extra',50.0,1)
GO

INSERT INTO [dbo].[TipoRemuneracion] ([nombreTipoRemuneracion], [porcentajeRemuneracion],[idEstado])
     VALUES('Día Feriado',100.0,1)
GO

INSERT INTO [dbo].[TipoRemuneracion] ([nombreTipoRemuneracion], [porcentajeRemuneracion],[idEstado])
     VALUES('Incapacidad por Enfermedad',50.0,1)
GO

INSERT INTO [dbo].[TipoRemuneracion] ([nombreTipoRemuneracion], [porcentajeRemuneracion],[idEstado])
     VALUES('Incapacidad por Maternidad',50.0,1)
GO

INSERT INTO [dbo].[TipoRemuneracion] ([nombreTipoRemuneracion], [porcentajeRemuneracion],[idEstado])
     VALUES('Vacaciones',100.0,1)
GO


-- Tipos de retenciones

INSERT INTO [dbo].[TipoRetenciones] ([nombreTipoRetencio], [porcentajeRetencion], [idEstado])
     VALUES ('C.C.S.S.', 10.67, 1)
GO

INSERT INTO [dbo].[TipoRetenciones] ([nombreTipoRetencio], [porcentajeRetencion], [idEstado])
     VALUES ('Pensión C.C.S.S.',7, 1)
GO

INSERT INTO [dbo].[TipoRetenciones] ([nombreTipoRetencio], [porcentajeRetencion], [idEstado])
     VALUES ('Tardía', 100.0, 1)
GO

INSERT INTO [dbo].[TipoRetenciones] ([nombreTipoRetencio], [porcentajeRetencion], [idEstado])
     VALUES ('Compras Internas', 100.0, 1)
GO

INSERT INTO [dbo].[TipoRetenciones] ([nombreTipoRetencio], [porcentajeRetencion], [idEstado])
     VALUES ('Permiso Sin Goce de Salario', 100.0, 1)
GO

INSERT INTO [dbo].[TipoRetenciones] ([nombreTipoRetencio], [porcentajeRetencion], [idEstado])
     VALUES ('Ministerio de Trabajo', 5.5, 1)
GO
INSERT INTO [dbo].[Cargos]
           ([idCargo]
           ,[nombreCargo]
           ,[idNumeroOcupacion])
     VALUES
           (9
           ,'Vendedor'
           ,1)
GO
INSERT INTO TipoRemuneracion (nombreTipoRemuneracion, porcentajeRemuneracion, idEstado)
VALUES ('Pago Quincenal',0, 1);
GO
CREATE OR ALTER PROCEDURE sp_GenerarRemuneracionesQuincenales
    @FechaProceso DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Si no se proporciona fecha, usar la actual
    IF @FechaProceso IS NULL
        SET @FechaProceso = GETDATE();
    
    DECLARE @DiaDelMes INT = DAY(@FechaProceso);
    DECLARE @EsPrimeraQuincena BIT;
    DECLARE @Mes INT = MONTH(@FechaProceso);
    DECLARE @Anio INT = YEAR(@FechaProceso);
    DECLARE @idTipoRemuneracionQuincenal INT;
    
    -- Determinar si es primera o segunda quincena
    IF @DiaDelMes BETWEEN 1 AND 15
        SET @EsPrimeraQuincena = 1;
    ELSE
        SET @EsPrimeraQuincena = 0;
    
    -- Obtener el ID del tipo de remuneración quincenal
    SELECT @idTipoRemuneracionQuincenal = idTipoRemuneracion 
    FROM TipoRemuneracion 
    WHERE nombreTipoRemuneracion = 'Pago Quincenal' AND idEstado = 1;
    
    IF @idTipoRemuneracionQuincenal IS NULL
    BEGIN
        RAISERROR('No se encontró el tipo de remuneración "Pago Quincenal" activo', 16, 1);
        RETURN;
    END
    
    -- Insertar remuneraciones para empleados activos con periodicidad quincenal
    INSERT INTO Remuneracion (
        idEmpleado,
        idTipoRemuneracion,
        fechaRemuneracion,
        pagoQuincenal,
        idEstado
    )
    SELECT 
        e.idEmpleado,
        @idTipoRemuneracionQuincenal,
        @FechaProceso,
        CASE 
            -- Si es vendedor (verifica si el cargo contiene "vendedor")
            WHEN EXISTS (SELECT 1 FROM Cargos c WHERE c.idCargo = e.idCargo 
                         AND (c.nombreCargo LIKE '%vendedor%' OR c.nombreCargo LIKE '%Vendedor%')) THEN 
                CASE 
                    WHEN @EsPrimeraQuincena = 1 THEN 350000 -- Primera quincena fija para vendedores
                    ELSE 
                        -- Segunda quincena para vendedores: salario aprobado - 350000
                        CASE 
                            WHEN e.salarioAprobado > 350000 THEN e.salarioAprobado - 350000
                            ELSE 0 -- En caso de que el salario sea menor
                        END
                END
            -- Para no vendedores: 15 días * salario diario
            ELSE 15 * e.salarioDiario
        END,
        1 -- Estado activo
    FROM 
        Empleado e
    WHERE 
        e.idEstado = 1 -- Empleados activos
        AND e.periocidadPago = 'Quincenal'
        AND NOT EXISTS (
            -- Verificar que no exista ya una remuneración para este empleado en esta quincena
            SELECT 1 FROM Remuneracion r
            WHERE r.idEmpleado = e.idEmpleado
              AND r.idTipoRemuneracion = @idTipoRemuneracionQuincenal
              AND YEAR(r.fechaRemuneracion) = @Anio
              AND MONTH(r.fechaRemuneracion) = @Mes
              AND (
                  (@EsPrimeraQuincena = 1 AND DAY(r.fechaRemuneracion) BETWEEN 1 AND 15)
                  OR 
                  (@EsPrimeraQuincena = 0 AND DAY(r.fechaRemuneracion) BETWEEN 16 AND 31)
              )
        );
    
    -- Retornar las remuneraciones generadas con todos los campos necesarios
    SELECT 
        r.idRemuneracion,
        r.idEmpleado,
        e.nombre + ' ' + e.primerApellido AS nombreEmpleado,
        r.idTipoRemuneracion,
        tr.nombreTipoRemuneracion,
        r.fechaRemuneracion,
        r.horasTrabajadas,
        r.horasExtras,
        r.comision,
        r.pagoQuincenal,
        r.horasFeriados,
        r.horasVacaciones,
        r.horasLicencias,
        r.idEstado,
        est.nombreEstado,
        CASE WHEN @EsPrimeraQuincena = 1 THEN 'Primera Quincena' ELSE 'Segunda Quincena' END AS quincena
    FROM 
        Remuneracion r
        INNER JOIN Empleado e ON r.idEmpleado = e.idEmpleado
        INNER JOIN TipoRemuneracion tr ON r.idTipoRemuneracion = tr.idTipoRemuneracion
        INNER JOIN Estado est ON r.idEstado = est.idEstado
    WHERE 
        r.idTipoRemuneracion = @idTipoRemuneracionQuincenal
        AND YEAR(r.fechaRemuneracion) = @Anio
        AND MONTH(r.fechaRemuneracion) = @Mes
        AND (
            (@EsPrimeraQuincena = 1 AND DAY(r.fechaRemuneracion) BETWEEN 1 AND 15)
            OR 
            (@EsPrimeraQuincena = 0 AND DAY(r.fechaRemuneracion) BETWEEN 16 AND 31)
        );
END

