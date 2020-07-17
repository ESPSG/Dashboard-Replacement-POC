export const TITLES = {
  NAME: "Name",
  GRADE: "Grade Level",
  TWENTY_DAY_ATTENDANCE: "Last 20 Days Attendance",
  FORTY_DAY_ATTENDANCE: "Last 40 Days Attendance",
  YTD_ATTENDANCE: "YTD Attendance",
  NUM_ABSENCES: "# Absences",
  NUM_UNEXCUSED_ABSENCES: "# Unexcused Absences",
  TWENTY_DAY_ABSENCES: "Last 20 Days Absences",
  FORTY_DAY_ABSENCES: "Last 40 Days Absences",
  YTD_ABSENCES: "YTD Absences",
  FAILING_GRADES: "Failing Grades",
  GRADES_BELOW_PROF: "# Grades Below Proficiency",
  GRADES_DROPPING_BELOW: "# Grades Dropping Below Proficiency",
  CREDIT_ACCUMULATION: "Credit Accumulation",
}

export const ROUTES = {
  STUDENT_LIST: "/StudentList",
  STUDENT_PAGE: "/StudentPage"
}

export const withParam = (endpoint: string, name: string, value: string | Number) => {
  return `${endpoint}?${name}=${value}`;
}

export const withInlineParam = (endpoint: string, value: string | Number) => {
  return `${endpoint}/${value}`;
}

export const ENDPOINTS = {
  GET_STUDENT: "/student"
}

export const displayProps = {
  xs: 12
}

export const ROOT_METRIC_ID = 65;