using System.Text;

namespace MicroServicesApi.Helpers;

public static class KmacHelper
{
    public static byte[] DeriveKmacKey(string userId, IEnumerable<string> roles, string email, byte[] secret)
    {
        var rolesStr = string.Join(",", roles.OrderBy(r => r));
        var inputData = Encoding.UTF8.GetBytes($"{userId}|{rolesStr}|{email}");

        var kmac = new Org.BouncyCastle.Crypto.Macs.KMac(256, secret);
        kmac.Init(new Org.BouncyCastle.Crypto.Parameters.KeyParameter(secret));
        kmac.BlockUpdate(inputData, 0, inputData.Length);

        var output = new byte[kmac.GetMacSize()];
        kmac.DoFinal(output, 0);
        return output;
    }
}
