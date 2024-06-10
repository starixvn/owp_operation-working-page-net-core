import { TProviderSelectionTemplate } from '../types/filterType';
import { HAS_PROVIDER_OPTIONS } from './filterConst.json';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';

const ProviderSelectionTemplate = (props: TProviderSelectionTemplate) => {
  return (
    <MultipleSelectionTemplate
      label={props.label}
      options={HAS_PROVIDER_OPTIONS}
      value={HAS_PROVIDER_OPTIONS}
      filterKey={props.filterKey}
      disabled={false} />
  );
};

export default ProviderSelectionTemplate;