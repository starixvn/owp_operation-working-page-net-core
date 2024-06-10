import isEqual from 'lodash.isequal';

const buildWritePermission = (bettypeName, policy) => {
  const { FullBetTypes, PHBetTypes } = policy;

  return (
    FullBetTypes.HasWrite ||
    (PHBetTypes.HasWrite &&
      (isEqual(bettypeName, 'FM') || isEqual(bettypeName, 'AIO')))
  );
};

export { buildWritePermission };
