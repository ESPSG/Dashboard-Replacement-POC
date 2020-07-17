import './NavigationPillBar.scss';
import React from 'react';
import PropTypes from 'prop-types';
import { toggledClass } from 'Utilities/HelperFunctions';

function NavigationPill(props) {
  const {label, active, ...others} = props;

  return (
    <div className={"navpill__wrapper flex-center" + toggledClass("active", active)} {...others}>
      {label}
      <div className="navpill--underline" />
    </div>
  )
}

NavigationPill.propTypes = {
}

export default NavigationPill;