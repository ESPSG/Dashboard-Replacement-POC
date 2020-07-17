import "./ErrorDisplay.scss";
import React, {useState} from 'react';
import PropTypes from 'prop-types';

function ErrorDisplay({error, className, ...others}) {
  return (
    <div className={"flex-center error-text " + className} {...others}>
      {error}
    </div>
  );
}

export default ErrorDisplay;