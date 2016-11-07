using System.CodeDom;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace XRoad.Wcf
{
    internal class DocumentationContractBehavior : IContractBehavior, IServiceContractGenerationExtension
    {
        private readonly IDictionary<string, string> documentation = new Dictionary<string, string>();

        public void AddComment(string language, string doc)
        {
            documentation.Add(language, doc);
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        { }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        { }

        public void GenerateContract(ServiceContractGenerationContext context)
        {
            var configuration = XRoadExtensionConfig.GetConfiguration(context.ServiceContractGenerator.Configuration);

            string doc = null;
            if (documentation.TryGetValue(configuration.CommentLanguage, out doc))
            {
                context.ContractType.Comments.Add(new CodeCommentStatement("<summary>", true));
                context.ContractType.Comments.Add(new CodeCommentStatement(doc, true));
                context.ContractType.Comments.Add(new CodeCommentStatement("</summary>", true));
            }
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        { }
    }
}
