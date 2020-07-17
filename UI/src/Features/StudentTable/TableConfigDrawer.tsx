import './TableConfigDrawer.scss';
import React, { useState, useEffect } from 'react';
import { Drawer, Tree, Button, Space } from 'antd';
import { MetricNode } from 'Utilities/StudentModelTypes';
import { useDashboardDispatch, useDashboardState, DashboardActionTypes } from 'Context/DashboardContext';
import LoadingSection from 'Components/LoadingSection';

type TableConfigDrawerProps = {
  treeData: any,
  nodeList: MetricNode[],
  isOpen: boolean,
  toggle: () => void,
  [x: string]: any
}

const TableConfigDrawer: React.FC<TableConfigDrawerProps> = ({treeData, toggle, isOpen, nodeList}) => {
  const [expandedKeys, setExpandedKeys] = useState<string[]>([]);
  const [checkedKeys, setCheckedKeys] = useState<string[]>([]);
  const [halfCheckedKeys, setHalfCheckedKeys] = useState<string[]>([]);
  const [isSaving, setIsSaving] = useState(false);
  const dispatch = useDashboardDispatch();
  const {shownColumnKeys} = useDashboardState();

  const onCheck = (checkedKeys: any, info: any) => {
    setCheckedKeys(checkedKeys);
    setHalfCheckedKeys(info.halfCheckedKeys);
  };

  const onExpand = (expandedKeys: any, info: any) => {
    setExpandedKeys(expandedKeys);
  }

  const handleSave = () => {
    dispatch({type: DashboardActionTypes.SetColumnKeys, payload: checkedKeys});
    dispatch({type: DashboardActionTypes.SetParentKeys, payload: halfCheckedKeys});
    toggle();
  }

  useEffect(() => {
    if(shownColumnKeys === undefined || shownColumnKeys.length === 0) {
      const keysChecked = nodeList.filter(x => x !== undefined && x.show).map(x => x.key);
      setExpandedKeys(keysChecked);
      setCheckedKeys(keysChecked);
    } else {
      setCheckedKeys(shownColumnKeys || []);
      setExpandedKeys(shownColumnKeys || []);
    }
  }, [])
  
  return (
    <Drawer title="Configure Table Columns" placement="right" onClose={toggle} visible={isOpen} 
      className="column-config-drawer"
      footer={<Space className="flex-normal">
        <Button onClick={toggle} type="text" className="flex-center mt-1 mb-1" style={{width: "8rem"}}>Cancel</Button>
        <Button onClick={handleSave} type="primary" className="flex-center mt-1 mb-1" style={{width: "8rem"}}>Save</Button>
        </Space>
      }
      footerStyle={{display: "flex", alignItems: "center", justifyContent: "center"}}  
    >
      <LoadingSection isLoading={isSaving} mask>
        <Tree checkable selectable={false} onExpand={onExpand} treeData={treeData} expandedKeys={expandedKeys} checkedKeys={checkedKeys} onCheck={onCheck} />
      </LoadingSection>
    </Drawer>
  )
}

export default TableConfigDrawer;