const noEventWarningMessage =
  '<div>WARNINGS! your upload file has no event assignments, this upload will affect to the system as below:\n' +
  '<ul style="text-align:left; margin:4px 0;padding-left: 15px;">\n' +
  '<li> Remove all the current traders assigned. </li>\n' +
  '<li> Remove all Live Status(red LIVE) on the member site. </li>\n' +
  '<li> Remove all odds compiler from Early. </li>\n' +
  '</ul>\n' +
  '</div> Are you sure?';

const noRecommendWarningMessage =
  'No recommend upload for match day(Today) assignments,  Do you want to continue?';

const toastOptions = {
  type: 'warning',
  allowOutsideClick: false,
  allowEnterKey: false,
  showCancelButton: true,
  confirmButtonText: 'Yes',
  cancelButtonText: 'No'
};

export { noEventWarningMessage, noRecommendWarningMessage, toastOptions };
