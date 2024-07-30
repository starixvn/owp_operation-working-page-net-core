import { handleCheckButton } from "../../helper/filterHelper";

const NoOfHdpEditableCell = ({
  value,
  leagueName
}: { value: string | undefined; leagueName: string; }) => {
  return (
    <div className="noOf__hdp-cell">
      {value && <p className='noOf__hdp__value'>
          {value}
        </p>}
      {!value &&
        <a className='noOf__hdp__check'
         onClick={() => handleCheckButton(leagueName)}>
        Check
      </a>
      }
    </div>
  );
};


export default NoOfHdpEditableCell;