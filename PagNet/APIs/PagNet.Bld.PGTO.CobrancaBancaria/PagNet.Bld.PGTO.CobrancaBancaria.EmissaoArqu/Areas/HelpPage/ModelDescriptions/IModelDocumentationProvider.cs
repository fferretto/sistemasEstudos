using System;
using System.Reflection;

namespace PagNet.Bld.PGTO.CobrancaBancaria.EmissaoArqu.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}