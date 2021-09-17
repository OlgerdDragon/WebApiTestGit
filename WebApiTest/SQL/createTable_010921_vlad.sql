CREATE TABLE Shops (
    ShopID int,
    ShopName varchar(255)
);
GO
CREATE TABLE Products (
    ProductID int,
    ProductName varchar(255),
	ProductPrice int,
);
GO
CREATE TABLE Availabil (
    ShopID int,
    ProductID int
);
GO
CREATE TABLE WantedList (
    ProductID int,
    StatusBought int
);
GO

INSERT INTO Shops (ShopID, ShopName) VALUES ('0','IKEA');
GO
INSERT INTO Shops (ShopID, ShopName) VALUES ('1','Silpo');
GO
INSERT INTO Shops (ShopID, ShopName) VALUES ('2','Ashan');
GO
INSERT INTO Shops (ShopID, ShopName) VALUES ('3','Toswa');
GO
INSERT INTO Shops (ShopID, ShopName) VALUES ('4','Bion');
GO
INSERT INTO Shops (ShopID, ShopName) VALUES ('5','Metro');
GO

INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('0','Milk', 25);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('1','Slime', 15);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('2', 'Sugar', 12);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('3','Iphone', 1);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('4','Asus24', 240);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('5','Bread', 20);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('6','Chocolate', 30);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('7','Briliant', 1000);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('8','Knife', 46);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('9','Book', 50);
GO
INSERT INTO Products(ProductID, ProductName, ProductPrice) VALUES ('10','Ball', 10);
GO

INSERT INTO Availabil(ShopID,ProductID) VALUES ('0', '0');
GO
INSERT INTO Availabil(ShopID,ProductID) VALUES ('0', '1');
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
