import Select from 'react-select-ts';
import { useDispatch } from 'react-redux';
import { BETON_OPTIONS, BETTYPE_OPTIONS, GHOST_OPTIONS } from '../../const/const';
import { Option } from '../../../Shared/MultipleSelection/type';
import { TRenderTicketInfo } from '../../type';
import { addTicketInfo, removeTicketInfo } from './directionTicketSlice';

const TicketInfo = (props: TRenderTicketInfo) => {
  const {
    ghostNameId,
    ghostDetail,
    bettypeId,
    betOnId,
    hdp_ou,
    odds,
    note,
    remark } = props.ticketInfo;

  const { index, isRenderRemoveCommand, isRenderAddCommand } = props;

  const dispatch = useDispatch();


  const handleChangeGhostName = (selectedOption: Option | null) => {
    const value = selectedOption?.value;

  };

  const handleAddTicketInfo = () => {
    dispatch(addTicketInfo());
  };

  const handleRemoveTicketInfo = () => {
    dispatch(removeTicketInfo({ index }));
  };

  const renderGhostName = () => (
    <div className='content'>
      <div className='left'>
        Ghost name
      </div>
      <Select
        options={GHOST_OPTIONS}
        className='basic-multi-select'
        classNamePrefix='select'
        onChange={handleChangeGhostName}
        placeholder="Ghost Name..."
        value={GHOST_OPTIONS[ghostNameId]} />
    </div>
  );

  const renderGhostDetail = () => (
    <div className='content'>
      <div className='left'>
        Ghost detail
      </div>
      <div className='input-item'>
        <input value={ghostDetail} className='input' />
      </div>
    </div>
  );

  const renderBettype = () => (
    <div className='content'>
      <div className='left'>
        Bet type
      </div>
      <Select
        options={GHOST_OPTIONS}
        className='basic-multi-select'
        classNamePrefix='select'
        onChange={handleChangeGhostName}
        placeholder="Bettype..."
        value={BETTYPE_OPTIONS.find(option => option.value === bettypeId)} />
    </div>
  );

  const renderBetOn = () => {
    const betOnOptions = (bettypeId === BETTYPE_OPTIONS[0].value
      || bettypeId === BETTYPE_OPTIONS[1].value)
      ? BETON_OPTIONS[0]
      : BETON_OPTIONS[1];

    return <div className='content'>
      <div className='left'>
        Bet on
      </div>
      <Select
        options={betOnOptions}
        className='basic-multi-select'
        classNamePrefix='select'
        onChange={handleChangeGhostName}
        placeholder="Bet on..."
        value={betOnOptions.find(option => option.value === betOnId)} />
    </div>;
  };

  const renderHdpOu = () => (
    <div className='content'>
      <div className='left hdp-ou--label'>
        HDP/OU
      </div>
      <div className='input-item hdp-ou-input'>
        <input className='input' type="number" value={hdp_ou} step={0.25} />
      </div>
    </div>
  );

  const renderOdds = () => (
    <div className='content'>
      <div className='left odds'>
        Odds
      </div>
      <div className='input-item'>
        <input value={odds} className='input' />
      </div>
    </div>
  );

  const renderNote = () => (
    <div className='content'>
      <div className='left'>
        Note
      </div>
      <div className='input-item'>
        <textarea
          className='common-textarea'
          value={note}
          rows={3}
        />
      </div>
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
          value={remark}
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
      <fieldset>
        <legend>Info {index + 1}:</legend>
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