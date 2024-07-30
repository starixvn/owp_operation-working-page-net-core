const createModelPerformance = data => {
  return {
    Trader: data.Username,
    Section: data.SectionName,
    Match: data.TotalMatch,
    Turnover: {
      FT_HDP: data.TurnOver_FT_HDP,
      FT_OU: data.TurnOver_FT_OU,
      ONEH_HDP: data.TurnOver_1H_HDP,
      ONEH_OU: data.TurnOver_1H_OU,
      Other_Products: data.TurnOver_Others
    },
    WinLoss: {
      FT_HDP: data.WinLoss_FT_HDP,
      FT_OU: data.Winloss_FT_OU,
      ONEH_HDP: data.Winloss_1H_HDP,
      ONEH_OU: data.Winloss_1H_OU,
      Other_Products: data.Winloss_Others
    }
  };
};

const createModelAssignment = response => {
  return {
    Trader: response.Username,
    Section: response.SectionName,
    Match: response.TotalMatch,
    Type1: {
      FT_HDP: response.Type1_FT_HDP,
      FT_OU: response.Type1_FT_OU,
      FT_HDP_OU: response.Type1_FT_HDPOU
    },
    Type2: {
      A_League: response.Type2_AUS_League,
      A_League_C: response.Type2_AUS_League_Corner,
      J1: response.Type2_JAP_Divison1,
      J2: response.Type2_JAP_Divison2
    },
    Robot3: {
      x2: response.Robot3_X2,
      x3: response.Robot3_X3,
      x3Plus: response.Robot3_X3_Plus
    },
    Type3: {
      SOLO_MJ: response.Type3_Solo_Major
    },
    Type4: {
      MAJOR: response.Type4_Major,
      MIN_1: response.Type4_Minor1,
      MIN_2: response.Type4_Minor2,
      MIN_3: response.Type4_Minor3,
      FT_OTHER: response.Type4_FT_Other,
      ONEST_MJ: response.Type4_1ST_Major,
      ONEST_Other: response.Type4_1ST_Other,
      FANTASY: response.Type4_Fantasy,
      CR_NCR: response.Type4_Corner_NextCorner,
      FIFTEENM: response.Type4_15M,
      TENM: response.Type4_10M,
      TB: response.Type4_TotalBooking,
      PK_SO: response.Type4_PKSO,
      Win: response.Type4_Winner
    },
    PH_1x2: {
      MAJOR: response.PH_1X2_Major,
      MIN_1: response.PH_1X2_Minor1,
      MIN_2: response.PH_1X2_Minor2,
      MIN_3: response.PH_1X2_Minor3,
      Others: response.PH_1X2_Others
    },
    E_SOCCERS: {
      ESoccer_5Mins: response.ESoccer_5Mins,
      ESoccer_6Mins: response.ESoccer_6Mins,
      ESoccer_15Mins: response.ESoccer_15Mins,
      ESoccer_16Mins: response.ESoccer_16Mins,
      ESoccer_20Mins: response.ESoccer_20Mins
    },
    X5: {
      x5: response.Robot5_X5
    }
  };
};

const replaceZeroBeforeMapping = response => {
  response.forEach(res => {
    for (const key of Object.keys(res)) {
      if (res[key] === 0) {
        res[key] = '-';
      }
    }
  });
};

const mappingResponseToModelPerformance = response => {
  replaceZeroBeforeMapping(response);
  return response.map(res => createModelPerformance(res));
};

const mappingResponseToModelAssignment = response => {
  replaceZeroBeforeMapping(response);
  return response.map(res => createModelAssignment(res));
};

const numberWithCommas = source =>
  source?.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');

const formatNumberWithTwoDecimal = source =>
  source % 1 === 0 ? source : source?.toFixed(2);

const mappingResponseToModelViewAll = response => {
  replaceZeroBeforeMapping(response);
  return response.map(res => {
    return {
      ...createModelAssignment(res),
      ...createModelPerformance(res)
    };
  });
};

const NormalizeDataHelper = {
  mappingResponseToModelAssignment,
  mappingResponseToModelPerformance,
  mappingResponseToModelViewAll,
  numberWithCommas,
  formatNumberWithTwoDecimal
};

export default NormalizeDataHelper;
