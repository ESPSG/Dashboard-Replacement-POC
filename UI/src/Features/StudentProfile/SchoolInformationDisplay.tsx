import React, { Fragment } from 'react';
import { Row } from 'antd';
import { StudentSchoolInformation } from 'Utilities/StudentModelTypes';
import DisplayBlock from 'Components/DisplayBlock';

type SchoolInformationDisplayProps = {
  schools: StudentSchoolInformation[]
}

function SchoolInformationDisplay(props: SchoolInformationDisplayProps) {
  const {schools} = props;

  const isLastRow = (index: number) => {
    return index === schools.length -1;
  }

  return (
    <Fragment>
      {
        schools.map((schoolData, index) => 
          <Row gutter={[16,4]} className={"mb-2 " + (isLastRow(index) ? "" : "parent-info-row")} key={index}>
            <DisplayBlock title="Grade Level" field={schoolData.gradeLevel} xs={12} sm={8} lg={12}/>
            <DisplayBlock title="Late Enrollment" field={schoolData.lateEnrollment} xs={12} sm={8} lg={12}/>
            <DisplayBlock title="Homeroom" field={schoolData.homeroom} xs={12} sm={8} lg={12}/>
            <DisplayBlock title="Date of Entry" field={schoolData.dateOfEntry} xs={12} sm={8} lg={12}/>
            <DisplayBlock title="Date of Withdrawal" field={schoolData.dateOfWithdrawal} xs={12} sm={8} lg={12}/>
            <DisplayBlock title="Graduation Plan" field={schoolData.graduationPlan} xs={12} sm={8} lg={12}/>
            <DisplayBlock title="Expected Graduation Year" field={schoolData.expectedGraduationYear} xs={12} sm={8} lg={12}/>
          </Row>
        )
      }
    </Fragment>
  )
}

export default SchoolInformationDisplay;