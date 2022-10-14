USE [ws_sifen]
GO
/****** Object:  Schema [api]    Script Date: 10/14/2022 1:24:08 PM ******/
CREATE SCHEMA [api]
GO
/****** Object:  UserDefinedFunction [dbo].[f_generar_CDC]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ==========================================================================================
-- Author:		Christian Balbuena
-- Create date:	06-2022
-- Description:	Esta funcion se encarga de generar del CDC del DE
-- ==========================================================================================
CREATE Function [dbo].[f_generar_CDC]
 (
	@id_documento_electronico bigint 
 ) 
Returns Varchar(44)
As
Begin

	Declare @cadena_nueva	Varchar(44);

	
	
	select @cadena_nueva =
			dbo.f_llenar_ceros(de.tipoDocumento,2) +
			dbo.f_llenar_ceros(u.documentoNumero,8) +
			cast(dbo.f_generar_digito_verificador(u.documentoNumero,11) as varchar(1))+
			dbo.f_llenar_ceros(de.establecimiento,3)+
			dbo.f_llenar_ceros(de.punto,3) +
			dbo.f_llenar_ceros(de.numero,7)+
			cast(de.tipoContribuyente as varchar(1))+
			cast(DATEPART(yyyy,de.fecha) as varchar(4))  + dbo.f_llenar_ceros(DATEPART(mm,de.fecha),2) + dbo.f_llenar_ceros(DATEPART(dd,de.fecha),2)+
			cast(de.tipoEmision as varchar(1))+
			dbo.f_llenar_ceros(de.codigoSeguridadAleatorio,9)
	from dbo.DocumentosElectronicos de
	left outer join  dbo.Usuarios u on u.id_usuario = de.id_usuario
	where de.id_documento_electronico = @id_documento_electronico;

	set @cadena_nueva = @cadena_nueva + cast(dbo.f_generar_digito_verificador(@cadena_nueva,11) as varchar(20));

	Return @cadena_nueva
	
End
GO
/****** Object:  UserDefinedFunction [dbo].[f_generar_digito_verificador]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Function [dbo].[f_generar_digito_verificador](

	@p_numero varchar(4000),
	@p_basemax float = 11)
RETURNS int 
AS
 BEGIN
 --Inicializacion
DECLARE @v_total INT;
DECLARE @v_resto SMALLINT;
DECLARE @k SMALLINT;
DECLARE @v_numero_aux INT;
DECLARE @v_numero_al VARCHAR(255);
DECLARE @v_caracter VARCHAR(1);
DECLARE @v_digit FLOAT;
 





set @v_numero_al=@p_numero;
-- Calcula el DV
SET @k = 2;
SET @v_total = 0;



 -- Al saber que mi variable i se mantendria en 0 en innecesaria la inicializacion
DEClARE @z INT = (len(@v_numero_al))
while (0 < @z)
BEGIN
	IF @k > @p_basemax BEGIN
		SET @k = 2;
	END 
	SET @v_numero_aux = CAST(SUBSTRING(@v_numero_al,@z,1) as FLOAT);
	SET @v_total = @v_total + (@v_numero_aux * @k);
	SET @k = @k + 1;
	SET @z=@z-1
END
SET @v_resto = (@v_total %11);
IF @v_resto > 1 BEGIN
	SET @v_digit = 11 - @v_resto;
END
ELSE BEGIN
	SET @v_Digit = 0;
END 
RETURN @v_digit;
END;

GO
/****** Object:  UserDefinedFunction [dbo].[f_llenar_ceros]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:		Christian Balbuena
-- Create date:	06-2022
-- Description:	Esta funcion se encarga de completar con ceros una valor numerico de acuerdo
--				al parametro que se le pase. Se utiliza en la generacion de valores con longitud fija
-- ==========================================================================================
CREATE Function [dbo].[f_llenar_ceros]
 (
	@cadena		decimal(30,3),
	@ancho		Tinyint
 ) 
Returns Varchar(50)
As
Begin

	Declare @cadena_nueva	Varchar(50)
	declare @cadena_decimal as varchar(50)
	declare @cadena_entera as varchar(50)
	
	set @cadena_decimal = substring( cast( (cast((@cadena-floor(@cadena)) as decimal(18,2))) as varchar(5)),3,2)
	set @cadena_entera = floor(@cadena)
	
	
	Set @cadena_nueva	= ''
	
	Set @cadena_nueva = Replicate( '0', (@ancho-Len(@cadena_entera))) + Cast(@cadena_entera AS Varchar(50))--+@cadena_decimal

	Return @cadena_nueva
	
End
GO
/****** Object:  Table [dbo].[ActividadesEconmicasEmpresa]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActividadesEconmicasEmpresa](
	[id_empresa] [int] NOT NULL,
	[codigo] [int] NOT NULL,
 CONSTRAINT [PK_ActividadesEconmicasEmpresa] PRIMARY KEY CLUSTERED 
(
	[id_empresa] ASC,
	[codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActividadesEconomicas]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActividadesEconomicas](
	[codigo] [int] NOT NULL,
	[descripcion] [varchar](512) NOT NULL,
 CONSTRAINT [PK_ActividadesEconomicas] PRIMARY KEY CLUSTERED 
(
	[codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certificados]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificados](
	[id_certificado] [bigint] IDENTITY(1,1) NOT NULL,
	[fecha_emision] [datetime] NOT NULL,
	[fecha_vencimiento] [datetime] NOT NULL,
	[emisor] [varchar](128) NOT NULL,
	[data] [nvarchar](max) NULL,
	[key_public] [nvarchar](50) NULL,
	[key_private] [nvarchar](50) NULL,
 CONSTRAINT [PK_Certificados] PRIMARY KEY CLUSTERED 
(
	[id_certificado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ciudades]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ciudades](
	[ciudad] [int] NOT NULL,
	[ciudadDescripcion] [varchar](128) NULL,
 CONSTRAINT [PK_Ciudades] PRIMARY KEY CLUSTERED 
(
	[ciudad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[id_cliente] [bigint] IDENTITY(1,1) NOT NULL,
	[contribuyente] [smallint] NULL,
	[ruc] [varchar](8) NULL,
	[razonSocial] [varchar](256) NULL,
	[nombreFantasia] [varchar](256) NULL,
	[tipoOperacion] [smallint] NULL,
	[direccion] [varchar](256) NULL,
	[numeroCasa] [varchar](20) NULL,
	[departamento] [int] NULL,
	[distrito] [int] NULL,
	[ciudad] [int] NULL,
	[pais] [varchar](50) NULL,
	[tipoContribuyente] [smallint] NULL,
	[documentoTipo] [smallint] NULL,
	[documentoNumero] [varchar](50) NULL,
	[telefono] [varchar](50) NULL,
	[celular] [varchar](50) NULL,
	[email] [varchar](50) NULL,
	[codigo] [varchar](50) NULL,
 CONSTRAINT [PK__Clientes__677F38F560F0D276] PRIMARY KEY CLUSTERED 
(
	[id_cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CodigoSeguridad]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CodigoSeguridad](
	[cod_seguridad] [bigint] IDENTITY(1,1) NOT NULL,
	[id_documento_electronico] [bigint] NOT NULL,
	[fecha] [datetime] NOT NULL,
 CONSTRAINT [PK_CodigoSeguridad] PRIMARY KEY CLUSTERED 
(
	[cod_seguridad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departamentos]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departamentos](
	[departamento] [int] NOT NULL,
	[departamentoDescripcion] [varchar](128) NULL,
 CONSTRAINT [PK_Departamentos] PRIMARY KEY CLUSTERED 
(
	[departamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Distritos]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Distritos](
	[distrito] [int] NOT NULL,
	[distritoDescripcion] [varchar](128) NULL,
 CONSTRAINT [PK_Distritos] PRIMARY KEY CLUSTERED 
(
	[distrito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentosElectronicos]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentosElectronicos](
	[id_documento_electronico] [bigint] IDENTITY(1,1) NOT NULL,
	[tipoDocumento] [int] NOT NULL,
	[establecimiento] [varchar](50) NULL,
	[codigoSeguridadAleatorio] [varchar](128) NULL,
	[punto] [varchar](50) NULL,
	[numero] [varchar](50) NULL,
	[descripcion] [varchar](512) NULL,
	[observacion] [varchar](512) NULL,
	[tipoContribuyente] [smallint] NULL,
	[fecha] [datetime] NULL,
	[tipoEmision] [smallint] NULL,
	[tipoTransaccion] [smallint] NULL,
	[tipoImpuesto] [smallint] NULL,
	[moneda] [varchar](128) NULL,
	[condicionAnticipo] [smallint] NULL,
	[condicionTipoCambio] [smallint] NULL,
	[cambio] [numeric](18, 2) NULL,
	[id_cliente] [bigint] NULL,
	[id_usuario] [int] NULL,
	[id_certificado] [bigint] NULL,
	[data] [nvarchar](max) NULL,
	[id_estado] [smallint] NULL,
 CONSTRAINT [PK__Document__ADD6C4CF9A294200] PRIMARY KEY CLUSTERED 
(
	[id_documento_electronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresas]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresas](
	[id_empresa] [int] IDENTITY(1,1) NOT NULL,
	[version] [numeric](18, 2) NULL,
	[fechaFirmaDigital] [date] NULL,
	[ruc] [varchar](50) NULL,
	[razonSocial] [varchar](128) NULL,
	[nombreFantasia] [varchar](256) NULL,
	[timbradoNumero] [varchar](50) NULL,
	[timbradoFecha] [date] NULL,
	[tipoContribuyente] [int] NOT NULL,
	[tipoRegimen] [int] NULL,
 CONSTRAINT [PK_Empresas] PRIMARY KEY CLUSTERED 
(
	[id_empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Establecimientos]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Establecimientos](
	[codigo] [varchar](50) NOT NULL,
	[direccion] [varchar](128) NOT NULL,
	[numeroCasa] [varchar](50) NULL,
	[complementoDireccion1] [varchar](50) NULL,
	[complementoDireccion2] [varchar](50) NULL,
	[departamento] [int] NULL,
	[distrito] [int] NULL,
	[ciudad] [int] NULL,
	[telefono] [varchar](50) NULL,
	[email] [varchar](50) NULL,
	[denominacion] [varchar](128) NULL,
 CONSTRAINT [PK_Establecimientos] PRIMARY KEY CLUSTERED 
(
	[codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstablecimientosEmpresas]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstablecimientosEmpresas](
	[id_empresa] [int] NOT NULL,
	[codigo] [varchar](50) NOT NULL,
 CONSTRAINT [PK_EstablecimientosEmpresas] PRIMARY KEY CLUSTERED 
(
	[id_empresa] ASC,
	[codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Estados]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Estados](
	[id_estado] [smallint] IDENTITY(1,1) NOT NULL,
	[estado] [varchar](128) NOT NULL,
 CONSTRAINT [PK_Estados] PRIMARY KEY CLUSTERED 
(
	[id_estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Factura]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factura](
	[id_factura] [bigint] IDENTITY(1,1) NOT NULL,
	[presencia] [smallint] NOT NULL,
	[id_documento_electronico] [bigint] NOT NULL,
	[fechaEnvio] [datetime] NOT NULL,
 CONSTRAINT [PK_Factura] PRIMARY KEY CLUSTERED 
(
	[id_factura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[id_item] [bigint] IDENTITY(1,1) NOT NULL,
	[id_documento_electronico] [bigint] NULL,
	[codigo] [varchar](50) NULL,
	[descripcion] [varchar](256) NULL,
	[observacion] [varchar](512) NULL,
	[partidaArancelaria] [int] NULL,
	[ncm] [varchar](128) NULL,
	[unidadMedida] [smallint] NULL,
	[cantidad] [numeric](18, 2) NULL,
	[precioUnitario] [numeric](18, 2) NULL,
	[cambio] [numeric](18, 2) NULL,
	[descuento] [numeric](18, 2) NULL,
	[anticipo] [numeric](18, 2) NULL,
	[pais] [varchar](50) NULL,
	[tolerancia] [smallint] NULL,
	[toleranciaCantidad] [int] NULL,
	[toleranciaPorcentaje] [numeric](18, 2) NULL,
	[cdcAnticipo] [varchar](50) NULL,
	[ivaTipo] [smallint] NULL,
	[ivaBase] [numeric](18, 2) NULL,
	[iva] [numeric](18, 2) NULL,
	[lote] [varchar](50) NULL,
	[vencimiento] [datetime] NULL,
	[numeroSerie] [varchar](128) NULL,
	[numeroPedido] [varchar](128) NULL,
	[numeroSeguimiento] [varchar](128) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_item] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Monedas]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Monedas](
	[moneda] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Monedas] PRIMARY KEY CLUSTERED 
(
	[moneda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Operaciones]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operaciones](
	[id_operacion] [bigint] IDENTITY(1,1) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[id_documento_electronico] [bigint] NULL,
	[comando] [nvarchar](50) NOT NULL,
	[response] [nvarchar](max) NULL,
	[status] [int] NULL,
	[numero] [nvarchar](50) NULL,
 CONSTRAINT [PK_Operaciones] PRIMARY KEY CLUSTERED 
(
	[id_operacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Paises]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paises](
	[pais] [varchar](50) NOT NULL,
	[paisDescripcion] [varchar](128) NOT NULL,
 CONSTRAINT [PK_Paises] PRIMARY KEY CLUSTERED 
(
	[pais] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Presencia]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Presencia](
	[presencia] [smallint] NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Presencia] PRIMARY KEY CLUSTERED 
(
	[presencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PuntosExpedicion]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PuntosExpedicion](
	[codigo] [varchar](50) NOT NULL,
	[punto] [varchar](128) NOT NULL,
 CONSTRAINT [PK_Puntos] PRIMARY KEY CLUSTERED 
(
	[codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SifenResults]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SifenResults](
	[Response] [nvarchar](max) NULL,
	[status] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoDocumentos]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoDocumentos](
	[documentoTipo] [smallint] NOT NULL,
	[Descripcion] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TipoDocumentos] PRIMARY KEY CLUSTERED 
(
	[documentoTipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TiposContribuyente]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TiposContribuyente](
	[tipo] [smallint] NOT NULL,
	[tipoContribuyenteDescripcion] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TiposContribuyente] PRIMARY KEY CLUSTERED 
(
	[tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TiposDocumentoElectronicos]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TiposDocumentoElectronicos](
	[tipoDocumento] [int] NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TiposDocumentoElectronicos] PRIMARY KEY CLUSTERED 
(
	[tipoDocumento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TiposEmision]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TiposEmision](
	[tipo] [smallint] NOT NULL,
	[tipoEmisionDescripcion] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TiposEmision] PRIMARY KEY CLUSTERED 
(
	[tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TiposImpuesto]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TiposImpuesto](
	[tipo] [smallint] NOT NULL,
	[tipoImpuestoDescripcion] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TiposImpuesto] PRIMARY KEY CLUSTERED 
(
	[tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TiposTransaccion]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TiposTransaccion](
	[tipo] [smallint] NOT NULL,
	[tipoTransaccionDescripcion] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TiposTransaccion] PRIMARY KEY CLUSTERED 
(
	[tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[id_usuario] [int] IDENTITY(1,1) NOT NULL,
	[documentoTipo] [smallint] NOT NULL,
	[documentoNumero] [varchar](50) NOT NULL,
	[nombre] [varchar](128) NOT NULL,
	[cargo] [varchar](50) NULL,
	[timbrado] [varchar](50) NULL,
	[inicio_timbrado] [datetime] NULL,
	[direccion] [varchar](128) NULL,
	[telefono] [varchar](128) NULL,
	[correo] [varchar](50) NULL,
	[CSC] [nvarchar](max) NULL,
	[observaciones] [nvarchar](max) NULL,
	[ID_CSC] [varchar](5) NULL,
	[entorno] [nvarchar](50) NULL,
	[id_certificado] [int] NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[id_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CodigoSeguridad] ADD  CONSTRAINT [DF_CodigoSeguridad_fecha]  DEFAULT (getdate()) FOR [fecha]
GO
ALTER TABLE [dbo].[Operaciones] ADD  CONSTRAINT [DF_Operaciones_fecha]  DEFAULT (getdate()) FOR [fecha]
GO
ALTER TABLE [dbo].[ActividadesEconmicasEmpresa]  WITH CHECK ADD  CONSTRAINT [FK_ActividadesEconmicasEmpresa_ActividadesEconomicas] FOREIGN KEY([codigo])
REFERENCES [dbo].[ActividadesEconomicas] ([codigo])
GO
ALTER TABLE [dbo].[ActividadesEconmicasEmpresa] CHECK CONSTRAINT [FK_ActividadesEconmicasEmpresa_ActividadesEconomicas]
GO
ALTER TABLE [dbo].[ActividadesEconmicasEmpresa]  WITH CHECK ADD  CONSTRAINT [FK_ActividadesEconmicasEmpresa_Empresas] FOREIGN KEY([id_empresa])
REFERENCES [dbo].[Empresas] ([id_empresa])
GO
ALTER TABLE [dbo].[ActividadesEconmicasEmpresa] CHECK CONSTRAINT [FK_ActividadesEconmicasEmpresa_Empresas]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Ciudades] FOREIGN KEY([ciudad])
REFERENCES [dbo].[Ciudades] ([ciudad])
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_Ciudades]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Departamentos] FOREIGN KEY([departamento])
REFERENCES [dbo].[Departamentos] ([departamento])
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_Departamentos]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Distritos] FOREIGN KEY([distrito])
REFERENCES [dbo].[Distritos] ([distrito])
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_Distritos]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Paises] FOREIGN KEY([pais])
REFERENCES [dbo].[Paises] ([pais])
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_Paises]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_TipoDocumentos] FOREIGN KEY([documentoTipo])
REFERENCES [dbo].[TipoDocumentos] ([documentoTipo])
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_TipoDocumentos]
GO
ALTER TABLE [dbo].[CodigoSeguridad]  WITH CHECK ADD  CONSTRAINT [FK_CodigoSeguridad_CodigoSeguridad] FOREIGN KEY([cod_seguridad])
REFERENCES [dbo].[CodigoSeguridad] ([cod_seguridad])
GO
ALTER TABLE [dbo].[CodigoSeguridad] CHECK CONSTRAINT [FK_CodigoSeguridad_CodigoSeguridad]
GO
ALTER TABLE [dbo].[DocumentosElectronicos]  WITH CHECK ADD  CONSTRAINT [FK_DocumentosElectronicos_Certificados] FOREIGN KEY([id_certificado])
REFERENCES [dbo].[Certificados] ([id_certificado])
GO
ALTER TABLE [dbo].[DocumentosElectronicos] CHECK CONSTRAINT [FK_DocumentosElectronicos_Certificados]
GO
ALTER TABLE [dbo].[DocumentosElectronicos]  WITH CHECK ADD  CONSTRAINT [FK_DocumentosElectronicos_Clientes] FOREIGN KEY([id_cliente])
REFERENCES [dbo].[Clientes] ([id_cliente])
GO
ALTER TABLE [dbo].[DocumentosElectronicos] CHECK CONSTRAINT [FK_DocumentosElectronicos_Clientes]
GO
ALTER TABLE [dbo].[DocumentosElectronicos]  WITH CHECK ADD  CONSTRAINT [FK_DocumentosElectronicos_Establecimientos] FOREIGN KEY([establecimiento])
REFERENCES [dbo].[Establecimientos] ([codigo])
GO
ALTER TABLE [dbo].[DocumentosElectronicos] CHECK CONSTRAINT [FK_DocumentosElectronicos_Establecimientos]
GO
ALTER TABLE [dbo].[DocumentosElectronicos]  WITH CHECK ADD  CONSTRAINT [FK_DocumentosElectronicos_Estados] FOREIGN KEY([id_estado])
REFERENCES [dbo].[Estados] ([id_estado])
GO
ALTER TABLE [dbo].[DocumentosElectronicos] CHECK CONSTRAINT [FK_DocumentosElectronicos_Estados]
GO
ALTER TABLE [dbo].[DocumentosElectronicos]  WITH CHECK ADD  CONSTRAINT [FK_DocumentosElectronicos_TiposDocumentoElectronicos] FOREIGN KEY([tipoDocumento])
REFERENCES [dbo].[TiposDocumentoElectronicos] ([tipoDocumento])
GO
ALTER TABLE [dbo].[DocumentosElectronicos] CHECK CONSTRAINT [FK_DocumentosElectronicos_TiposDocumentoElectronicos]
GO
ALTER TABLE [dbo].[DocumentosElectronicos]  WITH CHECK ADD  CONSTRAINT [FK_DocumentosElectronicos_Usuarios] FOREIGN KEY([id_usuario])
REFERENCES [dbo].[Usuarios] ([id_usuario])
GO
ALTER TABLE [dbo].[DocumentosElectronicos] CHECK CONSTRAINT [FK_DocumentosElectronicos_Usuarios]
GO
ALTER TABLE [dbo].[Establecimientos]  WITH CHECK ADD  CONSTRAINT [FK_Establecimientos_Ciudades] FOREIGN KEY([ciudad])
REFERENCES [dbo].[Ciudades] ([ciudad])
GO
ALTER TABLE [dbo].[Establecimientos] CHECK CONSTRAINT [FK_Establecimientos_Ciudades]
GO
ALTER TABLE [dbo].[Establecimientos]  WITH CHECK ADD  CONSTRAINT [FK_Establecimientos_Departamentos] FOREIGN KEY([departamento])
REFERENCES [dbo].[Departamentos] ([departamento])
GO
ALTER TABLE [dbo].[Establecimientos] CHECK CONSTRAINT [FK_Establecimientos_Departamentos]
GO
ALTER TABLE [dbo].[Establecimientos]  WITH CHECK ADD  CONSTRAINT [FK_Establecimientos_Distritos] FOREIGN KEY([distrito])
REFERENCES [dbo].[Distritos] ([distrito])
GO
ALTER TABLE [dbo].[Establecimientos] CHECK CONSTRAINT [FK_Establecimientos_Distritos]
GO
ALTER TABLE [dbo].[EstablecimientosEmpresas]  WITH CHECK ADD  CONSTRAINT [FK_EstablecimientosEmpresas_Empresas] FOREIGN KEY([id_empresa])
REFERENCES [dbo].[Empresas] ([id_empresa])
GO
ALTER TABLE [dbo].[EstablecimientosEmpresas] CHECK CONSTRAINT [FK_EstablecimientosEmpresas_Empresas]
GO
ALTER TABLE [dbo].[EstablecimientosEmpresas]  WITH CHECK ADD  CONSTRAINT [FK_EstablecimientosEmpresas_Establecimientos] FOREIGN KEY([codigo])
REFERENCES [dbo].[Establecimientos] ([codigo])
GO
ALTER TABLE [dbo].[EstablecimientosEmpresas] CHECK CONSTRAINT [FK_EstablecimientosEmpresas_Establecimientos]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [FK_Factura_DocumentosElectronicos] FOREIGN KEY([id_documento_electronico])
REFERENCES [dbo].[DocumentosElectronicos] ([id_documento_electronico])
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [FK_Factura_DocumentosElectronicos]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [FK_Factura_Presencia] FOREIGN KEY([presencia])
REFERENCES [dbo].[Presencia] ([presencia])
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [FK_Factura_Presencia]
GO
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_DocumentosElectronicos] FOREIGN KEY([id_documento_electronico])
REFERENCES [dbo].[DocumentosElectronicos] ([id_documento_electronico])
GO
ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_DocumentosElectronicos]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_TipoDocumentos] FOREIGN KEY([documentoTipo])
REFERENCES [dbo].[TipoDocumentos] ([documentoTipo])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_TipoDocumentos]
GO
/****** Object:  StoredProcedure [api].[sp_call_sifen]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [api].[sp_call_sifen] (@id_documento_electronico bigint,
								@command nvarchar(max) = '',
								@numero nvarchar(max) = ''
							)
as
begin

		--Declaro las variables locales
		declare @dte nvarchar(max) = '';
		declare @input nvarchar(max) = 'select 1 as id, @de as de,  @command as command, @numero as numero, @id_scs as id_scs, @scs as scs, @cert as cert, @key , @enviroment;';
		declare @response nvarchar(max) = '';
		declare @status int = 0;
		declare	@id_scs nvarchar(5) = '';
		declare	@scs nvarchar(max) = '';
		declare @cert nvarchar(max) ='';
		declare @key nvarchar(max) = '';
		declare @enviroment nvarchar(4) = 'DEV';
		declare @id_usuario int = null;

		declare @SifenResults table(
			[Response] [nvarchar](max) NULL,
			[status] [int] NULL
		) ;

		--Obtengo el ID Del usuario desde el DE
		select @id_usuario = id_usuario
		from dbo.DocumentosElectronicos de
		where de.id_documento_electronico = @id_documento_electronico;

		--Obtengo los datos del usuario + certficiado
		SELECT	@id_scs = u.[ID_CSC]
				,@scs = u.[CSC]
				,@enviroment = u.[entorno]
				,@cert = c.data
				,@key = c.key_private
		FROM [dbo].[Usuarios] u
		inner join dbo.Certificados c on c.id_certificado = u.id_certificado
		where u.id_usuario = @id_usuario;


		--Genero el XML desde el DE
		EXECUTE [dbo].[SP_GenerateDE] 
		   @id_documento_electronico
		  ,@dte OUTPUT;
	

		--Envio el DTE al Web services de la SIFEN, inserto el resultado en la variable tipo tabla @SifenResults
		Insert into @SifenResults
		EXEC sp_execute_external_script
		  @language = N'Java'
		, @script = N'com.roshka.sifen.testing'
		, @input_data_1 = @input
		, @params = N'@de nvarchar(max), @command nvarchar(max), @numero nvarchar(max), @id_scs nvarchar(5), @scs nvarchar(max), @cert nvarchar(max), @key nvarchar(max), @enviroment nvarchar(5)'
		, @de = @dte
		, @command = @command
		, @numero = @numero
		, @id_scs = @id_scs
		, @scs = @scs
		, @cert = @cert
		, @key = @key
		, @enviroment = @enviroment;
	

		--Leo los resultado dela variable tipo tabla
		select @response = response, @status = status 
		from @SifenResults;

		--Actualizo el campo de respuesta "DATA" en el Docuemnto Electronico
		update dbo.DocumentosElectronicos
		set data = @response
		where id_documento_electronico = @id_documento_electronico;

		--Reistro la operacion en la tabla de operaciones
		insert into operaciones (fecha,id_documento_electronico,comando,numero,response,status)
		select getdate(), @id_documento_electronico, @command, @numero, r.Response, r.status from @SifenResults r;

		declare @id_last bigint;
		select @id_last = @@IDENTITY;


		--Verfico la respuesta
		EXEC	 [dbo].[sp_consulta_xml]
		@comando = @command,
		@id_operacion = @id_last;

END
GO
/****** Object:  StoredProcedure [dbo].[sp_call_sifen]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[sp_call_sifen] (@id_documento_electronico bigint,
								@command nvarchar(max) = '',
								@numero nvarchar(max) = '',
								@id_scs nvarchar(5) = '',
								@scs nvarchar(max) = '',
								@cert nvarchar(max) ='',
								@key nvarchar(max) = '',
								@enviroment nvarchar(4) = 'DEV')
as
begin


DECLARE @det nvarchar(max) = '';
declare @input nvarchar(max) = 'select 1 as id, @de as de,  @command as command, @numero as numero, @id_scs as id_scs, @scs as scs, @cert as cert, @key , @enviroment;';


--Genero el XML desde el DE
EXECUTE [dbo].[SP_GenerateDE] 
   @id_documento_electronico
  ,@det OUTPUT;


  delete from dbo.SifenResults;

Insert into dbo.SifenResults
EXEC sp_execute_external_script
  @language = N'Java'
, @script = N'com.roshka.sifen.testing'
, @input_data_1 = @input
, @params = N'@de nvarchar(max), @command nvarchar(max), @numero nvarchar(max), @id_scs nvarchar(5), @scs nvarchar(max), @cert nvarchar(max), @key nvarchar(max), @enviroment nvarchar(5)'
, @de = @det
, @command = @command
, @numero = @numero
, @id_scs = @id_scs
, @scs = @scs
, @cert = @cert
, @key = @key
, @enviroment = @enviroment;
--with result sets ((Response nvarchar(max), Status int));

declare @response nvarchar(max);
declare @status int;

select @response = response, @status = status 
from SifenResults;

update dbo.DocumentosElectronicos
set data = @response
where id_documento_electronico = @id_documento_electronico;

insert into operaciones (fecha,id_documento_electronico,comando,numero,response,status)
select getdate(), @id_documento_electronico, @command, @numero, r.Response, r.status from SifenResults r;

END
GO
/****** Object:  StoredProcedure [dbo].[sp_clientes_delete]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--DELETE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_clientes_delete] (@id_cliente int)
		as
		begin
		delete from [dbo].[clientes] where id_cliente = @id_cliente; end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_clientes_get_all]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--SELECT ALL PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_clientes_get_all]
		as
		begin
		SELECT


id_cliente,
contribuyente,
ruc,
razonSocial,
nombreFantasia,
tipoOperacion,
direccion,
numeroCasa,
departamento,
distrito,
ciudad,
pais,
tipoContribuyente,
documentoTipo,
documentoNumero,
telefono,
celular,
email,
codigo

from [dbo].[clientes]; end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_clientes_get_clientes]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--SELECT ONE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_clientes_get_clientes]
		(@id_cliente int)
		as
		begin
		SELECT


id_cliente,
contribuyente,
ruc,
razonSocial,
nombreFantasia,
tipoOperacion,
direccion,
numeroCasa,
departamento,
distrito,
ciudad,
pais,
tipoContribuyente,
documentoTipo,
documentoNumero,
telefono,
celular,
email,
codigo

from [dbo].[clientes] where id_cliente = @id_cliente end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_clientes_insert]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--INSERT PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_clientes_insert](



@contribuyente smallint,
@ruc varchar (128),
@razonSocial varchar (128),
@nombreFantasia varchar (128),
@tipoOperacion smallint,
@direccion varchar (128),
@numeroCasa varchar (128),
@departamento int,
@distrito int,
@ciudad int,
@pais varchar (128),
@tipoContribuyente smallint,
@documentoTipo smallint,
@documentoNumero varchar (128),
@telefono varchar (128),
@celular varchar (128),
@email varchar (128),
@codigo varchar (128)

) as begin
		INSERT INTO [dbo].[clientes] (


contribuyente,
ruc,
razonSocial,
nombreFantasia,
tipoOperacion,
direccion,
numeroCasa,
departamento,
distrito,
ciudad,
pais,
tipoContribuyente,
documentoTipo,
documentoNumero,
telefono,
celular,
email,
codigo

) values (


@contribuyente,
@ruc,
@razonSocial,
@nombreFantasia,
@tipoOperacion,
@direccion,
@numeroCasa,
@departamento,
@distrito,
@ciudad,
@pais,
@tipoContribuyente,
@documentoTipo,
@documentoNumero,
@telefono,
@celular,
@email,
@codigo

); end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_clientes_update]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--UPDATE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_clientes_update](


@id_cliente bigint,
@contribuyente smallint,
@ruc varchar (128),
@razonSocial varchar (128),
@nombreFantasia varchar (128),
@tipoOperacion smallint,
@direccion varchar (128),
@numeroCasa varchar (128),
@departamento int,
@distrito int,
@ciudad int,
@pais varchar (128),
@tipoContribuyente smallint,
@documentoTipo smallint,
@documentoNumero varchar (128),
@telefono varchar (128),
@celular varchar (128),
@email varchar (128),
@codigo varchar (128)

) as begin
		UPDATE [dbo].[clientes] SET



contribuyente = @contribuyente,
ruc = @ruc,
razonSocial = @razonSocial,
nombreFantasia = @nombreFantasia,
tipoOperacion = @tipoOperacion,
direccion = @direccion,
numeroCasa = @numeroCasa,
departamento = @departamento,
distrito = @distrito,
ciudad = @ciudad,
pais = @pais,
tipoContribuyente = @tipoContribuyente,
documentoTipo = @documentoTipo,
documentoNumero = @documentoNumero,
telefono = @telefono,
celular = @celular,
email = @email,
codigo = @codigo

where id_cliente=@id_cliente end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_consulta_xml]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[sp_consulta_xml](@comando nvarchar(50), @id_operacion bigint)

as BEGIN

 

declare @xml  xml;

 

declare @aux  nvarchar(max)  = (select response from Operaciones where id_operacion=@id_operacion);

 

DECLARE @hdoc int

SET @xml = cast(@aux as varchar(max)) COLLATE Latin1_General_100_CI_AS_SC_UTF8

EXEC sp_xml_prepareDocument @hdoc OUTPUT, @xml

 

IF (@comando = 'envio_lote')

 

BEGIN

--Respuestas para envio de lotes

 

;WITH XMLNAMESPACES('http://www.w3.org/2003/05/soap-envelope' AS env,

                    'http://ekuatia.set.gov.py/sifen/xsd' AS ns2)

 

select @xml.value('(/env:Envelope/env:Body/ns2:rResEnviLoteDe/ns2:dCodRes)[1]','nvarchar(max) ') as CodeRes,

                   @xml.value('(/env:Envelope/env:Body/ns2:rResEnviLoteDe/ns2:dMsgRes)[1]','nvarchar(max) ') as MessageRes,

                   null as Status,

       @xml.value('(/env:Envelope/env:Body/ns2:rResEnviLoteDe/ns2:dProtConsLote)[1]','nvarchar(max) ') as Value,

                   null as CDC

 

END

ELSE IF (@comando = 'ruc')

 

BEGIN

 

--Respuestas para consulta de ruc

 

;WITH XMLNAMESPACES('http://www.w3.org/2003/05/soap-envelope' AS env,

                    'http://ekuatia.set.gov.py/sifen/xsd' AS ns2)

 

select  @xml.value('(/env:Envelope/env:Body/ns2:rResEnviConsRUC/ns2:dCodRes)[1]','nvarchar(max) ') as CodeRes,

                                @xml.value('(/env:Envelope/env:Body/ns2:rResEnviConsRUC/ns2:dMsgRes)[1]','nvarchar(max) ') as MessageRes,

                    @xml.value('(/env:Envelope/env:Body/ns2:rResEnviConsRUC/ns2:xContRUC/ns2:dDesEstCons)[1]','nvarchar(max) ') as Status,

                                @xml.value('(/env:Envelope/env:Body/ns2:rResEnviConsRUC/ns2:xContRUC/ns2:dRazCons)[1]','nvarchar(max) ') as Value,

                                null as CDC

END

 

ELSE IF (@comando = 'consulta_lote')

 

BEGIN

 

--consulta de lotes enviados

 

;WITH XMLNAMESPACES('http://www.w3.org/2003/05/soap-envelope' AS env,

                    'http://ekuatia.set.gov.py/sifen/xsd' AS ns2)

 

select @xml.value('(/env:Envelope/env:Body/ns2:rResEnviConsLoteDe/ns2:gResProcLote/ns2:gResProc/ns2:dCodRes)[1]','nvarchar(max) ') as CodeRes,

                   @xml.value('(/env:Envelope/env:Body/ns2:rResEnviConsLoteDe/ns2:dMsgResLot)[1]','nvarchar(max) ') as MessageRes, 

                   @xml.value('(/env:Envelope/env:Body/ns2:rResEnviConsLoteDe/ns2:gResProcLote/ns2:dEstRes)[1]','nvarchar(max) ') as Status,

                   @xml.value('(/env:Envelope/env:Body/ns2:rResEnviConsLoteDe/ns2:gResProcLote/ns2:gResProc/ns2:dMsgRes)[1]','nvarchar(max) ') as Value,

                   @xml.value('(/env:Envelope/env:Body/ns2:rResEnviConsLoteDe/ns2:gResProcLote/ns2:id)[1]','nvarchar(max) ') as CDC

 

END

 

END

GO
/****** Object:  StoredProcedure [dbo].[sp_DocumentosElectronicos_delete]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--DELETE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_DocumentosElectronicos_delete] (@id_documento_electronico bigint)
		as
		begin
		delete from [dbo].[DocumentosElectronicos] 
		where id_documento_electronico = @id_documento_electronico; 
end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_DocumentosElectronicos_get_all]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--SELECT ALL PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_DocumentosElectronicos_get_all]

(
@key varchar(128),
@parameter varchar(50),
@state varchar(50),
@user varchar(128)
)

		as
		begin
SELECT			de.id_documento_electronico, 
				de.tipoDocumento, 
				de.establecimiento, 
				de.codigoSeguridadAleatorio, 
				de.punto, 
				de.numero, 
				de.descripcion, 
				de.observacion, 
				de.tipoContribuyente, 
				de.fecha, 
				de.tipoEmision, 
				de.tipoTransaccion, 
				de.tipoImpuesto, 
				de.moneda, 
				de.condicionAnticipo, 
				de.condicionTipoCambio, 
				de.cambio, 
				de.id_cliente, 
				de.id_usuario, 
				de.id_certificado, 
				de.data, 
				de.id_estado, 
				TiposEmision.tipoEmisionDescripcion, 
				Estados.estado, 
				TiposImpuesto.TipoImpuestoDescripcion, 
				TiposDocumentoElectronicos.Descripcion AS TipoDocumentoElectronicoDescripcion, 
				Establecimientos.denominacion, 
				PuntosExpedicion.punto AS PuntoExpedicionDescripcion, 
				Clientes.razonSocial, 
				TiposTransaccion.tipoTransaccionDescripcion
FROM            DocumentosElectronicos AS de 
				inner join Estados on Estados.id_estado = de.id_estado 
				left outer join Clientes ON de.id_cliente = Clientes.id_cliente
				left outer join  TiposContribuyente on TiposContribuyente.tipo = de.tipoContribuyente
				left outer join TiposEmision on TiposEmision.tipo = de.tipoEmision
				left outer join TiposDocumentoElectronicos ON de.tipoDocumento = TiposDocumentoElectronicos.tipoDocumento 
				left outer join TiposImpuesto ON de.tipoImpuesto = TiposImpuesto.tipo 
				left outer join TiposTransaccion ON de.tipoTransaccion = TiposTransaccion.tipo 
				left outer join Establecimientos ON de.establecimiento = Establecimientos.codigo 
				left outer join PuntosExpedicion ON Establecimientos.codigo = PuntosExpedicion.codigo 
				
where Estados.estado = @state; 

end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_DocumentosElectronicos_get_DocumentosElectronicos]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/**************************************************/
--SELECT ONE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_DocumentosElectronicos_get_DocumentosElectronicos]
		(@id_documento_electronico bigint)
		as
		begin
		SELECT


