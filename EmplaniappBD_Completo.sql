-- =====================================================================================
-- SCRIPT COMPLETO PARA BASE DE DATOS EMPLANIAPP
-- Versión: Final - Funcional al 100%
-- Descripción: Script consolidado que crea la base de datos completa desde cero
-- =====================================================================================

-- Crear la base de datos
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

-- Tabla Estado
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Estado]') AND type in (N'U'))
BEGIN
    CREATE TABLE Estado (
        idEstado INT PRIMARY KEY NOT NULL,
        nombreEstado VARCHAR(100) NOT NULL
    );
    PRINT 'Tabla Estado creada';
END

-- Tabla Provincia
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Provincia]') AND type in (N'U'))
BEGIN
    CREATE TABLE Provincia (
        idProvincia INT PRIMARY KEY NOT NULL,
        nombreProvincia VARCHAR(100) NOT NULL
    );
    PRINT 'Tabla Provincia creada';
END

-- Tabla Canton
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Canton]') AND type in (N'U'))
BEGIN
    CREATE TABLE Canton (
        idCanton INT PRIMARY KEY NOT NULL,
        nombreCanton VARCHAR(100) NOT NULL,
        idProvincia INT NOT NULL,
        FOREIGN KEY (idProvincia) REFERENCES Provincia(idProvincia)
    );
    PRINT 'Tabla Canton creada';
END

-- Tabla Distrito
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Distrito]') AND type in (N'U'))
BEGIN
    CREATE TABLE Distrito (
        idDistrito INT PRIMARY KEY NOT NULL,
        nombreDistrito VARCHAR(100) NOT NULL,
        idCanton INT NOT NULL,
        FOREIGN KEY (idCanton) REFERENCES Canton(idCanton)
    );
    PRINT 'Tabla Distrito creada';
END

-- Tabla Calle
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Calle]') AND type in (N'U'))
BEGIN
    CREATE TABLE Calle (
        idCalle INT PRIMARY KEY NOT NULL,
        nombreCalle VARCHAR(100) NOT NULL,
        idDistrito INT NOT NULL,
        FOREIGN KEY (idDistrito) REFERENCES Distrito(idDistrito)
    );
    PRINT 'Tabla Calle creada';
END

-- Tabla Direccion
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Direccion]') AND type in (N'U'))
BEGIN
    CREATE TABLE Direccion (
        idDireccion INT PRIMARY KEY NOT NULL,
        idProvincia INT NOT NULL,
        idCanton INT NOT NULL,
        idDistrito INT NOT NULL,
        idCalle INT NOT NULL,
        FOREIGN KEY (idProvincia) REFERENCES Provincia(idProvincia),
        FOREIGN KEY (idCanton) REFERENCES Canton(idCanton),
        FOREIGN KEY (idDistrito) REFERENCES Distrito(idDistrito),
        FOREIGN KEY (idCalle) REFERENCES Calle(idCalle)
    );
    PRINT 'Tabla Direccion creada';
END

-- =====================================================================================
-- TABLAS DE EMPLEADOS Y CARGOS
-- =====================================================================================

-- Tabla NumeroOcupacion
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NumeroOcupacion]') AND type in (N'U'))
BEGIN
    CREATE TABLE NumeroOcupacion (
        idNumeroOcupacion INT PRIMARY KEY NOT NULL,
        numeroOcupacion INT NOT NULL
    );
    PRINT 'Tabla NumeroOcupacion creada';
END

-- Tabla Cargos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cargos]') AND type in (N'U'))
BEGIN
    CREATE TABLE Cargos (
        idCargo INT PRIMARY KEY NOT NULL,
        nombreCargo VARCHAR(100) NOT NULL,
        idNumeroOcupacion INT NOT NULL,
        FOREIGN KEY (idNumeroOcupacion) REFERENCES NumeroOcupacion(idNumeroOcupacion)
    );
    PRINT 'Tabla Cargos creada';
END

-- =====================================================================================
-- TABLAS FINANCIERAS
-- =====================================================================================

