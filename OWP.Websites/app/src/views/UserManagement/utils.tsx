import {
    TDataResponse,
    TQueryUserInfo,
    TRefResource,
    TUpdateUserInfo
} from './type';
import { Option } from '../Shared/MultipleSelection/type';
import store from './app/useStoreNative';
import { post } from '../Shared/Service/Service';

const normalizeOptions = (
    optionsSource: Array<TRefResource>,
    noneOption: Option
) => {
    const options = optionsSource.map(
        o =>
        ({
            value: o.resourceId,
            label: o.value
        } as Option)
    );
    options.push(noneOption);
    return options;
};

const buildOptions = (optionsSource: Array<TRefResource>) =>
    optionsSource.map(
        o =>
        ({
            value: o.resourceId,
            label: o.value
        } as Option)
    );

const normalizeLocationCodeOptions = (
    optionsSource: Array<TRefResource>,
    noneOption: Option
) => {
    const options = optionsSource.map(
        o =>
        ({
            value: o.value,
            label: o.value
        } as Option)
    );
    options.push(noneOption);
    return options;
};

const getValueFilter = (): TQueryUserInfo => {
    const { filterValue, permissions, currentSection } = store.getState();
    const { userManagementAllSections } = permissions;
    const { userName, role, section, department, category, locationCode } =
        filterValue;
    const sectionId = userManagementAllSections.isRead
        ? section.map(r => r.value).join(',')
        : currentSection?.resourceId?.toString() ?? '0';
    return {
        user: userName,
        roleId: role.map(r => r.value).join(','),
        sectionId,
        departmentId: department.map(r => r.value).join(','),
        categoryId: category.map(r => r.value).join(','),
        locationCode: locationCode.map(r => r.value).join(',')
    };
};

const urlExport = `/api/UserManagement/export-excel`;

const updateUserInfo = async (updateInfo: TUpdateUserInfo) => {
    const url = `/api/UserManagement/update-user-info`;

    return (
        await post({
            url,
            ...updateInfo
        })
    ).data as TDataResponse;
};
const isValidEmail = (email: string | null) => {
    const pattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    return pattern.test(email ?? '');
};

const hasPermissionUser = (userSection: string | null) => {
    const { permissions, currentSection } = store.getState();
    const { userManagementOwnSection, userManagementAllSections } = permissions;
    const temp = userSection ?? null;

    return (
        userManagementAllSections.isHost ||
        (userManagementOwnSection.isHost &&
            userManagementAllSections.isRead &&
            currentSection?.value === temp) ||
        (userManagementOwnSection.isHost && !userManagementAllSections.isRead)
    );
};

export {
    buildOptions,
    normalizeOptions,
    normalizeLocationCodeOptions,
    getValueFilter,
    urlExport,
    updateUserInfo,
    isValidEmail,
    hasPermissionUser
};
