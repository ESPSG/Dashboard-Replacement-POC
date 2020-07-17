import React, { useState } from 'react';
import { Input, Space, Button, Tooltip } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import { StudentMetrics } from './StudentModelTypes';
import { ColumnsType } from 'antd/lib/table';
import PersonIcon from '@material-ui/icons/Person';
import { useHistory } from 'react-router-dom';
import { withInlineParam, ROUTES, TITLES } from './Constants';
import { genericSorter } from './HelperFunctions';

export const useNameFilterFunctions = () => {
  const [searchText, setSearchText] = useState("");
  var searchInput:any = React.createRef();
  
  const handleSearch = (selectedKeys:any, confirm:any) => {
    confirm();
    setSearchText(selectedKeys[0]);
  }

  const handleReset = (clearFilters:any) => {
    clearFilters();
    setSearchText("");
  }

  return {
    filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters }:any) => (
      <div className="p-2">
        <Input
          ref={node => {
            searchInput = node;
          }}
          placeholder="Search students"
          value={selectedKeys[0]}
          onChange={e => setSelectedKeys(e.target.value ? [e.target.value] : [])}
          onPressEnter={() => handleSearch(selectedKeys, confirm)}
          className="mb-2"
        />
        <Space>
          <Button
            type="primary"
            onClick={() => handleSearch(selectedKeys, confirm)}
            icon={<SearchOutlined />}
            size="small"
            style={{ width: 90 }}
          >
            Search
          </Button>
          <Button onClick={() => handleReset(clearFilters)} size="small" style={{ width: 90 }}>
            Clear
          </Button>
        </Space>
      </div>
    ),
    filterIcon: (filtered:boolean) => <SearchOutlined className={filtered ? "font-color-secondary" : ""} />,
    onFilterDropdownVisibleChange: (visible:boolean) => {
      if (visible) {
        setTimeout(() => searchInput.select());
      }
    },
    onFilter: (value:string | number | boolean, record:StudentMetrics) =>
      record.fullName !== undefined && record.fullName !== null && record.fullName.toLowerCase().includes(value.toString().toLowerCase()),
  }
}

export const useInitialTableConfig = () => {
  const nameFilterFunctions = useNameFilterFunctions();
  let history = useHistory();

  const initialTableConfig: ColumnsType<StudentMetrics> = [
    { children: [ { children: [
      { dataIndex: "studentUsi", title: "Profile", fixed: "left", 
        render: (value) => <Tooltip title="Go to profile"><div className="full-size" onClick={() => history.push(withInlineParam(ROUTES.STUDENT_PAGE, value))}>
          <PersonIcon className="color-primary-light profile-link" /></div></Tooltip>,
        align: "center", className: "profile-link-cell"
      },
      { dataIndex:"fullName", title:TITLES.NAME, fixed: "left", sorter: (a:any,b:any) => {return a.fullName.localeCompare(b.fullName)}, defaultSortOrder: "ascend", 
        ...nameFilterFunctions
      }]}]
    },
    {children:[{children:[
      { dataIndex: "gradeLevelSortOrder", title: TITLES.GRADE, sorter: genericSorter("gradeLevelSortOrder"), align: "center" }]}
    ]}
  ];

  return initialTableConfig;
}