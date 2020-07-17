import './StudentProfile.scss';
import React, { useState, useEffect } from 'react';
import { useGraphQLQuery } from 'Utilities/CustomHooks';
import { ENDPOINTS, withInlineParam } from 'Utilities/Constants';
import {useParams} from "react-router-dom";
import { StudentModel, StudentParentInformation } from 'Utilities/StudentModelTypes';
import LoadingSection from 'Components/LoadingSection';
import NavigationPillBar from 'Components/NavigationPillBar';
import { Row, Col } from 'antd';
import DisplayBlock from 'Components/DisplayBlock';
import ContactDisplay from './ContactDisplay';
import CollapsibleCard from 'Components/CollapsibleCard';
import ParentInformationDisplay from './ParentInformationDisplay';
import SchoolInformationDisplay from './SchoolInformationDisplay';
import Render from 'Components/Render';
import StudentIndicatorDisplay from './StudentIndicatorDisplay';
import { studentProfileQuery } from 'Utilities/Queries';
import PersonIcon from '@material-ui/icons/Person';

const fakeStudent = () => {
  var student = new StudentModel();
  student.fakeData();
  return student;
}

const getStudentPills = () => {
  return [
    {label: "Student Info", active: true},
    {label: "Academics", active: false},
    {label: "Transcript", active: false},
    {label: "Early Warning", active: false},
    {label: "Interventions", active: false}
  ]
}

export const StudentProfile = () => {
  let {studentId} = useParams();
  const [studentData, isLoading] = useGraphQLQuery(studentProfileQuery(studentId), fakeStudent(), (res:any) => res.data.data.student || new StudentModel());
  const [studentDataTyped, setStudentDataTyped] = useState(new StudentModel());

  useEffect(() => {
    var temp = new StudentModel();
    temp.copy(studentData);
    setStudentDataTyped(temp);
  }, [studentData])

  return (
    <LoadingSection isLoading={isLoading}>
      <div className="animate-slide-down">
        <div className="full-width flex-column mb-2">
            <div className="full-width flex-normal">
              <div className="portrait__wrapper">
                <PersonIcon className="color-primary-light" fontSize="inherit" />
                {/* <img src={studentDataTyped.profileThumbnail ? studentDataTyped.profileThumbnail : ""} className="portrait__photo full-size"/> */}
              </div>
              <div className="ml-3 flex-column">
                <h2 className="color-secondary-dark mb-2">{studentDataTyped.fullName}</h2>
                <ContactDisplay title="Address" address={studentDataTyped} className="color-primary"/>
              </div>
            </div>
        </div>
        <NavigationPillBar className="teacher-pills mb-2" pills={getStudentPills()} />
        <div className="full-width">
            <Row gutter={[16, 16]}>
              <Col xs={24} md={10} xl={7}>
                <CollapsibleCard title="Demographics">
                  <Row gutter={[16,4]}>
                    <DisplayBlock title="Date of Birth" field={studentDataTyped.dateOfBirth} xs={12}/>
                    <DisplayBlock title="Place of Birth" field={studentDataTyped.placeOfBirth} xs={12}/>
                    <DisplayBlock title="Age" field={studentDataTyped.currentAge} xs={12}/>
                    <DisplayBlock title="Gender" field={studentDataTyped.gender} xs={12}/>
                    <DisplayBlock title="Hispanic or Latino" field={studentDataTyped.hispanicLatinoEthnicity} xs={12}/>
                    <DisplayBlock title="Race" field={studentDataTyped.race} xs={12}/>
                    <DisplayBlock title="Home Language" field={studentDataTyped.homeLanguage} xs={12}/>
                    <DisplayBlock title="Student Language" field={studentDataTyped.language} xs={12}/>
                    <DisplayBlock title="Parent in Military" field={studentDataTyped.parentMilitary} xs={12}/>
                  </Row>
                </CollapsibleCard>
              </Col>
              <Col xs={24} md={14} xl={10}>
                <CollapsibleCard title="Guardian / Parent Contacts">
                  <ParentInformationDisplay 
                    parents={studentDataTyped.studentParentInformation ? studentDataTyped.studentParentInformation : []}
                  />
                </CollapsibleCard>
              </Col>
              <Col xs={24} md={12} xl={7}>
                <CollapsibleCard title="School Information">
                  <SchoolInformationDisplay schools={studentDataTyped.studentSchoolInformation ? studentDataTyped.studentSchoolInformation : []}/>
                </CollapsibleCard>
              </Col>
              <Render condition={studentDataTyped.hasPrograms()}>
                <Col xs={24} md={12} xl={10}>
                  <CollapsibleCard title="Programs">
                    <StudentIndicatorDisplay indicators={studentDataTyped.programs()}/>
                  </CollapsibleCard>
                </Col>
              </Render>
              <Render condition={studentDataTyped.hasOtherIndicators()}>
                <Col xs={24} md={12} xl={10}>
                  <CollapsibleCard title="Other Indicators">
                    <StudentIndicatorDisplay indicators={studentDataTyped.otherIndicators()}/>
                  </CollapsibleCard>
                </Col>
              </Render>
            </Row>
        </div>
      </div>
    </LoadingSection>
  )
}

export default StudentProfile;