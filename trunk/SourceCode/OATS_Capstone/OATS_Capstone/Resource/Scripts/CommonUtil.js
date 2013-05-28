function convertJsonDatetoDate(jsondate)
{
    if (jsondate) {
        return new Date(parseInt(jsondate.substr(6)));
    } else {
        return null;
    }
}
//function convertJqueryArrayToJSArray(array) {
//    var arr = [];
//    for (var i = 0; i < array.length; i++) {
//        arr[0] = array[0];
//    }
//    return arr;
//}
$.fn.extend({
    convertJqueryArrayToJSArray: function () {
        var arr = [];
        for (var i = 0; i < this.length; i++) {
            arr[i] = this[i];
        }
        return arr;
    }
});
function showInfoMessage(message) {

}
function showErrorMessage(message) {
}