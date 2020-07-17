import React from 'react';
import { Col } from 'antd';
import { Contact } from 'Utilities/StudentModelTypes';

type ContactDisplayProps = {
  address: Contact,
  displayTitle?: boolean,
  showPhoneNumber?: boolean,
  [x: string]: any
}

function ContactDisplay(props: ContactDisplayProps) {
  const {address, displayTitle, showPhoneNumber, ...otherProps} = props;

  return (
    <Col className="flex-column display-block" {...otherProps}>
      {displayTitle && <div className="field-label">Address</div>}
      <div className="field-value">{address.addressLine1}</div>
      <div className="field-value">{address.cityStateZip()}</div>
      {showPhoneNumber && <div className="field-value">{address.telephoneNumber}</div>}
    </Col>
  )
}

ContactDisplay.defaultProps = {
  displayTitle: false,
  showPhoneNumber: true
}

export default ContactDisplay;