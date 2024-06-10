import withReactContent from 'sweetalert2-react-content';
import Swal from 'sweetalert2';
import { buildUrlRedirectToLogin } from '../Shared/RedirectToLogin';
const Toastr = withReactContent(Swal);

const show = async options =>
  Toastr.fire({
    ...options
  });

const redirectToLogin = () =>
  (window.location.href = buildUrlRedirectToLogin());

const showWarningSessionExpired = async () =>
  Toastr.fire({
    icon: 'warning',
    html: (
      <>
        <span>Your session has expired!</span>
        <br></br>
        <span>Because of the security, you have to re-login your account.</span>
      </>
    ),
    allowOutsideClick: false,

    allowEnterKey: false
  }).then(() => redirectToLogin());

const showInfo = async message =>
  Toastr.fire({
    icon: 'info',
    html: message,
    allowOutsideClick: false,
    allowEnterKey: false
  });

const showWarning = async message =>
  Toastr.fire({
    icon: 'warning',
    html: message,
    allowOutsideClick: false,
    allowEnterKey: false
  });

const showError = async message =>
  Toastr.fire({
    icon: 'error',
    html: message,
    allowOutsideClick: false,
    allowEnterKey: false
  });

const confirm = async message =>
  Toastr.fire({
    icon: 'question',
    html: message,
    allowOutsideClick: false,
    allowEnterKey: false,
    showCancelButton: true,
    reverseButtons: true
  });

const confirmYesNo = async message =>
  Toastr.fire({
    icon: 'question',
    html: message,
    allowOutsideClick: false,
    allowEnterKey: false,
    showCancelButton: true,
    reverseButtons: true,
    confirmButtonText: 'Yes',
    cancelButtonText: 'No'
  });

const showNotifySuccess = async message =>
  Toastr.fire({
    html: message,
    timer: 3000,
    showConfirmButton: false,
    position: 'top',
    customClass: 'toast-notify',
    showClass: {
      backdrop: '',
      popup: '',
      icon: ''
    },
    hideClass: {
      backdrop: '',
      popup: '',
      icon: ''
    }
  });

const ToastrWrapper = {
  show,
  showInfo,
  showError,
  showWarning,
  confirm,
  confirmYesNo,
  showWarningSessionExpired,
  showNotifySuccess
};

export default ToastrWrapper;
