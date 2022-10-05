using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Suporte
{
    class GetMac
    {
        private string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    int c = 0;
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    for (int i = 1; 6 > i; i++)
                    {
                        int b = 2 * i;

                        if (i == 1)
                            macAddresses = macAddresses.Insert(b, ".");
                        else
                            macAddresses = macAddresses.Insert(b + c, ".");
                        c++;
                    }

                    break;
                }
            }

            return macAddresses;
        }
    }
}
