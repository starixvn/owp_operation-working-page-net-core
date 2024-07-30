/* global messageBoxFactory */
"use strict";
function NotifyCopySuccess() {
    toastr.options = {
        'newestOnTop': false,
        "closeButton": false,
        "timeOut": 3000,
        "positionClass": "toast-coppy",
        "preventDuplicates": true,
    };
    toastr.info('Match information successfully copied to clipboard');
}
function executeCopy(...params) {
    let content = params.join("\n");
    function listener(e) {
        e.clipboardData.setData('text/plain', content);
        e.preventDefault();
    }
    document.addEventListener('copy', listener);
    document.execCommand('copy');
    document.removeEventListener('copy', listener);
    NotifyCopySuccess();
};
