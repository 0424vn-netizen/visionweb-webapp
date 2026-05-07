using System;
using System.Linq;
using System.ServiceModel.Description;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public class ApiPermissionAttribute : Attribute, IOperationBehavior
    {
        public string PermissionCode { get; set; }

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // Method intentionally left empty.
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
            // Method intentionally left empty.
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(new SecurityParameterInspector(PermissionCode));
            if (!dispatchOperation.Parent.ChannelDispatcher.ErrorHandlers.Any(x => x is GlobalExceptionHandler))
            {
                dispatchOperation.Parent.ChannelDispatcher.ErrorHandlers.Add(new GlobalExceptionHandler());
            }
        }

        public void Validate(OperationDescription operationDescription)
        { 
            // Method intentionally left empty.
        }       
    }
}
