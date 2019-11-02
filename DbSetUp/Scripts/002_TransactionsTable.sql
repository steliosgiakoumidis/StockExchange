USE [StockExchange]
GO

/****** Object:  Table [dbo].[Transactions]    Script Date: 2019-09-01 23:28:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Transactions](
	[Id] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
	[Action] [nvarchar](50) NULL,
	[CorrelatesTo] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Price] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Stockname] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO