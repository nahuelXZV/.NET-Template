CREATE SCHEMA Seguridad;
GO
CREATE TABLE [Seguridad].[Usuario] (
  [id] bigint IDENTITY(1,1) NOT NULL,
  [nombre] varchar(255) COLLATE Modern_Spanish_CI_AS  NOT NULL,
  [apellido] varchar(255) COLLATE Modern_Spanish_CI_AS  NULL,
  [email] varchar(255) COLLATE Modern_Spanish_CI_AS  NOT NULL,
  [password] varchar(255) COLLATE Modern_Spanish_CI_AS  NOT NULL,
  [rol] varchar(50) COLLATE Modern_Spanish_CI_AS  NOT NULL,
  PRIMARY Key (id)
)  
ON [PRIMARY]
GO

ALTER TABLE [Seguridad].[Usuario] SET (LOCK_ESCALATION = TABLE)