id_documento_electronico,
tipoDocumento,
establecimiento,
codigoSeguridadAleatorio,
punto,
numero,
descripcion,
observacion,
tipoContribuyente,
fecha,
tipoEmision,
tipoTransaccion,
tipoImpuesto,
moneda,
condicionAnticipo,
condicionTipoCambio,
cambio,
id_cliente,
id_usuario,
id_certificado,
data,
id_estado

from [dbo].[DocumentosElectronicos] 
where id_documento_electronico = @id_documento_electronico end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_DocumentosElectronicos_insert]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--INSERT PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_DocumentosElectronicos_insert](



@tipoDocumento int ,
@establecimiento varchar (50),
@codigoSeguridadAleatorio varchar (128),
@punto varchar (50),
@numero varchar (50),
@descripcion varchar (512),
@observacion varchar (512),
@tipoContribuyente smallint,
@fecha datetime,
@tipoEmision smallint,
@tipoTransaccion smallint,
@tipoImpuesto smallint,
@moneda varchar (128),
@condicionAnticipo smallint,
@condicionTipoCambio smallint,
@cambio numeric (18,2),
@id_cliente bigint,
@id_usuario int  =1 ,
@id_certificado bigint  = 1


) as begin
		INSERT INTO [dbo].[DocumentosElectronicos] (



tipoDocumento,
establecimiento,
codigoSeguridadAleatorio,
punto,
numero,
descripcion,
observacion,
tipoContribuyente,
fecha,
tipoEmision,
tipoTransaccion,
tipoImpuesto,
moneda,
condicionAnticipo,
condicionTipoCambio,
cambio,
id_cliente,
id_usuario,
id_certificado,
id_estado

) values (



@tipoDocumento,
@establecimiento,
@codigoSeguridadAleatorio,
@punto,
@numero,
@descripcion,
@observacion,
@tipoContribuyente,
@fecha,
@tipoEmision,
@tipoTransaccion,
@tipoImpuesto,
@moneda,
@condicionAnticipo,
@condicionTipoCambio,
@cambio,
@id_cliente,
1,
1,
1
); end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_DocumentosElectronicos_update]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--UPDATE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_DocumentosElectronicos_update](


@id_documento_electronico bigint,
@tipoDocumento int ,
@establecimiento varchar (50),
@codigoSeguridadAleatorio varchar (128),
@punto varchar (50),
@numero varchar (50),
@descripcion varchar (512),
@observacion varchar (512),
@tipoContribuyente smallint,
@fecha datetime,
@tipoEmision smallint,
@tipoTransaccion smallint,
@tipoImpuesto smallint,
@moneda varchar (128),
@condicionAnticipo smallint,
@condicionTipoCambio smallint,
@cambio numeric (18,2),
@id_cliente bigint,
@id_usuario int ,
@id_certificado bigint,
@data nvarchar(max),
@id_estado smallint
) as begin
		UPDATE [dbo].[DocumentosElectronicos] SET



