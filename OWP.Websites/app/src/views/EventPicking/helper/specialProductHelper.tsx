import TDataTable from '../types/dataTableType';

/**
 * Build mapping of ParentId to existing MinorBettypeIds from entities
 * @param parentIds - Array of parent match IDs
 * @param entities - All entities from the table
 * @returns JSON string mapping parentId to comma-separated MinorBettypeIds
 */
export const buildExistingMinorBettypeIdsMapping = (
  parentIds: number[],
  entities: TDataTable[]
): string => {
  const existingMinorBettypeIdsMap: { [key: string]: string } = {};

  parentIds.forEach(parentId => {
    // Get existing child matches for this specific parent
    const childMatchesForThisParent = entities.filter(
      e => e.isChildMatch && e.infoSiteMatchId === parentId
    );

    // Get unique existing MinorBettypeIds for this parent
    const minorBettypeIds = Array.from(
      new Set(childMatchesForThisParent.map(e => e.minorBettypeId))
    )
      .filter(id => id !== 0) // Filter out 0 values
      .join(',');

    if (minorBettypeIds) {
      existingMinorBettypeIdsMap[parentId.toString()] = minorBettypeIds;
    }
  });

  return JSON.stringify(existingMinorBettypeIdsMap);
};
