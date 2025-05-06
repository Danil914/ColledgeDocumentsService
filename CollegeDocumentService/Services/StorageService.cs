using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Xml;
using System.Xml.Linq;

namespace CollegeDocumentService.Services
{
    public interface IStorageService
    {
        string Get(string key);
        void Set(string key, string value);
        void Remove(string key);
    }

    public class StorageService : IStorageService
    {

        private const string fileName = "AppData.xml";

        public string Get(string key)
        {
            var doc = new XmlDocument();
            doc.Load(fileName);
            var root = doc.DocumentElement;
            var node = root.SelectSingleNode(key);
            return node?.InnerText;
        }

        public void Set(string key, string value)
        {
            var doc = new XmlDocument();
            doc.Load(fileName);
            var root = doc.DocumentElement;

            var existingNode = root.SelectSingleNode(key);

            if (existingNode != null)
            {
                existingNode.InnerText = value;
            }
            else
            {
                XmlElement newElement = doc.CreateElement(key);
                newElement.InnerText = value;
                root.AppendChild(newElement);
            }

            doc.Save(fileName);
        }

        public void Remove(string key)
        {
            var doc = new XmlDocument();
            doc.Load(fileName);
            var root = doc.DocumentElement;

            var nodeToRemove = root.SelectSingleNode(key);

            if (nodeToRemove != null)
            {
                root.RemoveChild(nodeToRemove);
            }

            doc.Save(fileName);
        }
    }
}