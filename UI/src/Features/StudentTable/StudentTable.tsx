import './StudentTable.scss';
import React from 'react';
import NavigationPillBar from 'Components/NavigationPillBar';
import StudentGrid from './StudentGrid';

const StudentTable: React.FC = () => {
  const teacherTabs = [
    {label: "Overview", active: true},
    {label: "Student Growth", active: false}
  ]

  return (
    <div className="flex-column">
      <div className="table-title"><h2>Henry O. Bentley</h2></div>
      <NavigationPillBar className="teacher-pills" pills={teacherTabs} />
      <StudentGrid />
    </div>
  )
}

export default StudentTable;