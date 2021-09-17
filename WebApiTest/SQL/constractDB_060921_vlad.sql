ALTER TABLE [dbo].[Products] 
ADD  CONSTRAINT DF_Products_ProductName_Unique UNIQUE (ProductName)
GO

ALTER TABLE [dbo].[Shops] 
ADD  CONSTRAINT DF_Shops_ShopName_Unique UNIQUE (ShopName)
GO
/****** Object:  Table [dbo].[Shops]    Script Date: 06.09.2021 5:29:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Shops]') AND type in (N'U'))
DROP TABLE [dbo].[Shops]
GO

/****** Object:  Table [dbo].[Shops]    Script Date: 06.09.2021 5:29:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Shops](
	[ShopID] [int] IDENTITY(1,1),
	[ShopName] [varchar](255) NOT NULL,
 CONSTRAINT [DF_Shops_ShopName_Unique] UNIQUE NONCLUSTERED 
(
	[ShopName] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Products]    Script Date: 06.09.2021 5:22:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
DROP TABLE [dbo].[Products]
GO

/****** Object:  Table [dbo].[Products]    Script Date: 06.09.2021 5:22:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1),
	[ProductName] [varchar](255) NOT NULL,
	[ProductPrice] [int] NOT NULL,
 CONSTRAINT [DF_Products_ProductName_Unique] UNIQUE NONCLUSTERED 
(
	[ProductName] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Products] 
ADD  CONSTRAINT PK_Products_ProductID PRIMARY KEY CLUSTERED (ProductID)
GO

ALTER TABLE [dbo].[Shops] 
ADD  CONSTRAINT PK_Products_ShopID PRIMARY KEY CLUSTERED (ShopID)
GO

INSERT INTO Shops (ShopName) VALUES ('IKEA');
GO
INSERT INTO Shops (ShopName) VALUES ('Silpo');
GO
INSERT INTO Shops (ShopName) VALUES ('Ashan');
GO
INSERT INTO Shops (ShopName) VALUES ('Toswa');
GO
INSERT INTO Shops (ShopName) VALUES ('Bion');
GO
INSERT INTO Shops (ShopName) VALUES ('Metro');
GO

INSERT INTO Products(ProductName, ProductPrice) VALUES ('Milk', 25);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Slime', 15);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Sugar', 12);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Iphone', 1);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Asus24', 240);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Bread', 20);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Chocolate', 30);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Briliant', 1000);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Knife', 46);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Book', 50);
GO
INSERT INTO Products(ProductName, ProductPrice) VALUES ('Ball', 10);
GO

/****** Object:  Table [dbo].[Availabil]    Script Date: 06.09.2021 6:11:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Availabil]') AND type in (N'U'))
DROP TABLE [dbo].[Availabil]
GO

/****** Object:  Table [dbo].[Availabil]    Script Date: 06.09.2021 6:11:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Availabil](
	[ShopID] [int] NULL,
	[ProductID] [int] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Availabil]
WITH CHECK ADD CONSTRAINT FK_Availabil_ShopID FOREIGN KEY(ShopID)
REFERENCES [dbo].[Shops] (ShopID)
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Availabil]
WITH CHECK ADD CONSTRAINT FK_Availabil_ProductID FOREIGN KEY(ProductID)
REFERENCES [dbo].[Products] (ProductID)
ON UPDATE CASCADE
ON DELETE CASCADE
GO

INSERT INTO Availabil(ShopID,ProductID) VALUES ('1', '1');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('1', '2');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('2', '3');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('3', '4');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('3', '5');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('3', '6');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('4', '7');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('5', '8');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('5', '9');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('5', '10');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('6', '11');
GO

ALTER TABLE [dbo].[WantedList]
ADD WantedNum INT IDENTITY(1,1)
GO 

ALTER TABLE [dbo].[WantedList]
ADD CONSTRAINT PK_WantedList_WantedNum PRIMARY KEY CLUSTERED (WantedNum)
GO

ALTER TABLE [dbo].[WantedList]
WITH CHECK ADD CONSTRAINT FK_WantedList_ProductID FOREIGN KEY(ProductID)
REFERENCES [dbo].[Products] (ProductID)
ON UPDATE CASCADE
ON DELETE CASCADE
GO