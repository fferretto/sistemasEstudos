using System.Text;
using System.Web;

namespace SIL
{
    public static class HttpResponseExtrensions
    {
        public static void SendUnexpectedExceptionPage(this HttpResponse self)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            builder.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=UTF8\">");
            builder.AppendLine();
            builder.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            builder.AppendLine("<body>");
            builder.AppendLine("    <h2 style=\"color:red\">Ocorreu uma falha inesperada!</h2>");
            builder.AppendLine("    <p>Informações foram coletadas para resolver o problema. Entretanto, você precisará reiniciar o sistema.</p>");
            builder.AppendLine("    <p>Caso o problema persista entre em contato com o administrador do sistema.</p>");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            self.ClearContent();
            self.Output.Write(builder.ToString());
        }
    }
}
