import { HAS_OTHER_PROVIDER } from './filterConst.json';
import ProviderSelectionTemplate from './ProviderSelectionTemplate';

const HasOtherProvider = () => {
  return (
    <ProviderSelectionTemplate
      label='Has Other Provider'
      filterKey={HAS_OTHER_PROVIDER}
    />
  );
};

export default HasOtherProvider;