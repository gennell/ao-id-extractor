﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace ao_id_extractor.Extractors
{
    public class LocationExtractor : BaseExtractor
    {
        public LocationExtractor(string outputFolderPath, ExportType exportType) : base(outputFolderPath, exportType)
        {
            
        }
        
        protected override List<IDContainer> ExtractFromXML(string xmlFile)
        {
            HashSet<IDContainer> outputList = new HashSet<IDContainer>();

            // Param 0 is the xml file
            byte[] encodedString = Encoding.UTF8.GetBytes(File.ReadAllText(xmlFile, Encoding.UTF8));

            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;

            // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ms);

            XmlNode rootNode = xmlDoc.LastChild.FirstChild;

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    string locID = node.Attributes["id"].Value;
                    string locName = node.Attributes["displayname"].Value;

                    outputList.Add(new IDContainer() { Index = locID, UniqueName = locName });
                }
            }

            ms.Close();

            return outputList.ToList();
        }

        protected override string GetBinFilePath()
        {
            return Path.Combine(AOLauncherFolder, @"..\game\Albion-Online_Data\StreamingAssets\GameData\cluster\world.bin");
        }
    }
}