tipoDocumento = @tipoDocumento,
establecimiento = @establecimiento,
codigoSeguridadAleatorio = @codigoSeguridadAleatorio,
punto = @punto,
numero = @numero,
descripcion = @descripcion,
observacion = @observacion,
tipoContribuyente = @tipoContribuyente,
fecha = @fecha,
tipoEmision = @tipoEmision,
tipoTransaccion = @tipoTransaccion,
tipoImpuesto = @tipoImpuesto,
moneda = @moneda,
condicionAnticipo = @condicionAnticipo,
condicionTipoCambio = @condicionTipoCambio,
cambio = @cambio,
id_cliente = @id_cliente,
id_usuario = @id_usuario,
id_certificado = @id_certificado,
data = @data,
id_estado=@id_estado

where id_documento_electronico=@id_documento_electronico end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_Empresas_delete]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--DELETE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Empresas_delete] (@id_Empresa int)
		as
		begin
		delete from [dbo].[Empresas] where id_Empresa = @id_Empresa; end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_Empresas_get_all]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--SELECT ALL PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Empresas_get_all]
		as
		begin
		SELECT


id_empresa,
version,
fechaFirmaDigital,
ruc,
razonSocial,
nombreFantasia,
timbradoNumero,
timbradoFecha,
tipoContribuyente,
tipoRegimen

