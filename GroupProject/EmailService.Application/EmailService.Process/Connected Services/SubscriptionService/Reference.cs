﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmailService.Process.SubscriptionService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SubscriptionService.ISubscriptionService")]
    public interface ISubscriptionService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Subscribe", ReplyAction="http://tempuri.org/ISubscriptionService/SubscribeResponse")]
        void Subscribe(string pTopic, string pCallerAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Subscribe", ReplyAction="http://tempuri.org/ISubscriptionService/SubscribeResponse")]
        System.Threading.Tasks.Task SubscribeAsync(string pTopic, string pCallerAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Unsubscribe", ReplyAction="http://tempuri.org/ISubscriptionService/UnsubscribeResponse")]
        void Unsubscribe(string pTopic, string pCallerAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISubscriptionService/Unsubscribe", ReplyAction="http://tempuri.org/ISubscriptionService/UnsubscribeResponse")]
        System.Threading.Tasks.Task UnsubscribeAsync(string pTopic, string pCallerAddress);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISubscriptionServiceChannel : EmailService.Process.SubscriptionService.ISubscriptionService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SubscriptionServiceClient : System.ServiceModel.ClientBase<EmailService.Process.SubscriptionService.ISubscriptionService>, EmailService.Process.SubscriptionService.ISubscriptionService {
        
        public SubscriptionServiceClient() {
        }
        
        public SubscriptionServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SubscriptionServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SubscriptionServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SubscriptionServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void Subscribe(string pTopic, string pCallerAddress) {
            base.Channel.Subscribe(pTopic, pCallerAddress);
        }
        
        public System.Threading.Tasks.Task SubscribeAsync(string pTopic, string pCallerAddress) {
            return base.Channel.SubscribeAsync(pTopic, pCallerAddress);
        }
        
        public void Unsubscribe(string pTopic, string pCallerAddress) {
            base.Channel.Unsubscribe(pTopic, pCallerAddress);
        }
        
        public System.Threading.Tasks.Task UnsubscribeAsync(string pTopic, string pCallerAddress) {
            return base.Channel.UnsubscribeAsync(pTopic, pCallerAddress);
        }
    }
}