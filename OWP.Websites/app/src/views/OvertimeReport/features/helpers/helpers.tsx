import { baseApiUrl } from '../../../Shared/Constants';

const buildRequestURL = (key: string) =>
  `${baseApiUrl}/api/OvertimeReport/${key}`;

const formatFractionDigit = (value: number | null | undefined, digit: number) =>
  value
    ? value.toLocaleString(undefined, {
        minimumFractionDigits: digit,
        maximumFractionDigits: digit
      })
    : '-';

export { formatFractionDigit, buildRequestURL };