from [dbo].[Empresas]; end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_Empresas_get_Empresas]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--SELECT ONE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Empresas_get_Empresas]
		(@id_empresa int)
		as
		begin
		SELECT


id_empresa,
version,
fechaFirmaDigital,
ruc,
razonSocial,
nombreFantasia,
timbradoNumero,
timbradoFecha,
tipoContribuyente,
tipoRegimen

from [dbo].[Empresas] where id_empresa = @id_empresa end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_Empresas_insert]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--INSERT PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Empresas_insert](



@version numeric (18,2),
@fechaFirmaDigital datetime,
@ruc varchar (128),
@razonSocial varchar (128),
@nombreFantasia varchar (128),
@timbradoNumero varchar (128),
@timbradoFecha datetime,
@tipoContribuyente int,
@tipoRegimen int

) as begin
		INSERT INTO [dbo].[Empresas] (


version,
fechaFirmaDigital,
ruc,
razonSocial,
nombreFantasia,
timbradoNumero,
timbradoFecha,
tipoContribuyente,
tipoRegimen

) values (


@version,
@fechaFirmaDigital,
@ruc,
@razonSocial,
@nombreFantasia,
@timbradoNumero,
@timbradoFecha,
@tipoContribuyente,
@tipoRegimen

); end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_Empresas_update]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/**************************************************/
--UPDATE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Empresas_update](


@id_empresa int,
@version numeric (18,2),
@fechaFirmaDigital datetime,
@ruc varchar (128),
@razonSocial varchar (128),
@nombreFantasia varchar (128),
@timbradoNumero varchar (128),
@timbradoFecha datetime,
@tipoContribuyente int,
@tipoRegimen int

) as begin
		UPDATE [dbo].[Empresas] SET



