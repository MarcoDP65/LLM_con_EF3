using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Microsoft.Win32;

namespace PannelloCharger
{
    public class ScannerPorte
    {
        public string PortaLadeLight;
        public string PortaSpyBatt;
        public int PorteLadeLight;
        public int PorteSpyBatt;

        public ScannerPorte()
        {
            PortaLadeLight = null;
            PortaSpyBatt = null;
            PorteLadeLight = 0;
            PorteSpyBatt = 0;
        }


       public Boolean cercaPorte()
        {
            PortaLadeLight = null;
            PortaSpyBatt = null;
            PorteLadeLight = 0;
            PorteSpyBatt = 0;
            string campo;
            string chiave;

            Dictionary<string, string> friendlyPorts = BuildPortNameHash(SerialPort.GetPortNames());
            foreach (KeyValuePair<string, string> kvp in friendlyPorts)
            {
                campo = kvp.Value;
                chiave = "LADE LIGHT";
                if (campo.IndexOf(chiave) >= 0)
                {
                    PorteLadeLight++;
                    PortaLadeLight = kvp.Key;
                }
                campo = kvp.Value;
                chiave = "SPY-BATT";
                if (campo.IndexOf(chiave) >= 0)
                {
                    PorteSpyBatt++;
                    PortaSpyBatt = kvp.Key;
                }

                //Console.WriteLine("Port '{0}' is better known as '{1}'", kvp.Key, kvp.Value);
            }
            if (PorteLadeLight > 0 | PorteSpyBatt > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

       }



        /// <summary>
        /// Begins recursive registry enumeration
        /// </summary>
        /// <param name="portsToMap">array of port names (i.e. COM1, COM2, etc)</param>
        /// <returns>a hashtable mapping Friendly names to non-friendly port values</returns>
        static Dictionary<string, string> BuildPortNameHash(string[] portsToMap)
        {
            Dictionary<string, string> oReturnTable = new Dictionary<string, string>();
            MineRegistryForPortName("SYSTEM\\CurrentControlSet\\Enum", oReturnTable, portsToMap);
            return oReturnTable;
        }
        /// <summary>
        /// Recursively enumerates registry subkeys starting with startKeyPath looking for 
        /// "Device Parameters" subkey. If key is present, friendly port name is extracted.
        /// </summary>
        /// <param name="startKeyPath">the start key from which to begin the enumeration</param>
        /// <param name="targetMap">dictionary that will get populated with 
        /// nonfriendly-to-friendly port names</param>
        /// <param name="portsToMap">array of port names (i.e. COM1, COM2, etc)</param>
        static void MineRegistryForPortName(string startKeyPath, Dictionary<string, string> targetMap,
            string[] portsToMap)
        {
            if (targetMap.Count >= portsToMap.Length)
                return;
            using (RegistryKey currentKey = Registry.LocalMachine)
            {
                try
                {
                    using (RegistryKey currentSubKey = currentKey.OpenSubKey(startKeyPath))
                    {
                        string[] currentSubkeys = currentSubKey.GetSubKeyNames();
                        if (currentSubkeys.Contains("Device Parameters") &&
                            startKeyPath != "SYSTEM\\CurrentControlSet\\Enum")
                        {
                            object portName = Registry.GetValue("HKEY_LOCAL_MACHINE\\" +
                                startKeyPath + "\\Device Parameters", "PortName", null);
                            if (portName == null ||
                                portsToMap.Contains(portName.ToString()) == false)
                                return;
                            object friendlyPortName = Registry.GetValue("HKEY_LOCAL_MACHINE\\" +
                                startKeyPath, "FriendlyName", null);
                            string friendlyName = "N/A";
                            if (friendlyPortName != null)
                                friendlyName = friendlyPortName.ToString();
                            if (friendlyName.Contains(portName.ToString()) == false)
                                friendlyName = string.Format("{0} ({1})", friendlyName, portName);
                            targetMap[portName.ToString()] = friendlyName;
                        }
                        else
                        {
                            foreach (string strSubKey in currentSubkeys)
                                MineRegistryForPortName(startKeyPath + "\\" + strSubKey, targetMap, portsToMap);
                        }
                    }
                }
                catch (Exception)
                {
                    
                    //Console.WriteLine("Error accessing key '{0}'.. Skipping..", startKeyPath);
                }
            }
        }




    }
}