-- Tabla TipoMoneda (con IDENTITY)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoMoneda]') AND type in (N'U'))
BEGIN
    CREATE TABLE TipoMoneda (
        idTipoMoneda INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        nombreMoneda VARCHAR(50) NOT NULL
    );
    PRINT 'Tabla TipoMoneda creada';
END

-- Tabla Bancos (con IDENTITY)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bancos]') AND type in (N'U'))
BEGIN
    CREATE TABLE Bancos (
        idBanco INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        nombreBanco VARCHAR(100) NOT NULL
    );
    PRINT 'Tabla Bancos creada';
END

-- =====================================================================================
-- TABLA EMPLEADO (CON IDENTITY CONFIGURADO CORRECTAMENTE)
-- =====================================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Empleado]') AND type in (N'U'))
BEGIN
    CREATE TABLE Empleado (
        idEmpleado INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        nombre VARCHAR(100) NOT NULL,
        segundoNombre VARCHAR(100) NOT NULL,
        primerApellido VARCHAR(100) NOT NULL,
        segundoApellido VARCHAR(100) NOT NULL,
        fechaNacimiento DATE NOT NULL,
        cedula INT NOT NULL CHECK (cedula BETWEEN 100000000 AND 999999999),
        numeroTelefonico VARCHAR(50) NOT NULL,
        correoInstitucional VARCHAR(100) NOT NULL,
        idDireccion INT NOT NULL,
        idCargo INT NOT NULL,
        fechaContratacion DATE NOT NULL,
        fechaSalida DATE NULL,
        periocidadPago VARCHAR(50) NOT NULL,
        salarioDiario DECIMAL(12,2) NOT NULL,
        salarioAprobado DECIMAL(12,2) NOT NULL,
        salarioPorMinuto DECIMAL(12,2) NOT NULL,
        salarioPoHora DECIMAL(12,2) NOT NULL,
        salarioPorHoraExtra DECIMAL(12,2) NOT NULL,
        idTipoMoneda INT NOT NULL,
        cuentaIBAN VARCHAR(100) NOT NULL,
        idBanco INT NOT NULL,
        idEstado INT NOT NULL,
        FOREIGN KEY (idDireccion) REFERENCES Direccion(idDireccion),
        FOREIGN KEY (idCargo) REFERENCES Cargos(idCargo),
        FOREIGN KEY (idTipoMoneda) REFERENCES TipoMoneda(idTipoMoneda),
        FOREIGN KEY (idBanco) REFERENCES Bancos(idBanco),
        FOREIGN KEY (idEstado) REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla Empleado creada con idEmpleado IDENTITY';
END

-- =====================================================================================
-- TABLAS DE REMUNERACIONES Y RETENCIONES
-- =====================================================================================

-- Tabla TipoRemuneracion
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoRemuneracion]') AND type in (N'U'))
BEGIN
    CREATE TABLE TipoRemuneracion (
        idTipoRemuneracion INT PRIMARY KEY NOT NULL,
        nombreTipoRemuneracion VARCHAR(100) NOT NULL,
        porcentajeRemuneracion INT NOT NULL,
        idEstado INT NOT NULL,
        FOREIGN KEY (idEstado) REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla TipoRemuneracion creada';
END

-- Tabla Remuneracion
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Remuneracion]') AND type in (N'U'))
BEGIN
    CREATE TABLE Remuneracion (
        idRemuneracion INT PRIMARY KEY NOT NULL,
        idEmpleado INT NOT NULL,
        idTipoRemuneracion INT NOT NULL,
        fechaRemuneracion DATE NOT NULL,
        horasTrabajadas INT NULL,
        horasExtras INT NULL,
        comision DECIMAL(12,2) NULL,
        pagoQuincenal DECIMAL(12,2) NULL,
        horasFeriados DECIMAL(12,2) NULL,
        horasVacaciones DECIMAL(12,2) NULL,
        horasLicencias DECIMAL(12,2) NULL,
        idEstado INT NOT NULL,
        FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado),
        FOREIGN KEY (idTipoRemuneracion) REFERENCES TipoRemuneracion(idTipoRemuneracion),
        FOREIGN KEY (idEstado) REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla Remuneracion creada';
