namespace PagNet.Interface.Helpers
{
    public class Cookies
    {
        public static string GetParamCookie(string valor, string cookie)
        {
            var nameEQ = valor + "=";
            var vcookie = cookie;

            if (vcookie.IndexOf(";") >= 0)
            {
                var vcookie2 = vcookie.Split(';');
                vcookie = vcookie2[1];
            }
            if (vcookie.IndexOf("DadosLogin=") >= 0)
            {
                vcookie = vcookie.Replace("DadosLogin=", "");
            }
            var ca = vcookie.Split('&');
            for (var i = 0; i < ca.Length; i++)
            {
                var c = ca[i];
                //while (c.charAt(0) == ' ') c = c.Substring(1, c.Length);
                if (c.IndexOf(nameEQ) == 0) return c.Substring(nameEQ.Length, (c.Length - nameEQ.Length));
            }
            return null;

        }
    }
}