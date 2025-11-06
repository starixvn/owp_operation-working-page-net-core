import Select from 'react-select-ts';
import { useDispatch } from 'react-redux';
import { BETON_OPTIONS, BETTYPE_OPTIONS, GHOST_OPTIONS } from '../../const/const';
import { TRenderTicketInfo } from '../../type';
import { addTicketInfo, removeTicketInfo, updateTicketInfo } from './directionTicketSlice';
import { useRef } from 'react';

const TicketInfo = (props: TRenderTicketInfo) => {
  const {
    ghostName,
    ghostDetail,
    betType,
    betOn,
    hdpOu,
    odds,
    note,
    remark,
    errorMessage } = props.ticketInfo;

  const ghostDetailInputRef = useRef<HTMLInputElement>(null);
  const hdpOuInputRef = useRef<HTMLInputElement>(null);
  const oddsInputRef = useRef<HTMLInputElement>(null);
  const noteInputRef = useRef<HTMLTextAreaElement>(null);
  const remarkInputRef = useRef<HTMLTextAreaElement>(null);
  const { index, isRenderRemoveCommand, isRenderAddCommand } = props;

  const dispatch = useDispatch();

  const handleAddTicketInfo = () => {
    dispatch(addTicketInfo());
  };

  const handleRemoveTicketInfo = () => {
    dispatch(removeTicketInfo({ index }));
  };

  const renderGhostName = () => (
    <>
      <div className='content'>
        <div className='flex'>
          <div className='left'>
            Ghost name
            <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
          </div>
          <Select
            options={GHOST_OPTIONS}
            className='basic-multi-select'
            classNamePrefix='select'
            onChange={selected => {
              selected?.value &&
                dispatch(updateTicketInfo({ index, field: 'ghostName', value: selected.value }));
            }}
            placeholder="Ghost Name..."
            value={GHOST_OPTIONS.find(option => option.value === ghostName)} />
        </div >

        <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>
          {errorMessage?.ghostName}</p>
      </div >
    </>
  );

  const renderGhostDetail = () => (
    <div className='content ghost-detail'>
      <div className='flex'>
        <div className='left'>
          Ghost detail
          <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
        </div>
        <div className='input-item'>
          <input
            defaultValue={ghostDetail}
            className='input'
            ref={ghostDetailInputRef}
            onBlur={() => {
              ghostDetailInputRef.current?.value !== undefined && dispatch(updateTicketInfo({ index, field: 'ghostDetail', value: ghostDetailInputRef.current?.value }));
            }}
          />
        </div>
      </div>
      <p style={{ 'color': 'red', 'paddingLeft': '5px', 'width': '100%' }}>
        {errorMessage?.ghostDetail}</p>
    </div>
  );

  const renderBettype = () => (
    <div className='content'>
      <div className='flex'>
        <div className='left'>
          Bet type
          <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
        </div>
        <Select
          options={BETTYPE_OPTIONS}
          className='basic-multi-select'
          classNamePrefix='select'
          onChange={selected => {
            selected?.value &&
              dispatch(updateTicketInfo({ index, field: 'betType', value: selected.value }));
          }}
          placeholder="Bettype..."
          value={BETTYPE_OPTIONS.find(option => option.value === betType)} />
      </div>
      <p style={{ 'color': 'red', 'paddingLeft': '5px', 'width': '100%' }}>
        {errorMessage?.bettype}</p>
    </div>
  );

  const renderBetOn = () => {
    const betOnOptions = (betType === BETTYPE_OPTIONS[0].value
      || betType === BETTYPE_OPTIONS[1].value)
      ? BETON_OPTIONS[0]
      : BETON_OPTIONS[1];

    return <div className='content'>
      <div className='flex'>
        <div className='left'>
          Bet on
          <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
        </div>
        <Select
          options={betOnOptions}
          className='basic-multi-select'
          classNamePrefix='select'
          onChange={selected => {
            selected?.value &&
              dispatch(updateTicketInfo({ index, field: 'betOn', value: selected.value }));
          }}
          placeholder="Bet on..."
          value={betOnOptions.find(option => option.value === betOn)} />
      </div>
      <p style={{ 'color': 'red', 'paddingLeft': '5px', 'width': '100%' }}>
        {errorMessage?.betOn}</p>
    </div>;
  };

  const renderHdpOu = () => (
    <div className='content hdp-ou'>
      <div className='flex'>
        <div className='left hdp-ou--label'>
          HDP/OU
          <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
        </div>
        <div className='input-item hdp-ou-input'>
          <input
            className='input'
            type="number"
            defaultValue={hdpOu}
            step={0.25}
            ref={hdpOuInputRef}
            onBlur={() => {
              hdpOuInputRef.current?.value
                && dispatch(updateTicketInfo({ index, field: 'hdpOu', value: hdpOuInputRef.current?.value }));

            }} />
        </div>
      </div>
      <p style={{ 'color': 'red', 'paddingLeft': '5px', 'width': '100%' }}>
        {errorMessage?.hdp_ou}</p>
    </div>
  );

  const renderOdds = () => (
    <div className='content'>
      <div className='flex'>
        <div className='left odds'>
          Odds
          <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
        </div>
        <div className='input-item'>
          <input defaultValue={odds}
            ref={oddsInputRef}
            className='input' onBlur={() => {
              oddsInputRef.current?.value !== undefined && dispatch(updateTicketInfo({ index, field: 'odds', value: oddsInputRef.current?.value }));
            }} />
        </div>
      </div>
      <p style={{ 'color': 'red', 'paddingLeft': '5px', 'width': '100%' }}>
        {errorMessage?.odds}</p>
    </div>
  );

  const renderNote = () => (
    <div className='content'>
      <div className='left'>
        Note
        <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
      </div>
      <div className='input-item'>
        <textarea
          defaultValue={note}
          ref={noteInputRef}
          className='common-textarea'
          rows={3}
          onBlur={() => {
            noteInputRef.current?.value !== undefined && dispatch(updateTicketInfo({ index, field: 'note', value: noteInputRef.current?.value }));
          }}
        />
      </div>
      <p style={{ 'color': 'red', 'paddingLeft': '5px', 'width': '100%' }}>
        {errorMessage?.note}</p>
    </div>
  );

  const renderRemark = () => (
    <div className='content'>
      <div className='left'>
        Remark
      </div>
      <div className='input-item'>
        <textarea
          className='common-textarea'
          defaultValue={remark}
          ref={remarkInputRef}
          onBlur={() => {
            remarkInputRef.current?.value && dispatch(updateTicketInfo({ index, field: 'remark', value: remarkInputRef.current?.value }));
          }}
          rows={3}
        />
      </div>
    </div>
  );

  const renderRemoveCommand = () => {
    return (
      <div className='content add-remove-command'>
        {renderButton('Delete', 'btn-remove-command', handleRemoveTicketInfo)}
      </div>
    );
  };

  const renderAddCommand = () => {
    return (
      <div className='content add-remove-command'>
        {renderButton('Add', 'btn-add-command', handleAddTicketInfo)}
      </div>
    );
  };

  const renderAddAndRemoveCommand = () => {
    return (
      <div className='content add-remove-command'>
        {renderButton('Delete', 'btn-remove-command', handleRemoveTicketInfo)}
        {renderButton('Add', 'btn-add-command', handleAddTicketInfo)}
      </div>
    );
  };

  const renderButton = (
    label: string,
    className: string,
    onClick: () => void) => {
    return (
      <button
        className={className}
        onClick={onClick}
      >{label}</button>
    );
  };

  const renderCommand = () => {
    if (isRenderRemoveCommand && isRenderAddCommand) {
      return renderAddAndRemoveCommand();
    }
    if (isRenderRemoveCommand) {
      return renderRemoveCommand();
    }
    if (isRenderAddCommand) {
      return renderAddCommand();
    }
  };


  return (
    <div className='list'>
      <fieldset className='fieldset'>
        <legend className='fieldset-legend'>Info {index + 1}:</legend>
        <div className='ghost-name__ghost-detail'>
          {renderGhostName()}
          {renderGhostDetail()}
        </div>
        <div className='bettype__beton__hdp-ou'>
          {renderBettype()}
          {renderBetOn()}
          {renderHdpOu()}
          {renderOdds()}
        </div>
        {renderNote()}
        {renderRemark()}
        {renderCommand()}
      </fieldset>
    </div>
  );
};


export default TicketInfo;