END

-- Tabla TipoRetenciones
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoRetenciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE TipoRetenciones (
        idTipoRetencion INT PRIMARY KEY NOT NULL,
        nombreTipoRetencio VARCHAR(100) NOT NULL,
        porcentajeRetencion INT NOT NULL,
        idEstado INT NOT NULL,
        FOREIGN KEY (idEstado) REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla TipoRetenciones creada';
END

-- Tabla Retenciones
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Retenciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE Retenciones (
        idRetencion INT PRIMARY KEY NOT NULL,
        idEmpleado INT NOT NULL,
        idTipoRetencion INT NOT NULL,
        rebajo DECIMAL(12,2) NOT NULL,
        fechaRetencion DATE NOT NULL,
        idEstado INT NOT NULL,
        FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado),
        FOREIGN KEY (idTipoRetencion) REFERENCES TipoRetenciones(idTipoRetencion),
        FOREIGN KEY (idEstado) REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla Retenciones creada';
END

-- Tabla Liquidaciones
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Liquidaciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE Liquidaciones (
        idLiquidacion INT PRIMARY KEY NOT NULL,
        idEmpleado INT NOT NULL,
        costoLiquidacion DECIMAL(12,2) NULL,
        motivoLiquidacion VARCHAR(255) NULL,
        observacionLiquidacion VARCHAR(255) NULL,
        fechaLiquidacion DATE NULL,
        idEstado INT NOT NULL,
        FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado),
        FOREIGN KEY (idEstado) REFERENCES Estado(idEstado)
    );
    PRINT 'Tabla Liquidaciones creada';
END

-- =====================================================================================
-- TABLAS ASP.NET IDENTITY
-- =====================================================================================

-- Tabla AspNetRoles
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetRoles](
        [Id] [nvarchar](128) NOT NULL,
        [Name] [nvarchar](256) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT 'Tabla AspNetRoles creada';
END

-- Tabla AspNetUsers
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUsers](
        [Id] [nvarchar](128) NOT NULL,
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
        [UserName] [nvarchar](256) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT 'Tabla AspNetUsers creada';
END

-- Tabla AspNetUserClaims
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserClaims](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [UserId] [nvarchar](128) NOT NULL,
        [ClaimType] [nvarchar](max) NULL,
        [ClaimValue] [nvarchar](max) NULL,
        CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    PRINT 'Tabla AspNetUserClaims creada';
END

-- Tabla AspNetUserLogins
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserLogins](
        [LoginProvider] [nvarchar](128) NOT NULL,
        [ProviderKey] [nvarchar](128) NOT NULL,
        [UserId] [nvarchar](128) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
        CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    PRINT 'Tabla AspNetUserLogins creada';
END

-- Tabla AspNetUserRoles
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserRoles](
        [UserId] [nvarchar](128) NOT NULL,
        [RoleId] [nvarchar](128) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
        CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    PRINT 'Tabla AspNetUserRoles creada';
END

-- =====================================================================================
-- TABLAS ADICIONALES DEL SISTEMA
-- =====================================================================================

-- Tabla Observaciones
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Observaciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE Observaciones (
        idObservaciones INT PRIMARY KEY NOT NULL,
        idEmpleado INT NOT NULL,
        observacionLiquidacion VARCHAR(255) NULL,
        fechaObservacion DATE NULL,
        idUsuario [nvarchar](128) NOT NULL,
        FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado),
        FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
    );
    PRINT 'Tabla Observaciones creada';
END

-- Tabla PeriodoPago
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PeriodoPago]') AND type in (N'U'))
BEGIN
    CREATE TABLE PeriodoPago (
        idPeriodoPago INT PRIMARY KEY NOT NULL,
        PeriodoPago VARCHAR(255) NOT NULL,
        aprobacion BIT NOT NULL,
        fechaAprobado DATE NULL,
        idUsuario [nvarchar](128) NOT NULL,
        registroPeriodoPago NVARCHAR(MAX) NOT NULL,
        FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
    );
    PRINT 'Tabla PeriodoPago creada';
