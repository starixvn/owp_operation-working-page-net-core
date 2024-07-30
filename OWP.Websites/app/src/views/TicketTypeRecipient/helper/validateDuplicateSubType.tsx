import { Subtype } from '../features/TypeNative';

const validateDuplicateSubtype = (value: string, subtypes: Subtype[]) => {
  return !subtypes.some(
    x =>
      x.subtypeName.toLocaleLowerCase().trim() ===
      value.toLocaleLowerCase().trim()
  );
};

export { validateDuplicateSubtype };
