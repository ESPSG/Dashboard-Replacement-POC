import React from 'react';
import { Row } from 'antd';
import { StudentIndicator } from 'Utilities/StudentModelTypes';

type StudentIndicatorDisplayProps = {
  indicators: StudentIndicator[]
}

function StudentIndicatorDisplay(props: StudentIndicatorDisplayProps) {
  const {indicators} = props;

  return (
    <Row gutter={[16,4]}>
      <ul className="indicator-list">
      {
        indicators.map((indicator, index) => 
          <li key={index} className="font-med">
            {indicator.name}
          </li>
        )
      }
      </ul>
    </Row>
  )
}

export default StudentIndicatorDisplay;