using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Build.Framework;

namespace BuildTasks
{
    public class MigratePackageDependenciesToNuSpec : ITask
    {
        [Required]
        public string RelativePath { get; set; }

        public bool Execute()
        {
            var currentDirectory = Path.Combine(Environment.CurrentDirectory, RelativePath);

            var files = Directory.GetFiles(currentDirectory, "packages.config", SearchOption.AllDirectories);

            foreach (var packageConfigFile in files)
            {
                var potentialNuSpecFile = packageConfigFile.ToLower().Replace("packages.config", "nuget.nuspec");
                if (File.Exists(potentialNuSpecFile))
                {
                    var packageDocument = new XmlDocument();
                    packageDocument.Load(packageConfigFile);

                    var packageNodes = packageDocument.SelectNodes("/packages/package");
                    foreach (XmlNode packageNode in packageNodes)
                    {
                        var packageIdentity = packageNode.Attributes["id"].Value;
                        var packageVersion = packageNode.Attributes["version"].Value;

                        var nuspecDocument = new XmlDocument();
                        nuspecDocument.Load(potentialNuSpecFile);

                        var dependencyXPath = string.Format("/package/metadata/dependencies/dependency[@id='{0}']", packageIdentity);
                        var nuspecDependencies = nuspecDocument.SelectNodes(dependencyXPath).Cast<XmlNode>().ToList();

                        if (!nuspecDependencies.Any())
                        {
                            var dependencies = nuspecDocument.SelectNodes("/package/metadata/dependencies").Cast<XmlNode>().First();
                            var newDependency = nuspecDocument.CreateElement("dependency");
                            newDependency.SetAttribute("id", packageIdentity);
                            newDependency.SetAttribute("version", packageVersion);
                            dependencies.AppendChild(newDependency);

                        }
                        else
                        {
                            var existingDependency = nuspecDependencies.First();
                            existingDependency.Attributes["version"].Value = packageVersion;
                        }

                        nuspecDocument.Save(potentialNuSpecFile);
                    }
                }
            }

            return true;
        }

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
    }
}