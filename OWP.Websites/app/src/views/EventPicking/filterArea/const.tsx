import Utils from '../../Shared/Utils';
import { PERIOD } from './filterConst.json';
const SELECT_TURNOVER_OPTIONS = [
  {
    'key': PERIOD.VALUE.ONE_MONTH,
    'value': 30,
    'label': 'Last 1 month',
    'fromDate': Utils.formatDate(
      new Date(new Date().setDate(new Date().getDate() - 30))
    ),
    'toDate': Utils.formatDate(new Date()),
    'order': 1
  },
  {
    'key': PERIOD.VALUE.THREE_MONTHS,
    'value': 90,
    'label': 'Last 3 months',
    'fromDate': Utils.formatDate(
      new Date(new Date().setDate(new Date().getDate() - 90))
    ),
    'toDate': Utils.formatDate(new Date()),
    'order': 2
  },
  {
    'key': PERIOD.VALUE.SIX_MONTHS,
    'value': 180,
    'label': 'Last 6 months',
    'fromDate': Utils.formatDate(
      new Date(new Date().setDate(new Date().getDate() - 180))
    ),
    'toDate': Utils.formatDate(new Date()),
    'order': 3
  },
  {
    'key': PERIOD.VALUE.TWELVE_MONTH,
    'value': 365,
    'label': 'Last 12 months',
    'fromDate': Utils.formatDate(
      new Date(new Date().setDate(new Date().getDate() - 365))
    ),
    'toDate': Utils.formatDate(new Date()),
    'order': 4
  }
];

export { SELECT_TURNOVER_OPTIONS };
