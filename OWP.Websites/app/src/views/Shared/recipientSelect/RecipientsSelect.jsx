import { useState, useEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import TextField from '@mui/material/TextField';
import { RecipientOptions } from './RecipientOptions';
import { RecipientsSelectedList } from './RecipientsSelectedList';
import Service from '../createTicketNew/Service';
import '../../../scss/components/_recipientSelect.scss';
import ToastsWrapper from '../../components/ToastrWrapper';
import useOnClickOutside from 'use-onclickoutside';
import { RECIPIENT_HINT } from './const.json';

const propTypes = {
  defaultValue: PropTypes.array,
  onAdd: PropTypes.func,
  onChange: PropTypes.func,
  onRemove: PropTypes.func,
  errorSubmit: PropTypes.bool,
  edit: PropTypes.bool,
  hasWrite: PropTypes.bool,
  onChangeErrorSubmit: PropTypes.func,
  ticketType: PropTypes.number
};

const RecipientsSelect = ({
  defaultValue,
  onAdd = f => f,
  onRemove = f => f,
  onChange,
  errorSubmit,
  onChangeErrorSubmit = f => f,
  hasWrite = true,
  edit,
  ticketType = 0
}) => {
  const [showSuggest, setShowSuggest] = useState(false);
  const [search, setSearch] = useState('');
  const [recipientOptions, setRecipientOptions] = useState([]);
  const [selectedRecipients, setSelectedRecipients] = useState([]);
  const [error, setError] = useState(false);
  const [collapsed, setCollapsed] = useState(false);
  const wrapperRef = useRef(null);

  useOnClickOutside(wrapperRef, () => {
    setShowSuggest(false);
    setCollapsed(true);
  });

  useEffect(() => {
    setRecipientOptions(opts =>
      opts.filter(o => !selectedRecipients.find(x => isRecipientEqual(x, o)))
    );
  }, [selectedRecipients]);

  useEffect(() => {
    setSelectedRecipients(defaultValue || []);
    setCollapsed(defaultValue?.length > 1);
  }, [defaultValue]);

  useEffect(() => {
    setError(errorSubmit);
  }, [errorSubmit]);

  const handleSearch = async value => {
    setError(false);
    onChangeErrorSubmit(false);
    setSearch(value);

    if (value.length < 2) {
      setRecipientOptions([]);
      setShowSuggest(false);
      return;
    }
    const data = await Service.recipient(value);
    setShowSuggest(true);
    setRecipientOptions(
      data.filter(d => !selectedRecipients.find(x => isRecipientEqual(x, d)))
    );
  };

  const handleDeleteItem = async item => {
    onChangeErrorSubmit(false);
    const confirm =
      edit &&
      (await ToastsWrapper.confirm(
        'Are you sure you want to remove this individual/group?'
      ));

    if (confirm?.dismiss) {
      return;
    }

    const { status, data } = await onRemove(item);
    setCollapsed(false);
    if (status === 203) {
      ToastsWrapper.showError(data);
      return;
    }
    const result = selectedRecipients.filter(d => !isRecipientEqual(d, item));
    setSelectedRecipients(result);
    setRecipientOptions([item, ...recipientOptions]);
    onChange && onChange(result);
  };

  const isRecipientEqual = (a, b) =>
    a.recipientId === b.recipientId && a.recipientType === b.recipientType;

  const handleAddItem = async item => {
    const { status, data } = await onAdd(item);
    if (status === 203) {
      ToastsWrapper.showError(data);
      setSearch('');
      return;
    }

    onChangeErrorSubmit(false);
    const newSelectedRecipients = [...selectedRecipients, item];
    setSelectedRecipients(newSelectedRecipients);
    setShowSuggest(false);
    setSearch('');
    setRecipientOptions([]);
    setCollapsed(false);

    onChange && onChange(newSelectedRecipients);
  };

  return (
    <>
      <div id='recipient-select' ref={wrapperRef}>
        <RecipientsSelectedList
          items={selectedRecipients}
          hasWrite={hasWrite}
          onDeleteItem={item => handleDeleteItem(item)}
          onToggleList={value => setCollapsed(value)}
          collapsed={collapsed}
        />
        {hasWrite && (
          <div className='search-bar'>
            <TextField
              onClick={() => {
                setError(false);
                onChangeErrorSubmit(false);
              }}
              placeholder={
                selectedRecipients.length === 0
                  ? 'Add by individual, group or section'
                  : 'Add New'
              }
              value={search}
              onChange={e => handleSearch(e.currentTarget.value)}
              error={error}
              helperText={error ? 'Please select at least one recipient' : ''}
              variant='standard'
            />
          </div>
        )}
        {showSuggest && (
          <RecipientOptions
            options={recipientOptions}
            onAddItem={item => handleAddItem(item)}
          />
        )}
      </div>
      <p
        style={{
          color: 'gray'
        }}>
        {RECIPIENT_HINT[ticketType]}
      </p>
    </>
  );
};

RecipientsSelect.propTypes = propTypes;
export default RecipientsSelect;
