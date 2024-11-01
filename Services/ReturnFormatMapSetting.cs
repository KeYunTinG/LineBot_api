namespace LineBot_api.Services
{
    public class ReturnFormatMapSetting
    {
        private readonly static Dictionary<string, string> returnFormatMapping;

        static ReturnFormatMapSetting()
        {
            returnFormatMapping = new Dictionary<string, string>
            {
                //通用
                {"0000","成功" },
                {"9999","失敗" },
                //登入
                {"9101","密碼逾時" },
                {"9102","第一次登入" },
                {"9103","帳號已重覆" },
                {"9104","授權失敗" },
                {"9105","密碼強度不符" },
                {"9106","密碼超過異動週期限制" },
                {"9107","密碼跟之前重覆" },
                {"9108","查無此帳號" },
                {"9109","登入超過錯誤次數" },
                {"9100","不可包含使用者的帳號名稱" },
                {"9111","不可包含使用者的帳號名稱" },
                //密碼規則
                {"9201","密碼不可空白" },
                {"9202","密碼非零正整數" },
                //資料庫
                {"1001", "查無資料"},
                {"1002","資料未更新" },
                //others
                {"8888","External Api Error" },
                {"9900","驗證錯誤" },
                {"9901","驗證錯誤-權限不足" },
                {"9001","驗證錯誤-使用者不存在" },
                {"9002","驗證錯誤-帳號密碼錯誤" },
                {"2001","資料異常-請立刻修改" },
                {"1100","違反資料庫規則" }
            };
        }

        public static string ConvertToMessage(string input)
        {
            if (returnFormatMapping.TryGetValue(input, out string? code))
            {
                return code;
            }
            else
            {
                throw new ArgumentException("Return Code格式錯誤");
            }
        }
    }
}
