﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CP.CW1._9518.APIService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="APIService.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SaveToDB", ReplyAction="http://tempuri.org/IService1/SaveToDBResponse")]
        string SaveToDB(string flightNumber, string destination, string departureDate, string status, string gate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SaveToDB", ReplyAction="http://tempuri.org/IService1/SaveToDBResponse")]
        System.Threading.Tasks.Task<string> SaveToDBAsync(string flightNumber, string destination, string departureDate, string status, string gate);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : CP.CW1._9518.APIService.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<CP.CW1._9518.APIService.IService1>, CP.CW1._9518.APIService.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string SaveToDB(string flightNumber, string destination, string departureDate, string status, string gate) {
            return base.Channel.SaveToDB(flightNumber, destination, departureDate, status, gate);
        }
        
        public System.Threading.Tasks.Task<string> SaveToDBAsync(string flightNumber, string destination, string departureDate, string status, string gate) {
            return base.Channel.SaveToDBAsync(flightNumber, destination, departureDate, status, gate);
        }
    }
}
