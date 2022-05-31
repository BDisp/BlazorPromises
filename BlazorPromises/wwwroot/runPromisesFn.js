const methodName = 'PromiseCallback';

callbackMethod = function (
    callbackHandler,
    callbackId,
    value) {
    //callbackHandler.invokeMethodAsync(methodName, callbackId, JSON.stringify(value));
    callbackHandler.invokeMethodAsync(methodName, callbackId, value);
}

const errorMethodName = 'PromiseError';

errorCallbackMethod = function (
    callbackHandler,
    callbackId,
    value) {
    //callbackHandler.invokeMethodAsync(errorMethodName, callbackId, JSON.stringify(value));
    callbackHandler.invokeMethodAsync(errorMethodName, callbackId, value);
}

runFunction = function (callbackHandler, callbackId, fnName, data) {

    let promise = window[fnName](data);

    promise.then(value => {
        if (value === undefined) {
            value = null;
        }
        const result = JSON.stringify(value);
        callbackMethod(
            callbackHandler,
            callbackId,
            result)
    }).catch(reason => {
        if (!reason) {
            reason = "Something went wrong";
        }
        const result = reason.toString();
        errorCallbackMethod(
            callbackHandler,
            callbackId,
            result)
    });

    // Your function currently has to return something.
    return true;
}
