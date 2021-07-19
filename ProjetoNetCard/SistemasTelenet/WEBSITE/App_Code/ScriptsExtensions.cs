using System;
using System.IO;
using System.Web.UI;

/// <summary>
/// Summary description for ScriptsExtensions
/// </summary>
public static class ScriptsExtensions
{
    public static string ResolveVersionScript(this Page page, string scriptName)
    {
        var versao = "v1";
        var versionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "site.version");
        if (File.Exists(versionFile))
            versao = "v" + File.ReadAllText(versionFile).Replace(Environment.NewLine, "").Trim();

        #if DEBUG
            return page.ResolveUrl(string.Format("~/ILL/{0}", scriptName));
        #endif

        return page.ResolveUrl(string.Format("~/ILL/{0}/{1}", versao, scriptName));
    }
}