function setLocalStorage(key, value) {
    if (typeof (Storage) !== "undefined") {
        // Yes! localStorage and sessionStorage support!
        // Some code.....
        localStorage.setItem(key, value);
    }
    else {
        // Sorry! No web storage support..
    }
}
function getLocalStorage(key) {
    var obj;
    if (typeof (Storage) !== "undefined") {
        // Yes! localStorage and sessionStorage support!
        // Some code.....
        obj=localStorage.getItem(key);
    }
    else {
        // Sorry! No web storage support..
    }
    return obj;
}