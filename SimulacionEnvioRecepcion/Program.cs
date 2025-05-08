using System;
using System.Text;
using System.Security.Cryptography;
using ClaveSimetricaClass;
using ClaveAsimetricaClass;

namespace SimuladorEnvioRecepcion
{
    class Program
    {   
        static string? UserName;
        static string? SecurePass;
        static byte[] Salt = Array.Empty<byte>(); 
        static ClaveAsimetrica Emisor = new ClaveAsimetrica();
        static ClaveAsimetrica Receptor = new ClaveAsimetrica();
        static ClaveSimetrica ClaveSimetricaEmisor = new ClaveSimetrica();
        static ClaveSimetrica ClaveSimetricaReceptor = new ClaveSimetrica();

        static string TextoAEnviar = "Me he dado cuenta que incluso las personas que dicen que todo está predestinado y que no podemos hacer nada para cambiar nuestro destino igual miran antes de cruzar la calle. Stephen Hawking.";
        
        static void Main(string[] args)
        {

            /****PARTE 1****/
            //Login / Registro
            Console.WriteLine ("¿Deseas registrarte? (S/N)");
            string? registroInput = Console.ReadLine();
            string registro = registroInput?.Trim().ToUpper() ?? "";

            if (registro.Trim().ToUpper() == "S")
            {
                //Realizar registro del cliente
                Registro();
            }

            //Realizar login
            bool login = Login();

            /***FIN PARTE 1***/

            if (login)
            {                  
                byte[] TextoAEnviar_Bytes = Encoding.UTF8.GetBytes(TextoAEnviar); 
                Console.WriteLine("Texto a enviar bytes: {0}", BytesToStringHex(TextoAEnviar_Bytes));    
                
                //LADO EMISOR

                //Firmar mensaje
                byte[] Firma = Emisor.FirmarMensaje(TextoAEnviar_Bytes);
                Console.WriteLine("Firma: {0}", BytesToStringHex(Firma));

                //Cifrar mensaje con la clave simétrica
                byte[] TextoCifrado = ClaveSimetricaEmisor.CifrarMensaje(TextoAEnviar);
                Console.WriteLine("Texto cifrado: {0}", BytesToStringHex(TextoCifrado));

                //Cifrar clave simétrica con la clave pública del receptor

                //LADO RECEPTOR

                //Descifrar clave simétrica

                
                //Descifrar clave simétrica
 

                //Descifrar mensaje con la clave simétrica


                //Comprobar firma

            }
        }

        public static void Registro()
        {
            Console.WriteLine ("Indica tu nombre de usuario:");
            string? userNameInput = Console.ReadLine();
            UserName = userNameInput ?? "";

            Console.WriteLine ("Indica tu password:");
            string? passwordInput = Console.ReadLine();
            string passwordRegister = passwordInput ?? "";

            // 1. Generar un salt aleatorio
            Salt = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(Salt);
            }

            // 2. Aplicar hash iterado con PBKDF2 (Rfc2898DeriveBytes)
            int iteraciones = 1000;
            using (var pbkdf2 = new Rfc2898DeriveBytes(passwordRegister, Salt, iteraciones, HashAlgorithmName.SHA512))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 256 bits
                SecurePass = BytesToStringHex(hash); // Guarda el hash como string hexadecimal
            }

            Console.WriteLine("Registro completado.");
            Console.WriteLine("Contraseña segura (hash): " + SecurePass);
        }


        public static bool Login()
        {
            bool auxlogin = false;
            do
            {
                Console.WriteLine ("Acceso a la aplicación");
                Console.WriteLine ("Usuario: ");
                string? userNameInput = Console.ReadLine();
                string userNameLogin = userNameInput ?? "";

                Console.WriteLine ("Password: ");
                string? passwordInput = Console.ReadLine();
                string passwordRegister = passwordInput ?? "";

                /***PARTE 1***/
                if (userNameLogin == UserName)
                {
                    int iteraciones = 1000;

                    // Generar el hash de la contraseña introducida con el mismo salt
                    using (var pbkdf2 = new Rfc2898DeriveBytes(passwordRegister, Salt, iteraciones, HashAlgorithmName.SHA512))
                    {
                        byte[] hash = pbkdf2.GetBytes(32); // mismo tamaño que el registro
                        byte[] storedHashBytes = StringHexToBytes(SecurePass ?? "");

                        if (CryptographicOperations.FixedTimeEquals(hash, storedHashBytes))
                        {
                            Console.WriteLine("Login correcto.");
                            auxlogin = true;
                        }
                        else
                        {
                            Console.WriteLine("Contraseña incorrecta. Inténtalo de nuevo.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Usuario no encontrado. Inténtalo de nuevo.");
                }


            }while (!auxlogin);

            return auxlogin;
        }

        static string BytesToStringHex (byte[] result)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in result)
                stringBuilder.AppendFormat("{0:x2}", b);

            return stringBuilder.ToString();
        }

        public static byte[] StringHexToBytes(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
