using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.Net;

namespace ussdtester
{
    class Program
    {
        static void Main(string[] args)
        {
            PayBill();
        }

        private static void PayBill()
        {
            ussdapi.Response resp = new ussdapi.Response();
            resp.Action = "start"; 
            ussdapi.Request r = new ussdapi.Request();
            ussdapi.PegasusUssd ussd = new ussdapi.PegasusUssd();
            Random rnd = new Random();
            r.Action = "CONTINUE";
            r.CalledNmber = "*272*2#";
            r.ClientId = "256787219625";
            r.Network = "MTN";
            r.RequestString = "CONTINUE";
            r.ShortCode = "*272*2#";//"*272*3#";// ;//"*290*1#" //"*272*101#"
            r.TrandactionDate = DateTime.Now.ToString();
            r.TransactionId = createID();//rnd.Next(00000, 99999).ToString();


            while (!resp.Action.Equals("end"))
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidation;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ussdapi.Response res = new ussdapi.Response();
                ussd.Timeout = 10000000;

                ussd.Url = @"http://localhost:33748/PegasusUssd.asmx?WSDL";
                //ussd.Url = @"https://192.168.120.44:8019/GenericUssd/PegasusUssd.asmx?WSDL";

                resp = ussd.ProcessUssdRequest(r);
                Console.WriteLine(resp.ResponseString);
                r.RequestString = Console.ReadLine();

            }
        }

        public static string createID() {

            Random r = new Random();
            int length = r.Next(16, 20);

            const string valid = "0!@#$%^&*?/abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--) {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            string newPassword = res.ToString();
                    
            return newPassword;

        }
        private static bool RemoteCertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
