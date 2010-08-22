USE [OKB]
GO
/****** Object:  Table [dbo].[User]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
	[Name] [varchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SolutionRepresentation]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SolutionRepresentation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProblemClass]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProblemClass](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Project]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DataType]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DataType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SqlName] [varchar](200) NOT NULL,
	[ClrName] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[DataType] ON
INSERT [dbo].[DataType] ([Id], [SqlName], [ClrName]) VALUES (1, N'Int', N'System.Int32')
INSERT [dbo].[DataType] ([Id], [SqlName], [ClrName]) VALUES (2, N'Float', N'System.Double')
INSERT [dbo].[DataType] ([Id], [SqlName], [ClrName]) VALUES (3, N'Char', N'System.String')
INSERT [dbo].[DataType] ([Id], [SqlName], [ClrName]) VALUES (4, N'Blob', N'System.Object')
INSERT [dbo].[DataType] ([Id], [SqlName], [ClrName]) VALUES (5, N'BLOB', N'IOperator')
INSERT [dbo].[DataType] ([Id], [SqlName], [ClrName]) VALUES (6, N'Bit', N'System.Boolean')
SET IDENTITY_INSERT [dbo].[DataType] OFF
/****** Object:  Table [dbo].[Client]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Client](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
	[Name] [varchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AlgorithmClass]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AlgorithmClass](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Platform]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Platform](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Platform] ON
INSERT [dbo].[Platform] ([Id], [Name], [Description]) VALUES (1, N'HL 2.5', N'HeuristicLab 2.5')
INSERT [dbo].[Platform] ([Id], [Name], [Description]) VALUES (2, N'HL 3.3', N'HeuristicLab 3.3')
SET IDENTITY_INSERT [dbo].[Platform] OFF
/****** Object:  Table [dbo].[Parameter]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Parameter](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](900) NULL,
	[Description] [varchar](max) NULL,
	[DataTypeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Algorithm]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Algorithm](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AlgorithmClassId] [int] NOT NULL,
	[PlatformId] [int] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Result]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Result](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
	[DataTypeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Problem]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Problem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProblemClassId] [int] NOT NULL,
	[PlatformId] [int] NOT NULL,
	[SolutionRepresentationid] [int] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProblemCharacteristic]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProblemCharacteristic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
	[DataTypeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Problem_Parameter]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Problem_Parameter](
	[ProblemId] [int] NOT NULL,
	[ParameterId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProblemId] ASC,
	[ParameterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProblemData]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProblemData](
	[ProblemId] [int] NOT NULL,
	[Data] [varbinary](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProblemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [DATA]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IntProblemCharacteristicValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IntProblemCharacteristicValue](
	[ProblemCharacteristicId] [int] NOT NULL,
	[ProblemId] [int] NOT NULL,
	[Value] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProblemCharacteristicId] ASC,
	[ProblemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlgorithmData]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AlgorithmData](
	[AlgorithmId] [int] NOT NULL,
	[Data] [varbinary](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AlgorithmId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [DATA]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Algorithm_Result]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Algorithm_Result](
	[AlgorithmId] [int] NOT NULL,
	[ResultId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AlgorithmId] ASC,
	[ResultId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Algorithm_Parameter]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Algorithm_Parameter](
	[AlgorithmId] [int] NOT NULL,
	[ParameterId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AlgorithmId] ASC,
	[ParameterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Experiment]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Experiment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[AlgorithmId] [int] NOT NULL,
	[ProblemId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [FullExperimentIndex] ON [dbo].[Experiment] 
(
	[ProjectId] ASC,
	[AlgorithmId] ASC,
	[ProblemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [SimpleExperiementIndex] ON [dbo].[Experiment] 
(
	[AlgorithmId] ASC,
	[ProblemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CharProblemCharacteristicValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CharProblemCharacteristicValue](
	[ProblemCharacteristicId] [int] NOT NULL,
	[ProblemId] [int] NOT NULL,
	[Value] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProblemCharacteristicId] ASC,
	[ProblemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FloatProblemCharacteristicValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FloatProblemCharacteristicValue](
	[ProblemCharacteristicId] [int] NOT NULL,
	[ProblemId] [int] NOT NULL,
	[Value] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProblemCharacteristicId] ASC,
	[ProblemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FloatParameterValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FloatParameterValue](
	[ParameterId] [int] NOT NULL,
	[ExperimentId] [int] NOT NULL,
	[Value] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ParameterId] ASC,
	[ExperimentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExperimentCreator]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExperimentCreator](
	[ExperimentId] [int] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ExperimentId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CharParameterValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CharParameterValue](
	[ParameterId] [int] NOT NULL,
	[ExperimentId] [int] NOT NULL,
	[Value] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ParameterId] ASC,
	[ExperimentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OperatorParameterValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OperatorParameterValue](
	[ParameterId] [int] NOT NULL,
	[ExperimentId] [int] NOT NULL,
	[Value] [varbinary](max) NOT NULL,
	[DataTypeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ParameterId] ASC,
	[ExperimentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [DATA]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Run]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Run](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExperimentId] [int] NOT NULL,
	[FinishedDate] [datetime2](7) NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[ClientId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [Run_FullIndex] ON [dbo].[Run] 
(
	[ExperimentId] ASC
)
INCLUDE ( [Id],
[FinishedDate],
[UserId],
[ClientId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IntParameterValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IntParameterValue](
	[ParameterId] [int] NOT NULL,
	[ExperimentId] [int] NOT NULL,
	[Value] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ParameterId] ASC,
	[ExperimentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FloatResultValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FloatResultValue](
	[ResultId] [int] NOT NULL,
	[RunId] [int] NOT NULL,
	[Value] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC,
	[RunId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IntResultValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IntResultValue](
	[ResultId] [int] NOT NULL,
	[RunId] [int] NOT NULL,
	[Value] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC,
	[RunId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlobResultValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BlobResultValue](
	[ResultId] [int] NOT NULL,
	[RunId] [int] NOT NULL,
	[Value] [varbinary](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC,
	[RunId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CharResultValue]    Script Date: 03/09/2010 16:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CharResultValue](
	[ResultId] [int] NOT NULL,
	[RunId] [int] NOT NULL,
	[Value] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC,
	[RunId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Check [CK__DataType__014935CB]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[DataType]  WITH CHECK ADD  CONSTRAINT [CK__DataType__014935CB] CHECK  (([SqlName]='Int' AND [ClrName]='System.Int32' OR [SqlName]='Float' AND [ClrName]='System.Double' OR [SqlName]='Char' AND [ClrName]='System.String' OR [SqlName]='Bit' AND [ClrName]='System.Boolean' OR [SqlName]='BLOB' AND NOT ([ClrName]='System.String' OR [ClrName]='System.Double' OR [ClrName]='System.Int32' OR [ClrName]='System.Boolean')))
GO
ALTER TABLE [dbo].[DataType] CHECK CONSTRAINT [CK__DataType__014935CB]
GO
/****** Object:  ForeignKey [FK__Algorithm__Algor__164452B1]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Algorithm]  WITH CHECK ADD FOREIGN KEY([AlgorithmClassId])
REFERENCES [dbo].[AlgorithmClass] ([Id])
GO
/****** Object:  ForeignKey [FK__Algorithm__Platf__173876EA]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Algorithm]  WITH CHECK ADD FOREIGN KEY([PlatformId])
REFERENCES [dbo].[Platform] ([Id])
GO
/****** Object:  ForeignKey [FK__Algorithm__Algor__7B5B524B]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Algorithm_Parameter]  WITH CHECK ADD FOREIGN KEY([AlgorithmId])
REFERENCES [dbo].[Algorithm] ([Id])
GO
/****** Object:  ForeignKey [FK__Algorithm__Param__7C4F7684]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Algorithm_Parameter]  WITH CHECK ADD FOREIGN KEY([ParameterId])
REFERENCES [dbo].[Parameter] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Algorithm__Algor__25518C17]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Algorithm_Result]  WITH CHECK ADD FOREIGN KEY([AlgorithmId])
REFERENCES [dbo].[Algorithm] ([Id])
GO
/****** Object:  ForeignKey [FK__Algorithm__Resul__2645B050]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Algorithm_Result]  WITH CHECK ADD FOREIGN KEY([ResultId])
REFERENCES [dbo].[Result] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Algorithm__Algor__1BFD2C07]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[AlgorithmData]  WITH CHECK ADD FOREIGN KEY([AlgorithmId])
REFERENCES [dbo].[Algorithm] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__BlobResul__Resul__1F98B2C1]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[BlobResultValue]  WITH CHECK ADD FOREIGN KEY([ResultId])
REFERENCES [dbo].[Result] ([Id])
GO
/****** Object:  ForeignKey [FK__BlobResul__RunId__208CD6FA]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[BlobResultValue]  WITH CHECK ADD FOREIGN KEY([RunId])
REFERENCES [dbo].[Run] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__CharParam__Exper__76969D2E]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[CharParameterValue]  WITH CHECK ADD FOREIGN KEY([ExperimentId])
REFERENCES [dbo].[Experiment] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__CharParam__Param__75A278F5]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[CharParameterValue]  WITH CHECK ADD FOREIGN KEY([ParameterId])
REFERENCES [dbo].[Parameter] ([Id])
GO
/****** Object:  ForeignKey [FK__CharProbl__Probl__3E1D39E1]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[CharProblemCharacteristicValue]  WITH CHECK ADD FOREIGN KEY([ProblemCharacteristicId])
REFERENCES [dbo].[ProblemCharacteristic] ([Id])
GO
/****** Object:  ForeignKey [FK__CharProbl__Probl__3F115E1A]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[CharProblemCharacteristicValue]  WITH CHECK ADD FOREIGN KEY([ProblemId])
REFERENCES [dbo].[Problem] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__CharResul__Resul__19DFD96B]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[CharResultValue]  WITH CHECK ADD FOREIGN KEY([ResultId])
REFERENCES [dbo].[Result] ([Id])
GO
/****** Object:  ForeignKey [FK__CharResul__RunId__1AD3FDA4]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[CharResultValue]  WITH CHECK ADD FOREIGN KEY([RunId])
REFERENCES [dbo].[Run] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Experimen__Algor__5070F446]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Experiment]  WITH CHECK ADD FOREIGN KEY([AlgorithmId])
REFERENCES [dbo].[Algorithm] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Experimen__Probl__5165187F]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Experiment]  WITH CHECK ADD FOREIGN KEY([ProblemId])
REFERENCES [dbo].[Problem] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Experimen__Proje__4F7CD00D]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Experiment]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Experimen__Exper__5629CD9C]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[ExperimentCreator]  WITH CHECK ADD FOREIGN KEY([ExperimentId])
REFERENCES [dbo].[Experiment] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Experimen__UserI__571DF1D5]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[ExperimentCreator]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
/****** Object:  ForeignKey [FK__FloatPara__Exper__70DDC3D8]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[FloatParameterValue]  WITH CHECK ADD FOREIGN KEY([ExperimentId])
REFERENCES [dbo].[Experiment] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__FloatPara__Param__6FE99F9F]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[FloatParameterValue]  WITH CHECK ADD FOREIGN KEY([ParameterId])
REFERENCES [dbo].[Parameter] ([Id])
GO
/****** Object:  ForeignKey [FK__FloatProb__Probl__3864608B]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[FloatProblemCharacteristicValue]  WITH CHECK ADD FOREIGN KEY([ProblemCharacteristicId])
REFERENCES [dbo].[ProblemCharacteristic] ([Id])
GO
/****** Object:  ForeignKey [FK__FloatProb__Probl__395884C4]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[FloatProblemCharacteristicValue]  WITH CHECK ADD FOREIGN KEY([ProblemId])
REFERENCES [dbo].[Problem] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__FloatResu__Resul__14270015]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[FloatResultValue]  WITH CHECK ADD FOREIGN KEY([ResultId])
REFERENCES [dbo].[Result] ([Id])
GO
/****** Object:  ForeignKey [FK__FloatResu__RunId__151B244E]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[FloatResultValue]  WITH CHECK ADD FOREIGN KEY([RunId])
REFERENCES [dbo].[Run] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__IntParame__Exper__6B24EA82]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[IntParameterValue]  WITH CHECK ADD FOREIGN KEY([ExperimentId])
REFERENCES [dbo].[Experiment] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__IntParame__Param__6A30C649]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[IntParameterValue]  WITH CHECK ADD FOREIGN KEY([ParameterId])
REFERENCES [dbo].[Parameter] ([Id])
GO
/****** Object:  ForeignKey [FK__IntProble__Probl__32AB8735]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[IntProblemCharacteristicValue]  WITH CHECK ADD FOREIGN KEY([ProblemCharacteristicId])
REFERENCES [dbo].[ProblemCharacteristic] ([Id])
GO
/****** Object:  ForeignKey [FK__IntProble__Probl__339FAB6E]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[IntProblemCharacteristicValue]  WITH CHECK ADD FOREIGN KEY([ProblemId])
REFERENCES [dbo].[Problem] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__IntResult__Resul__0E6E26BF]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[IntResultValue]  WITH CHECK ADD FOREIGN KEY([ResultId])
REFERENCES [dbo].[Result] ([Id])
GO
/****** Object:  ForeignKey [FK__IntResult__RunId__0F624AF8]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[IntResultValue]  WITH CHECK ADD FOREIGN KEY([RunId])
REFERENCES [dbo].[Run] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__OperatorP__DataT__02C769E9]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[OperatorParameterValue]  WITH CHECK ADD FOREIGN KEY([DataTypeId])
REFERENCES [dbo].[DataType] ([Id])
GO
/****** Object:  ForeignKey [FK__OperatorP__Exper__01D345B0]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[OperatorParameterValue]  WITH CHECK ADD FOREIGN KEY([ExperimentId])
REFERENCES [dbo].[Experiment] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__OperatorP__Param__00DF2177]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[OperatorParameterValue]  WITH CHECK ADD FOREIGN KEY([ParameterId])
REFERENCES [dbo].[Parameter] ([Id])
GO
/****** Object:  ForeignKey [FK__Parameter__DataT__656C112C]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Parameter]  WITH CHECK ADD FOREIGN KEY([DataTypeId])
REFERENCES [dbo].[DataType] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Problem__Platfor__31EC6D26]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Problem]  WITH CHECK ADD FOREIGN KEY([PlatformId])
REFERENCES [dbo].[Platform] ([Id])
GO
/****** Object:  ForeignKey [FK__Problem__Problem__30F848ED]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Problem]  WITH CHECK ADD FOREIGN KEY([ProblemClassId])
REFERENCES [dbo].[ProblemClass] ([Id])
GO
/****** Object:  ForeignKey [FK__Problem__Solutio__32E0915F]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Problem]  WITH CHECK ADD FOREIGN KEY([SolutionRepresentationid])
REFERENCES [dbo].[SolutionRepresentation] ([Id])
GO
/****** Object:  ForeignKey [FK__Problem_P__Param__02084FDA]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Problem_Parameter]  WITH CHECK ADD FOREIGN KEY([ParameterId])
REFERENCES [dbo].[Parameter] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Problem_P__Probl__01142BA1]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Problem_Parameter]  WITH CHECK ADD FOREIGN KEY([ProblemId])
REFERENCES [dbo].[Problem] ([Id])
GO
/****** Object:  ForeignKey [FK__ProblemCh__DataT__2DE6D218]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[ProblemCharacteristic]  WITH CHECK ADD FOREIGN KEY([DataTypeId])
REFERENCES [dbo].[DataType] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__ProblemDa__Probl__37A5467C]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[ProblemData]  WITH CHECK ADD FOREIGN KEY([ProblemId])
REFERENCES [dbo].[Problem] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Result__DataType__09A971A2]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([DataTypeId])
REFERENCES [dbo].[DataType] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Run__ClientId__5DCAEF64]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Run]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Client] ([Id])
GO
/****** Object:  ForeignKey [FK__Run__ExperimentI__5BE2A6F2]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Run]  WITH CHECK ADD FOREIGN KEY([ExperimentId])
REFERENCES [dbo].[Experiment] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__Run__UserId__5CD6CB2B]    Script Date: 03/09/2010 16:21:40 ******/
ALTER TABLE [dbo].[Run]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
