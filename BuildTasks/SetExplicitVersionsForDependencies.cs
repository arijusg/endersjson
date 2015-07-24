using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Build.Framework;

namespace BuildTasks
{
    public class NuspecPackage
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }

    public class SetExplicitVersionsForDependencies : ITask
    {
        [Required]
        public string RelativePath { get; set; }

        public bool Execute()
        {
            var packages = new List<NuspecPackage>();

            var currentDirectory = Path.Combine(Environment.CurrentDirectory, RelativePath);

            var files = Directory.GetFiles(currentDirectory, "nuget.nuspec", SearchOption.AllDirectories);

            foreach (var nuspecFile in files)
            {
                var nuspecDocument = new XmlDocument();
                nuspecDocument.Load(nuspecFile);

                var idNode = nuspecDocument.SelectNodes("/package/metadata/id").Cast<XmlNode>().First();
                var versionNode = nuspecDocument.SelectNodes("/package/metadata/version").Cast<XmlNode>().First();

                packages.Add(new NuspecPackage()
                {
                    Id = idNode.InnerText,
                    Version = versionNode.InnerText
                });
            }

            foreach (var nuspecFile in files)
            {
                var nuspecDocument = new XmlDocument();
                nuspecDocument.Load(nuspecFile);

                foreach (var package in packages)
                {
                    var dependencyXPath = string.Format("/package/metadata/dependencies/dependency[@id='{0}']", package.Id);
                    var nuspecDependencies = nuspecDocument.SelectNodes(dependencyXPath).Cast<XmlNode>().ToList();

                    if (nuspecDependencies.Any())
                    {
                        XmlNode dependency = nuspecDependencies.First();
                        dependency.Attributes["version"].Value = package.Version;
                    }
                }

                nuspecDocument.Save(nuspecFile);
            }

            return true;
        }

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
    }
}