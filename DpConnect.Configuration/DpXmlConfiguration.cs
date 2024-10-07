using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DpConnect.Configuration
{
    public class DpXmlConfiguration
    {
        public const string Tag_DpConfiguration = "DpConfiguration";
        public const string Tag_ProviderDefinition = "ProviderDefinition";
        public const string Tag_Provider = "Provider";
        public const string Tag_ProcessorDefinition = "ProcessorDefinition";
        public const string Tag_Processor = "Processor";
        public const string Tag_DataPointDefinition = "DataPointDefinition";
        public const string Tag_DpValue = "DpValue";
        public const string Tag_DpMethod = "DpMethod";
        public const string Tag_Description = "Name";
        public const string Tag_TargetProperty = "TargetProperty";
        public const string Tag_DpProperty = "DpProperty";        

        public DpXmlConfiguration(params XDocument[] docs)
        {
            CompileXmlToOne(docs);
        }

        public DpXmlConfiguration(params string[] path)
        {
            List<XDocument> docs = new List<XDocument>();
            foreach (var configPath in path)                                           
                docs.Add(XDocument.Load(configPath));

            CompileXmlToOne(docs.ToArray());
        }

        public void Add(DpXmlConfiguration config)
        {
            ProviderConfiguration = MergeElements(Tag_ProviderDefinition, config.ProviderConfiguration, ProviderConfiguration);
            ProcessorConfiguration = MergeElements(Tag_ProcessorDefinition, config.ProcessorConfiguration, ProcessorConfiguration);
            DataPointConfiguration = MergeElements(Tag_DataPointDefinition, config.DataPointConfiguration, DataPointConfiguration);
        }

        private XDocument MergeElements(string tag, XDocument newDoc, XDocument currentDoc)
        {

            IEnumerable<XElement> newContent = null;
            IEnumerable<XElement> currentContent = null;

            if (newDoc.Element(tag) != null)
                newContent = newDoc.Element(tag).Elements();

            if(currentDoc.Element(tag) != null)
                 currentContent = currentDoc.Element(tag).Elements();

            var compiledContent = new XElement(tag);

            if(currentContent != null)
                foreach (var content in currentContent)
                    compiledContent.Add(content);
            if(newContent != null)
                foreach (var content in newContent)
                    compiledContent.Add(content);
            
            return new XDocument(compiledContent);
        }

        public void Save(string filename)
        {
            var doc = new XDocument(new XElement(Tag_DpConfiguration, ProviderConfiguration.Elements(), ProcessorConfiguration.Elements(), DataPointConfiguration.Elements()));
            doc.Save(filename);
        }

        private void CompileXmlToOne(XDocument[] docs)
        {
            XElement compiledProviderConfig = new XElement(Tag_ProviderDefinition);
            XElement compiledProcessorConfig = new XElement(Tag_ProcessorDefinition);
            XElement compiledDataPoints = new XElement(Tag_DataPointDefinition);

            foreach (var doc in docs)
            {
                var dpConfiguration = doc.Element(Tag_DpConfiguration);

                compiledProviderConfig.Add(
                    dpConfiguration.Elements(Tag_ProviderDefinition).Elements());

                compiledProcessorConfig.Add(
                    dpConfiguration.Elements(Tag_ProcessorDefinition).Elements());

                compiledDataPoints.Add(
                    dpConfiguration.Elements(Tag_DataPointDefinition).Elements(Tag_DpValue));

                compiledDataPoints.Add(
                    dpConfiguration.Elements(Tag_DataPointDefinition).Elements(Tag_DpMethod));
            }

            ProviderConfiguration = new XDocument(compiledProviderConfig);
            ProcessorConfiguration = new XDocument(compiledProcessorConfig);
            DataPointConfiguration = new XDocument(compiledDataPoints);

        }

        public XDocument ProviderConfiguration { get; private set; }
        public XDocument ProcessorConfiguration { get; private set; }
        public XDocument DataPointConfiguration { get; private set; }

    }
}