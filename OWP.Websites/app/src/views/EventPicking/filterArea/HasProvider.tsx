import { HAS_PROVIDER } from './filterConst.json';
import ProviderSelectionTemplate from './ProviderSelectionTemplate';

const HasProvider = () => {
  return (
    <ProviderSelectionTemplate
      label='Has Provider'
      filterKey={HAS_PROVIDER}
    />
  );
};

export default HasProvider;