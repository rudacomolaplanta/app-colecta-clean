using System;

namespace desafiocoaniquem.Models
{
    public class JsonResponseModel
    {
        public static int COD_ERROR = 99;
        public static int COD_INFO = 98;
        public static int COD_SESSIONEXPIRED = 97;
        public static int COD_REDIRECT = 96;
        public static int COD_OK = 0;

        public JsonResponseModel()
        {
            this.Code = JsonResponseModel.COD_OK;
            this.Size = 0;
            this.FilterTotalCount = 0;
            this.Message = "Operación realizada correctamente";
        }

        public string Message { set; get; }
        public string redirectionURL { set; get; }
        public Array Data { set; get; }
        public int Code { set; get; }
        public int FilterTotalCount { set; get; }
        public int Size { set; get; }
    }
}