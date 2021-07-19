create function dbo.PagNet_ValorTotalTaxasPagar(@codTitulo int)  returns decimal(13,2)
as begin return(

    select ISNULL(sum(VALOR),0) from PAGNET_TAXAS_TITULOS WHERE CODTAXATITULO = @codTitulo

)
end