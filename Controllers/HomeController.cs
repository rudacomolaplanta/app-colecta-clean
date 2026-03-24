using desafiocoaniquem.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using Transbank.Webpay.WebpayPlus;
using Transaction = Transbank.Webpay.WebpayPlus.Transaction;

namespace desafiocoaniquem.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _config;

        public HomeController(IConfiguration iConfig, ILogger<HomeController> logger)
        {
            _config = iConfig;
        }

    }
}