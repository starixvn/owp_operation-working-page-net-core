import { isEqual } from 'lodash';

const buildUrlRedirectToLogin = () => {
  const segmentPrefix = [
    'V1',
    'V2',
    'V3',
    'V4',
    'V5',
    'V6',
    'V7',
    'Main',
    'v1',
    'v2',
    'OWP-Staging',
    'OWP'
  ];
  if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
    return ``;
  }
  const segmentFirst = window.location.pathname.split('/')[1];
  return segmentPrefix.some(seg => isEqual(seg, segmentFirst))
    ? `/${segmentFirst}/Auth?ReturnUrl=%2F`
    : `/Auth?ReturnUrl=%2F`;
};

export { buildUrlRedirectToLogin };
