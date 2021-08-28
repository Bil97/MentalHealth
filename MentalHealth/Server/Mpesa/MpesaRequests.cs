using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MentalHealth.Server.Mpesa
{
    public class MpesaRequests
    {
        private readonly MpesaData _mpesa = new MpesaData();

        public string AccessToken()
        {
            var client = new RestClient("https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials");

            var request = new RestRequest(Method.GET);

            var key = _mpesa.ConsumerKey + ":" + _mpesa.ConsumerSecret;

            var keyBytes = Encoding.UTF8.GetBytes(key);

            var test = Convert.ToBase64String(keyBytes);

            request.AddHeader("Authorization", "Basic " + test);

            IRestResponse response = client.Execute(request);

            try { return JObject.Parse(response.Content)["access_token"].ToString(); }
            catch { return "Invalid Token"; }
        }

        public Task<string> B2C(string accessToken, string amount, string phoneNumber, string resultUrl = null)
        {
            if (string.IsNullOrEmpty(resultUrl))
            {
                resultUrl = _mpesa.ResultUrl;
            }
            var client = new RestClient("https://sandbox.safaricom.co.ke/mpesa/b2c/v1/paymentrequest");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\r\n    " +
                "\"InitiatorName\": \"" + _mpesa.InitiatorName + "\",\r\n    " +
                "\"SecurityCredential\": \"" + _mpesa.SecurityCredential + "\",\r\n    " +
                "\"CommandID\": \"BusinessPayment\",\r\n    " +
                "\"Amount\": \"" + amount + "\",\r\n    " +
                "\"PartyA\": \"" + _mpesa.ShortCode + "\",\r\n    " +
                "\"PartyB\": \"" + phoneNumber + "\",\r\n    " +
                "\"Remarks\": \"" + _mpesa.Remarks + "\",\r\n    " +
                "\"QueueTimeOutURL\": \"" + resultUrl + "\",\r\n    " +
                "\"ResultURL\": \"" + resultUrl + "\",\r\n\t" +
                "\"Occasion\": \"" + _mpesa.Occassion + "\"\r\n}",

                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return Task.FromResult(response.Content);
        }

        public string B2B(string accessToken, string amount, string receiverShortCode, string resultUrl = null)
        {
            if (string.IsNullOrEmpty(resultUrl))
            {
                resultUrl = _mpesa.ResultUrl;
            }
            var client = new RestClient("https://sandbox.safaricom.co.ke/mpesa/b2b/v1/paymentrequest");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\r\n    " +
                "\"Initiator\": \"" + _mpesa.InitiatorName + "\",\r\n    " +
                "\"SecurityCredential\": \"" + _mpesa.SecurityCredential + "\",\r\n    " +
                "\"CommandID\": \"BusinessPayBill\",\r\n    " +
                "\"SenderIdentifierType\": \"4\",\r\n    " +
                "\"RecieverIdentifierType\": \"4\",\r\n    " +
                "\"Amount\": \"" + amount + "\",\r\n    " +
                "\"PartyA\": \"" + _mpesa.ShortCode + "\",\r\n    " +
                "\"PartyB\": \"" + receiverShortCode + "\",\r\n    " +
                "\"AccountReference\": \"" + _mpesa.AccountReference + "\",\r\n    " +
                "\"Remarks\": \"" + _mpesa.Remarks + "\",\r\n    " +
                "\"QueueTimeOutURL\": \"" + resultUrl + "\",\r\n    " +
                "\"ResultURL\": \"" + resultUrl + "\"\r\n}",

                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public string C2BRegister(string accessToken, string resultUrl = null)
        {
            if (string.IsNullOrEmpty(resultUrl))
            {
                resultUrl = _mpesa.ResultUrl;
            }
            var client = new RestClient("https://sandbox.safaricom.co.ke/mpesa/c2b/v1/registerurl");

            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Bearer " + accessToken);

            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("undefined", "{\r\n    " +

                "\"ShortCode\": \"" + _mpesa.ShortCode + "\",\r\n    " +

                "\"ResponseType\": \"Completed\",\r\n    " +

                "\"ConfirmationURL\": \"" + resultUrl + "\",\r\n    " +

                "\"ValidationURL\": \"" + resultUrl + "\"\r\n}",

                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public string C2BSimulate(string accessToken, string amount, string phoneNumber)
        {
            var client = new RestClient("https://sandbox.safaricom.co.ke/mpesa/c2b/v1/simulate");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\r\n    " +
                "\"ShortCode\":\"" + _mpesa.ShortCode + "\",\r\n    " +
                "\"CommandID\":\"CustomerPayBillOnline\",\r\n    " +
                "\"Amount\":\"" + amount + "\",\r\n    " +
                "\"Msisdn\":\"" + phoneNumber + "\",\r\n    " +
                "\"BillRefNumber\":\"0000\"\r\n}",

                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public Task<string> LipaNaMpesaOnline(string accessToken, string amount, string phoneNumber, string resultUrl = null)
        {

            if (string.IsNullOrEmpty(resultUrl))
            {
                resultUrl = _mpesa.ResultUrl;
            }
            var client = new RestClient("https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest");

            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            byte[] bytes = Encoding.UTF8.GetBytes(_mpesa.BusinessShortCode + _mpesa.PassKey + timestamp);
            string password = Convert.ToBase64String(bytes);

            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Bearer " + accessToken);

            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("undefined", "{\r\n    " +
                "\"BusinessShortCode\": \"" + _mpesa.BusinessShortCode + "\",\r\n    " +
                "\"Password\": \"" + password + "\",\r\n    " +
                "\"Timestamp\": \"" + timestamp + "\",\r\n    " +
                "\"TransactionType\": \"CustomerPayBillOnline\",\r\n    " +
                "\"Amount\": \"" + amount + "\",\r\n    " +
                "\"PartyA\": \"" + phoneNumber + "\",\r\n    " +
                "\"PartyB\": \"" + _mpesa.BusinessShortCode + "\",\r\n    " +
                "\"PhoneNumber\": \"" + phoneNumber + "\",\r\n    " +
                "\"CallBackURL\": \"" + resultUrl + "\",\r\n    " +
                "\"AccountReference\": \"" + _mpesa.AccountReference + "\",\r\n    " +
                "\"TransactionDesc\": \"" + _mpesa.TransactionDescription + "\"\r\n}",

                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return Task.FromResult(response.Content);
        }

        public Task<string> LipaNaMpesaQuery(string accessToken, string checkoutRequestId)
        {
            var client = new RestClient("https://sandbox.safaricom.co.ke/mpesa/stkpushquery/v1/query");

            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            byte[] bytes = Encoding.UTF8.GetBytes(_mpesa.BusinessShortCode + _mpesa.PassKey + timestamp);
            string password = Convert.ToBase64String(bytes);

            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Bearer " + accessToken);

            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("undefined", "{\r\n    " +

                "\"BusinessShortCode\": \"" + _mpesa.BusinessShortCode + " \" ,\r\n    " +

                "\"Password\": \"" + password + " \",\r\n    " +

                "\"Timestamp\": \" " + timestamp + "\",\r\n    " +

                "\"CheckoutRequestID\": \"" + checkoutRequestId + " \"\r\n}",

                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return Task.FromResult(response.Content);
        }

        public string Reversal(string accessToken, string amount, string transactionId, string receiverShortCode, string resultUrl = null)
        {
            if (string.IsNullOrEmpty(resultUrl))
            {
                resultUrl = _mpesa.ResultUrl;
            }
            var client = new RestClient("https://sandbox.safaricom.co.ke/mpesa/reversal/v1/request");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\r\n    " +
                "\"Initiator\": \"" + _mpesa.InitiatorName + "\",\r\n    " +
                "\"SecurityCredential\": \"" + _mpesa.SecurityCredential + "\",\r\n    " +
                "\"CommandID\": \"TransactionReversal\",\r\n    " +
                "\"TransactionID\": \"" + transactionId + "\",\r\n    " +
                "\"Amount\": \"" + amount + "\",\r\n    " +
                "\"ReceiverParty\": \"" + receiverShortCode + "\",\r\n    " +
                "\"RecieverIdentifierType\": \"4\",\r\n    " +
                "\"ResultURL\": \"" + resultUrl + "\",\r\n    " +
                "\"QueueTimeOutURL\": \"" + resultUrl + "\",\r\n    " +
                "\"Remarks\": \"" + _mpesa.Remarks + "\",\r\n    " +
                "\"Occasion\": \"" + _mpesa.Occassion + "\"\r\n}",

                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public string TranscationStatus(string accessToken, string transactionId, string resultUrl = null)
        {
            if (string.IsNullOrEmpty(resultUrl))
            {
                resultUrl = _mpesa.ResultUrl;
            }
            var client = new RestClient("https://sandbox.safaricom.co.ke/mpesa/transactionstatus/v1/query");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\r\n    " +
                "\"Initiator\": \"" + _mpesa.InitiatorName + "\",\r\n    " +
                "\"SecurityCredential\": \"" + _mpesa.SecurityCredential + "\",\r\n    " +
                "\"CommandID\": \"TransactionStatusQuery\",\r\n    " +
                "\"TransactionID\": \"" + transactionId + "\",\r\n    " +
                "\"PartyA\": \"" + _mpesa.ShortCode + "\",\r\n    " +
                "\"IdentifierType\": \"1\",\r\n    " +
                "\"ResultURL\": \"" + resultUrl + "\",\r\n    " +
                "\"QueueTimeOutURL\": \"" + resultUrl + "\",\r\n    " +
                "\"Remarks\": \"" + _mpesa.Remarks + "\",\r\n    " +
                "\"Occasion\": \"" + _mpesa.Occassion + "\"\r\n}",

                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public string AccountBalance(string accessToken, string resultUrl = null)
        {
            if (string.IsNullOrEmpty(resultUrl))
            {
                resultUrl = _mpesa.ResultUrl;
            }
            var client = new RestClient("https://sandbox.safaricom.co.ke/mpesa/accountbalance/v1/query");

            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Bearer " + accessToken);

            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("undefined", "{\r\n    " +

                "\"Initiator\": \"" + _mpesa.InitiatorName + "\",\r\n    " +

                "\"SecurityCredential\": \"" + _mpesa.SecurityCredential + "\",\r\n    " +

                "\"CommandID\": \"AccountBalance\",\r\n    " +

                "\"PartyA\": \"" + _mpesa.ShortCode + "\",\r\n    " +

                "\"IdentifierType\": \"4\",\r\n    " +

                "\"Remarks\": \"" + _mpesa.Remarks + "\",\r\n    " +

                "\"QueueTimeOutURL\": \"" + resultUrl + "\",\r\n    " +

                "\"ResultURL\": \"" + resultUrl + "\"\r\n}",

                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

    }

}
