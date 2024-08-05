import { buildRequestURL } from '../../../../Shared/Service/serviceHelper';
import { post } from '../../../../Shared/Service/Service';
import { TUpdateSettingSender } from '../component/Type';

const updateSettingService = async (model: TUpdateSettingSender) => {
  const url = buildRequestURL('NewLeagueSetting', 'update');
  const response = await post({ url, ...model });
  return response;
};

const addSettingService = async (model: TUpdateSettingSender) => {
  const url = buildRequestURL('NewLeagueSetting', 'insert123');
  const response = await post({ url, ...model });
  return response;
};

const removeSettingService = async (leagueSettingId: number) => {
  const url = buildRequestURL('NewLeagueSetting', 'remove');
  const response = await post({ url, leagueSettingId });
  return response;
};

export { updateSettingService, addSettingService, removeSettingService };
