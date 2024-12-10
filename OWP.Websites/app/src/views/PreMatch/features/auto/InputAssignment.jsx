import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { PlusCircleIcon, RemoveCircleIcon } from 'mdi-react';

import { validateAssignment } from '../league/Cell/AssignmentCell/AssignmentHelper';
import ToastrWrapper from '../../../components/ToastrWrapper';
import UserService from '../../../Shared/User/UserService';

const InputAssignment = ({ pairIndex, username, assignmentType, bettype, showAddOther, showRemoveOther, onChange, onAdd, onRemove }) => {
    const [inputUsername, setUsername] = useState(username);
    const [suggestions, setSuggestion] = useState([]);

    const handleChange = e => {
        const { value } = e.target;
        setUsername(value);

        if (value && value.trim() && value.length >= 2) {
            const response = UserService.getSuggestion(value);
            if (response) {
                setSuggestion(response.map(({ UserId, UserName, NickName }) => ({
                    userId: UserId,
                    username: UserName,
                    nickname: NickName
                })));
            }
        } else {
            setSuggestion([]);
        }
    };

    const handleAddAssignmentType = () => onAdd(pairIndex, bettype);

    const handleRemoveAssignmentType = () => onRemove(pairIndex, bettype);

    const handleBlur = async () => {
        if (inputUsername) {
            const { validateResult, valueAfterValidate, userId } = await validateAssignment(inputUsername, '');

            if (!validateResult) {
                await ToastrWrapper.showError('User not found');
                onChange(pairIndex, bettype, assignmentType, 0, '');
                setUsername('');
            } else {
                onChange(pairIndex, bettype, assignmentType, userId, valueAfterValidate);
                setUsername(valueAfterValidate);
            }
        } else {
            onChange(pairIndex, bettype, assignmentType, 0, '');
        }

        if (suggestions.length > 0) {
            setSuggestion([]);
        }
    };

    const renderSuggestion = () => {
        if (suggestions.length > 0) {
            return (
                <div className='input__suggestion'>
                    <ul className='suggestion__item__container'>
                        {suggestions.map(({ username, userId }) =>
                            <li className='suggestion__item' key={userId}
                                onMouseDown={event => event.preventDefault()}
                                onClick={() => { setUsername(username); setSuggestion([]); }}
                            >
                                {username}
                            </li>
                        )}
                    </ul>
                </div>
            );
        }

        return null;
    };

    return (
        <div className='input__assignment'
            tabIndex={pairIndex} onBlur={handleBlur}
        >
            <input
                type='text' name='league'
                className='input-field'
                autoFocus={false}
                value={inputUsername}
                maxLength='30' autoComplete='off'
                placeholder='Type a username'
                onChange={handleChange}
            />
            {
                showAddOther &&
                <PlusCircleIcon size={15} className='btn-add'
                    onClick={handleAddAssignmentType}
                />
            }
            {
                showRemoveOther &&
                <RemoveCircleIcon size={15} className='btn-remove'
                    onClick={handleRemoveAssignmentType}
                />
            }
            {renderSuggestion()}
        </div>
    );
};

InputAssignment.propTypes = {
    onChange: PropTypes.func.isRequired,
    username: PropTypes.string,
    assignmentType: PropTypes.number,
    bettype: PropTypes.number,
    showAddOther: PropTypes.bool,
    showRemoveOther: PropTypes.bool,
    pairIndex: PropTypes.number,
    onAdd: PropTypes.func,
    onRemove: PropTypes.func
};

export default InputAssignment;