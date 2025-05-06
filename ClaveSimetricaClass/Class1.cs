using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;

namespace ClaveSimetricaClass
{
    public class ClaveSimetrica
    {
        public byte[] Key {get;set;}
        public byte[] IV {get;set;}

        public ClaveSimetrica()
        {
            Aes aesAlg = Aes.Create();
            this.Key = aesAlg.Key;
            this.IV = aesAlg.IV;
        }

        public byte[] CifrarMensaje (string Mensaje, byte[] Key, byte[] IV)
        {            
            Aes aesAlg = Aes.Create();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(Key, IV); 

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                { 
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {                            
                        streamWriter.Write(Mensaje);  
                    }
                    return memoryStream.ToArray();                                          
                }
            }
        }


        public byte[] CifrarMensaje (string Mensaje)
        {            
            Aes aesAlg = Aes.Create();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(this.Key, this.IV); 

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                { 
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {                            
                        streamWriter.Write(Mensaje);  
                    }
                    return memoryStream.ToArray();                                          
                }
            }
        }

        public string DescifrarMensaje (byte[] MensajeCifrado, byte[] Key, byte[] IV)
        {
            Aes aesAlg = Aes.Create();
            
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(Key, IV); 
            using (MemoryStream memoryStreamd = new MemoryStream(MensajeCifrado))
            {
                using (CryptoStream cryptoStreamd = new CryptoStream((Stream)memoryStreamd, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStreamd))
                    { 
                        return streamReader.ReadToEnd();                             
                    }
                }
            }   

        }

        public string DescifrarMensaje (byte[] MensajeCifrado)
        {
            Aes aesAlg = Aes.Create();
            
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(this.Key, this.IV); 
            using (MemoryStream memoryStreamd = new MemoryStream(MensajeCifrado))
            {
                using (CryptoStream cryptoStreamd = new CryptoStream((Stream)memoryStreamd, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStreamd))
                    { 
                        return streamReader.ReadToEnd();                             
                    }
                }
            }   
        }
    }
}
