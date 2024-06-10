import { OVER_TIME_OPTIONS_VALUE } from "../features/TypeNative";

const OVERT_TIME = {
  OPTIONS: [
    {
      "value": OVER_TIME_OPTIONS_VALUE.ALL,
      "label": "All"
    },
    {
      "value": OVER_TIME_OPTIONS_VALUE.OVERTIME_PRE_SHIFT,
      "label": "Overtime Pre-Shift"
    },
    {
      "value": OVER_TIME_OPTIONS_VALUE.OVERTIME_POST_SHIFT,
      "label": "Overtime Post-Shift"
    },
    {
      "value": OVER_TIME_OPTIONS_VALUE.NO_OVERTIME,
      "label": "No Overtime"
    }
  ]
};

export {
  OVERT_TIME,
  OVER_TIME_OPTIONS_VALUE
};