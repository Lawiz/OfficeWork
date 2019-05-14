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
        bool Verify(string text, byte[] signature);
        byte[] Sign(string text);
    }
    public class Signer : ISigner
    {
        private readonly X509Certificate2 certificate;
        private readonly IHostingEnvironment _hostingEnvironment;
        private RSA _privateKey;
        public Signer(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            var path = Path.Combine(_hostingEnvironment.ContentRootPath,@"mycert.pfx");

            this.certificate = new X509Certificate2(path,"1234567890");
            this._privateKey = this.certificate.GetRSAPrivateKey();
        }
        public byte[] Sign(string text)
        {
            SHA1Managed sha1 = new SHA1Managed();

            UnicodeEncoding encoding = new UnicodeEncoding();

            byte[] data = encoding.GetBytes(text);

            byte[] hash = sha1.ComputeHash(data);

            return  _privateKey.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }
        public bool Verify(string text, byte[] signature)
        {
            RSA csp = this.certificate.GetRSAPublicKey();
            // Hash the data

            SHA1Managed sha1 = new SHA1Managed();

            UnicodeEncoding encoding = new UnicodeEncoding();

            byte[] data = encoding.GetBytes(text);

            byte[] hash = sha1.ComputeHash(data);


            // Verify the signature with the hash

            return csp.VerifyHash(hash, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

        }
    }
}
