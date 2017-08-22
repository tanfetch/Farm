using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farm.AppCommon
{
    public static class JSHelper
    {
        public static string ShowError(string msg)
        {
            if(!string.IsNullOrEmpty(msg))
                msg = msg.Replace("'", "\"").Replace("\n","").Replace("\r","");

            return string.Format("showError('{0}');", msg);
        }

        public static string ShowSuccess(string msg = "操作成功")
        {
            return string.Format("showSuccess('{0}');", msg);
        }

        public static BaseJsonMessage JsonMessage(string message, bool success=false, int statusFlag=0)
        {
            BaseJsonMessage js = new BaseJsonMessage
            {
                success = success,
                message = message,
                statusFlag = statusFlag
            };

            return js;
        }
    }

    public class BaseJsonMessage
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int statusFlag { get; set; }
    }
}
