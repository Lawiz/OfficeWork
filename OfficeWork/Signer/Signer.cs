using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OfficeWork.Signer
{
    interface ISigner
    {
        
    }
    public class Signer : ISigner
    {
        private readonly X509Certificate2 certificate;

        public Signer()
        {
            this.certificate = 
        }
    }
}
