import React, { useEffect, useRef, useState } from 'react';
import PropTypes from 'prop-types';
import { PlusCircleIcon, RemoveCircleIcon } from 'mdi-react';

import UserService from '../../../Shared/User/UserService';

const InputAssignment = ({ pairIndex, username, assignmentType, bettype, showAddOther, showRemoveOther, onChange, onAdd, onRemove }) => {
    const [inputUsername, setUsername] = useState(username);
    const [suggestions, setSuggestion] = useState([]);
    const inputRef = useRef(null);
    const inputAssignmentRef = useRef(null);

    const handleChange = e => {
        const { value } = e.target;
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

        if (value.length === 0 && username.length > 0) {
            onChange(pairIndex, bettype, assignmentType, 0, '');
        }

        setUsername(value);
    };

    useEffect(() => {
        const handleClickOutside = event => {
            if (inputAssignmentRef.current && !inputAssignmentRef.current.contains(event.target)) {
                setSuggestion([]);
            }
        };

        document.addEventListener('mousedown', handleClickOutside);

        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, [inputAssignmentRef]);

    const renderSuggestion = () => {
        if (suggestions.length > 0) {
            return (
                <div className='input__suggestion'>
                    <ul className='suggestion__item__container'>
                        {suggestions.map(({ username, userId }) =>
                            <li className='suggestion__item' key={userId}
                                onClick={() => onChange(pairIndex, bettype, assignmentType, userId, username)}
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

    const handleAddAssignmentType = () => onAdd(pairIndex, bettype);

    const handleRemoveAssignmentType = () => onRemove(pairIndex, bettype);

    return (
        <div className='input__assignment' ref={inputAssignmentRef}>
            <input
                ref={inputRef}
                type='text' name='league'
                className='input-field'
                autoFocus={false}
                value={inputUsername}
                maxLength='100' autoComplete='off'
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