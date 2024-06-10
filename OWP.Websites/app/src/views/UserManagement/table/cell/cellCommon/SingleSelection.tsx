import { ChangeEventHandler } from 'react';
import { Option } from '../../../../Shared/MultipleSelection/type';

const SingleSelection = (props: {
  defaultValue: string | number | undefined,
  onChange: ChangeEventHandler<HTMLSelectElement>,
  options: Option[],
  disabled?: boolean
}) =>
  <div className='select-wrapper-default'>
    <select
      disabled={props.disabled}
      className='browser-default'
      value={props.defaultValue}
      onChange={props.onChange}>
      {props.options.map(opt => opt.value !== null
        ? <option key={opt.value} value={opt.value}>
          {opt.label}
        </option>
        : undefined
      )}
    </select>
  </div>;

export default SingleSelection;