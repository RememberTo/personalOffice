namespace PersonalOffice.Backend.Application.Common.Global
{
    /// <summary>
    /// Строковые литералы названий микросервисов
    /// </summary>
    public static class MicroserviceNames
    {
        /// <summary>
        /// Название очереди для текущего проекта
        /// </summary>
        public static string Backend { get; set; } = "PO_API"; //по умолчанию, название задается в PersonalOffice.Backend.API/Program.cs
        /// <summary>
        /// Название очереди для MicroserviceDbConnector
        /// </summary>
        public static readonly string DbConnector = "DbConnector";
        /// <summary>
        /// Название очереди для MicroserviceManagerQuestion
        /// </summary>
        public static readonly string ManagerQuestion = "ManagerQuestion";
        /// <summary>
        /// Название очереди для MicroserviceFileManager
        /// </summary>
        public static readonly string FileManager  = "FileManager";
        /// <summary>
        /// Название очереди для MicroserviceAuthenticator
        /// </summary>
        public static readonly string Authenticator = "Authenticator";
        /// <summary>
        /// Название очереди для MicroserviceReportMaster
        /// </summary>
        public static readonly string ReportMaster = "ReportMaster";
        /// <summary>
        /// Название очереди для MicroservicePOClientOrder
        /// </summary>
        public static readonly string ClientOrder = "POClientOrder";
        /// <summary>
        /// Название очереди для MicroserviceFIAS
        /// </summary>
        public static readonly string Fias = "FIAS";
        /// <summary>
        /// Название очереди для MicroserviceSms
        /// </summary>
        public static readonly string Sms = "Sms";
        /// <summary>
        /// Название очереди для MicroserviceMailSender
        /// </summary>
        public static readonly string MailSender = "MailSender";
        /// <summary>
        /// Название очереди для MicroservicePONotify
        /// </summary>
        public static readonly string Notification = "PONotify";
        /// <summary>
        /// Название очереди для MicroserviceOneTimePass
        /// </summary>
        public static readonly string Otp = "OneTimePass";
        /// <summary>
        /// Название очереди для MicroserviceMobilePush
        /// </summary>
        public static readonly string Push = "MobilePush";
        /// <summary>
        /// Название очереди для MicroserviceUPRID
        /// </summary>
        public static readonly string Uprid = "UPRID";
        /// <summary>
        /// Название очереди для MicroserviceCRM
        /// </summary>
        public static readonly string Crm = "CRM";
        /// <summary>
        /// Название очереди для MicroserviceNSPK
        /// </summary>
        public static readonly string NSPK = "NSPK";
        //#if DEBUG 
        //        public static string Backend { get; set; }

        //        public static readonly string DbConnector = Environment.UserName.ToUpper() + "_DB";
        //        public static readonly string ManagerQuestion = Environment.UserName.ToUpper() + "_MNQ";
        //        public static readonly string FileManager = Environment.UserName.ToUpper() + "_FM";
        //        public static readonly string Authenticator = Environment.UserName.ToUpper() + "_AU";
        //        public static readonly string ReportMaster = Environment.UserName.ToUpper() + "_RP";
        //        public static readonly string ClientOrder = Environment.UserName.ToUpper() + "_POCO";
        //        public static readonly string Fias = Environment.UserName.ToUpper() + "_FI";
        //        public static readonly string Sms = Environment.UserName.ToUpper() + "_SMS";
        //        public static readonly string MailSender = Environment.UserName.ToUpper() + "_MS";
        //        public static readonly string Notification = Environment.UserName.ToUpper() + "_NF";
        //        public static readonly string Otp = Environment.UserName.ToUpper() + "_OTP";
        //        public static readonly string Push = Environment.UserName.ToUpper() + "_MOBILE_PUSH";
        //        public static readonly string Uprid = Environment.UserName.ToUpper() + "_UPRID";
        //        public static readonly string Crm = Environment.UserName.ToUpper() + "_CRM";
        //        public static readonly string NSPK = "NSPK_" + Environment.UserName.ToUpper() + "_" + Environment.MachineName;
        //        public static readonly string PO_Backend = Environment.UserName.ToUpper() + "_PO";
        //#else
        //#endif
    }
}
