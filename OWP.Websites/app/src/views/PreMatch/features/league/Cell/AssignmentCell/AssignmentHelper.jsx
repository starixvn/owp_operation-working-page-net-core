import { post } from '../../../../ImportPrematchHelperNative';
import { API_KEY } from '../../constants/preMatchConst';
import { simpleApiQueryUrl } from '../../../../const/const';

const validateAssignment = async (value, olderValue = '') => {
    let validateResult = '';
    let userId = 0;
    let valueAfterValidate = '';
    let valueBeforeValidate;
    const modelValidation = {
        url: simpleApiQueryUrl,
        apikey: API_KEY.VALIDATE_ASSIGNEE,
        Assignee: value
    };
    const validateResponse = await post(modelValidation);
    if (validateResponse.data.length) {
        valueAfterValidate = validateResponse.data[0].Username;
        userId = validateResponse.data[0].UserId;
        validateResult = true;
        valueBeforeValidate = '';
    } else {
        valueBeforeValidate = olderValue;
        validateResult = false;
    }
    return { validateResult, valueAfterValidate, valueBeforeValidate, userId };
};

export { validateAssignment };
