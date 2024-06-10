import { FormEvent, useEffect, useState } from 'react';
import { TicketSettingRecipient } from '../TypeNative';
import { post } from '../../Service/Service';
import ToastrWrapper from '../../../components/ToastrWrapper';
import SuggestionList from './SuggestionList';
import { NET_CORE_QUERY_SIMPLE_API, QUERY_SIMPLE_API } from '../../Constants';
import { getRecipientSuggestionKey } from '../Const.json';
import { orderBy } from 'lodash';

type PropsType = {
  currentRecipients: TicketSettingRecipient[];
  handleAddRecipient: Function;
  isIncludeGroup: boolean;
  isIncludeSection: boolean;
  siteId: number;
};

const RecipientSuggestion = (data: PropsType) => {
  const [allRecipients, setAllRecipients] = useState(
    [] as TicketSettingRecipient[]
  );
  const [suggestions, setSuggestions] = useState(
    [] as TicketSettingRecipient[]
  );
  const [searchValue, setSearchValue] = useState('');

  const url =
    process.env.NODE_ENV === 'development'
      ? `${NET_CORE_QUERY_SIMPLE_API}`
      : `${QUERY_SIMPLE_API}`;

  const postModel = {
    url,
    apiKey: getRecipientSuggestionKey,
    IncludeGroup: data.isIncludeGroup,
    IncludeSection: data.isIncludeSection,
    SiteId: data.siteId
  };

  useEffect(() => {
    post(postModel).then(
      response => {
        const result: TicketSettingRecipient[] =
          response.data as TicketSettingRecipient[];
        const sortedRecipient = orderBy(
          result,
          ['recipientType', 'recipientName'],
          ['desc', 'asc']
        );
        setAllRecipients(sortedRecipient);
      },
      error => {
        ToastrWrapper.showError(error);
      }
    );
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    setSuggestions(getSuggestions(searchValue));
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [data.currentRecipients]);

  const getSuggestions = (value: string) => {
    if (value === '') {
      return [];
    }

    return allRecipients.filter(
      recipient =>
        (recipient.recipientName.toLowerCase().includes(value.toLowerCase()) ||
          (recipient.recipientNickname ?? '')
            .toLowerCase()
            .includes(value.toLowerCase())) &&
        !isCurrentRecipientsContainsRecipient(recipient)
    );
  };

  const isCurrentRecipientsContainsRecipient = (
    recipient: TicketSettingRecipient
  ) =>
    data.currentRecipients.some(
      x =>
        x.recipientId === recipient.recipientId &&
        x.recipientType === recipient.recipientType
    );

  const onChange = (e: FormEvent<HTMLInputElement>) => {
    setSearchValue(e.currentTarget.value);
    setSuggestions(getSuggestions(e.currentTarget.value));
  };

  const handleAddRecipient = (recipient: TicketSettingRecipient) => {
    setSearchValue('');
    data.handleAddRecipient(recipient);
  };

  return (
    <div className='filter-suggestions'>
      <div className='group-form'>
        <div className='left-form'>
          <label className='label' htmlFor='recipient'>
            Recipient:
          </label>
        </div>
        <div className='right-form'>
          <input
            value={searchValue}
            id='recipient'
            type='text'
            className='input-field'
            onChange={onChange}
            autoComplete='off'
            placeholder={
              data.isIncludeSection
                ? 'Add by individual, group or section'
                : 'Add by individual'
            }
          />
        </div>
      </div>
      {searchValue.length > 1 && (
        <SuggestionList
          suggestions={suggestions}
          handleAddRecipient={handleAddRecipient}
        />
      )}
    </div>
  );
};

export default RecipientSuggestion;
