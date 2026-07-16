using System;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SC1300BO 的摘要描述
/// </summary>
public class CFB2SC1300BO : BaseService
{
	public CFB2SC1300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string updateData(CFB2SC1300DAO fb2sc)
    {
        string rtnmessage = "";
        try
        {

            //檢查OK更新
            if (rtnmessage == "")
            {

                BeginTransaction();
                try
                {
                    fb2sc.updateData();

                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}