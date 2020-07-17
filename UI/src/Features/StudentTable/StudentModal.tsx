import './StudentModal.scss';
import React from 'react';
import DisplayBlock from 'Components/DisplayBlock';
import { TITLES, displayProps } from 'Utilities/Constants';
import ModalCloseButton from 'Components/ModalCloseButton';
import { Card, Row, Col } from 'antd';
import { renderMetric } from 'Utilities/ValueRenderers';
import { MetricNode, StudentMetrics } from 'Utilities/StudentModelTypes';
import CollapsibleCard from 'Components/CollapsibleCard';

type StudentModalProps = {
  config: MetricNode[],
  dataItem: StudentMetrics | null,
  [x: string]: any
}

function StudentModal(props: StudentModalProps) {
  const {isOpen, toggle, dataItem, config} = props;

  const formatFieldProps = (field:any, isPercent=false, isMetric=false) => {
    const fieldProps:any = {
      field: field
    }

    if(isMetric) {
      fieldProps.render = () => renderMetric(field);
    }

    return fieldProps;
  }

  return (
    isOpen && dataItem !== null ?
      <div className="student-modal animate-slide-down">
        <div className="student-fields-wrapper">
          <ModalCloseButton toggle={toggle} />
          <Row>
            <DisplayBlock {...formatFieldProps(dataItem.fullName)} title={TITLES.NAME} xs={12} md={8} lg={6}/>
            <DisplayBlock {...formatFieldProps(dataItem.gradeLevel)} title={TITLES.GRADE} xs={12} md={8} lg={6}/>
          </Row>
          <Row gutter={[16, 16]}>
            {
              config.map(category => 
                category.children.map(subCategory =>
                  <Col xs={24} md={12} xl={8} key={subCategory.id + "_" + subCategory.name}>
                    <CollapsibleCard title={subCategory.name || ""}>
                      <Row gutter={[16, 4]}>
                        {subCategory.children.map(metricValue =>
                          <DisplayBlock key={metricValue.id + "_" + metricValue.name} title={metricValue.name || ""} field={dataItem[metricValue.id || ""]} xs={12} render={renderMetric}/>
                        )}
                      </Row>
                    </CollapsibleCard>
                  </Col>
              ))
            }
          </Row>
        </div>
      </div>
    : null
  )
}

export default StudentModal;