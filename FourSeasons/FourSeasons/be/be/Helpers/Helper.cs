namespace be.Helpers
{
    public static class Helper
    {
        /// <summary>
        /// convert normal string to encode string
        /// </summary>
        /// <param name="password">string that wnat to encoded</param>
        /// <returns>encoded string</returns>
        /// <exception cref="Exception"></exception>
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        /// <summary>
        /// this function Convert to Decord your Password
        /// </summary>
        /// <param name="encodedData">password that encoded</param>
        /// <returns>normal string</returns>
        public static string DecodeFrom64(string encodedData)
        {
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            string result = System.Text.Encoding.UTF8.GetString(todecode_byte);
            return result;
        }
    }
}
