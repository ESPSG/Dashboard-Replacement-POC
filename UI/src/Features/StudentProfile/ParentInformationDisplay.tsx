import React, { Fragment } from 'react';
import { Row, Col } from 'antd';
import { StudentParentInformation } from 'Utilities/StudentModelTypes';
import DisplayBlock from 'Components/DisplayBlock';
import ContactDisplay from './ContactDisplay';

type ParentInformationDisplayProps = {
  parents: StudentParentInformation[],
  [x: string]: any
}

function ParentInformationDisplay(props: ParentInformationDisplayProps) {
  const {parents, ...otherProps} = props;

  const isLastRow = (index: number) => {
    return index === parents.length -1;
  }

  return (
    <Fragment>
      {
        parents.sort(
          (a, b) => {return a.isPrimaryContact == b.isPrimaryContact ? 0 : a.isPrimaryContact? -1 : 1}
        ).map((parentData, index) => 
          <Row gutter={[16,4]} className={"mb-2 " + (isLastRow(index) ? "" : "parent-info-row")} key={index}>
            <Col xs={24} className="mb-2 flex-normal flex-vertical-center">
              <div className="color-secondary-dark font-med font-size-large">{parentData.fullName}</div>
              <span className="color-primary font-size-normal ml-1">({parentData.relation})</span>
              {parentData.isPrimaryContact && <span className="color-secondary font-size-normal ml-auto font-med">Primary</span>}
            </Col>
            
			<DisplayBlock title="Home Address" field={parentData.homeAddress} xs={12} sm={6} md={8} xl={6}/>
            <DisplayBlock title="Cell Phone" field={parentData.telephoneNumber} xs={12} sm={6} md={8} xl={6}/>
            <DisplayBlock title="Work Phone" field={parentData.workTelephoneNumber} xs={12} sm={6} md={8} xl={6}/>
            <DisplayBlock title="Email" field={parentData.emailAddress} xs={12} sm={6} md={12} lg={8} xl={8}/>
          </Row>
        )
      }
    </Fragment>

  )
}

ParentInformationDisplay.defaultProps = {
  showPhoneNumber: true
}

export default ParentInformationDisplay;