END

-- Tabla PagoQuincenal
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagoQuincenal]') AND type in (N'U'))
BEGIN
    CREATE TABLE PagoQuincenal (
        idPagoQuincenal INT PRIMARY KEY NOT NULL,
        fechaInicio DATE NOT NULL,
        fechaFin DATE NOT NULL,
        idPeriodoPago INT NOT NULL,
        idEmpleado INT NOT NULL,
        idRemuneracion INT NOT NULL,
        idRetencion INT NOT NULL,
        salarioNeto DECIMAL(12,2) NOT NULL,
        idLiquidacion INT NULL,
        total DECIMAL(12,2) NOT NULL,
        aprobacion BIT NOT NULL,
        idUsuario [nvarchar](128) NULL,
        FOREIGN KEY (idPeriodoPago) REFERENCES PeriodoPago(idPeriodoPago),
        FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado),
        FOREIGN KEY (idRemuneracion) REFERENCES Remuneracion(idRemuneracion),
        FOREIGN KEY (idRetencion) REFERENCES Retenciones(idRetencion),
        FOREIGN KEY (idLiquidacion) REFERENCES Liquidaciones(idLiquidacion),
        FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
    );
    PRINT 'Tabla PagoQuincenal creada';
END

-- Tabla HistorialAccionesEmpleados
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HistorialAccionesEmpleados]') AND type in (N'U'))
BEGIN
    CREATE TABLE HistorialAccionesEmpleados (
        idAccionEmpleado INT PRIMARY KEY NOT NULL,
        descripcion NVARCHAR(255),
        idEmpleado INT NOT NULL,
        idUsuario [nvarchar](128) NOT NULL,
        fecha DATE NOT NULL,
        documentoRegistro NVARCHAR(MAX) NULL,
        FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado),
        FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
    );
    PRINT 'Tabla HistorialAccionesEmpleados creada';
END

-- Tabla HistorialAccionesSistema
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HistorialAccionesSistema]') AND type in (N'U'))
BEGIN
    CREATE TABLE HistorialAccionesSistema (
        idAccionSistema INT PRIMARY KEY NOT NULL,
        descripcion NVARCHAR(255),
        idUsuario [nvarchar](128) NOT NULL,
        fecha DATE NOT NULL,
        documentoRegistro NVARCHAR(MAX) NULL,
        FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
    );
    PRINT 'Tabla HistorialAccionesSistema creada';
END

PRINT '=== TODAS LAS TABLAS CREADAS EXITOSAMENTE ===';

-- =====================================================================================
-- INSERCIÓN DE DATOS MAESTROS
-- =====================================================================================

PRINT '=== INICIANDO INSERCIÓN DE DATOS MAESTROS ===';

-- Insertar Estados
IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 1)
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (1, 'Activo');

IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 2)
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (2, 'Inactivo');

IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 3)
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (3, 'En Licencia');

PRINT 'Estados insertados';

-- Insertar NumeroOcupacion básicos
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 1)
    INSERT INTO NumeroOcupacion (idNumeroOcupacion, numeroOcupacion) VALUES (1, 1000);

IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 2)
    INSERT INTO NumeroOcupacion (idNumeroOcupacion, numeroOcupacion) VALUES (2, 2000);

IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 3)
    INSERT INTO NumeroOcupacion (idNumeroOcupacion, numeroOcupacion) VALUES (3, 3000);

PRINT 'NumeroOcupacion insertados';

-- Insertar Cargos básicos
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 1)
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (1, 'Administrador', 1);

IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 2)
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (2, 'Contador', 2);

IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 3)
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (3, 'Gerente', 3);

IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 4)
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (4, 'Asistente', 1);

IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 5)
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (5, 'Desarrollador', 2);

IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 6)
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (6, 'Analista', 2);

IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 7)
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (7, 'Supervisor', 3);

IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 8)
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (8, 'Coordinador', 3);

PRINT 'Cargos insertados';

