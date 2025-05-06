using System.Security.Cryptography;
using System.Text;

namespace ClaveAsimetricaClass
{
    public class ClaveAsimetrica
    {
        public RSACryptoServiceProvider RSA {get; set;} 
        public RSAParameters PublicKey = new RSAParameters();

        public ClaveAsimetrica()
        {
            RSA = new RSACryptoServiceProvider();
            PublicKey = RSA.ExportParameters(false);
        }

        public byte[] FirmarMensaje (byte[] MensajeBytes)
        {
            return RSA.SignData(MensajeBytes,0,MensajeBytes.Length,SHA512.Create());
        } 


        public byte[] FirmarMensaje (byte[] MensajeBytes, RSAParameters ClavePublicaExterna)
        {
            RSACryptoServiceProvider RSA_Externo = new RSACryptoServiceProvider();
            RSA_Externo.ImportParameters (ClavePublicaExterna);
            return RSA_Externo.SignData(MensajeBytes,0,MensajeBytes.Length,SHA512.Create());
        }


        public bool ComprobarFirma (byte[] FirmaBytes, byte[] textoDescifradoBytes)
        {
            return RSA.VerifyData(textoDescifradoBytes,SHA512.Create(),FirmaBytes);
        } 

        public bool ComprobarFirma (byte[] FirmaBytes, byte[] textoDescifradoBytes, RSAParameters ClavePublicaExterna)
        {
            RSACryptoServiceProvider RSA_Externo = new RSACryptoServiceProvider();
            RSA_Externo.ImportParameters (ClavePublicaExterna);            
            return RSA_Externo.VerifyData(textoDescifradoBytes,SHA512.Create(),FirmaBytes);
        } 

        public byte[] CifrarMensaje (byte[] MensajeBytes)
        {
            return RSA.Encrypt(MensajeBytes,false);
        }

        public byte[] CifrarMensaje (byte[] MensajeBytes, RSAParameters ClavePublicaExterna)
        {
            RSACryptoServiceProvider RSA_Externo = new RSACryptoServiceProvider();
            RSA_Externo.ImportParameters (ClavePublicaExterna);

            byte [] textoPlanoBytes = Encoding.UTF8.GetBytes("hola");
            return RSA_Externo.Encrypt(MensajeBytes,false);
        }

        public byte[] DescifrarMensaje (byte[] MensajeCifradoBytes)
        {
            return RSA.Decrypt(MensajeCifradoBytes,false);
        }

        public byte[] DescifrarMensaje (byte[] MensajeCifradoBytes, RSAParameters ClavePublicaExterna)
        {
            RSACryptoServiceProvider RSA_Externo = new RSACryptoServiceProvider();
            RSA_Externo.ImportParameters (ClavePublicaExterna);
            return RSA_Externo.Decrypt(MensajeCifradoBytes,false);            
        }
    }
}
