﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InsercionApp
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://bo.carnesdonfernando.com/", ConfigurationName="InsercionApp.InsercionAppSoap")]
    public interface InsercionAppSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://bo.carnesdonfernando.com/insertarCliente", ReplyAction="*")]
        System.Threading.Tasks.Task<InsercionApp.insertarClienteResponse> insertarClienteAsync(InsercionApp.insertarClienteRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://bo.carnesdonfernando.com/InsertaReferenciaramigoIdApp", ReplyAction="*")]
        System.Threading.Tasks.Task<InsercionApp.InsertaReferenciaramigoIdAppResponse> InsertaReferenciaramigoIdAppAsync(InsercionApp.InsertaReferenciaramigoIdAppRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class insertarClienteRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="insertarCliente", Namespace="http://bo.carnesdonfernando.com/", Order=0)]
        public InsercionApp.insertarClienteRequestBody Body;
        
        public insertarClienteRequest()
        {
        }
        
        public insertarClienteRequest(InsercionApp.insertarClienteRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://bo.carnesdonfernando.com/")]
    public partial class insertarClienteRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pidentificacionCliente;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string ptipoPersona;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string pNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string pApellido1;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string pApellido2;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string pEmail1;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string pFechaNacimiento;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string pSexo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string pTelefono;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string pProvincia;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string pCanton;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=11)]
        public string pDireccion;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=12)]
        public string pSucursal;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=13)]
        public string pSaldoPuntos;
        
        public insertarClienteRequestBody()
        {
        }
        
        public insertarClienteRequestBody(string pidentificacionCliente, string ptipoPersona, string pNombre, string pApellido1, string pApellido2, string pEmail1, string pFechaNacimiento, string pSexo, string pTelefono, string pProvincia, string pCanton, string pDireccion, string pSucursal, string pSaldoPuntos)
        {
            this.pidentificacionCliente = pidentificacionCliente;
            this.ptipoPersona = ptipoPersona;
            this.pNombre = pNombre;
            this.pApellido1 = pApellido1;
            this.pApellido2 = pApellido2;
            this.pEmail1 = pEmail1;
            this.pFechaNacimiento = pFechaNacimiento;
            this.pSexo = pSexo;
            this.pTelefono = pTelefono;
            this.pProvincia = pProvincia;
            this.pCanton = pCanton;
            this.pDireccion = pDireccion;
            this.pSucursal = pSucursal;
            this.pSaldoPuntos = pSaldoPuntos;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class insertarClienteResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="insertarClienteResponse", Namespace="http://bo.carnesdonfernando.com/", Order=0)]
        public InsercionApp.insertarClienteResponseBody Body;
        
        public insertarClienteResponse()
        {
        }
        
        public insertarClienteResponse(InsercionApp.insertarClienteResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://bo.carnesdonfernando.com/")]
    public partial class insertarClienteResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool insertarClienteResult;
        
        public insertarClienteResponseBody()
        {
        }
        
        public insertarClienteResponseBody(bool insertarClienteResult)
        {
            this.insertarClienteResult = insertarClienteResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class InsertaReferenciaramigoIdAppRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="InsertaReferenciaramigoIdApp", Namespace="http://bo.carnesdonfernando.com/", Order=0)]
        public InsercionApp.InsertaReferenciaramigoIdAppRequestBody Body;
        
        public InsertaReferenciaramigoIdAppRequest()
        {
        }
        
        public InsertaReferenciaramigoIdAppRequest(InsercionApp.InsertaReferenciaramigoIdAppRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://bo.carnesdonfernando.com/")]
    public partial class InsertaReferenciaramigoIdAppRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pidentificacionCliente;
        
        public InsertaReferenciaramigoIdAppRequestBody()
        {
        }
        
        public InsertaReferenciaramigoIdAppRequestBody(string pidentificacionCliente)
        {
            this.pidentificacionCliente = pidentificacionCliente;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class InsertaReferenciaramigoIdAppResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="InsertaReferenciaramigoIdAppResponse", Namespace="http://bo.carnesdonfernando.com/", Order=0)]
        public InsercionApp.InsertaReferenciaramigoIdAppResponseBody Body;
        
        public InsertaReferenciaramigoIdAppResponse()
        {
        }
        
        public InsertaReferenciaramigoIdAppResponse(InsercionApp.InsertaReferenciaramigoIdAppResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://bo.carnesdonfernando.com/")]
    public partial class InsertaReferenciaramigoIdAppResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool InsertaReferenciaramigoIdAppResult;
        
        public InsertaReferenciaramigoIdAppResponseBody()
        {
        }
        
        public InsertaReferenciaramigoIdAppResponseBody(bool InsertaReferenciaramigoIdAppResult)
        {
            this.InsertaReferenciaramigoIdAppResult = InsertaReferenciaramigoIdAppResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface InsercionAppSoapChannel : InsercionApp.InsercionAppSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class InsercionAppSoapClient : System.ServiceModel.ClientBase<InsercionApp.InsercionAppSoap>, InsercionApp.InsercionAppSoap
    {
        
        /// <summary>
        /// Implemente este método parcial para configurar el punto de conexión de servicio.
        /// </summary>
        /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
        /// <param name="clientCredentials">Credenciales de cliente</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public InsercionAppSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(InsercionAppSoapClient.GetBindingForEndpoint(endpointConfiguration), InsercionAppSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public InsercionAppSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(InsercionAppSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public InsercionAppSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(InsercionAppSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public InsercionAppSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<InsercionApp.insertarClienteResponse> InsercionApp.InsercionAppSoap.insertarClienteAsync(InsercionApp.insertarClienteRequest request)
        {
            return base.Channel.insertarClienteAsync(request);
        }
        
        public System.Threading.Tasks.Task<InsercionApp.insertarClienteResponse> insertarClienteAsync(string pidentificacionCliente, string ptipoPersona, string pNombre, string pApellido1, string pApellido2, string pEmail1, string pFechaNacimiento, string pSexo, string pTelefono, string pProvincia, string pCanton, string pDireccion, string pSucursal, string pSaldoPuntos)
        {
            InsercionApp.insertarClienteRequest inValue = new InsercionApp.insertarClienteRequest();
            inValue.Body = new InsercionApp.insertarClienteRequestBody();
            inValue.Body.pidentificacionCliente = pidentificacionCliente;
            inValue.Body.ptipoPersona = ptipoPersona;
            inValue.Body.pNombre = pNombre;
            inValue.Body.pApellido1 = pApellido1;
            inValue.Body.pApellido2 = pApellido2;
            inValue.Body.pEmail1 = pEmail1;
            inValue.Body.pFechaNacimiento = pFechaNacimiento;
            inValue.Body.pSexo = pSexo;
            inValue.Body.pTelefono = pTelefono;
            inValue.Body.pProvincia = pProvincia;
            inValue.Body.pCanton = pCanton;
            inValue.Body.pDireccion = pDireccion;
            inValue.Body.pSucursal = pSucursal;
            inValue.Body.pSaldoPuntos = pSaldoPuntos;
            return ((InsercionApp.InsercionAppSoap)(this)).insertarClienteAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<InsercionApp.InsertaReferenciaramigoIdAppResponse> InsercionApp.InsercionAppSoap.InsertaReferenciaramigoIdAppAsync(InsercionApp.InsertaReferenciaramigoIdAppRequest request)
        {
            return base.Channel.InsertaReferenciaramigoIdAppAsync(request);
        }
        
        public System.Threading.Tasks.Task<InsercionApp.InsertaReferenciaramigoIdAppResponse> InsertaReferenciaramigoIdAppAsync(string pidentificacionCliente)
        {
            InsercionApp.InsertaReferenciaramigoIdAppRequest inValue = new InsercionApp.InsertaReferenciaramigoIdAppRequest();
            inValue.Body = new InsercionApp.InsertaReferenciaramigoIdAppRequestBody();
            inValue.Body.pidentificacionCliente = pidentificacionCliente;
            return ((InsercionApp.InsercionAppSoap)(this)).InsertaReferenciaramigoIdAppAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.InsercionAppSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.InsercionAppSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.InsercionAppSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:8180/TM-PROD-01/InsercionApp.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.InsercionAppSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:8180/TM-PROD-01/InsercionApp.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            InsercionAppSoap,
            
            InsercionAppSoap12,
        }
    }
}
