using Khipu.Api;
using Khipu.Client;
using Khipu.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using desafiocoaniquem.Models;
using desafiocoaniquem.Services;

namespace desafiocoaniquem.Controllers
{
    public class KhipuController : Controller
    {
        private IConfiguration _config;
        private readonly IMailService _mailService;

        public KhipuController(IConfiguration iConfig, ILogger<KhipuController> logger, IMailService _MailService)
        {
            _config = iConfig;
            _mailService = _MailService;
        }

        // GET: Flow
        //public ActionResult Init(string m, string r, string c, string e) //r: Referencia, ch: Canal, m:Monto
        public ActionResult Init(string m, string r, string c, string e) //r: Referencia, ch: Canal, m:Monto
        {
            Khipu.Client.Configuration.ReceiverId = Convert.ToInt32(_config["AppSettings:khipu.ReceiverId"]);
            Khipu.Client.Configuration.Secret = _config["AppSettings:khipu.Secret"];
            PaymentsApi a = new PaymentsApi();
            m = m.Replace("$", "").Replace(".", "");
            int monto = Convert.ToInt32(m);
            //string baseUrl = "https" + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');

            string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            try
            {
                DateTime dt = DateTime.Now;
                dt = dt.AddDays(1);
                string buyOrder = Convert.ToString(db.insertTransaction(m, c, r, "", "", e));
                PaymentsCreateResponse response = a.PaymentsPost(
                    "COANIQUEM",
                    "CLP",
                    monto,
                    transactionId: buyOrder,
                    expiresDate: dt,
                    body: "Por concepto de donación",
                    returnUrl: baseUrl + "/Khipu/Response?i=" + buyOrder,
                    cancelUrl: baseUrl + "/Khipu/Error?i=" + buyOrder,
                    notifyUrl: "",
                    notifyApiVersion: "1.3"
                 );

                //Ingresamos el TOKEN
                db.updateTransactionToken(buyOrder, response.PaymentId);
                return Redirect(response.PaymentUrl);
            }
            catch (ApiException)
            {
                //TODO controlar acá
            }

            return View();
        }

        public ActionResult Error(string i)//i:id transacción en BDD
        {
            //Obtenemos el encabezado de la transacción
            Khipu.Client.Configuration.ReceiverId = Convert.ToInt32(_config["AppSettings:khipu.ReceiverId"]);
            Khipu.Client.Configuration.Secret = _config["AppSettings:khipu.Secret"];

            //Obtenemos el encabezado de la transacción
            desafiocoaniquem.Models.Transaction transaction = db.getRequestTransaction(i);

            string notificationToken = transaction.RequestToken;
            double amount = transaction.RequestAmount;

            string responseString = null, responseCode, authorizationCode;

            PaymentsApi a = new PaymentsApi();
            try
            {
                PaymentsResponse response = a.PaymentsIdGet(transaction.RequestToken);
                responseString = JsonSerializer.Serialize(response);
                if (response.ReceiverId.Equals(Khipu.Client.Configuration.ReceiverId)
                       && response.Status.Equals("done") && response.Amount == amount)
                {
                    responseCode = response.Status;
                    authorizationCode = "EXTERNAL_ERROR";
                }
                else
                {
                    responseCode = "EXTERNAL_ERROR";
                    authorizationCode = "EXTERNAL_ERROR";
                }

            }
            catch (ApiException e)
            {
                responseCode = "ERROR";
                authorizationCode = "ERROR";
                responseString = e.Message;
            }

            //Guardamos la data del response
            db.updateTransactionResponse(i, responseCode, authorizationCode, responseString);

            return RedirectToAction("Error", "Views");
        }

        public ActionResult Response(string i, string api_version, string notification_token)//i:id transacción en BDD
        {
            //Obtenemos el encabezado de la transacción
            Khipu.Client.Configuration.ReceiverId = Convert.ToInt32(_config["AppSettings:khipu.ReceiverId"]);
            Khipu.Client.Configuration.Secret = _config["AppSettings:khipu.Secret"];

            //Obtenemos el encabezado de la transacción
            desafiocoaniquem.Models.Transaction transaction = db.getRequestTransaction(i);

            //string notificationToken = transaction.RequestToken;
            double amount = transaction.RequestAmount;

            string responseString = null, responseCode, authorizationCode;

            PaymentsApi a = new PaymentsApi();
            try
            {
                PaymentsResponse response = a.PaymentsIdGet(transaction.RequestToken);
                responseString = JsonSerializer.Serialize(response);
                if (response.ReceiverId.Equals(Khipu.Client.Configuration.ReceiverId)
                       && response.Status.Equals("done") && response.Amount == amount)
                {
                    responseCode = "0";
                    authorizationCode = response.PayerEmail;
                }
                else
                {
                    responseCode = response.Status;
                    authorizationCode = response.StatusDetail;
                }

            }
            catch (ApiException e)
            {
                responseCode = "ERROR";
                authorizationCode = "ERROR";
                responseString = e.Message;
            }

            //Guardamos la data del response
            db.updateTransactionResponse(i, responseCode, authorizationCode, responseString);

            //Envíamos el mail de respuesta
            if (transaction.Email != null)
            {
                MailData mail = new MailData
                {
                    EmailToName = "Donante",
                    EmailSubject = "COANIQUEM | Gracias por tu aporte",
                    EmailBody = "<div style='width:100%; text-align:center'><img src='" + GlobalConfiguration.EmailImage() + "' width='650px' /><div style='text-align:center'><h5>Este es un correo automático por favor no responder<h5/><span>Para más información visitanos en <a href='https://coaniquem.cl' target='_blank'>coaniquem.cl</a></span></div></div>",
                    EmailToId = transaction.Email
                };
                //Enviamos el mail
                _mailService.SendMailAsync(mail);
            }

            return RedirectToAction("Exito", "Views", new { i = i.GetHashCode(), v = transaction.RequestAmount });

        }

    }
}