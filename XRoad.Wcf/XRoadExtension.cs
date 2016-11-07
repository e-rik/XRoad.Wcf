using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;

namespace XRoad.Wcf
{
    public class XRoadExtension : IWsdlImportExtension
    {
        public void BeforeImport(ServiceDescriptionCollection wsdlDocuments, XmlSchemaSet xmlSchemas, ICollection<XmlElement> policy)
        {
            Console.WriteLine("BeforeImport");
        }

        public void ImportContract(WsdlImporter importer, WsdlContractConversionContext context)
        {
            Console.WriteLine("ImportContract");
        }

        public void ImportEndpoint(WsdlImporter importer, WsdlEndpointConversionContext context)
        {
            ImportXRoadPort(context);
            ImportXRoadBinding(context);
        }

        private void ImportXRoadBinding(WsdlEndpointConversionContext context)
        {
            if (context.WsdlBinding == null)
                return;

            foreach (OperationBinding op in context.WsdlBinding.Operations)
            {
                var node = op.Extensions.Find("version", NamespaceConstants.XROAD31);
                if (node != null)
                    op.Extensions.Remove(node);
            }
        }

        private void ImportXRoadPort(WsdlEndpointConversionContext context)
        {
            if (context.WsdlPort == null)
                return;

            var addressNode = context.WsdlPort.Extensions.Find("address", NamespaceConstants.XROAD31);
            if (addressNode != null)
                context.WsdlPort.Extensions.Remove(addressNode);

            var titleExtensions = context.WsdlPort.Extensions.FindAll("title", NamespaceConstants.XROAD31).ToList();
            if (!titleExtensions.Any())
                return;

            var documentationContractBehavior = new DocumentationContractBehavior();
            foreach (var node in titleExtensions)
            {
                var languageAttribute = node.GetAttribute("lang", NamespaceConstants.XML);

                documentationContractBehavior.AddComment(languageAttribute ?? "", node.InnerText);
                context.WsdlPort.Extensions.Remove(node);
            }

            context.ContractConversionContext.Contract.Behaviors.Add(documentationContractBehavior);
        }
    }
}
