using System;
using System.Linq;
using System.Xml;
using Microsoft.Build.Framework;

namespace BuildTasks
{
    public class BumpVersion : ITask
    {
        [Required]
        public string FilePath { get; set; }

        [Required]
        public string XPathToVersionNumber { get; set; }

        public bool Execute()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(FilePath);

            var node = xmlDocument.SelectNodes(XPathToVersionNumber).Cast<XmlNode>().FirstOrDefault();

            if (node == null)
                throw new Exception("Think your xpath is wrong ... ");

            var nodeContent = node.InnerText;
            var versionNumbers = nodeContent.Split('.');
            var bumpVersion = int.Parse(versionNumbers.Last()) + 1;
            versionNumbers[versionNumbers.Length - 1] = bumpVersion.ToString();
            node.InnerText = string.Join(".", versionNumbers);

            xmlDocument.Save(FilePath);

            return true;
        }

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
    }
}
