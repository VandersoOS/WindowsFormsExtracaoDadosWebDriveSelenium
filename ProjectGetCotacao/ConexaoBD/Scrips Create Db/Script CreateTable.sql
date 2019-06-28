Use CotacaoBD
-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Cotacao'
CREATE TABLE [dbo].[Cotacao] (
    [CotacaoID] int IDENTITY(1,1) NOT NULL,
    [Valor] decimal(4,2)  NOT NULL,
    [DataInclusao] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CotacaoID] in table 'Cotacao'
ALTER TABLE [dbo].[Cotacao]
ADD CONSTRAINT [PK_Cotacao]
    PRIMARY KEY CLUSTERED ([CotacaoID] ASC);
GO
