import { createContext } from 'react';
import { noop } from 'lodash';

export const HighlightUserContext = createContext({ userName: '' });
export const UpdateNoteContext = createContext({ onUpdateNotes: noop });
export const UpdateAssignmentContext = createContext({
  onUpdateAssignments: noop
});
export const AppContext = createContext({
  detailInformationPolicy: { HasRead: false, HasWrite: false }
});