version = @version,
fechaFirmaDigital = @fechaFirmaDigital,
ruc = @ruc,
razonSocial = @razonSocial,
nombreFantasia = @nombreFantasia,
timbradoNumero = @timbradoNumero,
timbradoFecha = @timbradoFecha,
tipoContribuyente = @tipoContribuyente,
tipoRegimen = @tipoRegimen

where id_empresa =@id_empresa  end; 
GO
/****** Object:  StoredProcedure [dbo].[SP_GenerateDE]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Christian Balbuena
-- Create date: 2022-06
-- Description:	Generate DE / SIFEN XML
-- =============================================
CREATE  PROC [dbo].[SP_GenerateDE]
(
	@id_documento_electronico bigint,
	@de nvarchar(max) output
)

AS
BEGIN
	DECLARE @version NVARCHAR(max)
DECLARE @myDoc XML



set @version = '<?xml version="1.0" encoding="UTF-8"?>
<rDE xsi:schemaLocation="https://ekuatia.set.gov.py/sifen/xsd siRecepDE_v150.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://ekuatia.set.gov.py/sifen/xsd">';


set @myDoc =  (
SELECT 	
				--Atributos
				[dbo].[f_generar_CDC] (de.id_documento_electronico)		[@Id]
				
				--Campos AA
				,150 													[dVerFor]
				,dbo.f_generar_digito_verificador([dbo].[f_generar_CDC] (de.id_documento_electronico),11)	[dDVId] --MODULO 11 (Modificar por funcion)
				,firma.fecha_emision									[dFecFirma]
				,1														[dSisFact]

				-- Campos inherentes a la operación de Documentos Electrónicos (B001-B099) gOpeDE
				,1														[gOpeDE/iTipEmi]
				,'Normal'												[gOpeDE/dDesTipEmi]
				,dbo.f_llenar_ceros(de.id_documento_electronico,9)		[gOpeDE/dCodSeg]
				,1														[gOpeDE/dInfoEmi]
				,'Información de interés del Fisco respecto al DE'		[gOpeDE/dInfoFisc]


				--Campos de datos del Timbrado (C001-C099)
				,DE.tipoDocumento										[gTimb/iTiDE]
				,tipo_de.Descripcion									[gTimb/dDesTiDE]
				,usuario.timbrado											[gTimb/dNumTim]
				,dbo.f_llenar_ceros(DE.[establecimiento],3)				[gTimb/dEst]
				,dbo.f_llenar_ceros(DE.[punto],3)						[gTimb/dPunExp]
				,'1' +dbo.f_llenar_ceros(DE.[numero],6)					[gTimb/dNumDoc]
				,'AA'													[gTimb/dSerieNum]
				,format(usuario.inicio_timbrado,'yyyy-MM-dd')											[gTimb/dFeIniT] --'2022-04-21'
				--,'2023-04-21'											[gTimb/dFeFinT]

				--Campos Generales del Documento Electrónico DE (D001-D299)
				,DE.fecha												[gDatGralOpe/dFeEmiDE] --DE.[fecha]												[gDatGralOpe/dFeEmiDE]

				--D1. Campos inherentes a la operación comercial (D010-D099)
                ,1														[gDatGralOpe/gOpeCom/iTipTra]
                ,'Venta de mercadería'									[gDatGralOpe/gOpeCom/dDesTipTra]
                ,1														[gDatGralOpe/gOpeCom/iTImp]
                ,'IVA'													[gDatGralOpe/gOpeCom/dDesTImp]
                ,'PYG'													[gDatGralOpe/gOpeCom/cMoneOpe]
                ,'Guarani'												[gDatGralOpe/gOpeCom/dDesMoneOpe]
            

				--D2. Campos que identifican al emisor del Documento Electrónico DE (D100-D129)
				,usuario.documentoNumero											[gDatGralOpe/gEmis/dRucEm]
				,[dbo].[f_generar_digito_verificador](usuario.documentoNumero,11)		[gDatGralOpe/gEmis/dDVEmi]
				,2														[gDatGralOpe/gEmis/iTipCont]
				,3														[gDatGralOpe/gEmis/cTipReg]
				,usuario.nombre	[gDatGralOpe/gEmis/dNomEmi]
				,usuario.direccion									[gDatGralOpe/gEmis/dDirEmi]
				,0														[gDatGralOpe/gEmis/dNumCas]
				,1														[gDatGralOpe/gEmis/cDepEmi]
				,'CAPITAL'												[gDatGralOpe/gEmis/dDesDepEmi]
				,1														[gDatGralOpe/gEmis/cCiuEmi]
				,'ASUNCION (DISTRITO)'									[gDatGralOpe/gEmis/dDesCiuEmi]
				,usuario.telefono										[gDatGralOpe/gEmis/dTelEmi]
				,usuario.correo											[gDatGralOpe/gEmis/dEmailE]

				--D2.1 Campos que describen la actividad económica del emisor (D130-D139)
				,'62010' 											[gDatGralOpe/gEmis/gActEco/cActEco]					
				,'ACTIVIDADES DE PROGRAMACIÓN INFORMÁTICA'				[gDatGralOpe/gEmis/gActEco/dDesActEco]
				
				
				--D3. Campos que identifican al receptor del Documento Electrónico DE (D200-D299)
				,cli.contribuyente										[gDatGralOpe/gDatRec/iNatRec]
				,1														[gDatGralOpe/gDatRec/iTiOpe]
				,pais_cliente.pais										[gDatGralOpe/gDatRec/cPaisRec]
				,pais_cliente.paisDescripcion							[gDatGralOpe/gDatRec/dDesPaisRe]
				,2														[gDatGralOpe/gDatRec/iTiContRec]
				,cli.ruc												[gDatGralOpe/gDatRec/dRucRec]
				,dbo.f_generar_digito_verificador(cli.ruc,11)			[gDatGralOpe/gDatRec/dDVRec]
				,cli.nombreFantasia										[gDatGralOpe/gDatRec/dNomRec]
				,cli.direccion											[gDatGralOpe/gDatRec/dDirRec]
				,cli.numeroCasa											[gDatGralOpe/gDatRec/dNumCasRec]
				,cli.departamento										[gDatGralOpe/gDatRec/cDepRec]
				,departamento_cliente.departamentoDescripcion			[gDatGralOpe/gDatRec/dDesDepRec]
				,distrito_cliente.distrito								[gDatGralOpe/gDatRec/cDisRec]
				,distrito_cliente.distritoDescripcion					[gDatGralOpe/gDatRec/dDesDisRec]
				,ciudad_cliente.ciudad									[gDatGralOpe/gDatRec/cCiuRec]
				,ciudad_cliente.ciudadDescripcion						[gDatGralOpe/gDatRec/dDesCiuRec]
				,cli.telefono											[gDatGralOpe/gDatRec/dTelRec]
				,cli.codigo												[gDatGralOpe/gDatRec/dCodCliente]
            

			
			 
			--E1. Campos que componen la Factura Electrónica FE (E002-E099)
			,1															[gDtipDE/gCamFE/iIndPres]
             ,'Operación presencial'									[gDtipDE/gCamFE/dDesIndPres]
            
           
            ,2															[gDtipDE/gCamCond/iCondOpe]
			,'Crédito'													[gDtipDE/gCamCond/dDCondOpe]
            ,1															[gDtipDE/gCamCond/gPagCred/iCondCred]
			,'Plazo'													[gDtipDE/gCamCond/gPagCred/dDCondCred]
            ,30															[gDtipDE/gCamCond/gPagCred/dPlazoCre]
                
            
			--E8. Campos que describen los ítems de la operación (E700-E899)
							
				,(
				select	i.codigo										[dCodInt],
						i.descripcion									[dDesProSer],
						77												[cUniMed],
						'UNI'											[dDesUniMed],
						i.cantidad										[dCantProSer],
						21												[dInfItem],
						i.precioUnitario								[gValorItem/dPUniProSer],
						i.precioUnitario								[gValorItem/dTotBruOpeItem],
						0												[gValorItem/gValorRestaItem/dDescItem],
						0												[gValorItem/gValorRestaItem/dPorcDesIt],
						0												[gValorItem/gValorRestaItem/dDescGloItem],
						i.precioUnitario								[gValorItem/gValorRestaItem/dTotOpeItem],

						i.ivaTipo										[gCamIVA/iAfecIVA],
						'Gravado IVA'									[gCamIVA/dDesAfecIVA],
						100												[gCamIVA/dPropIVA],
						10												[gCamIVA/dTasaIVA],
						i.precioUnitario								[gCamIVA/dBasGravIVA],
						i.precioUnitario/10								[gCamIVA/dLiqIVAItem]
				from Items i
				where i.id_documento_electronico = DE.id_documento_electronico
				for xml path('gCamItem'), type
				) as [gDtipDE]

				--SUBTOTALES
				,(
				select	0									[dSubExe],
						0									[dSub5],
						sum(i.precioUnitario)				[dSub10],
						sum(i.precioUnitario)				[dTotOpe],
						0									[dTotDesc],
						0									[dTotDescGlotem],
						0									[dTotAntItem],
						0									[dTotAnt],
						0									[dPorcDescTotal],
						0.0									[dDescTotal],
						0									[dAnticipo],
						0.0									[dRedon],
						sum(i.precioUnitario)				[dTotGralOpe],
						0									[dIVA5],
						sum(i.precioUnitario)/10			[dIVA10],
						sum(i.precioUnitario)/10			[dTotIVA],
						0									[dBaseGrav5],
						sum(i.precioUnitario)				[dBaseGrav10],
						sum(i.precioUnitario)				[dTBasGraIVA]
				from Items i
				where i.id_documento_electronico = DE.id_documento_electronico
				group by i.id_documento_electronico
				for xml path('gTotSub'), type
				) 

				
  FROM [DocumentosElectronicos] DE
  LEFT OUTER JOIN Clientes cli ON CLI.id_cliente = DE.id_cliente
  left outer join Certificados firma on firma.id_certificado = DE.id_certificado
  left outer join TiposDocumentoElectronicos tipo_de on tipo_de.tipoDocumento = DE.tipoDocumento
  
  left outer join Paises pais_cliente on pais_cliente.pais = cli.pais
  left outer join Distritos distrito_cliente on distrito_cliente.distrito = cli.distrito
  left outer join Departamentos departamento_cliente on departamento_cliente.departamento =  cli.departamento
  left outer join Ciudades ciudad_cliente on ciudad_cliente.ciudad = cli.ciudad
  left outer join Usuarios usuario on usuario.id_usuario = DE.id_usuario

  WHERE DE.id_documento_electronico = @id_documento_electronico
  for xml path('DE') , type
  )

  declare @salida varchar(8000);

  set @salida = @version + CAST( @myDoc AS NVARCHAR(MAX) ) + '</rDE>' 

  set @de =  replace(@salida,'"','''');

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Items_delete]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--DELETE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Items_delete] (@id_item  int)
		as
		begin
		delete from [dbo].[Items] where id_item  = @id_item ; end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_Items_get_all]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--SELECT ALL PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Items_get_all]
		as
		begin
		SELECT


id_item,
id_documento_electronico,
codigo,
descripcion,
observacion,
partidaArancelaria,
ncm,
unidadMedida,
cantidad,
precioUnitario,
cambio,
descuento,
anticipo,
pais,
tolerancia,
toleranciaCantidad,
toleranciaPorcentaje,
cdcAnticipo,
ivaTipo,
ivaBase,
iva,
lote,
vencimiento,
numeroSerie,
numeroPedido,
numeroSeguimiento

from [dbo].[Items]; end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_Items_get_Items]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--SELECT ONE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Items_get_Items]
		(@id_item int)
		as
		begin
		SELECT


id_item,
id_documento_electronico,
codigo,
descripcion,
observacion,
partidaArancelaria,
ncm,
unidadMedida,
cantidad,
precioUnitario,
cambio,
descuento,
anticipo,
pais,
tolerancia,
toleranciaCantidad,
toleranciaPorcentaje,
cdcAnticipo,
ivaTipo,
ivaBase,
iva,
lote,
vencimiento,
numeroSerie,
numeroPedido,
numeroSeguimiento

from [dbo].[Items] where id_item = id_item end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_Items_insert]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**************************************************/
--INSERT PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Items_insert](@id_documento_electronico bigint,
									@codigo varchar (128),
									@descripcion varchar (128),
									@observacion varchar (128),
									@partidaArancelaria int,
									@ncm varchar (128),
									@unidadMedida smallint,
									@cantidad numeric(18,2),
									@precioUnitario numeric(18,2),
									@cambio numeric(18,2),
									@descuento numeric(18,2),
									@anticipo numeric(18,2),
									@pais varchar (128),
									@tolerancia smallint,
									@toleranciaCantidad int,
									@toleranciaPorcentaje numeric(18,2),
									@cdcAnticipo varchar (128),
									@ivaTipo smallint,
									@ivaBase numeric(18,2),
									@iva numeric(18,2),
									@lote varchar (128),
									@vencimiento datetime,
									@numeroSerie varchar (128),
									@numeroPedido varchar (128),
									@numeroSeguimiento varchar (128)

) as begin

