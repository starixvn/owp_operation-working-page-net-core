import { Dictionary } from "lodash";

interface MonthOvertimeData {
  dateColumns: DateColumnInfo[],
  totalHours: number;
  traderOvertime: TraderOvertime[];
  totalHoursByDate: number[];
}

interface TraderOvertime {
  location: string,
  trader: string,
  total: number,
  overtime: Dictionary<Overtime>
}

class Overtime {
  hour: number;
  isOffDay: boolean;

  constructor() {
    this.hour = 0;
    this.isOffDay = false;
  }
}

interface DateColumnInfo {
  date: string,
  isWeekend: boolean,
  dayOfWeek: string
}

export {
  Overtime,
  TraderOvertime,
  MonthOvertimeData,
  DateColumnInfo
};
