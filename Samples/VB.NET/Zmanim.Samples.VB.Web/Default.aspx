<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Zmanim.Samples.VB.Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Zmanim for Lakewood, NJ<br />
        'using lamda expression<br /><br /><br />
        Sunrise:
        <%= GetZman(Function(z) z.getSunrise).ToShortTimeString%><br />
        Sunset:
        <%= GetZman(Function(z) z.getSunset).ToShortTimeString%><br />
        Tzais: <%= GetZman(Function(z) z.getTzais).ToShortTimeString%>
        <br />
        <br />
        'using method calls<br />
        Sunrise:
        <%= GetSunrise().ToShortTimeString%><br />
        Sunset:
        <%= GetSunset().ToShortTimeString%>
    </div>
    </form>
</body>
</html>
