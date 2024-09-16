import store from '../../app/storeNative';
import { TSubTypeRow } from '../../type';
import { highlightSubtype } from '../consolidation/subtype/subtypeSlice';

const highlightSubtypeHandler = (subtype: TSubTypeRow) => {
  store.dispatch(highlightSubtype({subtype}));
};

export { highlightSubtypeHandler };
