import { buildRequestURL } from '../../../../../Shared/Service/serviceHelper';
import { post } from '../../../../../Shared/Service/Service';
import { TUpdateSettingSender } from '../EditType';

const updateSettingService = async (model: TUpdateSettingSender) => {
  const url = buildRequestURL('NewLeagueSetting', 'editLeagueSetting');
  const response = await post({ url, ...model });
  return response;
};

export { updateSettingService };
