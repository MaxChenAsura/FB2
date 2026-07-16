<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // 在應用程式啟動時執行的程式碼
        //設定log檔
        string log4netPath = Server.MapPath("~/log4net.config");
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(log4netPath));

        //設定共用訊息
        XElement xml = XElement.Load(Server.MapPath("~/Message.xml"));
        Dictionary<string, string> message = new Dictionary<string, string>();
        foreach (XElement el in xml.Elements())
            message.Add(el.Name.LocalName, el.Value);
        Application["Message"] = message;
        
       
    }

    void Application_End(object sender, EventArgs e)
    {
        //  在應用程式關閉時執行的程式碼

    }

    void Application_Error(object sender, EventArgs e)
    {
        // 在發生未處理的錯誤時執行的程式碼

    }

    void Session_Start(object sender, EventArgs e)
    {
        // 在新的工作階段啟動時執行的程式碼
        //Session["CurrentUI"] = System.Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag;
    }

    void Session_End(object sender, EventArgs e)
    {
        // 在工作階段結束時執行的程式碼
        // 注意: 只有在  Web.config 檔案中將 sessionstate 模式設定為 InProc 時，
        // 才會引起 Session_End 事件。如果將 session 模式設定為 StateServer 
        // 或 SQLServer，則不會引起該事件。

    }
       
</script>
