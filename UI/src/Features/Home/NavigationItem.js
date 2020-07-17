import './NavigationItem.scss';
import React from 'react';
import { toggledClass } from 'Utilities/HelperFunctions';

function NavigationItem(props) {
  const {label, active, icon, ...others} = props;

  return (
    <div className={"navbar__item full-width" + toggledClass("active", active)} {...others}>
      <div className="navbar__item--icon flex-center">
        {icon}
      </div>
      {label}
    </div>
  )
}

NavigationItem.propTypes = {
}

export default NavigationItem;