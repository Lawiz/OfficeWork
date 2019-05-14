using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeWork.Signer;

namespace OfficeWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private ISigner _signer;

        public SignController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _signer = (ISigner)serviceProvider.GetService(typeof(ISigner));
        }
        [HttpGet("{signText}")]
        public dynamic GetSignature(string signText)
        {
            return _signer.Sign(signText);
        }
    }
}