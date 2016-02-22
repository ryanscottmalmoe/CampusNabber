using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCode.UtilityFiles
{
    abstract class Utility
    {
        public static byte[] encryptPassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            return new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
        }
    }
}
