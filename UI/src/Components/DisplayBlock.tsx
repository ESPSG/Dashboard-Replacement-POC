import './NavigationPillBar.scss';
import React from 'react';
import { Col } from 'antd';

type DisplayBlockProps = {
  title: string,
  field: any,
  render?: Function,
  [x: string]: any
}

function DisplayBlock(props: DisplayBlockProps) {
  const {title, field, render, ...colProps} = props;

  return (
    <Col className="flex-column display-block" {...colProps}>
      <div className="field-label">{title}</div>
      <div className={"field-value" + (field && field !== null ? "" : " color-primary-light")}>{field && field !== null ? (render ? render(field) : field) : "—"}</div>
    </Col>
  )
}

export default DisplayBlock;