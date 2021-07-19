CREATE FUNCTION [dbo].[PagNet_RetOcorrenciaRetorno]   (@codigo varchar(10))
RETURNS VARCHAR(8000)

AS

BEGIN

    if (@codigo is null)    return ''
    if (@codigo = '')       return ''

      DECLARE @msgOcorrencia VARCHAR(255)
      DECLARE @retorno VARCHAR(8000)
      set @retorno = ''
      
    while (LEN(@codigo) > 0)
    BEGIN
         SELECT @msgOcorrencia = PG.Descricao FROM PAGNET_OCORRENCIARETPAG PG  where pg.codOcorrenciaRetPag = SUBSTRING(@codigo, 1, 2)
         
         if (len(@retorno) > 0)         
            select @retorno = @retorno + '; ' + ltrim(rtrim(@msgOcorrencia))
         else         
            select @retorno = ltrim(rtrim(@msgOcorrencia))

         set @codigo = SUBSTRING(@codigo, 3, LEN(@codigo) -  2)
         
    END

    return @retorno
END

