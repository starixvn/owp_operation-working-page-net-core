import { createContext } from 'react';
import { noop } from 'lodash';

export default createContext({
  onUpdateAssignments: noop
});
