CREATE TABLE [User]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Username] VARCHAR(50) NOT NULL, 
    [Salt] VARCHAR(50) NOT NULL, 
    [HashedPassword] VARCHAR(50) NOT NULL, 
    [RefreshToken] VARCHAR(50) NULL, 
    [Role] VARCHAR(50) NULL
)