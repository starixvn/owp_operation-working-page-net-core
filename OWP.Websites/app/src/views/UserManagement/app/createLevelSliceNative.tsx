import { StateCreator } from 'zustand';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { TLevelOption, TLevelOptionsResponse } from '../type';
import axios from 'axios';
import update from 'immutability-helper';
import { normalizeOptions } from '../utils';

const defaultNoneOption = {
  value: 0,
  label: ''
};

const createLevelSliceNative: StateCreator<
  TLevelOption,
  [],
  [],
  TLevelOption
> = (set, get) => ({
  queryLevelOptionStatus: REQUEST_STATUS.idle,
  levelOptions: {
    levelOfLiveTrading: [],
    levelOfLiveAssistant: [],
    levelOfOtherSection: []
  },
  async queryLevelOption() {
    set(state =>
      update(state, {
        queryLevelOptionStatus: { $set: REQUEST_STATUS.pending }
      })
    );
    const url = `api/UserManagement/query-level-options`;
    const options = (await axios.get(url)).data as TLevelOptionsResponse;

    set(state =>
      update(state, {
        queryLevelOptionStatus: { $set: REQUEST_STATUS.idle },
        levelOptions: {
          levelOfLiveTrading: {
            $set: normalizeOptions(
              options.levelOfLiveTrading,
              defaultNoneOption
            )
          },
          levelOfLiveAssistant: {
            $set: normalizeOptions(
              options.levelOfLiveAssistant,
              defaultNoneOption
            )
          },
          levelOfOtherSection: {
            $set: normalizeOptions(
              options.levelOfOtherSection,
              defaultNoneOption
            )
          }
        }
      })
    );
  },
  selectOption(sectionId: number | null) {
    const { levelOfLiveAssistant, levelOfLiveTrading, levelOfOtherSection } =
      get().levelOptions;

    switch (sectionId) {
      case 2:
      case 30:
        return levelOfLiveAssistant;
      case 3:
        return levelOfLiveTrading;
      default:
        return levelOfOtherSection;
    }
  }
});

export default createLevelSliceNative;
