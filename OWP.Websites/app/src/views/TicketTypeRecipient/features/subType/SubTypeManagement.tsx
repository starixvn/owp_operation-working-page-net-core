import { Subtype } from '../TypeNative';
import SubtypeInput from './SubTypeInput';
import SubtypeTable from './SubTypeTable';
import update from 'immutability-helper';
import { uniqueId } from 'lodash';
import useStore from '../../app/zustand/ticketTypeStore';
import { validateDuplicateSubtype } from '../../helper/validateDuplicateSubType';
import ToastrWrapper from '../../../components/ToastrWrapper';

type Prop = {
  ticketTypeId: number;
};

const SubTypeManagement = (props: Prop) => {
  const subtypes = useStore(state => state.subtypes);
  const storeSubtypeExternal = useStore(state => state.setSubtypes);
  const setSubtypeSelected = useStore(state => state.setSubtypeSelected);
  const recipient = useStore(state => state.recipients);
  const storeRecipientExternal = useStore(state => state.setRecipients);
  const handleAddNew = (subtype: Subtype) => {
    const newSubtypes = update(subtypes, {
      $push: [
        {
          ...subtype,
          typeId: props.ticketTypeId
        }
      ]
    });
    storeSubtypeExternal(newSubtypes);
  };

  const handleUpdateSubtype = (value: string, index: number) => {
    if (validateDuplicateSubtype(value, subtypes)) {
      const newSubtypes = update(subtypes, {
        [index]: {
          subtypeName: {
            $set: value
          }
        }
      });
      storeSubtypeExternal(newSubtypes);
      const assignRecipient = [...recipient];
      const updatedAssignRecipient = assignRecipient.map(element => {
        if (element.subtypeName === subtypes[index].subtypeName) {
          return {
            ...element,
            subtypeName: value
          };
        }
        return element;
      });
      storeRecipientExternal(updatedAssignRecipient);
    } else {
      ToastrWrapper.showError(
        `A subtype with this name <b>${value}</b> already exists. Please try again!`
      );
    }
  };

  const handleSelectAnotherSubtype = (subtype: Subtype) => {
    setSubtypeSelected(subtype);
  };

  return (
    <>
      <SubtypeInput onAddNew={handleAddNew} subtypes={subtypes} />
      <SubtypeTable
        key={uniqueId()}
        handleSelectAnotherSubtype={handleSelectAnotherSubtype}
        subtypes={subtypes}
        onUpdateSubtype={handleUpdateSubtype}
      />
    </>
  );
};

export default SubTypeManagement;
