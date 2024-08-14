type TNode={
  id: number
  label: string,
  childNode: TNode[]
}
type TDataSource= {
  dataSource: Array<TNode>
}

export {
  TNode,
  TDataSource
};