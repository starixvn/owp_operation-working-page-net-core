import { create } from 'zustand';
import { OsLeagueState, TPolicy } from '../typing/osLeagueStateType';
import produce from 'immer';
import { mountStoreDevtool } from 'simple-zustand-devtools';
import {
  getLeagueSetting,
  simpleApiKey,
  getLogSetting,
  optionValueNone
} from '../osLeague.json';
import {
  NET_CORE_QUERY_SIMPLE_API,
  REQUEST_STATUS
} from '../../Shared/Constants';
import axios from 'axios';
import {
  buildPostModelAddOrSaveSetting,
  formatLeagueDetailResponse,
  getURL,
  validateRequiredField,
  setDuplicateField
} from '../helper/OsLeagueSettingHelper';
import { post } from '../../Shared/Service/Service';

const simpleApi = `${NET_CORE_QUERY_SIMPLE_API}`;

const settingInfoDefault = {
  leagueSettingId: 0,
  leagueName: '',
  isActive: false,
  isActiveMaxBet: false,
  region: optionValueNone,
  category: optionValueNone,
  weekMatch: optionValueNone,
  betDelay: null,
  koFromTime: '',
  koToTime: '',
  remark: '',
  isManualUpdateScore: false,
  isIptvOnly: false,
  spreadHDP: null,
  spreadOU: null,
  maxbetHDP: null,
  maxbetOU: null,
  rounds: null,
  offerLines: null,
  providers: null,
  betTypes: null
};

const useStore = create<OsLeagueState>((set, get) => ({
  filter: {
    league: '',
    status: '',
    category: '',
    round: ''
  },
  settingInfo: settingInfoDefault,
  settingInfoDefault,
  settingInfoOrigin: settingInfoDefault,
  errorMessageDefault: {
    leagueName: null,
    offerLines: null,
    betDelay: null,
    maxbetHDP: null,
    maxbetOU: null,
    betTypes: null,
    provider: null,
    rangeTime: null,
    round: null,
    iptv: null,
    remark: null,
    valid: true
  },
  errorMessage: {
    leagueName: null,
    offerLines: null,
    betDelay: null,
    maxbetHDP: null,
    maxbetOU: null,
    betTypes: null,
    provider: null,
    rangeTime: null,
    round: null,
    iptv: null,
    remark: null,
    valid: true
  },
  policy: {
    Username: null,
    HasRead: null,
    HasWrite: null,
    ActorId: null,
    SiteId: null
  },
  loading: REQUEST_STATUS.idle,
  entities: [],
  logData: [],
  popupType: null,
  isRequireField: false,

  setPolicy: (policy: TPolicy) => {
    set(
      produce((state: OsLeagueState) => {
        state.policy = policy;
      })
    );
  },
  getPolicy: () => {
    return get().policy;
  },
  setFilter: ({ key, value }) => {
    set(
      produce(state => {
        state.filter[key] = value;
      })
    );
  },
  setPopupType: value =>
    set(
      produce((state: OsLeagueState) => {
        state.popupType = value;
      })
    ),
  setSettingInfo: (key, value) =>
    set(
      produce(state => {
        state.settingInfo[key] = value;
      })
    ),
  getLogSettingQuery: async leagueSettingId => {
    const url = getURL(getLogSetting);
    const result = await axios.get(url, {
      params: { leagueSettingId }
    });
    if (result.data) {
      set({ logData: result.data });
    }
  },
  queryDataSource: async () => {
    set({ loading: REQUEST_STATUS.pending });
    const url = getURL(getLeagueSetting);
    const { league, status, category, round } = get().filter;

    const result = await post({
      url,
      League: league,
      StatusId: status,
      CategoryId: category,
      RoundId: round
    });

    if (result.data) {
      set({ loading: REQUEST_STATUS.idle });
      set({ entities: result.data });
    }
  },
  queryLeagueDetail: async leagueId => {
    set({ loading: REQUEST_STATUS.pending });

    const response = await post({
      url: simpleApi,
      apiKey: simpleApiKey.leagueDetail,
      leagueSettingId: leagueId
    });

    const settingInfo = formatLeagueDetailResponse(response.data);

    if (settingInfo) {
      set({ settingInfo });
      set({ settingInfoOrigin: settingInfo });
    }

    set({ loading: REQUEST_STATUS.idle });
  },
  validateBeforeSubmit: () => {
    const { settingInfo } = get();

    const validationResult = validateRequiredField(settingInfo);

    set(
      produce((state: OsLeagueState) => {
        state.errorMessage = validationResult;
      })
    );
    return validationResult.valid;
  },
  saveSetting: async segment => {
    const url = getURL(segment);
    const { settingInfo } = get();

    let status = 0;
    const postModel = buildPostModelAddOrSaveSetting(settingInfo);
    await post({
      url,
      ...postModel
    })
      .then(x => (status = x.status))
      .catch(() => {
        set(
          produce((state: OsLeagueState) => {
            state.errorMessage = setDuplicateField();
          })
        );
      });
    return status;
  },
  isChangeData: isAdd => {
    const { settingInfoOrigin, settingInfo } = get();
    return isAdd
      ? JSON.stringify(settingInfoDefault) !== JSON.stringify(settingInfo)
      : JSON.stringify(settingInfoOrigin) !== JSON.stringify(settingInfo);
  },
  resetSetting: () => set({ settingInfo: get().settingInfoDefault }),
  resetErrorMessage: () => {
    const { errorMessageDefault } = get();
    set({ errorMessage: errorMessageDefault });
  }
}));

if (process.env.NODE_ENV === 'development') {
  mountStoreDevtool('OS-league-setting', useStore);
}

export type RootState = ReturnType<typeof useStore.getState>;

export default useStore;
