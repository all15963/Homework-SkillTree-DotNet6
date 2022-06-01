namespace MVCHomework6.Extensions
{
    public static class IntExtension
    {
        /// <summary>
        /// 將null或小於等於0的數字回傳1，不然就傳原數字
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int DoTryGetNumber(this int? x)
        {
            var result = x ?? 1;
            if (result <= 0)
                result = 1;
            return result;
        }
    }
}
