﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=2.0.50727.42.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="SimServiceSoap", Namespace="http://www.decisionpower.com/")]
public partial class SimService : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback CreateSimulationOperationCompleted;
    
    private System.Threading.SendOrPostCallback RunSimulationOperationCompleted;
    
    private System.Threading.SendOrPostCallback UpdateSimDataOperationCompleted;
    
    private System.Threading.SendOrPostCallback GetResultsOperationCompleted;
    
    /// <remarks/>
    public SimService() {
        this.Url = "http://www.noblerwe.org/SimService.asmx";
    }
    
    /// <remarks/>
    public event CreateSimulationCompletedEventHandler CreateSimulationCompleted;
    
    /// <remarks/>
    public event RunSimulationCompletedEventHandler RunSimulationCompleted;
    
    /// <remarks/>
    public event UpdateSimDataCompletedEventHandler UpdateSimDataCompleted;
    
    /// <remarks/>
    public event GetResultsCompletedEventHandler GetResultsCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.decisionpower.com/CreateSimulation", RequestNamespace="http://www.decisionpower.com/", ResponseNamespace="http://www.decisionpower.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public System.Guid CreateSimulation(System.Guid userId) {
        object[] results = this.Invoke("CreateSimulation", new object[] {
                    userId});
        return ((System.Guid)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginCreateSimulation(System.Guid userId, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("CreateSimulation", new object[] {
                    userId}, callback, asyncState);
    }
    
    /// <remarks/>
    public System.Guid EndCreateSimulation(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((System.Guid)(results[0]));
    }
    
    /// <remarks/>
    public void CreateSimulationAsync(System.Guid userId) {
        this.CreateSimulationAsync(userId, null);
    }
    
    /// <remarks/>
    public void CreateSimulationAsync(System.Guid userId, object userState) {
        if ((this.CreateSimulationOperationCompleted == null)) {
            this.CreateSimulationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateSimulationOperationCompleted);
        }
        this.InvokeAsync("CreateSimulation", new object[] {
                    userId}, this.CreateSimulationOperationCompleted, userState);
    }
    
    private void OnCreateSimulationOperationCompleted(object arg) {
        if ((this.CreateSimulationCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.CreateSimulationCompleted(this, new CreateSimulationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.decisionpower.com/RunSimulation", RequestNamespace="http://www.decisionpower.com/", ResponseNamespace="http://www.decisionpower.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public System.Guid RunSimulation(System.Guid userId, System.Guid simId) {
        object[] results = this.Invoke("RunSimulation", new object[] {
                    userId,
                    simId});
        return ((System.Guid)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginRunSimulation(System.Guid userId, System.Guid simId, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("RunSimulation", new object[] {
                    userId,
                    simId}, callback, asyncState);
    }
    
    /// <remarks/>
    public System.Guid EndRunSimulation(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((System.Guid)(results[0]));
    }
    
    /// <remarks/>
    public void RunSimulationAsync(System.Guid userId, System.Guid simId) {
        this.RunSimulationAsync(userId, simId, null);
    }
    
    /// <remarks/>
    public void RunSimulationAsync(System.Guid userId, System.Guid simId, object userState) {
        if ((this.RunSimulationOperationCompleted == null)) {
            this.RunSimulationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRunSimulationOperationCompleted);
        }
        this.InvokeAsync("RunSimulation", new object[] {
                    userId,
                    simId}, this.RunSimulationOperationCompleted, userState);
    }
    
    private void OnRunSimulationOperationCompleted(object arg) {
        if ((this.RunSimulationCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.RunSimulationCompleted(this, new RunSimulationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.decisionpower.com/UpdateSimData", RequestNamespace="http://www.decisionpower.com/", ResponseNamespace="http://www.decisionpower.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int UpdateSimData(System.Guid userId, System.Guid simId, SimInput input) {
        object[] results = this.Invoke("UpdateSimData", new object[] {
                    userId,
                    simId,
                    input});
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginUpdateSimData(System.Guid userId, System.Guid simId, SimInput input, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UpdateSimData", new object[] {
                    userId,
                    simId,
                    input}, callback, asyncState);
    }
    
    /// <remarks/>
    public int EndUpdateSimData(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public void UpdateSimDataAsync(System.Guid userId, System.Guid simId, SimInput input) {
        this.UpdateSimDataAsync(userId, simId, input, null);
    }
    
    /// <remarks/>
    public void UpdateSimDataAsync(System.Guid userId, System.Guid simId, SimInput input, object userState) {
        if ((this.UpdateSimDataOperationCompleted == null)) {
            this.UpdateSimDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateSimDataOperationCompleted);
        }
        this.InvokeAsync("UpdateSimData", new object[] {
                    userId,
                    simId,
                    input}, this.UpdateSimDataOperationCompleted, userState);
    }
    
    private void OnUpdateSimDataOperationCompleted(object arg) {
        if ((this.UpdateSimDataCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.UpdateSimDataCompleted(this, new UpdateSimDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.decisionpower.com/GetResults", RequestNamespace="http://www.decisionpower.com/", ResponseNamespace="http://www.decisionpower.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public SimOutput GetResults(System.Guid userId, System.Guid simId) {
        object[] results = this.Invoke("GetResults", new object[] {
                    userId,
                    simId});
        return ((SimOutput)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginGetResults(System.Guid userId, System.Guid simId, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("GetResults", new object[] {
                    userId,
                    simId}, callback, asyncState);
    }
    
    /// <remarks/>
    public SimOutput EndGetResults(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((SimOutput)(results[0]));
    }
    
    /// <remarks/>
    public void GetResultsAsync(System.Guid userId, System.Guid simId) {
        this.GetResultsAsync(userId, simId, null);
    }
    
    /// <remarks/>
    public void GetResultsAsync(System.Guid userId, System.Guid simId, object userState) {
        if ((this.GetResultsOperationCompleted == null)) {
            this.GetResultsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetResultsOperationCompleted);
        }
        this.InvokeAsync("GetResults", new object[] {
                    userId,
                    simId}, this.GetResultsOperationCompleted, userState);
    }
    
    private void OnGetResultsOperationCompleted(object arg) {
        if ((this.GetResultsCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.GetResultsCompleted(this, new GetResultsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.decisionpower.com/")]
public partial class SimInput {
    
    private MediaComp[] compsField;
    
    /// <remarks/>
    public MediaComp[] comps {
        get {
            return this.compsField;
        }
        set {
            this.compsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.decisionpower.com/")]
public partial class MediaComp {
    
    private System.Guid mediaPartIdField;
    
    private int startDateField;
    
    private int spanField;
    
    private int[] impressionsField;
    
    /// <remarks/>
    public System.Guid mediaPartId {
        get {
            return this.mediaPartIdField;
        }
        set {
            this.mediaPartIdField = value;
        }
    }
    
    /// <remarks/>
    public int startDate {
        get {
            return this.startDateField;
        }
        set {
            this.startDateField = value;
        }
    }
    
    /// <remarks/>
    public int span {
        get {
            return this.spanField;
        }
        set {
            this.spanField = value;
        }
    }
    
    /// <remarks/>
    public int[] impressions {
        get {
            return this.impressionsField;
        }
        set {
            this.impressionsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.decisionpower.com/")]
public partial class Metric {
    
    private string typeField;
    
    private float[] valuesField;
    
    /// <remarks/>
    public string Type {
        get {
            return this.typeField;
        }
        set {
            this.typeField = value;
        }
    }
    
    /// <remarks/>
    public float[] values {
        get {
            return this.valuesField;
        }
        set {
            this.valuesField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.decisionpower.com/")]
public partial class SimOutput {
    
    private Metric[] metricsField;
    
    /// <remarks/>
    public Metric[] metrics {
        get {
            return this.metricsField;
        }
        set {
            this.metricsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
public delegate void CreateSimulationCompletedEventHandler(object sender, CreateSimulationCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class CreateSimulationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal CreateSimulationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public System.Guid Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((System.Guid)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
public delegate void RunSimulationCompletedEventHandler(object sender, RunSimulationCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class RunSimulationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal RunSimulationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public System.Guid Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((System.Guid)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
public delegate void UpdateSimDataCompletedEventHandler(object sender, UpdateSimDataCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class UpdateSimDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal UpdateSimDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
public delegate void GetResultsCompletedEventHandler(object sender, GetResultsCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class GetResultsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal GetResultsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public SimOutput Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((SimOutput)(this.results[0]));
        }
    }
}