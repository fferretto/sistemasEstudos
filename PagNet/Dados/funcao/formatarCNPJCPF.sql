CREATE FUNCTION [dbo].[formatarCNPJCPF]   (@CnpjCpf char(14))
RETURNS CHAR(18)

AS

BEGIN
      DECLARE @retorno VARCHAR(18)

      IF LEN(@CnpjCpf)= 14 
          --CNPJ
           BEGIN
                SET @retorno = substring(@CnpjCpf ,1,2) + '.' + substring(@CnpjCpf ,3,3) + '.' + substring(@CnpjCpf ,6,3) + '/' + substring(@CnpjCpf ,9,4) + '-' + substring(@CnpjCpf ,13,2)
           END

   ELSE
         --CPF
           BEGIN

                SET @retorno = substring(@CnpjCpf ,1,3) + '.' + substring(@CnpjCpf ,4,3) + '.' + substring(@CnpjCpf ,7,3) + '-' + substring(@CnpjCpf ,10,2) 

           END

      RETURN @retorno
END