begin try
		INSERT INTO [dbo].[Items] (
		id_documento_electronico,
		codigo,
		descripcion,
		observacion,
		partidaArancelaria,
		ncm,
		unidadMedida,
		cantidad,
		precioUnitario,
		cambio,
		descuento,
		anticipo,
		pais,
		tolerancia,
		toleranciaCantidad,
		toleranciaPorcentaje,
		cdcAnticipo,
		ivaTipo,
		ivaBase,
		iva,
		lote,
		vencimiento,
		numeroSerie,
		numeroPedido,
		numeroSeguimiento

		) values (


		@id_documento_electronico,
		@codigo,
		@descripcion,
		@observacion,
		@partidaArancelaria,
		@ncm,
		@unidadMedida,
		@cantidad,
		@precioUnitario,
		@cambio,
		@descuento,
		@anticipo,
		@pais,
		@tolerancia,
		@toleranciaCantidad,
		@toleranciaPorcentaje,
		@cdcAnticipo,
		@ivaTipo,
		@ivaBase,
		@iva,
		@lote,
		@vencimiento,
		@numeroSerie,
		@numeroPedido,
		@numeroSeguimiento

		); 

end try
begin catch
 return -1;
end catch

end; 
GO
/****** Object:  StoredProcedure [dbo].[sp_Items_update]    Script Date: 10/14/2022 1:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/**************************************************/
--UPDATE PROCEDURE
/**************************************************/
CREATE proc [dbo].[sp_Items_update](


@id_item bigint,
@id_documento_electronico bigint,
@codigo varchar (128),
@descripcion varchar (128),
@observacion varchar (128),
@partidaArancelaria int,
@ncm varchar (128),
@unidadMedida smallint,
@cantidad numeric(18,2),
@precioUnitario numeric(18,2),
@cambio numeric(18,2),
@descuento numeric(18,2),
@anticipo numeric(18,2),
@pais varchar (128),
@tolerancia smallint,
@toleranciaCantidad int,
@toleranciaPorcentaje numeric(18,2),
@cdcAnticipo varchar (128),
@ivaTipo smallint,
@ivaBase numeric(18,2),
@iva numeric(18,2),
@lote varchar (128),
@vencimiento datetime,
@numeroSerie varchar (128),
@numeroPedido varchar (128),
@numeroSeguimiento varchar (128)

) as begin
		UPDATE [dbo].[Items] SET



id_documento_electronico = @id_documento_electronico,
codigo = @codigo,
descripcion = @descripcion,
observacion = @observacion,
partidaArancelaria = @partidaArancelaria,
ncm = @ncm,
unidadMedida = @unidadMedida,
cantidad = @cantidad,
precioUnitario = @precioUnitario,
cambio = @cambio,
descuento = @descuento,
anticipo = @anticipo,
pais = @pais,
tolerancia = @tolerancia,
toleranciaCantidad = @toleranciaCantidad,
toleranciaPorcentaje = @toleranciaPorcentaje,
cdcAnticipo = @cdcAnticipo,
ivaTipo = @ivaTipo,
ivaBase = @ivaBase,
iva = @iva,
lote = @lote,
vencimiento = @vencimiento,
numeroSerie = @numeroSerie,
numeroPedido = @numeroPedido,
numeroSeguimiento = @numeroSeguimiento

where id_item=@id_item end; 
GO
