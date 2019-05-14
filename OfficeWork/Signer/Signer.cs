using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace OfficeWork.Signer
{
    interface ISigner
    {
        
    }
    public class Signer : ISigner
    {
        private readonly X509Certificate2 certificate;
        private readonly IHostingEnvironment _hostingEnvironment;
        private RSACryptoServiceProvider _privateKey;
        public Signer(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            var path = Path.Combine(_hostingEnvironment.ContentRootPath,@"mycert.pfx");

            this.certificate = new X509Certificate2(path,"1234567890");
            this._privateKey = (RSACryptoServiceProvider) this.certificate.PrivateKey;
        }
        public byte[] Sign(string text, string certSubject)
        {
            // Hash the data

            SHA1Managed sha1 = new SHA1Managed();

            UnicodeEncoding encoding = new UnicodeEncoding();

            byte[] data = encoding.GetBytes(text);

            byte[] hash = sha1.ComputeHash(data);

            return  _privateKey.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
        }
        public bool Verify(string text, byte[] signature, string certPath)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)this.certificate.PublicKey.Key;
            // Hash the data

            SHA1Managed sha1 = new SHA1Managed();

            UnicodeEncoding encoding = new UnicodeEncoding();

            byte[] data = encoding.GetBytes(text);

            byte[] hash = sha1.ComputeHash(data);


            // Verify the signature with the hash

            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);

        }
    }
}
