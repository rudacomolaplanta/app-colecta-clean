//using log4net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using desafiocoaniquem.Models;
using desafiocoaniquem.Services;
using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace desafiocoaniquem.Controllers
{
    public class FlowController : Controller
    {
        private IConfiguration _config;
        private readonly IMailService _mailService;

        public FlowController(IConfiguration iConfig, ILogger<FlowController> logger, IMailService _MailService)
        {
            _config = iConfig;
            _mailService = _MailService;
        }
        public async Task<RedirectResult> Init(string m, string r, string c, string e, string tp) //r: Referencia, ch: Canal, m:Monto, e:email
        {

            m = m.Replace("$", "").Replace(".", "");

            int idTrx = db.insertTransaction(m, c, r, "", "", e);
            string buyOrder = Convert.ToString(idTrx);

            //string baseUrl = "https" + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
            string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            string returnUrl = baseUrl + "/Flow/Response?i=" + buyOrder;
            string confirmationUrl = baseUrl + "/Flow/Confirmation";

            // La documentación de flow dice que debemos ordenar los parametros que vamos enviar, así que aquí da lo mismo, SortedDictionary lo hace por ti,
            var values = new SortedDictionary<string, string>
            {
               { "apiKey", _config["AppSettings:Flow.ApiKey"] },
               { "commerceOrder", buyOrder },
               { "subject", "Por concepto de donación" },
               { "currency", "CLP" },
               { "amount", m },
               { "email", e != null ? e: "" },
               { "paymentMethod", tp }, //9 son todos y 2 es Servipag
               { "urlConfirmation", confirmationUrl },
               { "urlReturn", returnUrl }
            };

            // Intanciar el metodo QueryString hacer los parametros.
            string x = QueryString(values);

            // Agregar al diccionario el parametro S.
            values.Add("s", GetHash(x, _config["AppSettings:Flow.SecretKey"]));

            // Como ya esta incluido el parametro s en el SortedDictionary se lo pasamos al objeto FormUrlEncodedContent.
            var content = new FormUrlEncodedContent(values);

            using (var http = new HttpClient())
            {

                // Actualizamos el registro con la data a enviarx
                db.updateTransactionRequestDATA(idTrx, JsonSerializer.Serialize(content));

                // El resultado no entregará la respuesta que viene desde el servidor de flow si no otros detalles, para obtenerlo se usa el metodo ReadAsAsync;
                //var result = await http.PostAsync(_config["AppSettings:Flow.Endpoint"] + "/payment/create", content);
                //var response = await result.Content.ReadAsAsync<Object>();

                var request = new HttpRequestMessage(HttpMethod.Post, _config["AppSettings:Flow.Endpoint"] + "/payment/create");
                request.Headers.Accept.Clear();
                request.Content = content;//new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
                var result = await http.SendAsync(request);

                var response = await result.Content.ReadAsStringAsync();
                dynamic data = JsonSerializer.Deserialize<FlowResponseOut>(response);

                //Valores response
                string urlredirect = data.url;
                string token = data.token;

                //Guardamos el id de la transacción
                db.updateTransactionToken(buyOrder, Convert.ToString(data.flowOrder));

                //return Redirect(string.Format(urlredirect + "?token={0}", token));
                return Redirect(string.Format(urlredirect + "?token={0}", token));
            }
        }

        public async Task<StatusCodeResult> Confirmation(string token)
        {
            try
            {
                using (var http = new HttpClient())
                {

                    // La documentación de flow dice que debemos ordenar los parametros que vamos enviar, así que aquí da lo mismo, SortedDictionary lo hace por ti,
                    var values = new SortedDictionary<string, string>
                    {
                       { "apiKey", _config["AppSettings:Flow.ApiKey"] },
                       { "token", token },
                    };

                    // Intanciar el metodo QueryString hacer los parametros.
                    string x = QueryString(values);

                    string urlvars = "apiKey=" + _config["AppSettings:Flow.ApiKey"] + "&token=" + token + "&s=" + GetHash(x, _config["AppSettings:Flow.SecretKey"]);

                    // El resultado no entregará la respuesta que viene desde el servidor de flow si no otros detalles, para obtenerlo se usa el metodo ReadAsAsync;
                    var result = await http.GetAsync(_config["AppSettings:Flow.Endpoint"] + "/payment/getStatus?" + urlvars);
                    //var response = await result.Content.ReadAsAsync<Object>();

                    var response = await result.Content.ReadAsStringAsync();
                    dynamic data = JsonSerializer.Deserialize<FlowResponseIn>(response);

                    //Valores response
                    string i = data.commerceOrder;
                    int status = data.status;

                    if (status == 2) //Pagado
                    {
                    }

                    //Guardamos la data del response y la confirmación de la transacción
                    db.updateTransactionResponse(i, "0", Convert.ToString(data.flowOrder), response);
                    return StatusCode(200);

                }
            }
            catch (Exception)
            {
                //Guardamos la data del response y la confirmación de la transacción
                //desafiocoaniquem.DAO.Transactions.updateTransactionResponse(i, "99", "NA", e.Message);
            }

            return StatusCode(401);
        }

        public async Task<RedirectToActionResult> Response(string i)
        {
            desafiocoaniquem.Models.Transaction transaction = db.getRequestTransaction(i);

            try
            {
                using (var http = new HttpClient())
                {

                    // La documentación de flow dice que debemos ordenar los parametros que vamos enviar, así que aquí da lo mismo, SortedDictionary lo hace por ti,
                    var values = new SortedDictionary<string, string>
                    {
                       { "apiKey", _config["AppSettings:Flow.ApiKey"] },
                       { "token", transaction.RequestToken },
                    };

                    // Intanciar el metodo QueryString hacer los parametros.
                    string x = QueryString(values);

                    // Agregar al diccionario el parametro S.
                    values.Add("s", GetHash(x, _config["AppSettings:Flow.SecretKey"]));

                    // Como ya esta incluido el parametro s en el SortedDictionary se lo pasamos al objeto FormUrlEncodedContent.
                    var content = new FormUrlEncodedContent(values);

                    var request = new HttpRequestMessage(HttpMethod.Get, _config["AppSettings:Flow.Endpoint"] + "/payment/getStatus");
                    request.Headers.Accept.Clear();
                    request.Content = content;//new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
                    var result = await http.SendAsync(request);

                    // El resultado no entregará la respuesta que viene desde el servidor de flow si no otros detalles, para obtenerlo se usa el metodo ReadAsAsync;
                    //var result = await http.GetAsync(_config["AppSettings:Flow.Endpoint"] + "/payment/getStatus");

                    var response = await result.Content.ReadAsStringAsync();
                    dynamic data = JsonSerializer.Deserialize<FlowResponseIn>(response);

                    //Valores response
                    //string i = data.commerceOrder;
                    int status = data.status;

                    if (status == 2) //Pagado
                    {
                    }

                    //Guardamos la data del response y la confirmación de la transacción
                    db.updateTransactionResponse(i, "0", Convert.ToString(0), response);

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
            catch (Exception)
            {
                //Guardamos la data del response y la confirmación de la transacción
                //desafiocoaniquem.DAO.Transactions.updateTransactionResponse(i, "99", "NA", e.Message);
            }
            return RedirectToAction("Error", "Views");

        }

        public static String GetHash(string text, String key)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] keyBytes = encoding.GetBytes(key);
            Byte[] hashBytes = encoding.GetBytes(text);
            using (HMACSHA256 hash = new HMACSHA256(keyBytes)) hashBytes = hash.ComputeHash(hashBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
        public static string QueryString(IDictionary<string, string> dict)
        {
            var list = new List<string>();
            foreach (var item in dict)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            return string.Join("&", list);
        }

    }
}