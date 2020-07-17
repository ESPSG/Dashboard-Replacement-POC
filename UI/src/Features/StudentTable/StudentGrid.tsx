import './StudentGrid.scss';
import React, { useState, useEffect } from 'react';
import StudentModal from './StudentModal';
import { StudentMetrics, MetricNode } from 'Utilities/StudentModelTypes';
import { Table, Button } from 'antd';
import {  useInitialTableConfig } from 'Utilities/TypedCustomHooks';
import { setArrayTypes, metricsToGridConfig, setStudentMetricsObjects, removeDuplicateIds, filterTree, updatedArray } from 'Utilities/HelperFunctions';
import { useGraphQLQuery } from 'Utilities/CustomHooks';
import { allStudentMetricsQuery, studentMetricsQuery } from 'Utilities/Queries';
import { ColumnsType } from 'antd/lib/table';
import SettingsIcon from '@material-ui/icons/Settings';
import TableConfigDrawer from './TableConfigDrawer';
import LoadingSection from 'Components/LoadingSection';
import { useDashboardState, useDashboardDispatch, DashboardActionTypes } from 'Context/DashboardContext';

const StudentGrid: React.FC = () => {
  const {shownColumnKeys, parentColumnKeys, ...dashboardState} = useDashboardState();
  const dispatch = useDashboardDispatch();
  const [studentModalOpen, setStudentModalOpen] = useState(false);
  const [modalDataItem, setModalDataItem] = useState(null);
  const [studentMetricsQueryString, setStudentMetricsQueryString] = useState<string | null>(null);
  const [metricsTyped, setMetricsTyped] = useState<MetricNode[]>(dashboardState.metrics || []);
  const [studentMetricsTyped, setStudentMetricsTyped] = useState<StudentMetrics[]>();
  const [gridNodes, setGridNodes] = useState<MetricNode[]>([]);
  const [drawerOpen, setDrawerOpen] = useState(false);
  const initialTableConfig = useInitialTableConfig();
  const [gridConfig, setGridConfig] = useState<ColumnsType<StudentMetrics>>(initialTableConfig);
  const [metrics, isLoading] = useGraphQLQuery(allStudentMetricsQuery(), [], ({data}:any) => data.data.students[0].metrics || [], dashboardState.metrics === undefined || dashboardState.metrics.length === 0); 
  const [studentMetrics, studentMetricsLoading, {cancelRequest}] = useGraphQLQuery(studentMetricsQueryString, [], ({data}:any) => data.data.students || []); 

  useEffect(() => {
    if(metrics.length > 0) {
      var temp = removeDuplicateIds(setArrayTypes(MetricNode, metrics));
      setMetricsTyped(temp);
      dispatch({type: DashboardActionTypes.SetMetrics, payload: temp});
    }
  }, [metrics]);

  useEffect(() => {
    if(metricsTyped.length > 0) {
      cancelRequest();
      var selectedMetricIds = metricsTyped.filter(m => m.id).map(m => m.id || -1);
      setStudentMetricsQueryString(studentMetricsQuery(selectedMetricIds, 25));
      var config = metricsToGridConfig(metricsTyped);
      setGridNodes(config);
      setGridConfig([...initialTableConfig, ...filterTree(config, n => n.show)]);
    }
  }, [metricsTyped]);

  useEffect(() => {
    if(studentMetrics.length > 0) {
      var temp = setArrayTypes(StudentMetrics, studentMetrics);
      temp = setStudentMetricsObjects(temp);
      setStudentMetricsTyped(temp);
    }
  }, [studentMetrics]);

  useEffect(() => {
    updateShownMetrics();
  }, [shownColumnKeys, parentColumnKeys]);

  const updateShownMetrics = () => {
    var metrics = updateShowValues(metricsTyped);
    setMetricsTyped(metrics);
  }

  const updateShowValues = (metrics: MetricNode[]) => {
    return shownColumnKeys !== undefined && shownColumnKeys.length > 0
      ? metrics.map(m => {
        m.show = shownColumnKeys?.includes(m.key) 
              || parentColumnKeys?.includes(m.key) 
              || false;
        return m;
      })
      : metrics;
  }

  const toggleStudentModal = () => {
    setStudentModalOpen(prev => !prev);
  }

  const toggleDrawer = () => {
    setDrawerOpen(isOpen => !isOpen);
  }

  const handleRowClick = (record:any) => {
    setModalDataItem(record);
    setStudentModalOpen(true);
  }

  return (
    <LoadingSection isLoading={isLoading || studentMetricsLoading}>
      <Button onClick={() => setDrawerOpen(true)} type="primary" icon={<SettingsIcon className="mr-1"/>} className={"flex-center mt-1 mb-1" + (studentModalOpen ? " d-none" : "")} style={{width: "8rem"}}>Columns</Button>
      <Table className={"student-grid animate-slide-down" + (studentModalOpen ? " d-none" : "")}
        dataSource={studentMetricsTyped}
        onRow={(record) => {return {onClick: () => handleRowClick(record)}}}
        scroll={{x: true}}
        rowKey="fullName"
        columns={gridConfig}
        pagination={{position: ["bottomLeft"], showSizeChanger: true, defaultPageSize: 25}}
      />
      <StudentModal isOpen={studentModalOpen} toggle={toggleStudentModal} dataItem={modalDataItem} config={gridNodes}/>
      {metricsTyped.length > 0 && gridNodes.length > 0 &&
        <TableConfigDrawer nodeList={metricsTyped} treeData={gridNodes as any} toggle={toggleDrawer} isOpen={drawerOpen} />
      }
    </LoadingSection>
  )
}

export default StudentGrid;