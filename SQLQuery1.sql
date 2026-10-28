CREATE TABLE Products
(
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName VARCHAR(100) NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL
);
GO
INSERT INTO Products (ProductName, Quantity, Price)
VALUES
('Keyboard', 20, 500.00),
('Mouse', 30, 250.00),
('Monitor', 10, 7500.00);
Select * from Products;