-- Insertar datos geográficos básicos
IF NOT EXISTS (SELECT 1 FROM Provincia WHERE idProvincia = 1)
    INSERT INTO Provincia (idProvincia, nombreProvincia) VALUES (1, 'San José');

IF NOT EXISTS (SELECT 1 FROM Canton WHERE idCanton = 1)
    INSERT INTO Canton (idCanton, nombreCanton, idProvincia) VALUES (1, 'San José', 1);

IF NOT EXISTS (SELECT 1 FROM Distrito WHERE idDistrito = 1)
    INSERT INTO Distrito (idDistrito, nombreDistrito, idCanton) VALUES (1, 'Carmen', 1);

IF NOT EXISTS (SELECT 1 FROM Calle WHERE idCalle = 1)
    INSERT INTO Calle (idCalle, nombreCalle, idDistrito) VALUES (1, 'Avenida Central', 1);

IF NOT EXISTS (SELECT 1 FROM Direccion WHERE idDireccion = 1)
    INSERT INTO Direccion (idDireccion, idProvincia, idCanton, idDistrito, idCalle) 
    VALUES (1, 1, 1, 1, 1);

PRINT 'Datos geográficos básicos insertados';

-- Insertar tipos de moneda
IF NOT EXISTS (SELECT 1 FROM TipoMoneda WHERE nombreMoneda = 'Colones')
    INSERT INTO TipoMoneda (nombreMoneda) VALUES ('Colones');

IF NOT EXISTS (SELECT 1 FROM TipoMoneda WHERE nombreMoneda = 'Dólares')
    INSERT INTO TipoMoneda (nombreMoneda) VALUES ('Dólares');

PRINT 'Tipos de moneda insertados';

-- Insertar bancos
IF NOT EXISTS (SELECT 1 FROM Bancos WHERE nombreBanco = 'Banco Nacional')
    INSERT INTO Bancos (nombreBanco) VALUES ('Banco Nacional');

IF NOT EXISTS (SELECT 1 FROM Bancos WHERE nombreBanco = 'Banco de Costa Rica')
    INSERT INTO Bancos (nombreBanco) VALUES ('Banco de Costa Rica');

IF NOT EXISTS (SELECT 1 FROM Bancos WHERE nombreBanco = 'BAC San José')
    INSERT INTO Bancos (nombreBanco) VALUES ('BAC San José');

PRINT 'Bancos insertados';

-- =====================================================================================
-- VERIFICACIÓN FINAL
-- =====================================================================================

PRINT '=== VERIFICACIÓN DE DATOS INSERTADOS ===';

SELECT 'Estados' as Tabla, COUNT(*) as Total FROM Estado;
SELECT 'Cargos' as Tabla, COUNT(*) as Total FROM Cargos;
SELECT 'Direcciones' as Tabla, COUNT(*) as Total FROM Direccion;
SELECT 'TipoMoneda' as Tabla, COUNT(*) as Total FROM TipoMoneda;
SELECT 'Bancos' as Tabla, COUNT(*) as Total FROM Bancos;

PRINT '=== BASE DE DATOS EMPLANIAPPBD CONFIGURADA EXITOSAMENTE ===';
PRINT 'La base de datos está lista para usarse con el proyecto EmplaniApp';
PRINT 'Tabla Empleado configurada con idEmpleado IDENTITY(1,1)';
PRINT 'Todos los datos maestros han sido insertados';

-- Mostrar información útil
SELECT 'INFORMACIÓN IMPORTANTE' as Mensaje, 'La tabla Empleado tiene idEmpleado como IDENTITY - NO asignar ID manualmente' as Detalle
UNION ALL
SELECT 'DATOS DISPONIBLES', CAST(COUNT(*) as VARCHAR) + ' cargos disponibles para asignar empleados' FROM Cargos
UNION ALL
SELECT 'MONEDAS', CAST(COUNT(*) as VARCHAR) + ' tipos de moneda configurados' FROM TipoMoneda
UNION ALL
SELECT 'BANCOS', CAST(COUNT(*) as VARCHAR) + ' bancos configurados' FROM Bancos;

GO 