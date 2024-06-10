import isEqual from 'lodash.isequal';
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

const buildPermissionDetailLink = (detailURl = 'api/application/authorize') => {
  if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
    return `/${detailURl}`;
  }
  const segmentFirst = window.location.pathname.split('/')[1];
  return segmentPrefix.some(seg => isEqual(seg, segmentFirst))
    ? `/${segmentFirst}/${detailURl}`
    : `/${detailURl}`;
};
export { buildPermissionDetailLink };
