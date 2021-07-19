using System;

public partial class Logout : TelenetPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.RemoveSession();
        Session.Abandon();
    }
}
