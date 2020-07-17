import React from 'react';
import "./ModalCloseButton.scss";

function ModalCloseButton(props) {
  const {toggle, ...others} = props;

  return (
    <button onClick={toggle} className="close-button" aria-label="Close">
      <span aria-hidden={true}>×</span>
    </button>
  )
}

export default ModalCloseButton;

