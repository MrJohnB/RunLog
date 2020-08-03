function javascriptMethod() {
    var message = "JavaScript method called from C# that writes to <p> tag.";
    var element = document.getElementById("paragraph");
    var node = document.createTextNode(message);
    element.appendChild(node);
}

function javascriptFunction() {
    return "Response string from JavaScript function";
}

function javaScriptThatInvokesCallback() {
    DotNet.invokeMethodAsync("LiteBulb.RunLog.Web", "CallbackMethod", );
}

function javaScriptThatInvokesCallbackWithData() {
    //DotNet.invokeMethodAsync("LiteBulb.RunLog.Web", "CallbackMethodRecievesData", "String data sent from Javascript to Blazor callback method");
    // Send back an object (note: don't need to call JSON.stringify)
    DotNet.invokeMethodAsync("LiteBulb.RunLog.Web", "CallbackMethodRecievesData", { message: "Data sent from Javascript to Blazor callback method" });
}