const buildRequestURL = (key: string) => `/api/OvertimeReport/${key}`;

const formatFractionDigit = (value: number | null | undefined, digit: number) =>
  value
    ? value.toLocaleString(undefined, {
        minimumFractionDigits: digit,
        maximumFractionDigits: digit
      })
    : '-';

export { formatFractionDigit, buildRequestURL };
