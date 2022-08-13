CREATE TABLE Customers(
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(50)
);

INSERT INTO Customers
VALUES
  (1, "Max"),
  (2, "Pavel"),
  (3, "Ivan"),
  (4, "Leonid");

CREATE TABLE Orders(
Id INTEGER PRIMARY KEY AUTOINCREMENT,
CustomerID INTEGER
);

INSERT INTO Orders
VALUES
  (1, 2),
  (2, 4);

SELECT name FROM Customers C
LEFT JOIN Orders O ON C.id = O.CustomerID WHERE O.CustomerID ISNULL