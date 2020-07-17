import React from 'react';
import './Footer.scss';
import logo from 'Assets/Images/site-logo.png';

export default function Footer(props) {
  return (
    <div className="footer__wrapper flex-center full-width">
      <div className="footer__content flex-column">
        <span>Dashboard Prototype by ESP Solutions Group, Inc.</span>
        <span>Funded by Ed-fi Alliance</span>
      </div>
      <div className="footer__logo full-width flex-center">
        <img  alt="Ed-Fi Logo" src={logo}/>
      </div>
    </div>
  );
}
