using System.Data;
using ClosedXML.Excel;
using desafiocoaniquem.Models;
using desafiocoaniquem.Services;
using Microsoft.AspNetCore.Mvc;

namespace desafiocoaniquem.Controllers
{
    public class CoaniquemController : Controller
    {

        public static IConfiguration? _config;
        private readonly IMailService _mailService;
        public CoaniquemController(IConfiguration iConfig, ILogger<CoaniquemController> logger, IMailService _MailService)
        {
            _config = iConfig;
            _mailService = _MailService;
        }

        /// <summary>
        /// Método para actualización de monto manual.
        /// El nombre del método es para tener un nombre ofuscado
        /// </summary>
        /// <param name="K28yhJ"></param> Monto a modificar
        /// <returns></returns>
        [HttpGet]
        public JsonResult HFKLRwZupzgH8aFEyJRVstXCvmm8by78Etx84k83KvZXJPwKT7hkkzYndqH9H5V(string K28yhJ)
        {
            JsonResponseModel jsonResponse = new JsonResponseModel();
            try
            {
                db.updateMonto(Convert.ToInt32(K28yhJ));
                jsonResponse.Message = "Monto modificado correctamente";
            }
            catch (Exception)
            {
                jsonResponse.Message = "Error actualizando monto, verificar valor";
            }
            return this.Json(jsonResponse);
        }

        /*
         * Método de bypass para las urls de métodos de págo no integrados
         */
        [HttpPost]
        public JsonResult UrlByPass([FromBody] UrlByPassIn m) //r: Referencia, ch: Canal, m:Monto, e:email
        {
            db.insertTransaction(m.m, m.c, m.r, "", "", m.e);
            JsonResponseModel jsonResponse = new JsonResponseModel();
            jsonResponse.Code = JsonResponseModel.COD_REDIRECT;
            jsonResponse.redirectionURL = m.url;
            return this.Json(jsonResponse);
        }

        /*
         * Método para consulta de total recaudado por alcancía, 
         * no se puede poner en el controlador de alcancía por el enrutamiento 
         */
        [HttpGet]
        public JsonResult ConsultaAlcanciaRef(string id)
        {
            JsonResponseModel res = new JsonResponseModel();
            try
            {
                res.Data = db.getTotalAlcancia(id, GlobalConfiguration.colectaConfig.Fecha).ToArray();
                res.Size = res.Data.Length;
                res.Message = "Consulta realizada";
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Code = JsonResponseModel.COD_ERROR;
            }
            return this.Json(res);
        }

        /*
         * Método para para consulta de valor total recaudado
         */
        [HttpGet]
        public JsonResult GetTotalAmount()
        {
            JsonResponseModel res = new JsonResponseModel();
            try
            {
                List<int> list = new List<int>();
                list.Add(db.getTotalAmount(GlobalConfiguration.colectaConfig.Fecha));
                res.Data = list.ToArray();
                res.Size = res.Data.Length;
                res.Message = "Consulta realizada";
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Code = JsonResponseModel.COD_ERROR;
            }
            return this.Json(res);
        }

        /// <summary>
        /// Método para actualización de monto manual.
        /// El nombre del método es para tener un nombre ofuscado
        /// </summary>
        /// <param name="K28yhJ"></param> Monto a modificar
        /// <returns></returns>
        [HttpGet("JAaCFKnEGrdkwYmePD6LZ394NWypV58tT7UbzXxMfh2HqcBvsRGsPRmz25tcrX4KhbQHkJUDN3ay8qLf7wnVZuAxvMpW6YeBEdS9")]
        public async Task JAaCFKnEGrdkwYmePD6LZ394NWypV58tT7UbzXxMfh2HqcBvsR()
        {
            DataTable dt = db.getAllColectaTrxs();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Transacciones");
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Headers.Add("content-disposition", "attachment;filename=ReporteTransacionalDFQM_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    MyMemoryStream.Seek(0, SeekOrigin.Begin);
                    MyMemoryStream.CopyToAsync(Response.Body);
                    Response.Body.FlushAsync();
                }
            }
        }
    }
}