using desafiocoaniquem.Models;
//using log4net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Transbank.Webpay.Common;
using Transaction = Transbank.Webpay.WebpayPlus.Transaction;
using System.Collections;
using Transbank.Common;
using desafiocoaniquem.Services;

namespace desafiocoaniquem.Controllers
{
    public class WebPayController : Controller
    {
        private IConfiguration _config;
        private Transaction tx;
        private readonly IMailService _mailService;

        public WebPayController(IConfiguration iConfig, ILogger<WebPayController> logger, IMailService _MailService)
        {
            _config = iConfig;
            tx = new Transaction(new Options(_config["AppSettings:Webpay.CommerceCode"], _config["AppSettings:Webpay.ApiKey"], WebpayIntegrationType.Live));
            _mailService = _MailService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Init(string m, string r, string c, string e) //r: Referencia, ch: Canal, m:Monto
        {
            string sessionID = HttpContext.Session.Id;
            TokenUrlClass rec = new TokenUrlClass();
            m = m.Replace("$", "").Replace(".", "");
            //Insertamos la transacción y recuperamos el ID
            string buyOrder = Convert.ToString(db.insertTransaction(m, c, r, "", "", e));
            string sessionId = sessionID;
            decimal amount = Decimal.Parse(m);
            //string baseUrl = "https" + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
            string baseUrl = $"https://{this.Request.Host}{this.Request.PathBase}";
            string returnUrl = baseUrl + "/WebPay/Response?i=" + buyOrder;
            var response = tx.Create(buyOrder, sessionId, amount, returnUrl);
            //Seteamos el Token del request
            db.updateTransactionToken(buyOrder, response.Token);
            return Redirect(string.Format(response.Url + "?TBK_TOKEN={0}", response.Token));
        }

        public ActionResult Response(string i)//i:id transacción en BDD
        {
            //Log.Debug("Se recibe origen " + i);
            TokenUrlClass rec = new TokenUrlClass();

            //Obtenemos el encabezado de la transacción
            desafiocoaniquem.Models.Transaction transaction = db.getRequestTransaction(i);
            string responseString = null, responseCode, authorizationCode;

            try
            {
                //Confirmamos la transacción
                var response = tx.Commit(transaction.RequestToken);
                responseString = JsonSerializer.Serialize(response);
                responseCode = response.ResponseCode.ToString();
                authorizationCode = response.AuthorizationCode;
            }
            catch (Exception e)
            {
                responseCode = "ERROR";
                authorizationCode = "ERROR";
                responseString = e.Message;
            }

            //Guardamos la data del response
            db.updateTransactionResponse(i, responseCode, authorizationCode, responseString);

            if (responseCode == "ERROR" && responseCode != "0")
            {
                return RedirectToAction("Error", "Views");
            }
            else
            {
                //Envíamos el mail de respuesta
                if (transaction.Email != null)
                {
                    MailData mail = mail = new MailData
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


        public JsonResult checkTrx(string token)//i:id transacción en BDD
        {

            JsonResponseModel jsonResponse = new JsonResponseModel();
            TokenUrlClass rec = new TokenUrlClass();

            //Obtenemos el encabezado de la transacción
            //desafiocoaniquem.Models.Transaction transaction = desafiocoaniquem.DAO.Transactions.getRequestTransaction(i);
            string responseString = null;

            try
            {
                //Confirmamos la transacción
                var response = tx.Commit(token);
                responseString = JsonSerializer.Serialize(response);

                ArrayList array = new ArrayList();
                array.Add(responseString);
                jsonResponse.Data = array.ToArray();

                //responseCode = response.ResponseCode.ToString();
                //authorizationCode = response.AuthorizationCode;
            }
            catch (Exception e)
            {
                responseString = e.Message;
                ArrayList array = new ArrayList();
                array.Add(responseString);
                jsonResponse.Data = array.ToArray();
            }

            return this.Json(jsonResponse);

        }

    }
}