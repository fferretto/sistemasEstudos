namespace PagNet.Bld.PGTO.Santander.Web.Setup
{
    public static class ServiceConsts
    {
        // Constantes de configuração de autorização de acesso.
        public static class Authorization
        {
            public const string AppIdClaimType = "app_id";
            public const string RestrictPersonalCardAppAccessPolicy = "PersonalCardRestrictAppAccess";
        }

        // Constantes de chaves de configuração da aplicação.
        public static class Configuration
        {
            // Chave com o identificador único da chave da aplicação Personal Card  no servidor de autorização.
            public const string AppId = "PersonalCard:AppId";
        }
    }
}
