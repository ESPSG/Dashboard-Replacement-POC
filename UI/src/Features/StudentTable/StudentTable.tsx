import "./StudentTable.scss";
import React from "react";
import NavigationPillBar from "Components/NavigationPillBar";
import StudentGrid from "./StudentGrid";
import { useDashboardState } from "Context/DashboardContext";

const StudentTable: React.FC = () => {
  const teacherTabs = [
    { label: "Overview", active: true },
    { label: "Student Growth", active: false },
  ];
  const { username } = useDashboardState();

  return (
    <div className="flex-column">
      <div className="table-title">
        <h2>{username}</h2>
      </div>
      <NavigationPillBar className="teacher-pills" pills={teacherTabs} />
      <StudentGrid />
    </div>
  );
};

export default StudentTable;
