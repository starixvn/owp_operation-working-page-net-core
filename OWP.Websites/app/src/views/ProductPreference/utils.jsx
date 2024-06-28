import queryString from 'query-string';

const buildQueryDropdownOptionUrl = () =>
  `/api/ProductAssignment/get-dropdown-option`;

const buildQueryProductUrl = filterInfo => {
  const locationId = filterInfo.LocationId.join(',');
  const roleId = filterInfo.RoleId.join(',');
  const sectionId = filterInfo.SectionId.join(',');
  const user = filterInfo.User;

  const params = queryString.stringify({ locationId, roleId, sectionId, user });
  return `/api/ProductAssignment?${params}`;
};
const buildUpdateProductUrl = () => `/api/ProductAssignment`;

const buildLastUpdateUrl = () => `/api/ProductAssignment/last-update`;

const convertRefResourceResponseToDropdownOption = data =>
  data.map(d => ({
    value: d.resourceId,
    label: d.value
  }));

const buildDropdownOptionLocation = data =>
  data.map(d => ({
    value: d.value,
    label: d.value
  }));

const buildDropdownOptionFromResponse = response => {
  const { data } = response;

  const locationOption = buildDropdownOptionLocation(data.locationOption);
  const sectionOption = convertRefResourceResponseToDropdownOption(
    data.sectionOption
  );
  const roleOption = convertRefResourceResponseToDropdownOption(
    data.roleOption
  );
  const multipleMaxGamesOption = convertRefResourceResponseToDropdownOption(
    data.multipleMaxGamesOption
  );
  const levelOptionDefault = convertRefResourceResponseToDropdownOption(
    data.levelOption.levelOptionDefault
  );
  const levelOptionLiveAssistant = convertRefResourceResponseToDropdownOption(
    data.levelOption.levelOptionLiveAssistant
  );
  const levelOptionLiveTrading = convertRefResourceResponseToDropdownOption(
    data.levelOption.levelOptionLiveTrading
  );
  const mainBettypeOptionDefault = convertRefResourceResponseToDropdownOption(
    data.mainBettypeOption.mainBettypeOptionDefault
  );
  const mainBettypeOptionLiveTrading =
    convertRefResourceResponseToDropdownOption(
      data.mainBettypeOption.mainBettypeOptionLiveTrading
    );
  const mainBettypeOptionLiveAssistant =
    convertRefResourceResponseToDropdownOption(
      data.mainBettypeOption.mainBettypeOptionLiveAssistant
    );
  return {
    locationOption,
    sectionOption,
    roleOption,
    multipleMaxGamesOption,
    levelOptionDefault,
    levelOptionLiveAssistant,
    levelOptionLiveTrading,
    mainBettypeOptionDefault,
    mainBettypeOptionLiveTrading,
    mainBettypeOptionLiveAssistant
  };
};

const compareTwoNumberArray = (arraySrc, arrayDest) =>
  arraySrc.length === arrayDest.length &&
  arraySrc.every((element, index) => element === arrayDest[index]);

const buildRoleOptionsFromResponseData = data => {
  const options = convertRefResourceResponseToDropdownOption(data);
  return [
    ...options,
    {
      value: 0,
      label: ''
    }
  ];
};

const getValueIsSelectedFromDropdown = (event, option) => {
  const index = event.nativeEvent.target.selectedIndex;
  return option[index].value;
};

const buildPostModelWhenUpdateUserInfo = (
  userId,
  username,
  label,
  newSelectedValue
) => ({
  userId,
  username,
  label,
  value: newSelectedValue === 0 ? null : newSelectedValue
});

export {
  buildQueryDropdownOptionUrl,
  buildQueryProductUrl,
  buildDropdownOptionFromResponse,
  convertRefResourceResponseToDropdownOption,
  buildRoleOptionsFromResponseData,
  buildUpdateProductUrl,
  getValueIsSelectedFromDropdown,
  buildPostModelWhenUpdateUserInfo,
  buildLastUpdateUrl,
  compareTwoNumberArray
};
