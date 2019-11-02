USE [StockExchange]
GO

/****** Object:  Table [dbo].[Orders]    Script Date: 2019-09-01 23:27:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Orders](
	[OrderId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Stockname] [nvarchar](max) NOT NULL,
	[Price] [int] NOT NULL,
	[Actions] [nvarchar](50) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[NumberOfStocks] [int] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO