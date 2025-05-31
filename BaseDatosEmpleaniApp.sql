CREATE DATABASE EmplaniappBD;
USE EmplaniappBD;

-- Tabla Estado
CREATE TABLE Estado (
    idEstado INT PRIMARY KEY NOT NULL,
    nombreEstado VARCHAR(100) NOT NULL
);

-- Tabla Provincia
CREATE TABLE Provincia (
    idProvincia INT PRIMARY KEY NOT NULL,
    nombreProvincia VARCHAR(100) NOT NULL,
);

-- Tabla Canton
CREATE TABLE Canton (
    idCanton INT PRIMARY KEY NOT NULL,
    nombreCanton VARCHAR(100) NOT NULL,
    idProvincia INT NOT NULL,
    FOREIGN KEY (idProvincia) REFERENCES Provincia(idProvincia)
);

-- Tabla Distrito
CREATE TABLE Distrito (
    idDistrito INT PRIMARY KEY NOT NULL,
    nombreDistrito VARCHAR(100) NOT NULL,
    idCanton INT NOT NULL,
    FOREIGN KEY (idCanton) REFERENCES Canton(idCanton)
);

-- Tabla Calle
CREATE TABLE Calle (
    idCalle INT PRIMARY KEY NOT NULL,
    nombreCalle VARCHAR(100) NOT NULL,
    idDistrito INT NOT NULL,
    FOREIGN KEY (idDistrito) REFERENCES Distrito(idDistrito)
);

-- Tabla Direccion
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

-- Tabla NumeroOcupacion
CREATE TABLE NumeroOcupacion (
    idNumeroOcupacion INT PRIMARY KEY NOT NULL,
    numeroOcupacion INT NOT NULL
);

-- Tabla Cargos
CREATE TABLE Cargos (
    idCargo INT PRIMARY KEY NOT NULL,
    nombreCargo VARCHAR(100) NOT NULL,
    idNumeroOcupacion INT NOT NULL
    FOREIGN KEY (idNumeroOcupacion) REFERENCES NumeroOcupacion(idNumeroOcupacion)
);

-- Tabla TipoMoneda
CREATE TABLE TipoMoneda (
    idTipoMoneda INT PRIMARY KEY NOT NULL,
    nombreMoneda VARCHAR(50) NOT NULL
);

-- Tabla Bancos
CREATE TABLE Bancos (
    idBanco INT PRIMARY KEY NOT NULL,
    nombreBanco VARCHAR(100) NOT NULL
);

-- Tabla Empleado
CREATE TABLE Empleado (
    idEmpleado INT PRIMARY KEY NOT NULL,
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

-- Tabla TipoRemuneracion
CREATE TABLE TipoRemuneracion (
    idTipoRemuneracion INT PRIMARY KEY NOT NULL,
    nombreTipoRemuneracion VARCHAR(100) NOT NULL,
    porcentajeRemuneracion int not null,
    idEstado INT NOT NULL,
    FOREIGN KEY (idEstado) REFERENCES Estado(idEstado)
);

-- Tabla Remuneracion
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

-- Tabla TipoRetenciones
CREATE TABLE TipoRetenciones (
    idTipoRetencion INT PRIMARY KEY NOT NULL,
    nombreTipoRetencio VARCHAR(100) NOT NULL,
    porcentajeRetencion int not null,
    idEstado INT NOT NULL,
    FOREIGN KEY (idEstado) REFERENCES Estado(idEstado)
);

-- Tabla Retenciones
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

-- Tabla Liquidaciones
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 12/11/2024 13:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 12/11/2024 13:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 12/11/2024 13:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 12/11/2024 13:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO

-- Tabla Observaciones
CREATE TABLE Observaciones (
    idObservaciones INT PRIMARY KEY NOT NULL,
    idEmpleado INT NOT NULL,
    observacionLiquidacion VARCHAR(255) NULL,
    fechaObservacion DATE NULL,
    idUsuario [nvarchar](128) NOT NULL, -- AUTOR DE OBSERVACION
    FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado),
    FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
);


-- Tabla Períodos de Pago
CREATE TABLE PeriodoPago (
	idPeriodoPago INT PRIMARY KEY NOT NULL,
	PeriodoPago VARCHAR(255) NOT NULL,
	aprobacion BIT NOT NULL,
	fechaAprobado DATE NULL,
	idUsuario [nvarchar](128) NOT NULL,
	registroPeriodoPago NVARCHAR(MAX) NOT NULL,
	FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
);


-- Tabla Aprobación (la lista entera)

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
	aprobacion  BIT NOT NULL,
	idUsuario [nvarchar](128) NULL,
	FOREIGN KEY (idPeriodoPago) REFERENCES PeriodoPago(idPeriodoPago),
	FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado),
	FOREIGN KEY (idRemuneracion) REFERENCES Remuneracion(idRemuneracion),
	FOREIGN KEY (idRetencion) REFERENCES Retenciones(idRetencion),
	FOREIGN KEY (idLiquidacion) REFERENCES Liquidaciones(idLiquidacion),
	FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
);


-- Ejecución tareas empleados (cálculo de pagos, aprobaciones...)
CREATE TABLE HistorialAccionesEmpleados (
	idAccionEmpleado INT PRIMARY KEY NOT NULL,
	descripcion NVARCHAR(255),
	idEmpleado INT NOT NULL, -- Al empleado que está dirigido
	idUsuario [nvarchar](128) NOT NULL,
	fecha DATE NOT NULL,
	documentoRegistro NVARCHAR(MAX) NULL,
	FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado),
	FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
);


-- Ejecución tareas del sistema (cambio de valores anuales, cálculos automáticos...) 
CREATE TABLE HistorialAccionesSistema (
	idAccionSistema INT PRIMARY KEY NOT NULL,
	descripcion NVARCHAR(255),
	idUsuario [nvarchar](128) NOT NULL,
	fecha DATE NOT NULL,
	documentoRegistro NVARCHAR(MAX) NULL,
	FOREIGN KEY (idUsuario) REFERENCES AspNetUsers(Id)
);

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 12/11/2024 13:29:30 ******/

select * from AspNetUsers;