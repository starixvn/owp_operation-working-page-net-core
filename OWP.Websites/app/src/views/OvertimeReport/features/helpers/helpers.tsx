import { netCoreURL } from '../../Const.json';

const buildRequestURL = (key: string) => `${netCoreURL}/${key}`;

const formatFractionDigit = (value: number | null | undefined, digit: number) =>
  value
    ? value.toLocaleString(undefined, {
        minimumFractionDigits: digit,
        maximumFractionDigits: digit
      })
    : '-';

export { formatFractionDigit, buildRequestURL };
