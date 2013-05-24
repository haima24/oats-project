function convertJsonDatetoDate(jsondate)
{
    if (jsondate) {
        return new Date(parseInt(jsondate.substr(6)));
    } else {
        return null;
    }
}
function convertJqueryArrayToJSArray(array) {
    var arr = [];
    for (var i = 0; i < array.length; i++) {
        arr[0] = array[0];
    }
    return arr;
}