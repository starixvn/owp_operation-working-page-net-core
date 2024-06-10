import { netCoreURL, netFwURL } from '../../Const.json';

const buildRequestURL = (key: string) => process.env.NODE_ENV === 'development'
  ? `${netCoreURL}/${key}`
  : `${window.netCoreRootUrl}/${netFwURL}/${key}`;

const formatFractionDigit = (
  value: number | null | undefined,
  digit: number
) => value
    ? value.toLocaleString(undefined, {
      minimumFractionDigits: digit,
      maximumFractionDigits: digit
    })
    : '-';

export { formatFractionDigit, buildRequestURL };
