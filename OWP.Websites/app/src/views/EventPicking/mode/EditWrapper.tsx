import Edit from "./Edit";
import Save from "./Save";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { setMode } from "./modeSlice";
import { VIEW, EDIT } from './mode.json';

const EditWrapper = () => {
  const dispatch = useAppDispatch();
  const { mode } = useAppSelector(s => s.mode);
  const component = {
    [VIEW]: <Edit onSetMode={() => dispatch(setMode(EDIT))} />,
    [EDIT]: <Save onSetMode={() => dispatch(setMode(VIEW))} />
  };
  return component[mode];
};

export default EditWrapper;